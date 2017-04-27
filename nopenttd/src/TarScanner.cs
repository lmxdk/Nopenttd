using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using Nopenttd.Os.Windows;
using Nopenttd.src;

namespace Nopenttd
{
    /** The mode of tar scanning. */
    [Flags]
    public enum TarScannerMode
    {    /// Scan nothing.
        NONE = 0,     /// Scan for base sets. 
		BASESET = 1 << 0,/// Scan for non-base sets. 
		NEWGRF = 1 << 1,/// Scan for AIs and its libraries. 
		AI = 1 << 2,/// Scan for scenarios and heightmaps. 
		SCENARIO = 1 << 3,/// Scan for game scripts. 
		GAME = 1 << 4, /// Scan for everything.
        ALL = BASESET | NEWGRF | AI | SCENARIO | GAME,
    };

    /** Helper for scanning for files with tar as extension */

    public class TarScanner : FileScanner
    {

        private static readonly ILogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        /**
         * Perform the scanning of a particular subdirectory.
         * @param subdir The subdirectory to scan.
         * @return The number of found tar files.
         */

        uint DoScan(Subdirectory sd)
        {
            FileIO._tar_filelist[(int) sd].Clear();
            FileIO._tar_list[(int) sd].Clear();
            uint num = Scan(".tar", sd, false);
            if (sd == Nopenttd.Subdirectory.BASESET_DIR || sd == Nopenttd.Subdirectory.NEWGRF_DIR)
            {
                num += Scan(".tar", Nopenttd.Subdirectory.OLD_DATA_DIR, false);
            }
            return num;
        }

        /* static */

        public static uint DoScan(TarScannerMode mode)
        {
            Log.Debug("Scanning for tars");
            var fs = new TarScanner();
            uint num = 0;
            if (mode.HasFlag(TarScannerMode.BASESET))
            {
                num += fs.DoScan(Subdirectory.BASESET_DIR);
            }
            if (mode.HasFlag(TarScannerMode.NEWGRF))
            {
                num += fs.DoScan(Subdirectory.NEWGRF_DIR);
            }
            if (mode.HasFlag(TarScannerMode.AI))
            {
                num += fs.DoScan(Subdirectory.AI_DIR);
                num += fs.DoScan(Subdirectory.AI_LIBRARY_DIR);
            }
            if (mode.HasFlag(TarScannerMode.GAME))
            {
                num += fs.DoScan(Subdirectory.GAME_DIR);
                num += fs.DoScan(Subdirectory.GAME_LIBRARY_DIR);
            }
            if (mode.HasFlag(TarScannerMode.SCENARIO))
            {
                num += fs.DoScan(Subdirectory.SCENARIO_DIR);
                num += fs.DoScan(Subdirectory.HEIGHTMAP_DIR);
            }
            Log.Debug($"Scan complete, found {num} files");
            return num;
        }

/**
 * Add a single file to the scanned files of a tar, circumventing the scanning code.
 * @param sd       The sub directory the file is in.
 * @param filename The name of the file to add.
 * @return True if the additions went correctly.
 */

        public bool AddFile(Subdirectory sd, string filename)
        {
            this.subdir = sd;
            return this.AddFile(filename, null);
        }

        public class TarHeader
        {
            public string name; //new [100];      ///Name of the file
            public string mode; //[8];
            public string uid; //[8];
            public string gid; //[8];

            /// Size of the file, in ASCII
            public int size; //[12];       

            public string mtime; //[12];
            public string chksum; //[8];
            public char typeflag;
            public string linkname; //[100];
            public string magic; //[6];
            public string version; //[2];
            public string uname; //[32];
            public string gname; //[32];
            public string devmajor; //[8];
            public string devminor; //[8];

            /// Path of the file
            public string prefix; //[155];

            public string unused; //[12];

            public const int HeaderSize = 512;

            public TarHeader(byte[] buffer)
            {
                if (buffer.Length != HeaderSize)
                    throw new ArgumentOutOfRangeException(nameof(buffer), buffer,
                        $"TarHeader must be {HeaderSize} bytes long");
                var index = 0;

                name = Read(100);
                mode = Read(8);
                uid = Read(8);
                gid = Read(8);
                /* Calculate the size of the file.. for some strange reason this is stored as a string */
                size = int.Parse(Read(12)); //originally ulong
                mtime = Read(12);
                chksum = Read(8);
                typeflag = Read(1)[0];
                linkname = Read(100);
                magic = Read(6);
                version = Read(2);
                uname = Read(32);
                gname = Read(32);
                devmajor = Read(8);
                devminor = Read(8);
                prefix = Read(155);
                unused = Read(12);
                return;

                string Read(int length)
                {
                    var s = buffer.ReadNullTerminatedAsciiString(index, length);
                    index += length;
                    return s;
                }
            }
        }

        public override bool AddFile(string filename, string tar_filename = null)
        {
            /* No tar within tar. */
            Debug.Assert(tar_filename == null);

            /* The TAR-header, repeated for every file */


            /* Check if we already seen this file */
            if (FileIO._tar_list[(int) this.subdir].TryGetValue(filename, out var it) == false)
            {
                return false;
            }

            var links = new Dictionary<string, string>();

            try
            {
                using (var f = new FileInfo(filename).OpenRead())
                {


                    FileIO._tar_list[(int) this.subdir][filename].filename = filename;
                    FileIO._tar_list[(int) this.subdir][filename].dirname = null;
                    var dupped_filename = filename;
                    /// Temporary list to collect links    

                    string name = null; //char[sizeof(th.prefix) + 1 + sizeof(th.name) + 1];
                    string link = null; //char link[sizeof(th.linkname) + 1];
                    string dest = null;
                        //char dest[sizeof(th.prefix) + 1 + sizeof(th.name) + 1 + 1 + sizeof(th.linkname) + 1];
                    var num = 0;
                    var pos = 0;


                    var buffer = new byte[TarHeader.HeaderSize];
                    for (;;)
                    {
                        // Note: feof() always returns 'false' after 'fseek()'. Cool, isn't it?
                        var num_bytes_read = f.Read(buffer, 1, TarHeader.HeaderSize);
                        if (num_bytes_read != TarHeader.HeaderSize) break;
                        pos += num_bytes_read;

                        var th = new TarHeader(buffer);

                        /* Check if we have the new tar-format (ustar) or the old one (a lot of zeros after 'link' field) */
                        if (th.magic != "ustar" && th.magic != "")
                        {
                            /* If we have only zeros in the block, it can be an end-of-file indicator */
                            if (buffer.Any(b => b != 0)) continue;

                            Log.Debug($"The file '{filename}' isn't a valid tar-file");
                            f.Close();
                            return false;
                        }

                        name = null;

                        /* The prefix contains the directory-name */
                        if (th.prefix != "")
                        {
                            name = th.prefix + Path.PathSeparator;
                        }

                        /* Copy the name of the file in a safe way at the end of 'name' */
                        name += th.name;

                        switch (th.typeflag)
                        {
                            case '\0':
                            case '0':
                            {
                                // regular file
                                /* Ignore empty files */
                                if (th.size == 0) break;

                                if (name.Length == 0) break;

                                /* Store this entry in the list */
                                var entry = new TarFileListEntry()
                                {
                                    tar_filename = dupped_filename,
                                    size = th.size,
                                    position = pos
                                };

                                /* Convert to lowercase and our PATHSEPCHAR */
                                name = FileIO.SimplifyFileName(name);

                                Log.Debug($"Found file in tar: ${name} ({th.size} bytes, {pos} offset)");
                                if (FileIO._tar_filelist[(int) this.subdir].ContainsKey(name) == false)
                                {
                                    FileIO._tar_filelist[(int) this.subdir].Add(name, entry);
                                    num++;
                                }

                                break;
                            }

                            case '1': // hard links
                            case '2':
                            {
                                // symbolic links
                                /* Copy the destination of the link in a safe way at the end of 'linkname' */
                                link = th.linkname;

                                if (name.Length == 0 || link.Length == 0) break;

                                /* Convert to lowercase and our PATHSEPCHAR */
                                name = FileIO.SimplifyFileName(name);
                                link = FileIO.SimplifyFileName(link);

                                /* Only allow relative links */
                                if (link[0] == Path.PathSeparator)
                                {
                                    Log.Debug($"Ignoring absolute link in tar: {name} . {link}");
                                    break;
                                }

                                /* Process relative path.
				 * Note: The destination of links must not contain any directory-links. */
                                dest = name;
                                //var destIndex = dest.LastIndexOf(Path.PathSeparator);
                                var destpos = dest;
                                //if (destIndex >= 0)
                                //{
                                //    destpos = dest.Substring(destIndex + 1);
                                //}

                                //TODO THIS MAKES NO SENSE
                                var linkParts = link.Split(Path.PathSeparator);
                                foreach (var linkPart in linkParts)
                                {
                                    if (linkPart == ".")
                                    {
                                        /* Skip '.' (current dir) */
                                    }
                                    else if (linkPart == "..")
                                    {
                                        /* level up */
                                        if (dest == "")
                                        {
                                            Log.Debug(
                                                $"Ignoring link pointing outside of data directory: {name} . {link}");
                                            break;
                                        }

                                        /* Truncate 'dest' after last PATHSEPCHAR.
						 * This assumes that the truncated part is a real directory and not a link. */
                                        destpos = linkParts.Last();
                                        break;
                                    }
                                    else
                                    {
                                        /* Append at end of 'dest' */
                                        if (destpos.Any())
                                        {
                                            destpos += Path.PathSeparator;
                                        }
                                        destpos = dest;
                                    }

                                    //if (destpos >= lastof(dest)) {
                                    //	Log.Debug("The length of a link in tar-file '{filename}' is too large (malformed?)");
                                    //	f.Close();
                                    //	return false;
                                    //}
                                }

                                /* Store links in temporary list */
                                Log.Debug($"Found link in tar: {name} . {dest}");
                                links.Add(name, dest);

                                break;
                            }

                            case '5': // directory
                                /* Convert to lowercase and our PATHSEPCHAR */
                                name = FileIO.SimplifyFileName(name);

                                /* Store the first directory name we detect */
                                Log.Debug($"Found dir in tar: {name}");
                                if (FileIO._tar_list[(int) this.subdir][filename].dirname == null)
                                {
                                    FileIO._tar_list[(int) this.subdir][filename].dirname = name;
                                }
                                break;

                            default:
                                /* Ignore other types */
                                break;
                        }

                        /* Skip to the next block.. */
                        //var skip = Align(th.size, 512);
                        if (f.Seek(th.size, SeekOrigin.Current) < 0)
                        {
                            Log.Debug($"The file '{filename}' can't be read as a valid tar-file");
                            f.Close();
                            return false;
                        }
                        pos += th.size;
                    }

                    Log.Debug($"Found tar '{filename}' with {num} new files");
                    f.Close();
                }
            }
            catch (IOException ex)
            {
                /* Although the file has been found there can be
                 * a number of reasons we cannot open the file.
                 * Most common case is when we simply have not
                 * been given read access. */
                Log.Error(ex);
                return false;
            }



            /* Resolve file links and store directory links.
             * We restrict usage of links to two cases:
             *  1) Links to directories:
             *      Both the source path and the destination path must NOT contain any further links.
             *      When resolving files at most one directory link is resolved.
             *  2) Links to files:
             *      The destination path must NOT contain any links.
             *      The source path may contain one directory link.
             */
            foreach (var link in links)
            {
                var src = link.Key;
                var dest = link.Value;
                FileIO.TarAddLink(src, dest, this.subdir);
            }

            return true;
        }

/**
 * Extract the tar with the given filename in the directory
 * where the tar resides.
 * @param tar_filename the name of the tar to extract.
 * @param subdir The sub directory the tar is in.
 * @return false on failure.
 */

        public bool ExtractTar(string tar_filename, Subdirectory subdir)
        {

            /* We don't know the file. */
            if (FileIO._tar_list[(int) subdir].TryGetValue(tar_filename, out var it) == false)
            {
                return false;
            }

            var dirname = it.dirname;

            /* The file doesn't have a sub directory! */
            if (dirname == null) return false;

            var p = tar_filename.LastIndexOf(Path.PathSeparator);
            /* The file's path does not have a separator? */
            if (p < 0) return false;

            p++;

            dirname = tar_filename.Substring(0, p) + dirname;
            Log.Debug($"Extracting {tar_filename} to directory {dirname}");
            Directory.CreateDirectory(dirname);

            foreach (var it2 in FileIO._tar_filelist[(int) subdir])
            {
                if (it2.Value.tar_filename != tar_filename)
                {
                    continue;
                }
                var filename = dirname + it2.Key;

                Log.Debug($"  extracting {filename}");

                /* First open the file in the .tar. */
                long to_copy = 0;
                using (var @in = FileIO.FioFOpenFileTar(it2.Value, out to_copy))
                {
                    if (@in == null)
                    {
                        Log.Debug($"Extracting {filename} failed; could not open {tar_filename}");
                        return false;
                    }

                    /* Now open the 'output' file. */
                    try
                    {
                        using (var @out = File.Open(filename, FileMode.Create))
                        {
                            /* Now read from the tar and write it into the file. */
                            var buffer = new byte[4096];
                            long read;
                            for (; to_copy != 0; to_copy -= read)
                            {
                                read = @in.Read(buffer, 0, Math.Min((int) to_copy, buffer.Length));
                                if (read <= 0)
                                {
                                    break;
                                }
                                @out.Write(buffer, 0, (int) read);

                            }
                            @out.Close();
                        }
                    }
                    catch (IOException ex)
                    {
                        Log.Error(ex);
                        Log.Debug($"Extracting {filename} failed; could not open {filename}");
                        @in.Close();
                        return false;
                    }


                    /* Close everything up. */
                    @in.Close();
                }

                if (to_copy != 0)
                {
                    Log.Debug("Extracting {filename} failed; still {to_copy} bytes to copy");
                    return false;
                }
            }

            Log.Debug("  extraction successful");
            return true;
        }


        private static readonly Searchpath[] new_openttd_cfg_order =
        {
            Searchpath.SP_PERSONAL_DIR, Searchpath.SP_BINARY_DIR, Searchpath.SP_WORKING_DIR, Searchpath.SP_SHARED_DIR,
            Searchpath.SP_INSTALLATION_DIR
        };


        private static readonly Subdirectory[] default_subdirs =
        {
            Subdirectory.SAVE_DIR, Subdirectory.AUTOSAVE_DIR, Subdirectory.SCENARIO_DIR, Subdirectory.HEIGHTMAP_DIR,
            Subdirectory.BASESET_DIR, Subdirectory.NEWGRF_DIR, Subdirectory.AI_DIR, Subdirectory.AI_LIBRARY_DIR,
            Subdirectory.GAME_DIR, Subdirectory.GAME_LIBRARY_DIR, Subdirectory.SCREENSHOT_DIR
        };

        private static readonly Subdirectory[] dirs =
        {
            Subdirectory.SCENARIO_DIR, Subdirectory.HEIGHTMAP_DIR,
            Subdirectory.BASESET_DIR, Subdirectory.NEWGRF_DIR, Subdirectory.AI_DIR, Subdirectory.AI_LIBRARY_DIR,
            Subdirectory.GAME_DIR, Subdirectory.GAME_LIBRARY_DIR
        };

        /**
         * Acquire the base paths (personal dir and game data dir),
         * fill all other paths (save dir, autosave dir etc) and
         * make the save and scenario directories.
         * @param exe the path from the current path to the executable
         */

        void DeterminePaths(string exe)
        {
            Win32.DetermineBasePaths(exe);

            foreach (var sp in FileIO.FOR_ALL_SEARCHPATHS())
            {
                if (sp == Searchpath.SP_WORKING_DIR && !FileIO._do_scan_working_directory)
                {
                    continue;
                }
                Log.Debug($"{FileIO._searchpaths[(int)sp]} added as search path");
            }


            string config_dir;
            if (FileIO._config_file != null)
            {
                config_dir = FileIO._config_file;
                var index = FileIO._config_file.LastIndexOf(Path.PathSeparator);
                if (index < 0)
                {
                    config_dir = "";
                }
                else
                {
                    config_dir = config_dir.Substring(0, index + 1);
                }
            }
            else
            {
                var personal_dir = FileIO.FioFindFullPath(Subdirectory.BASE_DIR, "openttd.cfg");

                if (personal_dir != null)
                {
                    var end = personal_dir.LastIndexOf(Path.PathSeparator);
                    if (end >= 0)
                    {
                        personal_dir.Substring(0, end); //end[1] = '\0';
                    }
                    config_dir = personal_dir;
                }
                else
                {

                    config_dir = null;
                    foreach (var path in new_openttd_cfg_order)
                    {
                        if (FileIO.IsValidSearchPath(path))
                        {
                            config_dir = FileIO._searchpaths[(int) path];
                            break;
                        }
                    }
                    Debug.Assert(config_dir != null);
                }
                FileIO._config_file = config_dir + "openttd.cfg";
            }

            Log.Debug(config_dir + " found as config directory");

            FileIO._highscore_file = config_dir + "hs.dat";
            Hotkeys._hotkeys_file = config_dir + "hotkeys.cfg";
            Window._windows_file = config_dir + "windows.cfg";

            FileIO._personal_dir = config_dir;

            /* Make the necessary folders */
            Directory.CreateDirectory(config_dir);
            if (config_dir != FileIO._personal_dir)
            {
                Directory.CreateDirectory(FileIO._personal_dir);
            }

            Log.Debug(FileIO._personal_dir + " found as personal directory");


            foreach (var subdir in default_subdirs)
            {
                var dir = FileIO._personal_dir + FileIO._subdirs[(int) subdir];
                Directory.CreateDirectory(dir);
            }

            /* If we have network we make a directory for the autodownloading of content */
            FileIO._searchpaths[(int) Searchpath.SP_AUTODOWNLOAD_DIR] =
                $"{FileIO._personal_dir}content_download{Path.PathSeparator}";
            Directory.CreateDirectory(FileIO._searchpaths[(int) Searchpath.SP_AUTODOWNLOAD_DIR]);

            /* Create the directory for each of the types of content */
            foreach (var dir in dirs)
            {
                var tmp = FileIO._searchpaths[(int) Searchpath.SP_AUTODOWNLOAD_DIR] + FileIO._subdirs[(int) dir];
                Directory.CreateDirectory(tmp);
            }

            Dedicated._log_file = FileIO._personal_dir + "openttd.log";
        }

/**
 * Scan the given tar and add graphics sets when it finds one.
 * @param fs        the file scanner to scan for
 * @param extension the extension of files to search for.
 * @param tar       the tar to search in.
 */

        public static uint ScanTar(FileScanner fs, string extension, KeyValuePair<string, TarFileListEntry> tar)
            //Dictionary<string, TarFileListEntry> 
        {

            uint num = 0;
            var filename = tar.Key;

            if (FileIO.MatchesExtension(extension, filename) && fs.AddFile(filename, tar.Value.tar_filename))
            {
                num++;
            }

            return num;
        }
    }
}
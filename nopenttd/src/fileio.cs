/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file fileio.cpp Standard In/Out file operations */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using NLog;
using Nopenttd;

namespace Nopenttd
{
    enum FileSlots
    {
        /**
         * Slot used for the GRF scanning and such.
         * This slot is used for all temporary accesses to files when scanning/testing files,
         * and thus cannot be used for files, which are continuously accessed during a game.
         */
        CONFIG_SLOT = 0,
        /** Slot for the sound. */
        SOUND_SLOT = 1,
        /** First slot usable for (New)GRFs used during the game. */
        FIRST_GRF_SLOT = 2,
        /** Maximum number of slots. */
        MAX_FILE_SLOTS = 128,
    };

    /** Structure for keeping several open files with just one data buffer. */

    public class Fio
    {
        /// position pointer in local buffer and last valid byte of buffer
        public byte[] buffer;

        public int bufferPos;
        public int bufferContentSize;

        /// current (system) position in file
        public int pos;

        /// current file handle
        public FileStream cur_fh;

        /// current filename
        public string filename;

        /// array of file handles we can have open
        public FileStream[] handles = new FileStream[(int)FileSlots.MAX_FILE_SLOTS];

        /// array of filenames we (should) have open
        public string[] filenames = new string[(int)FileSlots.MAX_FILE_SLOTS];

        /// array of short names for spriteloader's use
        public string[] shortnames = new string[(int)FileSlots.MAX_FILE_SLOTS];
    };

    public static class FileIO
    {

        public static string _personal_dir;

        private static readonly ILogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);

        /** Size of the #Fio data buffer. */
        public const int FIO_BUFFER_SIZE = 512;

        /// #Fio instance.
        public static Fio _fio = new Fio();

        /** Whether the working directory should be scanned. */
        public static bool _do_scan_working_directory = true;

        public static string _config_file;
        public static string _highscore_file;

        //private
        public static readonly string[] _subdirs = new[]
        {
            "",
            "save" + Path.PathSeparator,
            "save" + Path.PathSeparator + "autosave" + Path.PathSeparator,
            "scenario" + Path.PathSeparator,
            "scenario" + Path.PathSeparator + "heightmap" + Path.PathSeparator,
            "gm" + Path.PathSeparator,
            "data" + Path.PathSeparator,
            "baseset" + Path.PathSeparator,
            "newgrf" + Path.PathSeparator,
            "lang" + Path.PathSeparator,
            "ai" + Path.PathSeparator,
            "ai" + Path.PathSeparator + "library" + Path.PathSeparator,
            "game" + Path.PathSeparator,
            "game" + Path.PathSeparator + "library" + Path.PathSeparator,
            "screenshot" + Path.PathSeparator,
        };

        //assert_compile(lengthof(_subdirs) == NUM_SUBDIRS); //TODO test in automatic test

        public static TarList[] _tar_list = new TarList[(int) Nopenttd.Subdirectory.NUM_SUBDIRS];

        public static Dictionary<string, TarFileListEntry>[] _tar_filelist =
            new Dictionary<string, TarFileListEntry>[(int) Nopenttd.Subdirectory.NUM_SUBDIRS];

        //typedef std::map<std::string, std::string> TarLinkList;
        /// List of directory links
        public static Dictionary<string, string>[] _tar_linklist =
            new Dictionary<string, string>[(int) Nopenttd.Subdirectory.NUM_SUBDIRS];


        /**
            * The search paths OpenTTD could search through.
            * At least one of the slots has to be filled with a path.
            * null paths tell that there is no such path for the
            * current operating system.
            */
        public static string[] _searchpaths = new string[(int) Nopenttd.Searchpath.NUM_SEARCHPATHS];

        /**
         * Checks whether the given search path is a valid search path
         * @param sp the search path to check
         * @return true if the search path is valid
         */
        //inline
        public static bool IsValidSearchPath(Searchpath sp)
        {
            return sp < Nopenttd.Searchpath.NUM_SEARCHPATHS && _searchpaths[(int) sp] != null;
        }

        /** Iterator for all the search paths */

        public static IEnumerable<Searchpath> FOR_ALL_SEARCHPATHS()
        {
            for (var sp = Nopenttd.Searchpath.SP_FIRST_DIR; sp < Nopenttd.Searchpath.NUM_SEARCHPATHS; sp++)
            {
                if (IsValidSearchPath(sp))
                {
                    yield return sp;
                }
            }
        }

        public static FileStream FioFOpenFileSp(string filename, FileMode mode, Searchpath sp, Subdirectory subdir,
            out long filesize)
        {
            var path = filename;

            if (subdir != Nopenttd.Subdirectory.NO_DIRECTORY)
            {

                path = $"{_searchpaths[(int) sp]}{_subdirs[(int) subdir]}{filename}";
            }
            var file = new FileInfo(path);
            filesize = 0;
            if (file.Exists == false)
            {
                return null;
            }
            var stream = file.Open(mode);
            filesize = stream.Length;

            return stream;
        }



        /**
         * Opens a OpenTTD file somewhere in a personal or global directory.
         * @param filename Name of the file to open.
         * @param subdir Subdirectory to open.
         * @param filename Name of the file to open.
         * @return File handle of the opened file, or \c NULL if the file is not available.
         */

        public static FileStream FioFOpenFile(string filename, FileMode mode, Subdirectory subdir, out long filesize)
        {
            FileStream f = null;
            filesize = 0;

            Debug.Assert(subdir >= Nopenttd.Subdirectory.NUM_SUBDIRS && subdir != Nopenttd.Subdirectory.NO_DIRECTORY);

            foreach (var sp in FOR_ALL_SEARCHPATHS())
            {
                f = FioFOpenFileSp(filename, mode, sp, subdir, out filesize);
                if (f != null || subdir == Nopenttd.Subdirectory.NO_DIRECTORY)
                {
                    break;
                }
            }

            /* We can only use .tar in case of data-dir, and read-mode */
            if (f == null && mode == FileMode.Open && subdir != Nopenttd.Subdirectory.NO_DIRECTORY)
            {
                /* Filenames in tars are always forced to be lowercase */
                string resolved_name = filename.ToLower();

                int resolved_len = resolved_name.Length;

                /* Resolve ONE directory link */
                foreach (var link in _tar_linklist[(int) subdir])
                {
                    var src = link.Key;
                    var len = src.Length;
                    if (resolved_len >= len && resolved_name[len - 1] == Path.PathSeparator &&
                        src == resolved_name.Substring(0, len))
                    {
                        /* Apply link */
                        resolved_name = link.Value + resolved_name.Substring(len);
                        break; // Only resolve one level
                    }
                }

                if (_tar_filelist[(int) subdir].TryGetValue(resolved_name, out var entry))
                {
                    f = FioFOpenFileTar(entry, out filesize);
                }
            }

            /* Sometimes a full path is given. To support
             * the 'subdirectory' must be 'removed'. */
            if (f == null && subdir != Nopenttd.Subdirectory.NO_DIRECTORY)
            {
                switch (subdir)
                {
                    case Nopenttd.Subdirectory.BASESET_DIR:
                        f = FioFOpenFile(filename, mode, Nopenttd.Subdirectory.OLD_GM_DIR, out filesize);
                        if (f == null)
                        {
                            /* originally FALL THROUGH */
                            f = FioFOpenFile(filename, mode, Nopenttd.Subdirectory.OLD_DATA_DIR, out filesize);
                        }
                        break;
                    case Nopenttd.Subdirectory.NEWGRF_DIR:
                        f = FioFOpenFile(filename, mode, Nopenttd.Subdirectory.OLD_DATA_DIR, out filesize);
                        break;

                    default:
                        f = FioFOpenFile(filename, mode, Nopenttd.Subdirectory.NO_DIRECTORY, out filesize);
                        break;
                }
            }

            return f;
        }

/**
 * Get position in the current file.
 * @return Position in the file.
 */

        public static int FioGetPos()
        {
            return _fio.pos;
        }

/**
 * Get the filename associated with a slot.
 * @param slot Index of queried file.
 * @return Name of the file.
 */

        public static string FioGetFilename(byte slot)
        {
            return _fio.shortnames[slot];
        }

/**
 * Seek in the current file.
 * @param pos New position.
 * @param mode Type of seek (\c SEEK_CUR means \a pos is relative to current position, \c SEEK_SET means \a pos is absolute).
 */

        public static void FioSeekTo(int pos, SeekOrigin mode)
        {
            if (mode == SeekOrigin.Begin)
            {
                pos += FioGetPos();
            }
            _fio.pos = pos;
            if (_fio.cur_fh.Seek(_fio.pos, SeekOrigin.Begin) < 0)
            {
                Log.Debug($"Seeking in {_fio.filename} failed");
            }
        }

        /**
         * Switch to a different file and seek to a position.
         * @param slot Slot number of the new file.
         * @param pos New absolute position in the new file.
         */

        public static void FioSeekToFile(int slot, int pos)
        {
            var f = _fio.handles[slot];
            Debug.Assert(f != null);
            _fio.cur_fh = f;
            _fio.filename = _fio.filenames[slot];
            FioSeekTo(pos, SeekOrigin.Begin);
        }

        /**
         * Read a byte from the file.
         * @return Read byte.
         */

        public static byte FioReadByte()
        {
            if (_fio.bufferPos >= _fio.bufferContentSize)
            {
                _fio.bufferPos = 0;
                _fio.bufferContentSize = _fio.cur_fh.Read(_fio.buffer, 0, FIO_BUFFER_SIZE);
                _fio.pos += _fio.bufferContentSize;

                if (_fio.bufferContentSize == 0) return 0;
            }


            var value = _fio.buffer[_fio.bufferPos++];
            _fio.pos++;
            return value;
        }

        /**
         * Skip \a n bytes ahead in the file.
         * @param n Number of bytes to skip reading.
         */

        public static void FioSkipBytes(int n)
        {
            for (;;)
            {
                var m = Math.Min(_fio.bufferContentSize - _fio.bufferPos, n);
                _fio.bufferPos += m;
                n -= m;
                if (n == 0) break;
                FioReadByte();
                n--;
            }
        }

        /**
         * Read a word (16 bits) from the file (in low endian format).
         * @return Read word.
         */

        public static ushort FioReadWord()
        {
            var lower = FioReadByte();
            return (ushort) ((FioReadByte() << 8) | lower);
        }

        /**
         * Read a double word (32 bits) from the file (in low endian format).
         * @return Read word.
         */

        public static uint FioReadDword()
        {
            var lower = FioReadWord();
            return (uint) ((FioReadWord() << 16) | lower);
        }

        /**
         * Read a block.
         * @param ptr Destination buffer.
         * @param size Number of bytes to read.
         */

        [Obsolete("Use FileStream.Read", false)]
        public static void FioReadBlock(byte[] ptr, int offset, int count)
        {
            FioSeekTo(FioGetPos(), SeekOrigin.Begin);
            _fio.pos += _fio.cur_fh.Read(ptr, offset, count);
        }

/**
 * Close the file at the given slot number.
 * @param slot File index to close.
 */
        //inline
        public static void FioCloseFile(int slot)
        {
            if (_fio.handles[slot] != null)
            {
                _fio.handles[slot].Close();
                _fio.shortnames[slot] = null;
                _fio.handles[slot] = null;
            }
        }

        /** Close all slotted open files. */

        public static void FioCloseAll()
        {
            for (var i = 0; i < _fio.handles.Length; i++)
            {
                FioCloseFile(i);
            }
        }


        /**
         * Open a slotted file.
         * @param slot Index to assign.
         * @param filename Name of the file at the disk.
         * @param subdir The sub directory to search this file in.
         */

        public static void FioOpenFile(int slot, string filename, Subdirectory subdir)
        {
            var f = FioFOpenFile(filename, FileMode.Open, subdir, out var filesize);
            if (f == null)
            {
                usererror("Cannot open file '%s'", filename);
            }
            long pos = f.Position;
            if (pos < 0)
            {
                usererror("Cannot read file '%s'", filename);
            }

            FioCloseFile(slot); // if file was opened before, close it
            _fio.handles[slot] = f;
            _fio.filenames[slot] = filename;

            /* Store the filename without path and extension */
            var t = filename.LastIndexOf(Path.PathSeparator);
            _fio.shortnames[slot] = t != -1 ? filename.Substring(t) : filename;
            var t2 = _fio.shortnames[slot].LastIndexOf('.');
            if (t2 != -1)
            {
                _fio.shortnames[slot] = _fio.shortnames[slot].Substring(0, t2);
            }
            _fio.shortnames[slot].ToLower();

            FioSeekToFile(slot, (int) pos);
        }

        /**
         * Check whether the given file exists
         * @param filename the file to try for existence.
         * @param subdir the subdirectory to look in
         * @return true if and only if the file can be opened
         */

        public static bool FioCheckFileExists(string filename, Subdirectory subdir)
        {
            var f = FioFOpenFile(filename, FileMode.Open, subdir, out var filesize);
            if (f == null) return false;

            f.Close();
            return true;
        }

/**
 * Test whether the given filename exists.
 * @param filename the file to test.
 * @return true if and only if the file exists.
 */

        [Obsolete("Use File.Exists", false)]
        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

/**
 * Close a file in a safe way.
 */

        [Obsolete("use FileStream.Close()", false)]
        public static void FioFCloseFile(FileStream f)
        {
            f.Close();
        }

        public static string FioGetFullPath(Searchpath sp, Subdirectory subdir, string filename)
        {
            Debug.Assert(subdir < Subdirectory.NUM_SUBDIRS);
            Debug.Assert(sp < Searchpath.NUM_SEARCHPATHS);

            return $"{_searchpaths[(int) sp]}{_subdirs[(int) subdir]}{filename}";
        }

        /**
         * Find a path to the filename in one of the search directories.
         * @param buf [out] Destination buffer for the path.
         * @param last End of the destination buffer.
         * @param subdir Subdirectory to try.
         * @param filename Filename to look for.
         * @return \a buf containing the path if the path was found, else \c NULL.
         */

        public static string FioFindFullPath(Subdirectory subdir, string filename)
        {
            Debug.Assert(subdir < Subdirectory.NUM_SUBDIRS);

            foreach (var sp in FOR_ALL_SEARCHPATHS())
            {
                var path = FioGetFullPath(sp, subdir, filename);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        public static string FioAppendDirectory(Searchpath sp, Subdirectory subdir)
        {
            Debug.Assert(subdir < Subdirectory.NUM_SUBDIRS);
            Debug.Assert(sp < Searchpath.NUM_SEARCHPATHS);

            return _searchpaths[(int) sp] + _subdirs[(int) subdir];
        }

        public static string FioGetDirectory(Subdirectory subdir)
        {
            /* Find and return the first valid directory */
            foreach (var sp in FOR_ALL_SEARCHPATHS())
            {
                var ret = FioAppendDirectory(sp, subdir);
                if (File.Exists(ret))
                {
                    return ret;
                }
            }

            /* Could not find the directory, fall back to a base path */
            return _personal_dir;
        }


/**
 * Opens a file from inside a tar archive.
 * @param entry The entry to open.
 * @param filesize [out] If not \c NULL, size of the opened file.
 * @return File handle of the opened file, or \c NULL if the file is not available.
 * @note The file is read from within the tar file, and may not return \c EOF after reading the whole file.
 */

        public static FileStream FioFOpenFileTar(TarFileListEntry entry, out long filesize)
        {
            filesize = 0;
            try
            {
                var f = new FileStream(entry.tar_filename, FileMode.Open);
                if (f.Seek(entry.position, SeekOrigin.Begin) < 0)
                {
                    f.Close();
                    return null;
                }

                filesize = entry.size;
                return f;
            }
            catch (IOException e)
            {
                Log.Error(e);
                return null;
            }
        }

/**
 * Create a directory with the given name
 * @param name the new name of the directory
 */

        [Obsolete("Please use Directory.CreateDirectory", false)]
        public static void FioCreateDirectory(string name)
        {
            /* Ignore directory creation errors; they'll surface later on, and most
             * of the time they are 'directory already exists' errors anyhow. */
            Directory.CreateDirectory(name);
        }

/**
 * Appends, if necessary, the path separator character to the end of the string.
 * It does not add the path separator to zero-sized strings.
 * @param buf  string to append the separator to
 * @param last the last element of \a buf.
 * @return true iff the operation succeeded
 */

        public static string AppendPathSeparator(string buf)
        {
            if (buf == null)
            {
                return null;
            }
            var s = buf.Length;

            /* Length of string + path separator + '\0' */
            if (s > 0 && buf[s - 1] != Path.PathSeparator)
            {
                return buf + Path.PathSeparator;
            }

            return buf;
        }

/**
 * Allocates and files a variable with the full path
 * based on the given directory.
 * @param dir the directory to base the path on
 * @return the malloced full path
 */

        public static string BuildWithFullPath(string dir)
        {
            string dest = dir;

            /* Check if absolute or relative path */
            var s = dest.IndexOf(Path.PathSeparator);

            /* Add absolute path */
            if (s > 0)
            {
                dest = Directory.GetCurrentDirectory();
                dest = AppendPathSeparator(dest) + dir;
            }
            dest = AppendPathSeparator(dest);

            return dest;
        }

/**
 * Find the first directory in a tar archive.
 * @param tarname the name of the tar archive to look in.
 * @param subdir  the subdirectory to look in.
 */

        public static string FioTarFirstDir(string tarname, Subdirectory subdir)
        {
            if (_tar_list[(int) subdir].TryGetValue(tarname, out var it))
            {

                return it.dirname;
            }
            return null;
        }

        public static void TarAddLink(string srcParam, string destParam, Subdirectory subdir)
        {
            /* Tar internals assume lowercase */
            string src = srcParam.ToLower();
            string dest = destParam.ToLower();


            if (_tar_filelist[(int) subdir].TryGetValue(dest, out var dest_file))
            {
                /* Link to file. Process the link like the destination file. */
                _tar_filelist[(int) subdir].Add(src, dest_file);
            }
            else
            {
                /* Destination file not found. Assume 'link to directory'
                 * Append PATHSEPCHAR to 'src' and 'dest' if needed */

                string src_path = AppendPathSeparator(src);
                string dst_path = AppendPathSeparator(dest);
                _tar_linklist[(int) subdir].Add(src_path, dst_path);
            }
        }

        public static void FioTarAddLink(string src, string dest, Subdirectory subdir)
        {
            TarAddLink(src, dest, subdir);
        }

/**
 * Simplify filenames from tars.
 * Replace '/' by #PATHSEPCHAR, and force 'name' to lowercase.
 * @param name Filename to process.
 */

        public static string SimplifyFileName(string name)
        {
            /* Force lowercase */
            name = name.ToLower();

            /* Tar-files always have '/' path-separator, but we want our PATHSEPCHAR */
            if (Path.PathSeparator != '/')
            {
                name = name.Replace('/', Path.PathSeparator);
            }
            return name;
        }


        /**
         * Sanitizes a filename, i.e. removes all illegal characters from it.
         * @param filename the "\0" terminated filename
         */
        public static string SanitizeFilename(string filename)
        {
            var builder = new StringBuilder();
            foreach (var c in filename)
            {
                switch (c)
                {
                    /* The following characters are not allowed in filenames
			 * on at least one of the supported operating systems: */
                    case ':':
                    case '\\':
                    case '*':
                    case '?':
                    case '/':
                    case '<':
                    case '>':
                    case '|':
                    case '"':
                        builder.Append('_');
                        break;
                    default:
                        builder.Append(c);
                        break;
                        ;
                }
            }
            return builder.ToString();
        }

        /**
         * Load a file into memory.
         * @param filename Name of the file to load.
         * @param lenp [out] Length of loaded data.
         * @param maxsize Maximum size to load.
         * @return Pointer to new memory containing the loaded data, or \c null if loading failed.
         * @note If \a maxsize less than the length of the file, loading fails.
         */
        public static byte[] ReadFileToMem(string filename, int maxsize) //lenp removed
        {
            var file = new FileInfo(filename);
            if (file.Length > maxsize)
            {
                return null;
            }
            return File.ReadAllBytes(filename);
        }

        /**
         * Helper to see whether a given filename matches the extension.
         * @param extension The extension to look for.
         * @param filename  The filename to look in for the extension.
         * @return True iff the extension is null, or the filename ends with it.
         */
        public static bool MatchesExtension(string extension, string filename)
        {
            if (extension == null)
            {
                return true;
            }

            return filename?.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase) ?? false;
        }

        /**
         * Scan a single directory (and recursively its children) and add
         * any graphics sets that are found.
         * @param fs              the file scanner to add the files to
         * @param extension       the extension of files to search for.
         * @param path            full path we're currently at
         * @param basepath_length from where in the path are we 'based' on the search path
         * @param recursive       whether to recursively search the sub directories
         */
        public static uint ScanPath(FileScanner fs, string extension, string path, bool recursive = true)
        {
            uint num = 0;

            if (path == null || Directory.Exists(path) == false)
            {
                return 0;
            }
            var dir = new DirectoryInfo(path);
            foreach (var file in dir.GetFiles())
            {
                if (file.Exists == false)
                {
                    continue;
                }

                var filename = path + file.Name;

                if (MatchesExtension(extension, filename) && fs.AddFile(filename, null)) { num++; }
            }
            if (recursive)
            {
                foreach (var subDir in dir.GetDirectories())
                {
                    var filename = FileIO.AppendPathSeparator(path + subDir.Name);

                    //if (!FileIO.AppendPathSeparator(filename)) continue;
                    num += ScanPath(fs, extension, filename, recursive);
                }
            }

            return num;
        }

    }
}
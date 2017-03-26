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
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using Nopenttd;
using Nopenttd.src;

namespace Nopenttd
{ 

//#include "stdafx.h"
//#include "fileio_func.h"
//#include "debug.h"
//#include "fios.h"
//#include "string_func.h"
//#include "tar_type.h"
//#include <windows.h>
//# define access _taccess
//#include <sys/stat.h>
//#include <algorithm>
//#include "safeguards.h"

/** Structure for keeping several open files with just one data buffer. */
public class Fio {                                 /// position pointer in local buffer and last valid byte of buffer
	public byte[] buffer;                           /// current (system) position in file
	public int pos;                                 /// current file handle
	public FileStream cur_fh;                       /// current filename
	public string filename;                         /// array of file handles we can have open
	public FileStream[] handles = new FileStream[MAX_FILE_SLOTS];      /// local buffer when read from file
	public byte[] buffer_start = new byte[FIO_BUFFER_SIZE];      /// array of filenames we (should) have open
	public string[] filenames = new string[MAX_FILE_SLOTS];        /// array of short names for spriteloader's use
    public string[] shortnames = new string[MAX_FILE_SLOTS];   
};

    public static class FileIO
    {
        private static readonly ILogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);

        /** Size of the #Fio data buffer. */
        public const int FIO_BUFFER_SIZE = 512;

        /// #Fio instance.
        public static Fio _fio = new Fio();

        /** Whether the working directory should be scanned. */
        public static bool _do_scan_working_directory = true;

        static string _config_file;
        static string _highscore_file;

        private static readonly string[] _subdirs = new[]
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
                    f = FioFOpenFileTar(entry, filesize);
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
                        if (f != null) break;
                    /* FALL THROUGH */
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
            if (mode == SeekOrigin.Begin) pos += FioGetPos();
            _fio.buffer = _fio.buffer_end = _fio.buffer_start + FIO_BUFFER_SIZE;
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

        public static void FioSeekToFile(uint8 slot, size_t pos)
        {
            FILE* f;
            f = _fio.handles[slot];
            assert(f != NULL);
            _fio.cur_fh = f;
            _fio.filename = _fio.filenames[slot];
            FioSeekTo(pos, SEEK_SET);
        }

        /**
         * Read a byte from the file.
         * @return Read byte.
         */

        public static byte FioReadByte()
        {
            if (_fio.buffer == _fio.buffer_end)
            {
                _fio.buffer = _fio.buffer_start;
                size_t size = fread(_fio.buffer, 1, FIO_BUFFER_SIZE, _fio.cur_fh);
                _fio.pos += size;
                _fio.buffer_end = _fio.buffer_start + size;

                if (size == 0) return 0;
            }
            return *_fio.buffer++;
        }

        /**
         * Skip \a n bytes ahead in the file.
         * @param n Number of bytes to skip reading.
         */

        public static void FioSkipBytes(int n)
        {
            for (;;)
            {
                int m = min(_fio.buffer_end - _fio.buffer, n);
                _fio.buffer += m;
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
            byte b = FioReadByte();
            return (FioReadByte() << 8) | b;
        }

        /**
         * Read a double word (32 bits) from the file (in low endian format).
         * @return Read word.
         */

        public static uint FioReadDword()
        {
            uint b = FioReadWord();
            return (FioReadWord() << 16) | b;
        }

        /**
         * Read a block.
         * @param ptr Destination buffer.
         * @param size Number of bytes to read.
         */
         [Obsolete("Use FileStream.Read",false)]
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
            FileStream f;

            f = FioFOpenFile(filename, FileMode.Open, subdir);
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
            
            FioSeekToFile(slot, (uint) pos);
        }

        /**
         * Check whether the given file exists
         * @param filename the file to try for existence.
         * @param subdir the subdirectory to look in
         * @return true if and only if the file can be opened
         */
        public static bool FioCheckFileExists(string filename, Subdirectory subdir)
        {
            var f = FioFOpenFile(filename, "rb", subdir);
            if (f == null) return false;

            FioFCloseFile(f);
            return true;
        }

/**
 * Test whether the given filename exists.
 * @param filename the file to test.
 * @return true if and only if the file exists.
 */
        [Obsolete("Use File.Exists", false)]
        public static FileExists(string filename)
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

        public static string FioGetFullPath(string buf, Searchpath sp, Subdirectory subdir, string filename)
        {
            Debug.Assert(subdir < Subdirectory.NUM_SUBDIRS);
            Debug.Assert(sp < Searchpath.NUM_SEARCHPATHS);

            return $"{_searchpaths[(int)sp]}{_subdirs[(int)subdir]}{filename}";
        }

        /**
         * Find a path to the filename in one of the search directories.
         * @param buf [out] Destination buffer for the path.
         * @param last End of the destination buffer.
         * @param subdir Subdirectory to try.
         * @param filename Filename to look for.
         * @return \a buf containing the path if the path was found, else \c NULL.
         */
        public static string FioFindFullPath(string buf, Subdirectory subdir, string filename)
        {
            Searchpath sp;
            Debug.Assert(subdir < Subdirectory.NUM_SUBDIRS);

            foreach (var sp in FOR_ALL_SEARCHPATHS())
            {
                buf = FioGetFullPath(buf, sp, subdir, filename);
                if (File.Exists(buf))
                {
                    return buf;
                }
            }

            return null;
        }

        public static string FioAppendDirectory(Searchpath sp,Subdirectory subdir)
        {
            Debug.Assert(subdir < Subdirectory.NUM_SUBDIRS);
            Debug.Assert(sp < Searchpath.NUM_SEARCHPATHS);

            return _searchpaths[(int)sp] + _subdirs[(int)subdir];
        }

        public static string FioGetDirectory(string buf, Subdirectory subdir)
        {
            Searchpath sp;

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

        public string BuildWithFullPath(string dir)
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

        public string FioTarFirstDir(string tarname, Subdirectory subdir)
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

        void FioTarAddLink(string src, string dest, Subdirectory subdir)
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
    }

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
	FileIO._tar_filelist[(int)sd].Clear();
	FileIO._tar_list[(int)sd].Clear();
	uint num = this.Scan(".tar", sd, false);
    if (sd == Nopenttd.Subdirectory.BASESET_DIR || sd == Nopenttd.Subdirectory.NEWGRF_DIR)
    {
        num += this.Scan(".tar", Nopenttd.Subdirectory.OLD_DATA_DIR, false);
    }
	return num;
}

        /* static */
        public static uint DoScan(TarScannerMode mode)
{
            Log.Debug("Scanning for tars");
	var fs = new TarScanner();
	uint num = 0;
	if (mode.HasFlag(TarScannerMode.BASESET)) {
		num += fs.DoScan(Subdirectory.BASESET_DIR);
	}
	if (mode.HasFlag(TarScannerMode.NEWGRF)) {
		num += fs.DoScan(Subdirectory.NEWGRF_DIR);
	}
	if (mode.HasFlag(TarScannerMode.AI)) {
		num += fs.DoScan(Subdirectory.AI_DIR);
		num += fs.DoScan(Subdirectory.AI_LIBRARY_DIR);
	}
	if (mode.HasFlag(TarScannerMode.GAME)) {
		num += fs.DoScan(Subdirectory.GAME_DIR);
		num += fs.DoScan(Subdirectory.GAME_LIBRARY_DIR);
	}
	if (mode.HasFlag(TarScannerMode.SCENARIO)) {
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
	return this.AddFile(filename, 0);
}

        public class TarHeader
        {
            public string name;//new [100];      ///Name of the file
            public string mode;//[8];
            public string uid;//[8];
            public string gid;//[8];

            /// Size of the file, in ASCII
            public int size; //[12];       
            public string mtime; //[12];
            public string chksum; //[8];
            public char typeflag;
            public string linkname; //[100];
            public string magic; //[6];
            public string version;//[2];
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
                if (buffer.Length != HeaderSize) throw new ArgumentOutOfRangeException(nameof(buffer), buffer, $"TarHeader must be {HeaderSize} bytes long");
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
        public bool AddFile(string filename, int basepath_length, string tar_filename)
{
	/* No tar within tar. */
	Debug.Assert(tar_filename == null);

	/* The TAR-header, repeated for every file */

            
	/* Check if we already seen this file */
    if (FileIO._tar_list[(int)this.subdir].TryGetValue(filename, out var it) == false)
    {
        return false;
    }
    
    try
    {
        using (var f = new FileInfo(filename).OpenRead())
        {
            
    
	FileIO._tar_list[(int)this.subdir][filename].filename = filename;
        FileIO._tar_list[(int)this.subdir][filename].dirname = null;
    var dupped_filename = filename;
            /// Temporary list to collect links
    var links = new Dictionary<string, string>(); 
            
    string name = null; //char[sizeof(th.prefix) + 1 + sizeof(th.name) + 1];
    string link = null; //char link[sizeof(th.linkname) + 1];
    string dest = null; //char dest[sizeof(th.prefix) + 1 + sizeof(th.name) + 1 + 1 + sizeof(th.linkname) + 1];
    var num = 0;
    var pos = 0;


                    var buffer = new byte[TarHeader.HeaderSize];
                    for (;;) { // Note: feof() always returns 'false' after 'fseek()'. Cool, isn't it?
		var num_bytes_read = f.Read(buffer, 1, TarHeader.HeaderSize);
		if (num_bytes_read != TarHeader.HeaderSize) break;
		pos += num_bytes_read;

        var th = new TarHeader(buffer);

		/* Check if we have the new tar-format (ustar) or the old one (a lot of zeros after 'link' field) */
		if (th.magic != "ustar" && th.magic != "") {
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
		
		switch (th.typeflag) {
			case '\0':
			case '0': { // regular file
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
		        if (FileIO._tar_filelist[(int)this.subdir].ContainsKey(name) == false)
		        {
		            FileIO._tar_filelist[(int) this.subdir].Add(name, entry);
                    num++;
		        }

				break;
			}

			case '1': // hard links
			case '2': { // symbolic links
				/* Copy the destination of the link in a safe way at the end of 'linkname' */
				link = th.linkname;

				if (name.Length == 0 || link.Length == 0) break;

				/* Convert to lowercase and our PATHSEPCHAR */
				name = FileIO.SimplifyFileName(name);
				link = FileIO.SimplifyFileName(link);

				/* Only allow relative links */
				if (link[0] == Path.PathSeparator) {
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
					if (linkPart == ".") {
						/* Skip '.' (current dir) */
					} else if (linkPart == "..") {
						/* level up */
						if (dest == "") {
							Log.Debug($"Ignoring link pointing outside of data directory: {name} . {link}");
							break;
						}

						/* Truncate 'dest' after last PATHSEPCHAR.
						 * This assumes that the truncated part is a real directory and not a link. */
					    destpos = linkParts.Last();
                                    break;
					} else {
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
		            FileIO._tar_list[(int)this.subdir][filename].dirname = name;
		        }
				break;

			default:
				/* Ignore other types */
				break;
		}

		/* Skip to the next block.. */
		//var skip = Align(th.size, 512);
		if (f.Seek(th.size, SeekOrigin.Current) < 0) {
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
        var src = link.first;
        var dest = link.second;
        TarAddLink(src, dest, this.subdir);
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
    if (FileIO._tar_list[(int)subdir].TryGetValue(tar_filename, out var it) == false)
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

	foreach (var it2 in FileIO._tar_filelist[(int)subdir]) {
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
                        read = @in.Read(buffer, 0, Math.Min((int)to_copy, buffer.Length));
                        if (read <= 0)
                        {
                            break;
                        }
                        @out.Write(buffer, 0, (int)read);

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

        if (to_copy != 0) {
			Log.Debug("Extracting {filename} failed; still {to_copy} bytes to copy");
			return false;
		}
	}

	Log.Debug("  extraction successful");
	return true;
}

/**
 * Determine the base (personal dir and game data dir) paths
 * @param exe the path from the current path to the executable
 * @note defined in the OS related files (os2.cpp, win32.cpp, unix.cpp etc)
 */
extern void DetermineBasePaths(string exe);

/**
 * Changes the working directory to the path of the give executable.
 * For OSX application bundles '.app' is the required extension of the bundle,
 * so when we crop the path to there, when can remove the name of the bundle
 * in the same way we remove the name from the executable name.
 * @param exe the path to the executable
 */
public static bool ChangeWorkingDirectoryToExecutable(string exe)
{
	var success = false;
	var s = const_cast<char *>(strrchr(exe, PATHSEPCHAR));
	if (s != null) {
		*s = '\0';
//#if defined(__DJGPP__)
//		/* If we want to go to the root, we can't use cd C:, but we must use '/' */
//		if (s[-1] == ':') chdir("/");
//#endif
		if (Directory.Exists(exe) == false) {
			Log.Debug($"Directory '{exe}' with the binary does not exist?");
		} else {
			success = true;
		}
		*s = PATHSEPCHAR;
	}
	return success;
}

/**
 * Whether we should scan the working directory.
 * It should not be scanned if it's the root or
 * the home directory as in both cases a big data
 * directory can cause huge amounts of unrelated
 * files scanned. Furthermore there are nearly no
 * use cases for the home/root directory to have
 * OpenTTD directories.
 * @return true if it should be scanned.
 */
public bool DoScanWorkingDirectory()
{
	/* No working directory, so nothing to do. */
	if (FileIO._searchpaths[(int)Searchpath.SP_WORKING_DIR] == null) return false;

	/* Working directory is root, so do nothing. */
	if (FileIO._searchpaths[(int)Searchpath.SP_WORKING_DIR] == Path.PathSeparator.ToString()) return false;

	/* No personal/home directory, so the working directory won't be that. */
	if (FileIO._searchpaths[(int)Searchpath.SP_PERSONAL_DIR] == null) return true;

    var tmp = FileIO._searchpaths[(int)Searchpath.SP_WORKING_DIR] +  PERSONAL_DIR;
	tmp = FileIO.AppendPathSeparator(tmp);
	return tmp != FileIO._searchpaths[(int)Searchpath.SP_PERSONAL_DIR];
}

/**
 * Determine the base (personal dir and game data dir) paths
 * @param exe the path to the executable
 */
void DetermineBasePaths(const char *exe)
{
	char tmp[MAX_PATH];
	/* getenv is highly unsafe; duplicate it as soon as possible,
	 * or at least before something else touches the environment
	 * variables in any way. It can also contain all kinds of
	 * unvalidated data we rather not want internally. */
	string homedir = Environment.GetEnvironmentVariable("HOME");
	
	if (homedir == null) {
		const struct passwd *pw = getpwuid(getuid());
		homedir = (pw == null) ? null : stredup(pw.pw_dir);
	}

	if (homedir != null) {
		ValidateString(homedir);
		seprintf(tmp, lastof(tmp), "%s" PATHSEP "%s", homedir, PERSONAL_DIR);
		AppendPathSeparator(tmp, lastof(tmp));

		_searchpaths[SP_PERSONAL_DIR] = stredup(tmp);
		free(homedir);
	} else {
		_searchpaths[SP_PERSONAL_DIR] = null;
	}

#if defined(WITH_SHARED_DIR)
	seprintf(tmp, lastof(tmp), "%s", SHARED_DIR);
	AppendPathSeparator(tmp, lastof(tmp));
	_searchpaths[SP_SHARED_DIR] = stredup(tmp);
#else
	_searchpaths[SP_SHARED_DIR] = null;
#endif

	if (getcwd(tmp, MAX_PATH) == null) *tmp = '\0';
	AppendPathSeparator(tmp, lastof(tmp));
	_searchpaths[SP_WORKING_DIR] = stredup(tmp);

	_do_scan_working_directory = DoScanWorkingDirectory();

	/* Change the working directory to that one of the executable */
	if (ChangeWorkingDirectoryToExecutable(exe)) {
		if (getcwd(tmp, MAX_PATH) == null) *tmp = '\0';
		AppendPathSeparator(tmp, lastof(tmp));
		_searchpaths[SP_BINARY_DIR] = stredup(tmp);
	} else {
		_searchpaths[SP_BINARY_DIR] = null;
	}

	if (_searchpaths[SP_WORKING_DIR] != null) {
		/* Go back to the current working directory. */
		if (chdir(_searchpaths[SP_WORKING_DIR]) != 0) {
			Log.Debug(misc, 0, "Failed to return to working directory!");
		}
	}

#if defined(__MORPHOS__) || defined(__AMIGA__) || defined(DOS) || defined(OS2)
	_searchpaths[SP_INSTALLATION_DIR] = null;
#else
	seprintf(tmp, lastof(tmp), "%s", GLOBAL_DATA_DIR);
	AppendPathSeparator(tmp, lastof(tmp));
	_searchpaths[SP_INSTALLATION_DIR] = stredup(tmp);
#endif
#ifdef WITH_COCOA
extern void cocoaSetApplicationBundleDir();
	cocoaSetApplicationBundleDir();
#else
	_searchpaths[SP_APPLICATION_BUNDLE_DIR] = null;
#endif
}
#endif /* defined(WIN32) || defined(WINCE) */

const char *_personal_dir;

/**
 * Acquire the base paths (personal dir and game data dir),
 * fill all other paths (save dir, autosave dir etc) and
 * make the save and scenario directories.
 * @param exe the path from the current path to the executable
 */
void DeterminePaths(const char *exe)
{
	DetermineBasePaths(exe);

#if defined(WITH_XDG_BASEDIR) && defined(WITH_PERSONAL_DIR)
	char config_home[MAX_PATH];

	const char *xdg_config_home = xdgConfigHome(null);
	seprintf(config_home, lastof(config_home), "%s" PATHSEP "%s", xdg_config_home,
			PERSONAL_DIR[0] == '.' ? &PERSONAL_DIR[1] : PERSONAL_DIR);
	free(xdg_config_home);

	AppendPathSeparator(config_home, lastof(config_home));
#endif

	Searchpath sp;
	FOR_ALL_SEARCHPATHS(sp) {
		if (sp == SP_WORKING_DIR && !_do_scan_working_directory) continue;
		Log.Debug(misc, 4, "%s added as search path", _searchpaths[sp]);
	}

	char *config_dir;
	if (_config_file != null) {
		config_dir = stredup(_config_file);
		char *end = strrchr(config_dir, PATHSEPCHAR);
		if (end == null) {
			config_dir[0] = '\0';
		} else {
			end[1] = '\0';
		}
	} else {
		char personal_dir[MAX_PATH];
		if (FioFindFullPath(personal_dir, lastof(personal_dir), BASE_DIR, "openttd.cfg") != null) {
			char *end = strrchr(personal_dir, PATHSEPCHAR);
			if (end != null) end[1] = '\0';
			config_dir = stredup(personal_dir);
			_config_file = str_fmt("%sopenttd.cfg", config_dir);
		} else {
#if defined(WITH_XDG_BASEDIR) && defined(WITH_PERSONAL_DIR)
			/* No previous configuration file found. Use the configuration folder from XDG. */
			config_dir = config_home;
#else
			static const Searchpath new_openttd_cfg_order[] = {
					SP_PERSONAL_DIR, SP_BINARY_DIR, SP_WORKING_DIR, SP_SHARED_DIR, SP_INSTALLATION_DIR
				};

			config_dir = null;
			for (uint i = 0; i < lengthof(new_openttd_cfg_order); i++) {
				if (IsValidSearchPath(new_openttd_cfg_order[i])) {
					config_dir = stredup(_searchpaths[new_openttd_cfg_order[i]]);
					break;
				}
			}
			assert(config_dir != null);
#endif
			_config_file = str_fmt("%sopenttd.cfg", config_dir);
		}
	}

	Log.Debug(misc, 3, "%s found as config directory", config_dir);

	_highscore_file = str_fmt("%shs.dat", config_dir);
	extern char *_hotkeys_file;
	_hotkeys_file = str_fmt("%shotkeys.cfg", config_dir);
	extern char *_windows_file;
	_windows_file = str_fmt("%swindows.cfg", config_dir);

#if defined(WITH_XDG_BASEDIR) && defined(WITH_PERSONAL_DIR)
	if (config_dir == config_home) {
		/* We are using the XDG configuration home for the config file,
		 * then store the rest in the XDG data home folder. */
		_personal_dir = _searchpaths[SP_PERSONAL_DIR_XDG];
		FioCreateDirectory(_personal_dir);
	} else
#endif
	{
		_personal_dir = config_dir;
	}

	/* Make the necessary folders */
#if !defined(__MORPHOS__) && !defined(__AMIGA__) && defined(WITH_PERSONAL_DIR)
	FioCreateDirectory(config_dir);
	if (config_dir != _personal_dir) FioCreateDirectory(_personal_dir);
#endif

	Log.Debug(misc, 3, "%s found as personal directory", _personal_dir);

	static const Subdirectory default_subdirs[] = {
		SAVE_DIR, AUTOSAVE_DIR, SCENARIO_DIR, HEIGHTMAP_DIR, BASESET_DIR, NEWGRF_DIR, AI_DIR, AI_LIBRARY_DIR, GAME_DIR, GAME_LIBRARY_DIR, SCREENSHOT_DIR
	};

	for (uint i = 0; i < lengthof(default_subdirs); i++) {
		char *dir = str_fmt("%s%s", _personal_dir, _subdirs[default_subdirs[i]]);
		FioCreateDirectory(dir);
		free(dir);
	}

	/* If we have network we make a directory for the autodownloading of content */
	_searchpaths[SP_AUTODOWNLOAD_DIR] = str_fmt("%s%s", _personal_dir, "content_download" PATHSEP);
#ifdef ENABLE_NETWORK
	FioCreateDirectory(_searchpaths[SP_AUTODOWNLOAD_DIR]);

	/* Create the directory for each of the types of content */
	const Subdirectory dirs[] = { SCENARIO_DIR, HEIGHTMAP_DIR, BASESET_DIR, NEWGRF_DIR, AI_DIR, AI_LIBRARY_DIR, GAME_DIR, GAME_LIBRARY_DIR };
	for (uint i = 0; i < lengthof(dirs); i++) {
		char *tmp = str_fmt("%s%s", _searchpaths[SP_AUTODOWNLOAD_DIR], _subdirs[dirs[i]]);
		FioCreateDirectory(tmp);
		free(tmp);
	}

	extern char *_log_file;
	_log_file = str_fmt("%sopenttd.log",  _personal_dir);
#else /* ENABLE_NETWORK */
	/* If we don't have networking, we don't need to make the directory. But
	 * if it exists we keep it, otherwise remove it from the search paths. */
	if (!FileExists(_searchpaths[SP_AUTODOWNLOAD_DIR]))  {
		free(_searchpaths[SP_AUTODOWNLOAD_DIR]);
		_searchpaths[SP_AUTODOWNLOAD_DIR] = null;
	}
#endif /* ENABLE_NETWORK */
}

/**
 * Sanitizes a filename, i.e. removes all illegal characters from it.
 * @param filename the "\0" terminated filename
 */
void SanitizeFilename(char *filename)
{
	for (; *filename != '\0'; filename++) {
		switch (*filename) {
			/* The following characters are not allowed in filenames
			 * on at least one of the supported operating systems: */
			case ':': case '\\': case '*': case '?': case '/':
			case '<': case '>': case '|': case '"':
				*filename = '_';
				break;
		}
	}
}

/**
 * Load a file into memory.
 * @param filename Name of the file to load.
 * @param lenp [out] Length of loaded data.
 * @param maxsize Maximum size to load.
 * @return Pointer to new memory containing the loaded data, or \c null if loading failed.
 * @note If \a maxsize less than the length of the file, loading fails.
 */
void *ReadFileToMem(const char *filename, size_t *lenp, size_t maxsize)
{
	FILE *in = fopen(filename, "rb");
	if (in == null) return null;

	fseek(in, 0, SEEK_END);
	size_t len = ftell(in);
	fseek(in, 0, SEEK_SET);
	if (len > maxsize) {
		fclose(in);
		return null;
	}
	byte *mem = MallocT<byte>(len + 1);
	mem[len] = 0;
	if (fread(mem, len, 1, in) != 1) {
		fclose(in);
		free(mem);
		return null;
	}
	fclose(in);

	*lenp = len;
	return mem;
}

/**
 * Helper to see whether a given filename matches the extension.
 * @param extension The extension to look for.
 * @param filename  The filename to look in for the extension.
 * @return True iff the extension is null, or the filename ends with it.
 */
static bool MatchesExtension(const char *extension, const char *filename)
{
	if (extension == null) return true;

	const char *ext = strrchr(filename, extension[0]);
	return ext != null && strcasecmp(ext, extension) == 0;
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
static uint ScanPath(FileScanner *fs, const char *extension, const char *path, size_t basepath_length, bool recursive)
{
	extern bool FiosIsValidFile(const char *path, const struct dirent *ent, struct stat *sb);

	uint num = 0;
	struct stat sb;
	struct dirent *dirent;
	DIR *dir;

	if (path == null || (dir = ttd_opendir(path)) == null) return 0;

	while ((dirent = readdir(dir)) != null) {
		const char *d_name = FS2OTTD(dirent.d_name);
		char filename[MAX_PATH];

		if (!FiosIsValidFile(path, dirent, &sb)) continue;

		seprintf(filename, lastof(filename), "%s%s", path, d_name);

		if (S_ISDIR(sb.st_mode)) {
			/* Directory */
			if (!recursive) continue;
			if (strcmp(d_name, ".") == 0 || strcmp(d_name, "..") == 0) continue;
			if (!AppendPathSeparator(filename, lastof(filename))) continue;
			num += ScanPath(fs, extension, filename, basepath_length, recursive);
		} else if (S_ISREG(sb.st_mode)) {
			/* File */
			if (MatchesExtension(extension, filename) && fs.AddFile(filename, basepath_length, null)) num++;
		}
	}

	closedir(dir);

	return num;
}

/**
 * Scan the given tar and add graphics sets when it finds one.
 * @param fs        the file scanner to scan for
 * @param extension the extension of files to search for.
 * @param tar       the tar to search in.
 */
static uint ScanTar(FileScanner *fs, const char *extension, TarFileList::iterator tar)
{
	uint num = 0;
	const char *filename = (*tar).first.c_str();

	if (MatchesExtension(extension, filename) && fs.AddFile(filename, 0, (*tar).second.tar_filename)) num++;

	return num;
}

/**
 * Scan for files with the given extension in the given search path.
 * @param extension the extension of files to search for.
 * @param sd        the sub directory to search in.
 * @param tars      whether to search in the tars too.
 * @param recursive whether to search recursively
 * @return the number of found files, i.e. the number of times that
 *         AddFile returned true.
 */
uint FileScanner::Scan(const char *extension, Subdirectory sd, bool tars, bool recursive)
{
	this.subdir = sd;

	Searchpath sp;
	char path[MAX_PATH];
	TarFileList::iterator tar;
	uint num = 0;

	FOR_ALL_SEARCHPATHS(sp) {
		/* Don't search in the working directory */
		if (sp == SP_WORKING_DIR && !_do_scan_working_directory) continue;

		FioAppendDirectory(path, lastof(path), sp, sd);
		num += ScanPath(this, extension, path, strlen(path), recursive);
	}

	if (tars && sd != NO_DIRECTORY) {
		FOR_ALL_TARS(tar, sd) {
			num += ScanTar(this, extension, tar);
		}
	}

	switch (sd) {
		case BASESET_DIR:
			num += this.Scan(extension, OLD_GM_DIR, tars, recursive);
			/* FALL THROUGH */
		case NEWGRF_DIR:
			num += this.Scan(extension, OLD_DATA_DIR, tars, recursive);
			break;

		default: break;
	}

	return num;
}

/**
 * Scan for files with the given extension in the given search path.
 * @param extension the extension of files to search for.
 * @param directory the sub directory to search in.
 * @param recursive whether to search recursively
 * @return the number of found files, i.e. the number of times that
 *         AddFile returned true.
 */
uint FileScanner::Scan(const char *extension, const char *directory, bool recursive)
{
	char path[MAX_PATH];
	strecpy(path, directory, lastof(path));
	if (!AppendPathSeparator(path, lastof(path))) return 0;
	return ScanPath(this, extension, path, strlen(path), recursive);
}

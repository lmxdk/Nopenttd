/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file fileio_func.h Functions for Standard In/Out file operations */

using System;

namespace Nopenttd
{
    
//void FioSeekTo(size_t pos, int mode);
//void FioSeekToFile(uint8 slot, size_t pos);
//size_t FioGetPos();
//const char *FioGetFilename(uint8 slot);
//byte FioReadByte();
//uint16 FioReadWord();
//uint32 FioReadDword();
//void FioCloseAll();
//void FioOpenFile(int slot, const char *filename, Subdirectory subdir);
//void FioReadBlock(void *ptr, size_t size);
//void FioSkipBytes(int n);


//void FioFCloseFile(FILE *f);
//FILE *FioFOpenFile(const char *filename, const char *mode, Subdirectory subdir, size_t *filesize = NULL);
//bool FioCheckFileExists(const char *filename, Subdirectory subdir);
//char *FioGetFullPath(char *buf, const char *last, Searchpath sp, Subdirectory subdir, const char *filename);
//char *FioFindFullPath(char *buf, const char *last, Subdirectory subdir, const char *filename);
//char *FioAppendDirectory(char *buf, const char *last, Searchpath sp, Subdirectory subdir);
//char *FioGetDirectory(char *buf, const char *last, Subdirectory subdir);

//const char *FiosGetScreenshotDir();

//void SanitizeFilename(char *filename);
//bool AppendPathSeparator(char *buf, const char *last);
//void DeterminePaths(const char *exe);
//void *ReadFileToMem(const char *filename, size_t *lenp, size_t maxsize);
//bool FileExists(const char *filename);
//const char *FioTarFirstDir(const char *tarname, Subdirectory subdir);
//void FioTarAddLink(const char *src, const char *dest, Subdirectory subdir);
//bool ExtractTar(const char *tar_filename, Subdirectory subdir);

//extern const char *_personal_dir; ///< custom directory for personal settings, saves, newgrf, etc.

/** Helper for scanning for files with a given name */
public abstract class FileScanner {
	protected Subdirectory subdir; ///< The current sub directory we are searching through


        /**
 * Scan for files with the given extension in the given search path.
 * @param extension the extension of files to search for.
 * @param sd        the sub directory to search in.
 * @param tars      whether to search in the tars too.
 * @param recursive whether to search recursively
 * @return the number of found files, i.e. the number of times that
 *         AddFile returned true.
 */
        public uint Scan(string extension, Subdirectory sd, bool tars, bool recursive)
{
	this.subdir = sd;

	Searchpath sp;
        TarFileList::iterator tar;
        uint num = 0;


        foreach (var sp in FileIO.FOR_ALL_SEARCHPATHS())
        {
            /* Don't search in the working directory */
            if (sp == Searchpath.SP_WORKING_DIR && !FileIO._do_scan_working_directory) continue;

            var path = FileIO.FioAppendDirectory(sp, sd);
            num += ScanPath(this, extension, path, recursive);
        }

	if (tars && sd != Subdirectory.NO_DIRECTORY) {

        FOR_ALL_TARS(tar, sd)
        {
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
uint FileScanner::Scan(const char* extension, const char* directory, bool recursive)
{
    char path[MAX_PATH];
    strecpy(path, directory, lastof(path));
    if (!AppendPathSeparator(path, lastof(path))) return 0;
    return ScanPath(this, extension, path, strlen(path), recursive);
}


/**
 * Add a file with the given filename.
 * @param filename        the full path to the file to read
 * @param basepath_length amount of characters to chop of before to get a
 *                        filename relative to the search path.
 * @param tar_filename    the name of the tar file the file is read from.
 * @return true if the file is added.
 */
public abstract bool AddFile(string filename, int basepath_length, string tar_filename);
}

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
    public class TarScanner : FileScanner {
	uint DoScan(Subdirectory sd)


	/* virtual */ bool AddFile(const char *filename, size_t basepath_length, const char *tar_filename = NULL);

	bool AddFile(Subdirectory sd, const char *filename);

	/** Do the scan for Tars. */
	static uint DoScan(Mode mode);
};


///* Implementation of opendir/readdir/closedir for Windows */
//#if defined(WIN32)
//struct DIR;

//struct dirent { // XXX - only d_name implemented
//	TCHAR *d_name; // name of found file
//	/* little hack which will point to parent DIR struct which will
//	 * save us a call to GetFileAttributes if we want information
//	 * about the file (for example in function fio_bla) */
//	DIR *dir;
//};

//DIR *opendir(const TCHAR *path);
//struct dirent *readdir(DIR *d);
//int closedir(DIR *d);
//#else
///* Use system-supplied opendir/readdir/closedir functions */
//# include <sys/types.h>
//# include <dirent.h>
//#endif /* defined(WIN32) */

/**
 * A wrapper around opendir() which will convert the string from
 * OPENTTD encoding to that of the filesystem. For all purposes this
 * function behaves the same as the original opendir function
 * @param path string to open directory of
 * @return DIR pointer
 */
public static DIR ttd_opendir(string path)
{
	return opendir(OTTD2FS(path));
}

}
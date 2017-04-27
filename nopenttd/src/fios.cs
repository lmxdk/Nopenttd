/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file fios.h Declarations for savegames operations */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;
using NLog.LayoutRenderers;
using Nopenttd.Os.Windows;
using Nopenttd.src;

namespace Nopenttd
{



    //typedef SmallMap<uint, CompanyProperties *> CompanyPropertiesMap;

    /**
     * Container for loading in mode SL_LOAD_CHECK.
     */
    public class LoadCheckData { /// True if the savegame could be checked by SL_LOAD_CHECK. (Old savegames are not checkable.)
    public bool checkable;    /// Error message from loading. INVALID_STRING_ID if no error. 
	public StringID error;    /// Data to pass to SetDParamStr when displaying #error. 
	public string error_data;

    public uint map_size_x;
    public uint map_size_y;
	public Date current_date;

	public GameSettings settings;
        /// Company information.
        public CompanyPropertiesMap companies;
        /// NewGrf configuration from save.
        public GRFConfig* grfconfig;                       /// Summary state of NewGrfs, whether missing files or only compatible found.  
        public GRFListCompatibility grf_compatibility;       
												/// Gamelog actions
	struct LoggedAction *gamelog_action;        /// Number of gamelog actions  
	uint gamelog_actions;

    public LoadCheckData()
    {
        error_data = null;
            grfconfig = null;
			grf_compatibility = GLC_NOT_FOUND;
        gamelog_action = null;
        gamelog_actions = 0;
		Clear();
	}
		
	/**
	 * Check whether loading the game resulted in errors.
	 * @return true if errors were encountered.
	 */
	bool HasErrors()
	{
		return this.checkable && this.error != StringConstants.INVALID_STRING_ID;
	}

	/**
	 * Check whether the game uses any NewGrfs.
	 * @return true if NewGrfs are used.
	 */
	bool HasNewGrfs()
	{
		return this.checkable && this.error == StringConstants.INVALID_STRING_ID && this.grfconfig != null;
	}

	void Clear();
}


/** Deals with finding savegames */
public class FiosItem {
	public FiosType type;
	public ulong mtime;
	public string title; //64;
    public string name; //MAX_PATH;
}

		[Flags]
enum SortingBits {
	SORT_ASCENDING  = 0,
	SORT_DESCENDING = 1,
	SORT_BY_DATE    = 0,
	SORT_BY_NAME    = 2
}

    public class FiosItemComparer : IComparer<FiosItem>
    {

        /**
         * Compare two FiosItem's. Used with sort when sorting the file list.
         * @param da A pointer to the first FiosItem to compare.
         * @param db A pointer to the second FiosItem to compare.
         * @return -1, 0 or 1, depending on how the two items should be sorted.
         */
        public int Compare(FiosItem da, FiosItem db)
        {

            int r = 0;

            if (Fios._savegame_sort_order.HasFlag(SortingBits.SORT_BY_NAME) == false && da.mtime != db.mtime)
            {
                r = da.mtime < db.mtime ? -1 : 1;
            }
            else
            {
                r = string.Compare(da.title, db.title, StringComparison.CurrentCultureIgnoreCase);
            }

            if (Fios._savegame_sort_order.HasFlag(SortingBits.SORT_DESCENDING))
            {
                r = -r;
            }
            return r;
        }
    }


    public class Fios
    {
        private static readonly ILogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        public static LoadCheckData _load_check_data;

        /* Variables to display file lists */
        static string _fios_path;
        static SortingBits _savegame_sort_order = SortingBits.SORT_BY_DATE | SortingBits.SORT_DESCENDING;




        /**
         * Get descriptive texts. Returns the path and free space
         * left on the device
         * @param path string describing the path
         * @param total_free total free space in megabytes, optional (can be null)
         * @return StringID describing the path (free space or failure)
         */
        public static (StringID stringId, long totalFreeSpace) FiosGetDescText(string path)
        {
            var result = Win32.FiosGetDiskFreeSpace(path);
            return (result.found ? STR_SAVELOAD_BYTES_FREE : STR_ERROR_UNABLE_TO_READ_DRIVE, result.totalFreeSpace);
        }


        /**
 * Browse to a new path based on the passed \a item, starting at #_fios_path.
 * @param *item Item telling us what to do.
 * @return A filename w/path if we reached a file, otherwise \c null.
 */
        public string FiosBrowseTo(FiosItem item)
        {
            switch (item.type) {
                case FiosType.FIOS_TYPE_DRIVE:
                    _fios_path = $"{item.title[0]}:{Path.PathSeparator}";
                    break;
                case FiosType.FIOS_TYPE_INVALID:
                    break;

                case FiosType.FIOS_TYPE_PARENT: 
                    _fios_path = new DirectoryInfo(_fios_path).Parent.FullName;
                    break;

                case FiosType.FIOS_TYPE_DIR:
                    _fios_path = $"{_fios_path}{item.name}{Path.PathSeparator}";
                    break;

                case FiosType.FIOS_TYPE_DIRECT:
                    _fios_path = item.name;
                    break;

                case FiosType.FIOS_TYPE_FILE:
                case FiosType.FIOS_TYPE_OLDFILE:
                case FiosType.FIOS_TYPE_SCENARIO:
                case FiosType.FIOS_TYPE_OLD_SCENARIO:
                case FiosType.FIOS_TYPE_PNG:
                case FiosType.FIOS_TYPE_BMP:
                    return item.name;
            }

            return null;
        }


        /**
         * Construct a filename from its components in destination buffer \a buf.
         * @param buf Destination buffer.
         * @param path Directory path, may be \c null.
         * @param name Filename.
         * @param ext Filename extension (use \c "" for no extension).
         * @param last Last element of buffer \a buf.
         */
        static string FiosMakeFilename(string path, string name, string ext)
        {

            var extension = Path.GetExtension(name);
            /* Don't append the extension if it is already there */
            if (string.Equals(extension, ext, StringComparison.CurrentCultureIgnoreCase))
            {
                ext = "";
            }

            return $"{path}{Path.PathSeparator}{name}{ext}";
        }

        /**
         * Make a save game or scenario filename from a name.
         * @param buf Destination buffer for saving the filename.
         * @param name Name of the file.
         * @param last Last element of buffer \a buf.
         */
        static string FiosMakeSavegameName(string name)
        {
            var extension = (_game_mode == GM_EDITOR) ? ".scn" : ".sav";

            return FiosMakeFilename(_fios_path, name, extension);
        }

        /**
 * Construct a filename for a height map.
 * @param buf Destination buffer.
 * @param name Filename.
 * @param last Last element of buffer \a buf.
 */
        static string FiosMakeHeightmapName(string name)
        {
            var ext = "." + GetCurrentScreenshotExtension();
            
            return FiosMakeFilename(_fios_path, name, ext);
        }

        /**
         * Delete a file.
         * @param name Filename to delete.
         * @return Whether the file deletion was successful.
         */
        static bool FiosDelete(string name)
        {
            var filename = FiosMakeSavegameName(name);
            try
            {
                File.Delete(filename);
            } catch (Exception ex)
            {

                Log.Warn(ex);
                return false;
            }
            return true;
        }

        /**
 * Fill the list of the files in a directory, according to some arbitrary rule.
 * @param fop Purpose of collecting the list.
 * @param callback_proc The function that is called where you need to do the filtering.
 * @param subdir The directory from where to start (global) searching.
 * @param file_list Destination of the found files.
 */
        static void FiosGetFileList(SaveLoadOperation fop, fios_getlist_callback_proc callback_proc, Subdirectory subdir, FileList file_list)
        {

            int sort_start;

            file_list.Clear();

            /* A parent directory link exists if we are not in the root directory */
            if (Win32.FiosIsRoot(_fios_path) == false)
            {
                var fios = new FiosItem();
                fios.type = FiosType.FIOS_TYPE_PARENT;
                fios.mtime = 0;

                fios.name = "..";
                fios.title = ".. (Parent directory)";
            
                file_list.Add(fios);
            }

            /* Show subdirectories */
            var dir = Win32.opendir(_fios_path);
            if (dir != null) {

                /* found file must be directory, but not '.' or '..' */
                foreach (var directory in dir.GetDirectories())
                {
                    var d_name = directory.Name;
                    
                    if (Win32.FiosIsHiddenFile(directory) == false || string.Equals(d_name, PERSONAL_DIR, StringComparison.CurrentCultureIgnoreCase))
                    {
                        var fios = new FiosItem();
                        fios.type = FiosType.FIOS_TYPE_DIR;
                        fios.mtime = 0;
                        file_list.Add(fios);
                        fios.name = d_name;
                        fios.title = $"{d_name}{Path.PathSeparator} (Directory)";

                        str.str_validate(fios.title);
                    }
                }
            }

            /* Sort the subdirs always by name, ascending, remember user-sorting order */
            var CompareFiosItems = new FiosItemComparer();
            {
                SortingBits order = _savegame_sort_order;
                _savegame_sort_order = SortingBits.SORT_BY_NAME | SortingBits.SORT_ASCENDING;

                
                file_list.files.Sort(CompareFiosItems);
                _savegame_sort_order = order;
            }

            /* This is where to start sorting for the filenames */
            sort_start = file_list.Count;

            /* Show files */
            var scanner = new FiosFileScanner(fop, callback_proc, file_list);
            if (subdir == Subdirectory.NO_DIRECTORY) {
                scanner.Scan(null, _fios_path, false);
            } else {
                scanner.Scan(null, subdir, true, true);
            }


            file_list.files.Sort(sort_start, file_list.files.Count - sort_start, CompareFiosItems);

            /* Show drives */
            FiosGetDrives(file_list);
        }


        /**
         * Get the title of a file, which (if exists) is stored in a file named
         * the same as the data file but with '.title' added to it.
         * @param file filename to get the title for
         * @param title the title buffer to fill
         * @param last the last element in the title buffer
         * @param subdir the sub directory to search in
         */
        static string GetFileTitle(string file, Subdirectory subdir)
        {
            var buf = $"{file}.title";

            using (var f = FileIO.FioFOpenFile(buf, FileMode.Open, subdir, out var filesize))
            {
                if (f == null) return null;

                using (var reader = new StreamReader(f,Encoding.Unicode))
                {
                    var title = reader.ReadToEnd();
                    str.str_validate(title);
                    return title;
                }
            }
        }



        /**
         * Callback for FiosGetFileList. It tells if a file is a savegame or not.
         * @param fop Purpose of collecting the list.
         * @param file Name of the file to check.
         * @param ext A pointer to the extension identifier inside file
         * @param title Buffer if a callback wants to lookup the title of the file; null to skip the lookup
         * @param last Last available byte in buffer (to prevent buffer overflows); not used when title == null
         * @return a FIOS_TYPE_* type of the found file, FIOS_TYPE_INVALID if not a savegame
         * @see FiosGetFileList
         * @see FiosGetSavegameList
         */
        (FiosType type, string title) FiosGetSavegameListCallback(SaveLoadOperation fop, string file, string ext, string title)
        {
            /* Show savegame files
             * .SAV OpenTTD saved game
             * .SS1 Transport Tycoon Deluxe preset game
             * .SV1 Transport Tycoon Deluxe (Patch) saved game
             * .SV2 Transport Tycoon Deluxe (Patch) saved 2-player game */

            /* Don't crash if we supply no extension */
            if (ext == null) return (FiosType.FIOS_TYPE_INVALID, null);

            if (string.Equals(ext, ".sav", StringComparison.CurrentCultureIgnoreCase)) {

                title = GetFileTitle(file, Subdirectory.SAVE_DIR);
                return (FiosType.FIOS_TYPE_FILE, title);
            }

            if (fop == SaveLoadOperation.SLO_LOAD) {
                if (string.Equals(ext, ".ss1", StringComparison.CurrentCultureIgnoreCase) || string.Equals(ext, ".sv1", StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(ext, ".sv2", StringComparison.CurrentCultureIgnoreCase)) {
                    if (title != null)
                    {
                        title = GetOldSaveGameName(file);
                    }
                    return (FiosType.FIOS_TYPE_OLDFILE, title);
                }
            }

            return (FiosType.FIOS_TYPE_INVALID, title);
        }


        private static string fios_save_path = null;

        /**
         * Get a list of savegames.
         * @param fop Purpose of collecting the list.
         * @param file_list Destination of the found files.
         * @see FiosGetFileList
         */
        void FiosGetSavegameList(SaveLoadOperation fop, FileList file_list)
        {
        if (fios_save_path == null)
        {
            fios_save_path = FileIO.FioGetDirectory(Subdirectory.SAVE_DIR);
        }

        _fios_path = fios_save_path;

        FiosGetFileList(fop, FiosGetSavegameListCallback, Subdirectory.NO_DIRECTORY, file_list);
    }


        /**
         * Callback for FiosGetFileList. It tells if a file is a scenario or not.
         * @param fop Purpose of collecting the list.
         * @param file Name of the file to check.
         * @param ext A pointer to the extension identifier inside file
         * @param title Buffer if a callback wants to lookup the title of the file
         * @param last Last available byte in buffer (to prevent buffer overflows)
         * @return a FIOS_TYPE_* type of the found file, FIOS_TYPE_INVALID if not a scenario
         * @see FiosGetFileList
         * @see FiosGetScenarioList
         */
        static (FiosType type, string title) FiosGetScenarioListCallback(SaveLoadOperation fop, string file, string ext)
        {
            string title = null;
            /* Show scenario files
             * .SCN OpenTTD style scenario file
             * .SV0 Transport Tycoon Deluxe (Patch) scenario
             * .SS0 Transport Tycoon Deluxe preset scenario */
            if (string.Equals(ext, ".scn", StringComparison.CurrentCultureIgnoreCase)) {

                title = GetFileTitle(file, Subdirectory.SCENARIO_DIR);
                return (FiosType.FIOS_TYPE_SCENARIO, title);
            }

            if (fop == SaveLoadOperation.SLO_LOAD) {
                if (string.Equals(ext, ".sv0", StringComparison.CurrentCultureIgnoreCase) || string.Equals(ext, ".ss0", StringComparison.CurrentCultureIgnoreCase)) {

                    title = GetOldSaveGameName(file);
                    return (FiosType.FIOS_TYPE_OLD_SCENARIO, title);
                }
            }

            return (FiosType.FIOS_TYPE_INVALID, title);
        }


        private static string fios_scn_path = null;

        /**
         * Get a list of scenarios.
         * @param fop Purpose of collecting the list.
         * @param file_list Destination of the found files.
         * @see FiosGetFileList
         */
        void FiosGetScenarioList(SaveLoadOperation fop, FileList file_list)
        {
            /* Copy the default path on first run or on 'New Game' */
            if (fios_scn_path == null)
        {
            fios_scn_path = FileIO.FioGetDirectory(Subdirectory.SCENARIO_DIR);
        }

        _fios_path = fios_scn_path;

        var base_path = FileIO.FioGetDirectory(Subdirectory.SCENARIO_DIR);

        var subdir = (fop == SaveLoadOperation.SLO_LOAD && string.Equals(base_path, _fios_path, StringComparison.CurrentCultureIgnoreCase)) ? Subdirectory.SCENARIO_DIR : Subdirectory.NO_DIRECTORY;
        FiosGetFileList(fop, FiosGetScenarioListCallback, subdir, file_list);
    }

        static (FiosType type, string title) FiosGetHeightmapListCallback(SaveLoadOperation fop, string file, string ext)
        {
            /* Show heightmap files
             * .PNG PNG Based heightmap files
             * .BMP BMP Based heightmap files
             */

            var type = FiosType.FIOS_TYPE_INVALID;
            string title = null;

            if (string.Equals(ext, ".png", StringComparison.CurrentCultureIgnoreCase)) type = FiosType.FIOS_TYPE_PNG;
            if (string.Equals(ext, ".bmp", StringComparison.CurrentCultureIgnoreCase)) type = FiosType.FIOS_TYPE_BMP;

            if (type == FiosType.FIOS_TYPE_INVALID) return (FiosType.FIOS_TYPE_INVALID, title);

            if (FileIO._tar_filelist[(int)Subdirectory.SCENARIO_DIR].TryGetValue(file, out var value)) {
                /* If the file is in a tar and that tar is not in a heightmap
                 * directory we are for sure not supposed to see it.
                 * Examples of this are pngs part of documentation within
                 * collections of NewGRFs or 32 bpp graphics replacement PNGs.
                 */
                bool match = false;

                foreach (var sp in FileIO.FOR_ALL_SEARCHPATHS())
                {
                    var dir = FileIO.FioAppendDirectory(sp, Subdirectory.HEIGHTMAP_DIR);

                    if (string.Equals(dir, value.tar_filename, StringComparison.CurrentCultureIgnoreCase))
                    {
                        match = true;
                        break;
                    }
                }

                if (match == false)
                {
                    return (FiosType.FIOS_TYPE_INVALID, title);
                }
            }

            title = GetFileTitle(file, Subdirectory.HEIGHTMAP_DIR);

            return (type, title);
        }


        private static string fios_hmap_path = null;

        /**
         * Get a list of heightmaps.
         * @param fop Purpose of collecting the list.
         * @param file_list Destination of the found files.
         */
        void FiosGetHeightmapList(SaveLoadOperation fop, FileList file_list)
        {

            if (fios_hmap_path == null)
        {
            fios_hmap_path = FileIO.FioGetDirectory(Subdirectory.HEIGHTMAP_DIR);
        }

        _fios_path = fios_hmap_path;

        var base_path = FileIO.FioGetDirectory(Subdirectory.HEIGHTMAP_DIR);

        var subdir = string.Equals(base_path, _fios_path) ? Subdirectory.HEIGHTMAP_DIR : Subdirectory.NO_DIRECTORY;
        FiosGetFileList(fop, FiosGetHeightmapListCallback, subdir, file_list);
    }


        private static string fios_screenshot_path = null;
        /**
         * Get the directory for screenshots.
         * @return path to screenshots
         */
        string FiosGetScreenshotDir()
        {
            if (fios_screenshot_path == null) {

                fios_screenshot_path = FileIO.FioGetDirectory(Subdirectory.SCREENSHOT_DIR);
        }

        return fios_screenshot_path;
    }


        /** Scanner for scenarios */
        private static ScenarioScanner _scanner = new ScenarioScanner();

        /**
         * Find a given scenario based on its unique ID.
         * @param ci The content info to compare it to.
         * @param md5sum Whether to look at the md5sum or the id.
         * @return The filename of the file, else \c null.
         */
        public string FindScenario(ContentInfo ci, bool md5sum)
        {

            _scanner.Scan(false);

            foreach (var item in _scanner.items) {
                if (md5sum? (item.md5sum.SequenceEqual(ci.md5sum))
                    : (item.scenid == ci.unique_id)) {
                    return item.filename;
                }
            }

            return null;
        }

/**
 * Check whether we've got a given scenario based on its unique ID.
 * @param ci The content info to compare it to.
 * @param md5sum Whether to look at the md5sum or the id.
 * @return True iff we've got the scenario.
 */
        public bool HasScenario(ContentInfo ci, bool md5sum)
        {
            return (FindScenario(ci, md5sum) != null);
        }

/**
 * Force a (re)scan of the scenarios.
 */
        void ScanScenarios()
        {
            _scanner.Scan(true);
        }

        void ShowSaveLoadDialog(AbstractFileType abstract_filetype, SaveLoadOperation fop);


        void FiosMakeSavegameName(char*buf,  const char* name,  const char* last);
    }
}
/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file fileio_type.h Types for Standard In/Out file operations */

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Nopenttd
{

/** The different abstract types of files that the system knows about. */

    public enum AbstractFileType
    {
        /// nothing to do
        FT_NONE,

        /// old or new savegame
        FT_SAVEGAME,

        /// old or new scenario
        FT_SCENARIO,

        /// heightmap file
        FT_HEIGHTMAP,

        /// Invalid or unknown file type.
        FT_INVALID = 7,

        /// Number of bits required for storing a #AbstractFileType value.
        FT_NUMBITS = 3,

        /// Bitmask for extracting an abstract file type.
        FT_MASK = (1 << FT_NUMBITS) - 1,
    }

/** Kinds of files in each #AbstractFileType. */

    public enum DetailedFileType
    {
        /* Save game and scenario files. */

        /// Old save game or scenario file.
        DFT_OLD_GAME_FILE,

        /// Save game or scenario file.
        DFT_GAME_FILE,

        /* Heightmap files. */

        /// BMP file.
        DFT_HEIGHTMAP_BMP,

        /// PNG file.
        DFT_HEIGHTMAP_PNG,

        /* fios 'files' */
        DFT_FIOS_DRIVE,

        /// A drive (letter) entry.
        DFT_FIOS_PARENT,

        /// A parent directory entry.
        DFT_FIOS_DIR,

        /// A directory entry.
        DFT_FIOS_DIRECT,

        /// Direct filename.
        /// Unknown or invalid file.
        DFT_INVALID = 255,
    }

/** Operation performed on the file. */

    public enum SaveLoadOperation
    {
        /// Load file for checking and/or preview.
        SLO_CHECK,

        /// File is being loaded.
        SLO_LOAD,

        /// File is being saved.
        SLO_SAVE,

        /// Unknown file operation.
        SLO_INVALID,
    }

/**
 * Construct an enum value for #FiosType as a combination of an abstract and a detailed file type.
 * @param abstract Abstract file type (one of #AbstractFileType).
 * @param detailed Detailed file type (one of #DetailedFileType).
 */

    internal static class FiosTypeHelper
    {
        /**
         * Extract the abstract file type from a #FiosType.
         * @param fios_type Type to query.
         * @return The Abstract file type of the \a fios_type.
         */
        //inline 
        public static AbstractFileType GetAbstractFileType(this FiosType fios_type)
        {
            return (AbstractFileType) ((int) fios_type & (int) AbstractFileType.FT_MASK);
        }

        /**
         * Extract the detailed file type from a #FiosType.
         * @param fios_type Type to query.
         * @return The Detailed file type of the \a fios_type.
         */
        //inline
        public static DetailedFileType GetDetailedFileType(this FiosType fios_type)
        {
            return (DetailedFileType) ((int) fios_type >> (int) AbstractFileType.FT_NUMBITS);
        }
    }

/**
 * Elements of a file system that are recognized.
 * Values are a combination of #AbstractFileType and #DetailedFileType.
 * @see GetAbstractFileType GetDetailedFileType
 */

    public enum FiosType
    {
        FIOS_TYPE_DRIVE = AbstractFileType.FT_NONE | DetailedFileType.DFT_FIOS_DRIVE << (int) AbstractFileType.FT_NUMBITS,
        FIOS_TYPE_PARENT = AbstractFileType.FT_NONE | DetailedFileType.DFT_FIOS_PARENT << (int) AbstractFileType.FT_NUMBITS,
        FIOS_TYPE_DIR = AbstractFileType.FT_NONE | DetailedFileType.DFT_FIOS_DIR << (int) AbstractFileType.FT_NUMBITS,
        FIOS_TYPE_DIRECT = AbstractFileType.FT_NONE | DetailedFileType.DFT_FIOS_DIRECT << (int) AbstractFileType.FT_NUMBITS,

        FIOS_TYPE_FILE = AbstractFileType.FT_SAVEGAME | DetailedFileType.DFT_GAME_FILE << (int) AbstractFileType.FT_NUMBITS,

        FIOS_TYPE_OLDFILE = AbstractFileType.FT_SAVEGAME | DetailedFileType.DFT_OLD_GAME_FILE << (int) AbstractFileType.FT_NUMBITS,
        FIOS_TYPE_SCENARIO = AbstractFileType.FT_SCENARIO | DetailedFileType.DFT_GAME_FILE << (int) AbstractFileType.FT_NUMBITS,

        FIOS_TYPE_OLD_SCENARIO = AbstractFileType.FT_SCENARIO | DetailedFileType.DFT_OLD_GAME_FILE << (int) AbstractFileType.FT_NUMBITS,
        FIOS_TYPE_PNG = AbstractFileType.FT_HEIGHTMAP | DetailedFileType.DFT_HEIGHTMAP_PNG << (int) AbstractFileType.FT_NUMBITS,
        FIOS_TYPE_BMP = AbstractFileType.FT_HEIGHTMAP | DetailedFileType.DFT_HEIGHTMAP_BMP << (int) AbstractFileType.FT_NUMBITS,

        FIOS_TYPE_INVALID = AbstractFileType.FT_INVALID | DetailedFileType.DFT_INVALID << (int) AbstractFileType.FT_NUMBITS
    }


/**
 * The different kinds of subdirectories OpenTTD uses
 */

    public enum Subdirectory
    {
        /// Base directory for all subdirectories
        BASE_DIR,

        /// Base directory for all savegames
        SAVE_DIR,

        /// Subdirectory of save for autosaves
        AUTOSAVE_DIR,

        /// Base directory for all scenarios
        SCENARIO_DIR,

        /// Subdirectory of scenario for heightmaps
        HEIGHTMAP_DIR,

        /// Old subdirectory for the music
        OLD_GM_DIR,

        /// Old subdirectory for the data.
        OLD_DATA_DIR,

        /// Subdirectory for all base data (base sets, intro game)
        BASESET_DIR,

        /// Subdirectory for all NewGRFs
        NEWGRF_DIR,

        /// Subdirectory for all translation files
        LANG_DIR,

        /// Subdirectory for all %AI files
        AI_DIR,

        /// Subdirectory for all %AI libraries
        AI_LIBRARY_DIR,

        /// Subdirectory for all game scripts
        GAME_DIR,

        /// Subdirectory for all GS libraries
        GAME_LIBRARY_DIR,

        /// Subdirectory for all screenshots
        SCREENSHOT_DIR,

        /// Number of subdirectories   
        NUM_SUBDIRS,

        /// A path without any base directory
        NO_DIRECTORY,
    }

/**
 * Types of searchpaths OpenTTD might use
 */

    public enum Searchpath
    {
        SP_FIRST_DIR,

        /// Search in the working directory
        SP_WORKING_DIR = SP_FIRST_DIR,
//#if defined(WITH_XDG_BASEDIR) && defined(WITH_PERSONAL_DIR)
//		/// Search in the personal directory from the XDG specification
//	SP_PERSONAL_DIR_XDG,          
//#endif

        /// Search in the personal directory
        SP_PERSONAL_DIR,

        /// Search in the shared directory, like 'Shared Files' under Windows
        SP_SHARED_DIR,

        /// Search in the directory where the binary resides
        SP_BINARY_DIR,

        /// Search in the installation directory
        SP_INSTALLATION_DIR,

        /// Search within the application bundle
        SP_APPLICATION_BUNDLE_DIR,

        /// Search within the autodownload directory
        SP_AUTODOWNLOAD_DIR,
        NUM_SEARCHPATHS
    }

    //DECLARE_POSTFIX_INCREMENT(Searchpath)

    

       

}

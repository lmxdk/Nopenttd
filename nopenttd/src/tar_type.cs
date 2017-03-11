/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file tar_type.h Structs, typedefs and macros used for TAR file handling. */

using System.Collections.Generic;

namespace Nopenttd
{

/** The define of a TarList. */

    public struct TarListEntry
    {
        string filename;
        string dirname;

        /* MSVC goes copying around this struct after initialisation, so it tries
         * to free filename, which isn't set at that moment... but because it
         * initializes the variable with garbage, it's going to segfault. */

        //public TarListEntry()
        //{
        //    filename = null;
        //    dirname = null;
        //}
    }

    public struct TarFileListEntry
    {
        string tar_filename;
        int size;
        int position;
    }

    //typedef std::map<std::string, TarListEntry> TarList;
    public class TarList : Dictionary<string, TarListEntry>
    {
    }

//typedef std::map<std::string, TarFileListEntry> TarFileList;
    public class TarFileList : Dictionary<string, TarFileListEntry>
    {
    }

//extern TarList _tar_list[NUM_SUBDIRS];
//extern TarFileList _tar_filelist[NUM_SUBDIRS];

//#define FOR_ALL_TARS(tar, sd) for (tar = _tar_filelist[sd].begin(); tar != _tar_filelist[sd].end(); tar++)
}
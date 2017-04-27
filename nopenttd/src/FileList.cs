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
using Nopenttd.Core;
using Nopenttd.src.Core.Exceptions;

namespace Nopenttd
{
    /** List of file information. */

    public class FileList : IList<FiosItem>
    {
        /// The list of files.
        public List<FiosItem> files = new List<FiosItem>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<FiosItem> GetEnumerator() => files.GetEnumerator();

        public void Add(FiosItem item)
        {
            files.Add(item);
        }

        public bool Contains(FiosItem item) => files.Contains(item);

        public void CopyTo(FiosItem[] array, int arrayIndex)
        {
            files.CopyTo(array, arrayIndex, Count);
        }

        public bool Remove(FiosItem item) => files.Remove(item);

        public int Count => files.Count;
        public bool IsReadOnly => false;
        public int IndexOf(FiosItem item) => files.IndexOf(item);

        public void Insert(int index, FiosItem item)
        {
            files.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            files.RemoveAt(index);
        }

        public FiosItem this[int index]
        {
            get { return files[index]; }
            set { files[index] = value; }
        }

        public void Clear()
        {
            files.Clear();
        }
        
        /**
         * Construct a file list with the given kind of files, for the stated purpose.
         * @param abstract_filetype Kind of files to collect.
         * @param fop Purpose of the collection, either #SLO_LOAD or #SLO_SAVE.
         */
        public void BuildFileList(AbstractFileType abstract_filetype, SaveLoadOperation fop)
        {
            Clear();

            Debug.Assert(fop == SaveLoadOperation.SLO_LOAD ||  fop == SaveLoadOperation.SLO_SAVE);
            switch (abstract_filetype)
            {
                case AbstractFileType.FT_NONE:
                    break;

                case AbstractFileType.FT_SAVEGAME:
                    FiosGetSavegameList(fop, this);
                    break;

                case AbstractFileType.FT_SCENARIO:
                    FiosGetScenarioList(fop, this);
                    break;

                case AbstractFileType.FT_HEIGHTMAP:
                    FiosGetHeightmapList(fop, this);
                    break;

                default:
                    throw new NotReachedException();
            }
        }

        /**
         * Find file information of a file by its name from the file list.
         * @param file The filename to return information about. Can be the actual name
         *             or a numbered entry into the filename list.
         * @return The information on the file, or \c NULL if the file is not available.
         */
        public FiosItem FindItem(string file)
{
	foreach (var item in files) {
		if (file == item.name || file == item.title) return item;
	}

    /* If no name matches, try to parse it as number */
    if (int.TryParse(file, out var i) && MathFuncs.IsInsideMM(i, 0, (uint)this.files.Count))
    {
        return this[i];
    }
    
	/* As a last effort assume it is an OpenTTD savegame and
	 * that the ".sav" part was not given. */
    var long_file = file + ".sav";
            foreach (var item in files)
            {
                if (long_file == item.name || long_file == item.title) return item;
            }

	return null;
    }

}
}
   
using System;
using System.IO;
using Nopenttd.src;

namespace Nopenttd
{
    public delegate (FiosType type, string title) fios_getlist_callback_proc(SaveLoadOperation fop, string filename, string ext);
    /**
    * Scanner to scan for a particular type of FIOS file.
    */
    public class FiosFileScanner : FileScanner
    {
        SaveLoadOperation fop;   ///< The kind of file we are looking for.
            fios_getlist_callback_proc callback_proc; ///< Callback to check whether the file may be added
            FileList file_list;     ///< Destination of the found files.
        /**
         * Create the scanner
         * @param fop Purpose of collecting the list.
         * @param callback_proc The function that is called where you need to do the filtering.
         * @param file_list Destination of the found files.
         */
        public FiosFileScanner(SaveLoadOperation fop, fios_getlist_callback_proc callback_proc, FileList file_list) 
        {
            this.fop = fop;
            this.callback_proc = callback_proc;
            this.file_list = file_list;
        }


        /**
         * Try to add a fios item set with the given filename.
         * @param filename        the full path to the file to read
         * @param basepath_length amount of characters to chop of before to get a relative filename
         * @return true if the file is added.
         */

        public override bool AddFile(string filename, string tar_filename = null)
        {
            var ext = Path.GetExtension(filename);
            if (string.IsNullOrEmpty(ext))
            {
                return false;
            }

            var (type, title) = this.callback_proc(this.fop, filename, ext);
            if (type == FiosType.FIOS_TYPE_INVALID)
            {
                return false;
            }

            foreach (var item in file_list)
            {
                if (string.Equals(item.name, filename, StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }
            }

            //FiosItem* fios = file_list.Append();
            var fios = new FiosItem();
            var fileInfo = new FileInfo(filename);

                //struct _stat sb;
            if (fileInfo.Exists) {
                fios.mtime = (ulong)fileInfo.LastWriteTime.Ticks;
            } else {
                fios.mtime = 0;
            }

            fios.type = type;
            fios.name = filename;

/* If the file doesn't have a title, use its filename */
            var t = title;
            if (string.IsNullOrEmpty(title))
            {
                t = Path.GetFileName(filename);
                if (string.IsNullOrEmpty(t))
                {
                    t = filename;
                }
            }

            fios.title = t;
            
            str.str_validate(fios.title);

            return true;

        }
    }
}
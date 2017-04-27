namespace Nopenttd
{
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
        public uint Scan(string extension, Subdirectory sd, bool tars, bool recursive = true)
        {
            this.subdir = sd;

            uint num = 0;


            foreach (var sp in FileIO.FOR_ALL_SEARCHPATHS())
            {
                /* Don't search in the working directory */
                if (sp == Searchpath.SP_WORKING_DIR && !FileIO._do_scan_working_directory) continue;

                var path = FileIO.FioAppendDirectory(sp, sd);
                num += FileIO.ScanPath(this, extension, path, recursive);
            }

            if (tars && sd != Subdirectory.NO_DIRECTORY) {

                foreach (var tar in FileIO._tar_filelist[(int)sd])
                {
                    num += TarScanner.ScanTar(this, extension, tar);
                }
            }

            switch (sd) {
                case Subdirectory.BASESET_DIR:
                    num += this.Scan(extension, Subdirectory.OLD_GM_DIR, tars, recursive);
                    num += this.Scan(extension, Subdirectory.OLD_DATA_DIR, tars, recursive); //was /* FALL THROUGH */
                    break;
                case Subdirectory.NEWGRF_DIR:
                    num += this.Scan(extension, Subdirectory.OLD_DATA_DIR, tars, recursive);
                    break;
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
        public uint Scan(string extension, string directory, bool recursive = true)
        {
            if (string.IsNullOrEmpty(directory)) return 0;
            var path = FileIO.AppendPathSeparator(directory);
            return FileIO.ScanPath(this, extension, path, recursive);
        }


/**
 * Add a file with the given filename.
 * @param filename        the full path to the file to read
 * @param basepath_length amount of characters to chop of before to get a
 *                        filename relative to the search path.
 * @param tar_filename    the name of the tar file the file is read from.
 * @return true if the file is added.
 */
        public abstract bool AddFile(string filename, string tar_filename = null);
    }
}
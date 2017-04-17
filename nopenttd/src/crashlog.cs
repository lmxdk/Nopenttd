/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file crashlog.h Functions to be called to log a crash */

/**
 * Helper class for creating crash logs.
 */

using System;
using System.IO;
using System.Text;
using Nopenttd;
using Nopenttd.src;

namespace Nopenttd
{

    public abstract class CrashLog
    {

        /** Pointer to the error message. */
        private static string message;

        /** Temporary 'local' location of the buffer. */
        protected static readonly StringBuilder gamelog_buffer = new StringBuilder();

        /**
     * Helper function for printing the gamelog.
     * @param s the string to print.
     */

        private static void GamelogFillCrashLog(string s)
        {
            gamelog_buffer.AppendLine(s);
        }

        /**
     * Writes the gamelog data to the buffer.
     * @param buffer The begin where to write at.
     * @param last   The last position in the buffer to write to.
     * @return the position of the \c '\0' character after the buffer.
     */

        private void LogGamelog()
        {
            gamelog_buffer.AppendLine();
            gamelog.GamelogPrint(GamelogFillCrashLog);
        }



/**
 * Writes OS' version to the buffer.
 * @param buffer The begin where to write at.
 * @param last   The last position in the buffer to write to.
 * @return the position of the \c '\0' character after the buffer.
 */
        protected abstract void LogOSVersion();

        /**
         * Writes compiler (and its version, if available) to the buffer.
         * @param buffer The begin where to write at.
         * @param last   The last position in the buffer to write to.
         * @return the position of the \c '\0' character after the buffer.
         */

        protected virtual void LogCompiler()
        {
            //gamelog_buffer.AppendLine($" Compiler: MSVC {GetType().Assembly.ImageRuntimeVersion}{Environment.NewLine}");
            gamelog_buffer.AppendLine(
                $" Assembly runtime version: MSVC {GetType().Assembly.ImageRuntimeVersion}{Environment.NewLine}");
            gamelog_buffer.AppendLine($" Environment version: MSVC {Environment.Version}{Environment.NewLine}");
        }


        /**
         * Writes actually encountered error to the buffer.
         * @param buffer  The begin where to write at.
         * @param last    The last position in the buffer to write to.
         * @param message Message passed to use for possible errors. Can be NULL.
         * @return the position of the \c '\0' character after the buffer.
         */
        protected abstract void LogError(string message);

        /**
         * Writes the stack trace to the buffer, if there is information about it
         * available.
         * @param buffer The begin where to write at.
         * @param last   The last position in the buffer to write to.
         * @return the position of the \c '\0' character after the buffer.
         */
        protected abstract void LogStacktrace();

        /**
         * Writes information about the data in the registers, if there is
         * information about it available.
         * @param buffer The begin where to write at.
         * @param last   The last position in the buffer to write to.
         * @return the position of the \c '\0' character after the buffer.
         */
        protected abstract void LogRegisters();

        /**
         * Writes the dynamically linked libraries/modules to the buffer, if there
         * is information about it available.
         * @param buffer The begin where to write at.
         * @param last   The last position in the buffer to write to.
         * @return the position of the \c '\0' character after the buffer.
         */
        protected abstract void LogModules();

        /**
     * Writes OpenTTD's version to the buffer.
     * @param buffer The begin where to write at.
     * @param last   The last position in the buffer to write to.
     * @return the position of the \c '\0' character after the buffer.
     */

        protected void LogOpenTTDVersion()
        {
            throw new NotImplementedException();
            //TODO implement
            //        gamelog_buffer.AppendLine(
            //$@"OpenTTD version:{Environment.NewLine}
            // Version:    {_openttd_revision} ({_openttd_revision_modified}) { Environment.NewLine}
            // NewGRF ver: {_openttd_newgrf_version:x8}{Environment.NewLine}
            // Bits:       32{Environment.NewLine}
            // Endian:     little{Environment.NewLine}
            // Dedicated:  no{Environment.NewLine}
            // Build date: {_openttd_build_date}{Environment.NewLine}"        
            //    );
        }


/**
 * Writes the (important) configuration settings to the buffer.
 * E.g. graphics set, sound set, blitter and AIs.
 * @param buffer The begin where to write at.
 * @param last   The last position in the buffer to write to.
 * @return the position of the \c '\0' character after the buffer.
 */

        protected void LogConfiguration()
        {
            throw new NotImplementedException();
            //TODO implement
            //        var none = "none";
            //    gamelog_buffer.AppendLine(
            //$@"Configuration:{Environment.NewLine}
            // Blitter:      {BlitterFactory.GetCurrentBlitter().GetName() ?? none}{Environment.NewLine}
            // Graphics set: {BaseGraphics.GetUsedSet().name ?? none} ({BaseGraphics.GetUsedSet()?.version ?? uint.MaxValue}){ Environment.NewLine}
            // Language:     {_current_language?.file ?? none}{Environment.NewLine}
            // Music driver: {MusicDriver.GetInstance()?.GetName() ?? none}{Environment.NewLine}
            // Music set:    {BaseMusic.GetUsedSet()?.name ?? none} ({BaseMusic.GetUsedSet()?.version ?? uint.MaxValue}){Environment.NewLine}
            // Network:      {(_networking ? (_network_server ? "server" : "client") : "no")}{Environment.NewLine}
            // Sound driver: {SoundDriver.GetInstance()?.GetName() ?? none}{Environment.NewLine}
            // Sound set:    {BaseSounds.GetUsedSet()?.name ?? none} ({BaseSounds.GetUsedSet()?.version ?? uint.MaxValue}){Environment.NewLine}
            // Video driver: {VideoDriver.GetInstance()?.GetName() ?? none}{Environment.NewLine}"
            //	);

            //        gamelog_buffer.AppendLine(
            //$@"Fonts:{Environment.NewLine}
            // Small:  {FontCache.Get(FS_SMALL).GetFontName()}{Environment.NewLine}
            // Medium: {FontCache.Get(FS_NORMAL).GetFontName()}{Environment.NewLine}
            // Large:  {FontCache.Get(FS_LARGE).GetFontName()}{Environment.NewLine}
            // Mono:   {FontCache.Get(FS_MONO).GetFontName()}{Environment.NewLine}"            
            //    );

            //        gamelog_buffer.AppendLine("AI Configuration (local: {(int) _local_company}):");
            //    var c;

            //    FOR_ALL_COMPANIES(c)
            //    {
            //        if (c.ai_info == null)
            //        {
            //                gamelog_buffer.AppendLine($" {(int)c.index}: Human");
            //        }
            //        else
            //        {
            //                gamelog_buffer.AppendLine($" {(int)c.index}: {c.ai_info.GetName()} (v{c.ai_info.GetVersion()})");
            //        }
            //    }

            //	if (Game.GetInfo() != null) {
            //		gamelog_buffer.AppendLine($" GS: {Game.GetInfo().GetName()} (v{Game.GetInfo().GetVersion()}");
            //}
            //gamelog_buffer.AppendLine();

        }



        /**
         * Writes information (versions) of the used libraries.
         * @param buffer The begin where to write at.
         * @param last   The last position in the buffer to write to.
         * @return the position of the \c '\0' character after the buffer.
         */

        protected void LogLibraries()
        {

            gamelog_buffer.AppendLine("Libraries:");

            throw new NotImplementedException();
            //TODO implement
            //FT_Library library;
            //int major, minor, patch;

            //FT_Init_FreeType(&library);

            //FT_Library_Version(library, &major, &minor, &patch);

            //FT_Done_FreeType(library);
            //buffer += seprintf(buffer, last, " FreeType:   %d.%d.%d\n", major, minor, patch);

            /* 4 times 0-255, separated by dots (.) and a trailing '\0' */
            //char[] buf = new char[4 * 3 + 3 + 1];
            //UVersionInfo ver;

            //u_getVersion(ver);

            //u_versionToString(ver, buf);
            //gamelog_buffer.AppendLine($" ICU i18n:   {buf}");
            //gamelog_buffer.AppendLine($" ICU lx:     {buf}");
            //gamelog_buffer.AppendLine($" LZMA:       {lzma_version_string()}");
            //gamelog_buffer.AppendLine($" LZO:        {lzo_version_string()}");
            //gamelog_buffer.AppendLine($" PNG:        {png_get_libpng_ver(null)}");
            //gamelog_buffer.AppendLine($" Zlib:       {zlibVersion()}");    	
        }


        protected string LogGamelog(string buffer);


/**
 * Fill the crash log buffer with all data of a crash log.
 * @param buffer The begin where to write at.
 * @param last   The last position in the buffer to write to.
 * @return the position of the \c '\0' character after the buffer.
 */

        public void FillCrashLog()
        {
            var cur_time = time(null);
            gamelog_buffer.AppendLine("*** OpenTTD Crash Report ***");
            gamelog_buffer.AppendLine();
            gamelog_buffer.Append($"Crash at: {asctime(gmtime(cur_time))}");

            YearMonthDay ymd;

            ConvertDateToYMD(_date, &ymd);
            gamelog_buffer.AppendLine($"In game date: {ymd.year:}-{ymd.month + 1:00}-{ymd.day:00} ({_date_fract})");
            gamelog_buffer.AppendLine();

            LogError(CrashLog.message);
            LogOpenTTDVersion();
            LogRegisters();
            LogStacktrace();
            LogOSVersion();
            LogCompiler();
            LogConfiguration();
            LogLibraries();
            LogModules();
            LogGamelog();


            gamelog_buffer.AppendLine("*** End of OpenTTD Crash Report ***");
        }


/**
 * Write the crash log to a file.
 * @note On success the filename will be filled with the full path of the
 *       crash log file. Make sure filename is at least \c MAX_PATH big.
 * @param buffer The begin of the buffer to write to the disk.
 * @param filename      Output for the filename of the written file.
 * @param filename_last The last position in the filename buffer.
 * @return true when the crash log was successfully written.
 */

        public (bool success, string filename) WriteCrashLog()
        {
            var filename = $"{FileIO._personal_dir}scrash.log";

            using (var file = FileIO.FioFOpenFile(filename, FileMode.Append, Subdirectory.NO_DIRECTORY, out var filesize))
            {
                if (file == null)
                {
                    return (false, filename);
                }
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(gamelog_buffer.ToString());
                }
            }
            return (true, filename);
        }

        /**
         * Write the (crash) dump to a file.
         * @note On success the filename will be filled with the full path of the
         *       crash dump file. Make sure filename is at least \c MAX_PATH big.
         * @param filename      Output for the filename of the written file.
         * @param filename_last The last position in the filename buffer.
         * @return if less than 0, error. If 0 no dump is made, otherwise the dump
         *         was successful (not all OSes support dumping files).
         */
        public virtual int WriteCrashDump(string filename) => 0; // Stub implementation; not all OSes support this. 


/**
 * Write the (crash) savegame to a file.
 * @note On success the filename will be filled with the full path of the
 *       crash save file. Make sure filename is at least \c MAX_PATH big.
 * @param filename      Output for the filename of the written file.
 * @param filename_last The last position in the filename buffer.
 * @return true when the crash save was successfully made.
 */

        public bool WriteSavegame(string filename)
        {
            throw new NotImplementedException();
            //TODO Implement
            ///* If the map array doesn't exist, saving will fail too. If the map got
            // * initialised, there is a big chance the rest is initialised too. */
            //   if (Map._m == null)
            //   {
            //       return false;
            //   }

            //try {
            //       GamelogEmergency();

            //       filename = $"{filename}{FileIO._personal_dir}scrash.sav";

            //	/* Don't do a threaded saveload. */
            //	return SaveOrLoad(filename, SLO_SAVE, DFT_GAME_FILE, NO_DIRECTORY, false) == SL_OK;
            //} catch (Exception) {
            //	return false;
            //}
        }

/**
 * Write the (crash) screenshot to a file.
 * @note On success the filename will be filled with the full path of the
 *       screenshot. Make sure filename is at least \c MAX_PATH big.
 * @param filename      Output for the filename of the written file.
 * @param filename_last The last position in the filename buffer.
 * @return true when the crash screenshot was successfully made.
 */

        public bool WriteScreenshot(string filename)
        {
            throw new NotImplementedException();
            //TODO implement
            ///* Don't draw when we have invalid screen size */
            //   if (_screen.width < 1 || _screen.height < 1 || _screen.dst_ptr == null)
            //   {
            //       return false;
            //   }

            //bool res = MakeScreenshot(SC_CRASHLOG, "crash");
            //if (res) strecpy(filename, _full_screenshot_name, filename_last);
            //return res;
        }


        private static bool crashlogged = false;

/**
 * Makes the crash log, writes it to a file and then subsequently tries
 * to make a crash dump and crash savegame. It uses DEBUG to write
 * information like paths to the console.
 * @return true when everything is made successfully.
 */

        public bool MakeCrashLog()

        {
            /* Don't keep looping logging crashes. */
            if (crashlogged)
            {
                return false;
            }
            crashlogged = true;
            
            bool ret = true;


            Console.WriteLine("Crash encountered, generating crash log..."); //printf

            FillCrashLog();
            Console.WriteLine(gamelog_buffer.ToString());
            Console.WriteLine("Crash log generated.");
            Console.WriteLine();

            Console.WriteLine("Writing crash log to disk...");
            var (bret, filename) = WriteCrashLog();
            if (bret)
            {
                Console.WriteLine($"Crash log written to {filename}. Please add this file to any bug reports.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Writing crash log failed. Please attach the output above to any bug reports.");
                Console.WriteLine();
                ret = false;
            }

            /* Don't mention writing crash dumps because not all platforms support it. */
            int dret = WriteCrashDump(filename);
            if (dret < 0)
            {
                Console.WriteLine("Writing crash dump failed.");
                Console.WriteLine();
                ret = false;
            }
            else if (dret > 0)
            {
                Console.WriteLine($"Crash dump written to {filename}. Please add this file to any bug reports.");
                Console.WriteLine();
            }

            Console.WriteLine("Writing crash savegame...");
            bret = WriteSavegame(filename);
            if (bret)
            {
                Console.WriteLine(
                    "Crash savegame written to {filename}. Please add this file and the last (auto)save to any bug reports.");
                Console.WriteLine();
            }
            else
            {
                ret = false;
                Console.WriteLine("Writing crash savegame failed. Please attach the last (auto)save to any bug reports.");
                Console.WriteLine();
            }

            Console.WriteLine("Writing crash screenshot...");
            bret = WriteScreenshot(filename);
            if (bret)
            {
                Console.WriteLine("Crash screenshot written to {filename}. Please add this file to any bug reports.");
                Console.WriteLine();
            }
            else
            {
                ret = false;
                Console.WriteLine("Writing crash screenshot failed.");
                Console.WriteLine();
            }

            return ret;
        }


/**
 * Initialiser for crash logs; do the appropriate things so crashes are
 * handled by our crash handler instead of returning straight to the OS.
 * @note must be implemented by all implementers of CrashLog.
 */
        public static void InitialiseCrashLog();

/**
 * Sets a message for the error message handler.
 * @param message The error message of the error.
 */
/* static */

        public static void SetErrorMessage(string message)
        {
            CrashLog.message = message;
        }

/**
 * Try to close the sound/video stuff so it doesn't keep lingering around
 * incorrect video states or so, e.g. keeping dpmi disabled.
 */

        public static void AfterCrashLogCleanup()
        {
            throw new NotImplementedException();
            //TODO Implement
            //MusicDriver.GetInstance()?.Stop();
            //SoundDriver.GetInstance()?.Stop();
            //VideoDriver.GetInstance()?.Stop();
        }
    }
}
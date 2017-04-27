using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
/* $Id$ */

/*
* This file is part of OpenTTD.
* OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
* OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
*/

/** @file win32.h declarations of functions for MS windows systems */



typedef void(*Function)(int);
bool LoadLibraryList(Function proc
using NLog;
using Nopenttd.Core;
using Nopenttd.src;

[], const char *dll);

char *convert_from_fs(const TCHAR *name, char *utf8_buf, size_t buflen);
TCHAR *convert_to_fs(const char *name, TCHAR *utf16_buf, size_t buflen, bool console_cp = false);

/* Function shortcuts for UTF-8 <> UNICODE conversion. When unicode is not
* defined these macros return the string passed to them, with UNICODE
* they return a pointer to the converted string. These functions use an
* internal buffer of max 512 characters. */
# define MB_TO_WIDE(str) OTTD2FS(str)
# define WIDE_TO_MB(str) FS2OTTD(str)

#if defined(__MINGW32__) && !defined(__MINGW64__)
#define SHGFP_TYPE_CURRENT 0
#endif /* __MINGW32__ */

void SetWin32ThreadName(DWORD dwThreadID, const char* threadName);


namespace Nopenttd.Os.Windows
{
    /* Code below for windows version of opendir/readdir/closedir copied and
 * modified from Jan Wassenberg's GPL implementation posted over at
 * http://www.gamedev.net/community/forums/topic.asp?topic_id=364584&whichpage=1&#2398903 */

    //public class DIR
    //{
    //    HANDLE hFind;
    //    /* the dirent returned by readdir.
    //     * note: having only one global instance is not possible because
    //     * multiple independent opendir/readdir sequences must be supported. */
    //    dirent ent;
    //    WIN32_FIND_DATA fd;
    //    /* since opendir calls FindFirstFile, we need a means of telling the
    //     * first call to readdir that we already have a file.
    //     * that's the case iff this is true */
    //    bool at_first_entry;
    //};


    public class Win32
    {
        private static readonly ILogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        /* $Id$ */

        /*
         * This file is part of OpenTTD.
         * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
         * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
         * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
         */

        /** @file win32.cpp Implementation of MS Windows system calls */

        //#include "../../stdafx.h"
        //#include "../../debug.h"
        //#include "../../gfx_func.h"
        //#include "../../textbuf_gui.h"
        //#include "../../fileio_func.h"
        //#include <windows.h>
        //#include <fcntl.h>
        //#include <regstr.h>
        //#include <shlobj.h> /* SHGetFolderPath */
        //#include <shellapi.h>
        //#include "win32.h"
        //#include "../../fios.h"
        //#include "../../core/alloc_func.hpp"
        //#include "../../openttd.h"
        //#include "../../core/random_func.hpp"
        //#include "../../string_func.h"
        //#include "../../crashlog.h"
        //#include <errno.h>
        //#include <sys/stat.h>

        ///* Due to TCHAR, strncat and strncpy have to remain (for a while). */
        //#include "../../safeguards.h"
        //#undef strncat
        //#undef strncpy

        static bool _has_console;
static bool _cursor_disable = true;
static bool _cursor_visible = true;

public bool MyShowCursor(bool show, bool toggle)
{
	if (toggle) {_cursor_disable = !_cursor_disable;}
	if (_cursor_disable) {return show;}
	if (_cursor_visible == show) {return show;}

	_cursor_visible = show;
	ShowCursor(show);

	return !show;
}

/**
 * Helper function needed by dynamically loading libraries
 * XXX: Hurray for MS only having an ANSI GetProcAddress function
 * on normal windows and no Wide version except for in Windows Mobile/CE
 */
public bool LoadLibraryList(Function proc[], string dll)
{
	while (dll != '\0') {
		HMODULE lib;
		lib = LoadLibrary(MB_TO_WIDE(dll));

		if (lib == NULL) return false;
		for (;;) {
			FARPROC p;

			while (*dll++ != '\0') { /* Nothing */ }
			if (*dll == '\0') {break;}
			p = GetProcAddress(lib, dll);
			if (p == NULL) {return false;}
			*proc++ = (Function)p;
		}
		dll++;
	}
	return true;
}
		
public void ShowOSErrorBox(string buf, bool system)
{
    MessageBox.Show(buf, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);	
}

void OSOpenBrowser(string url)
{
			//verify url?
	Process.Start(url);
}

/* suballocator - satisfies most requests with a reusable static instance.
 * this avoids hundreds of alloc/free which would fragment the heap.
 * To guarantee concurrency, we fall back to malloc if the instance is
 * already in use (it's important to avoid surprises since this is such a
 * low-level routine). */
//static DIR _global_dir;
//static LONG _global_dir_is_in_use = false;

//static inline DIR *dir_calloc()
//{
//	DIR *d;

//	if (InterlockedExchange(&_global_dir_is_in_use, true) == (LONG)true) {
//		d = CallocT<DIR>(1);
//	} else {
//		d = &_global_dir;
//		memset(d, 0, sizeof(*d));
//	}
//	return d;
//}

//static inline void dir_free(DIR *d)
//{
//	if (d == &_global_dir) {
//		_global_dir_is_in_use = (LONG)false;
//	} else {
//		free(d);
//	}
//}

public static DirectoryInfo opendir(string path)
{
	var dir = new DirectoryInfo(path);
    return dir.Exists == false ? null : dir;
}
        
public static bool FiosIsRoot(string file)
{
    return new DirectoryInfo(file).Parent == null;
}

public static void FiosGetDrives(FileList file_list)
{
    var drives = DriveInfo.GetDrives();
	        
	foreach (var drive in drives)
    {
        var fios = new FiosItem();
		fios.type = FiosType.FIOS_TYPE_DRIVE;
		fios.mtime = 0;
        fios.name = drive.Name;
		fios.title = drive.Name;
                file_list.Add(fios);
	}
}

        [Obsolete("Use exists directly")]
public static bool FiosIsValidFile(FileSystemInfo path)
        {
            return path.Exists;
        }

public static bool FiosIsHiddenFile(FileSystemInfo file)
{
	return file.Attributes.HasFlag(FileAttributes.Hidden | FileAttributes.System);
}

public (bool found, long totalFreeSpace) FiosGetDiskFreeSpace(string path)
{		
    var root = Path.GetPathRoot(path);
    
    var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.Name == root);
    if (drive != null)
    {
        return (true, drive.AvailableFreeSpace);        
    }
	return (false, 0l);
}
        
void CreateConsole()
{
	HANDLE hand;
	CONSOLE_SCREEN_BUFFER_INFO coninfo;

	if (_has_console) return;
	_has_console = true;

	AllocConsole();

	hand = GetStdHandle(STD_OUTPUT_HANDLE);
	GetConsoleScreenBufferInfo(hand, &coninfo);
	coninfo.dwSize.Y = 500;
	SetConsoleScreenBufferSize(hand, coninfo.dwSize);

	/* redirect unbuffered STDIN, STDOUT, STDERR to the console */

	/* Check if we can open a handle to STDOUT. */
	int fd = _open_osfhandle((intptr_t)hand, _O_TEXT);
	if (fd == -1) {
		/* Free everything related to the console. */
		FreeConsole();
		_has_console = false;
		_close(fd);
		CloseHandle(hand);

		ShowInfo("Unable to open an output handle to the console. Check known-bugs.txt for details.");
		return;
	}
    
	freopen("CONOUT$", "a", stdout);
	freopen("CONIN$", "r", stdin);
	freopen("CONOUT$", "a", stderr);
           

	setvbuf(stdin, NULL, _IONBF, 0);
	setvbuf(stdout, NULL, _IONBF, 0);
	setvbuf(stderr, NULL, _IONBF, 0);
}

/** Temporary pointer to get the help message to the window */
static const char *_help_msg;

/** Callback function to handle the window */
static INT_PTR CALLBACK HelpDialogFunc(HWND wnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	switch (msg) {
		case WM_INITDIALOG: {
			char help_msg[8192];
			const char *p = _help_msg;
			char *q = help_msg;
			while (q != lastof(help_msg) && *p != '\0') {
				if (*p == '\n') {
					*q++ = '\r';
					if (q == lastof(help_msg)) {
						q[-1] = '\0';
						break;
					}
				}
				*q++ = *p++;
			}
			*q = '\0';
			/* We need to put the text in a separate buffer because the default
			 * buffer in OTTD2FS might not be large enough (512 chars). */
			TCHAR help_msg_buf[8192];
			SetDlgItemText(wnd, 11, convert_to_fs(help_msg, help_msg_buf, lengthof(help_msg_buf)));
			SendDlgItemMessage(wnd, 11, WM_SETFONT, (WPARAM)GetStockObject(ANSI_FIXED_FONT), FALSE);
		} return TRUE;

		case WM_COMMAND:
			if (wParam == 12) ExitProcess(0);
			return TRUE;
		case WM_CLOSE:
			ExitProcess(0);
	}

	return FALSE;
}

public void ShowInfo(string str)
{
	if (_has_console) {
            Console.WriteLine(str);
	} else {
		bool old;
		ReleaseCapture();
		_left_button_clicked = _left_button_down = false;

		old = MyShowCursor(true);
		if (str.Length > 2048) {
			/* The minimum length of the help message is 2048. Other messages sent via
			 * ShowInfo are much shorter, or so long they need this way of displaying
			 * them anyway. */
			_help_msg = str;                    
			DialogBox(GetModuleHandle(NULL), MAKEINTRESOURCE(101), NULL, HelpDialogFunc);
		} else {
			/* We need to put the text in a separate buffer because the default
			 * buffer in OTTD2FS might not be large enough (512 chars). */
		    MessageBox.Show(str, "OpenTTD", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		MyShowCursor(old);
	}
}

public static void Main(params string[] args)
//int APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	CrashLog::InitialiseCrashLog();

	/* Check if a win9x user started the win32 version */
	if (HasBit(GetVersion(), 31)) usererror("This version of OpenTTD doesn't run on windows 95/98/ME.\nPlease download the win9x binary and try again.");

	/* Convert the command line to UTF-8. We need a dedicated buffer
	 * for this because argv[] points into this buffer and this needs to
	 * be available between subsequent calls to FS2OTTD(). */
	
//#if defined(_DEBUG)
//	CreateConsole();
//#endif

	//_set_error_mode(_OUT_TO_MSGBOX); // force assertion output to messagebox

	/* setup random seed to something quite random */
	Randomizer.SetRandomSeed((uint)DateTime.Now.Ticks);
	

	/* Make sure our arguments contain only valid UTF-8 characters. */
    for (int i = 0; i < args.Length; i++)
    {
        args[i] = ValidateString(args[i].Trim());
    }

	openttd_main(args);
}

[Obsolete("use Directory.GetCurrentDirectory()")]
public static string getcwd()
{
    return Directory.GetCurrentDirectory();
}



        /**
         * Determine the base (personal dir and game data dir) paths
         * @param exe the path from the current path to the executable
         * @note defined in the OS related files (os2.cpp, win32.cpp, unix.cpp etc)
         */
        public static void DetermineBasePaths(string exe)
{
    var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    FileIO._searchpaths[(int)Searchpath.SP_PERSONAL_DIR] = FileIO.AppendPathSeparator(FileIO.AppendPathSeparator(myDocs) + PERSONAL_DIR);
    
    var commonDocs = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
    FileIO._searchpaths[(int)Searchpath.SP_SHARED_DIR] = FileIO.AppendPathSeparator(FileIO.AppendPathSeparator(commonDocs) + PERSONAL_DIR);
    
	/* Get the path to working directory of OpenTTD */
    FileIO._searchpaths[(int)Searchpath.SP_WORKING_DIR] = FileIO.AppendPathSeparator(Directory.GetCurrentDirectory());

    var binary = Assembly.GetEntryAssembly().Location;
    
	if (binary == null) {
		Log.Debug("No asembly path");
	}

    FileIO._searchpaths[(int) Searchpath.SP_BINARY_DIR] = binary;
    FileIO._searchpaths[(int)Searchpath.SP_INSTALLATION_DIR]       = null;
	FileIO._searchpaths[(int)Searchpath.SP_APPLICATION_BUNDLE_DIR] = null;
}


public static (bool success, string content) GetClipboardContents()
{
            
	if (Clipboard.ContainsText(TextDataFormat.UnicodeText))
	{
	    var content = Clipboard.GetText(TextDataFormat.Text);

	    if (content.Any())
	    {
	        return (true, content);
	    }
	} 

	return (false, null);
}


[Obsolete("Use Thread.Sleep")]
public static void CSleep(int milliseconds)
{
	Thread.Sleep(milliseconds);
}


/**
 * Convert to OpenTTD's encoding from that of the local environment.
 * When the project is built in UNICODE, the system codepage is irrelevant and
 * the input string is wide. In ANSI mode, the string is in the
 * local codepage which we'll convert to wide-char, and then to UTF-8.
 * OpenTTD internal encoding is UTF8.
 * The returned value's contents can only be guaranteed until the next call to
 * this function. So if the value is needed for anything else, use convert_from_fs
 * @param name pointer to a valid string that will be converted (local, or wide)
 * @return pointer to the converted string; if failed string is of zero-length
 * @see the current code-page comes from video\win32_v.cpp, event-notification
 * WM_INPUTLANGCHANGE
 */
[Obsolete("use string directly")]
public static string FS2OTTD(string name)
{
    return name;
}

/**
 * Convert from OpenTTD's encoding to that of the local environment.
 * When the project is built in UNICODE the system codepage is irrelevant and
 * the converted string is wide. In ANSI mode, the UTF8 string is converted
 * to multi-byte.
 * OpenTTD internal encoding is UTF8.
 * The returned value's contents can only be guaranteed until the next call to
 * this function. So if the value is needed for anything else, use convert_from_fs
 * @param name pointer to a valid string that will be converted (UTF8)
 * @param console_cp convert to the console encoding instead of the normal system encoding.
 * @return pointer to the converted string; if failed string is of zero-length
 */
[Obsolete("use string directly")]
public static string OTTD2FS(string name, bool console_cp)
{
    return name;
}


/**
 * Convert to OpenTTD's encoding from that of the environment in
 * UNICODE. OpenTTD encoding is UTF8, local is wide
 * @param name pointer to a valid string that will be converted
 * @param utf8_buf pointer to a valid buffer that will receive the converted string
 * @param buflen length in characters of the receiving buffer
 * @return pointer to utf8_buf. If conversion fails the string is of zero-length
 */
[Obsolete("use name directly")]
public static string convert_from_fs(string name, ref string utf8_buf, int buflen)
{
    utf8_buf = name;
    return name;
}


/**
 * Convert from OpenTTD's encoding to that of the environment in
 * UNICODE. OpenTTD encoding is UTF8, local is wide
 * @param name pointer to a valid string that will be converted
 * @param utf16_buf pointer to a valid wide-char buffer that will receive the
 * converted string
 * @param buflen length in wide characters of the receiving buffer
 * @param console_cp convert to the console encoding instead of the normal system encoding.
 * @return pointer to utf16_buf. If conversion fails the string is of zero-length
 */
[Obsolete("use name directly")]
public static string convert_to_fs(string name, ref string system_buf, int buflen, bool console_cp)
{
    system_buf = name;
    return name;
}

/**
 * Our very own SHGetFolderPath function for support of windows operating
 * systems that don't have this function (eg Win9x, etc.). We try using the
 * native function, and if that doesn't exist we will try a more crude approach
 * of environment variables and hope for the best
 */
 [Obsolete("Use Environment.GetFolderPath")]
public static string OTTDSHGetFolderPath(Environment.SpecialFolder folder)
{
    return Environment.GetFolderPath(folder);
}

/** Determine the current user's locale. */
[Obsolete("use CultureInfo.CurrentCulture")]
public static string GetCurrentLocale()
{
    return CultureInfo.CurrentCulture.Name.Replace("-", "_");    
	//char lang[9], country[9];
	//if (GetLocaleInfoA(LOCALE_USER_DEFAULT, LOCALE_SISO639LANGNAME, lang, lengthof(lang)) == 0 ||
	//    GetLocaleInfoA(LOCALE_USER_DEFAULT, LOCALE_SISO3166CTRYNAME, country, lengthof(country)) == 0) {
	//	/* Unable to retrieve the locale. */
	//	return NULL;
	//}
	///* Format it as 'en_us'. */
	//static char retbuf[6] = {lang[0], lang[1], '_', country[0], country[1], 0};
	//return retbuf;
}

public static uint GetCPUCoreCount()
{
    return (uint)Environment.ProcessorCount;
}

///* Code from MSDN: https://msdn.microsoft.com/en-us/library/xcb2z8hs.aspx */
//const DWORD MS_VC_EXCEPTION = 0x406D1388;
//typedef struct {
//	DWORD dwType;     ///< Must be 0x1000.
//	LPCSTR szName;    ///< Pointer to name (in user addr space).
//	DWORD dwThreadID; ///< Thread ID (-1=caller thread).
//	DWORD dwFlags;    ///< Reserved for future use, must be zero.
//} THREADNAME_INFO;

/**
 * Signal thread name to any attached debuggers.
 */
 [Obsolete("Please set on thread when constructing")]
public static void SetWin32ThreadName(int dwThreadID, string threadName)
{
	//THREADNAME_INFO info;
	//info.dwType = 0x1000;
	//info.szName = threadName;
	//info.dwThreadID = dwThreadID;
	//info.dwFlags = 0;

	//__try {
	//	RaiseException(MS_VC_EXCEPTION, 0, sizeof(info) / sizeof(ULONG_PTR), (ULONG_PTR*)&info);
	//} __except (EXCEPTION_EXECUTE_HANDLER) {
	//}
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nopenttd.src
{

    public class LinkGraphOverlay
    {
    }
    public struct Town //TODO DUMMY
    {
    };

    public struct Window
    {
        public static string _windows_file;
    }



    public class Shared<TStruct> where TStruct : struct
    {
        public Shared(TStruct value)
        {
            Value = value;
        }
        public TStruct Value { get; set; }
    }

    public static class StringExtensions
    {
        public static string ReadNullTerminatedAsciiString(this byte[] arr, int index = 0, int? length = null)
        {
            var s = Encoding.ASCII.GetString(arr, 0, length ?? arr.Length);
            var nullIndex = s.IndexOf('\0');
            if (nullIndex >= 0)
            {
                s = s.Substring(0, nullIndex);
            }
            return s;
        }

        public static string ReadNullTerminatedString(this char[] arr)
        {
            var nullIndex = Array.IndexOf(arr, '\0');
            if (nullIndex > 0)
            {
                return new String(arr, 0, nullIndex - 1);
            }
            return new String(arr);
        }
    }



    [Obsolete("Use DirectoryInfo.GetFiles()")]
    public class dirent { }

    public class gamelog
    {
        public static void GamelogPrint(Action<string> logMethod)
        {
            
        }
    }

    public class Hotkeys
    {
        public static string _hotkeys_file;
    }
public class Dedicated
    {
        public static string _log_file;
    }

    public class str
    {
        public static void str_validate(string str, StringValidationSettings settings = StringValidationSettings.SVS_NONE)
        {
            throw new NotImplementedException();
        }
    }


    
}

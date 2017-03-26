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
    { }



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
    }
}

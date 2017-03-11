using System;
using System.Collections.Generic;
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
        public Shared(bool value)
        {
            Value = value;
        }
        public TStruct Value { get; set; }
    }
}

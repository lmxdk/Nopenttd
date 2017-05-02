using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nopenttd.src
{
    public static class CharEnumeratorEtensions
    {
        public static bool MoveNext(this CharEnumerator enumerator, int steps)
        {
            for (var i = 0; i < steps; i++)
            {
                if (enumerator.MoveNext() == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class StringEnumerator : IEnumerator<char>
    {
        public StringEnumerator(string str)
        {
            this.str = str;
            this.Index = -1;
            MoveNext();
        }
        public StringEnumerator(StringEnumerator other)
        {
            Apply(other);
        }

        public void Apply(StringEnumerator other)
        {
            this.str = other.str;
            this.Index = other.Index;
            this.HasContent = other.HasContent;
            this.Enumerator = (CharEnumerator)other.Enumerator.Clone();
        }

        public string str { get; private set; }
        private CharEnumerator Enumerator;
        public bool HasContent { get; private set; }
        public int Index { get; private set; }
        public const char NullChar = '\0';

        public bool MoveNext()
        {
            if (HasContent)
            {
                Index++;
                HasContent = Enumerator.MoveNext();
            }
            return HasContent;
        }

        void IEnumerator.Reset() => Reset();

        object IEnumerator.Current => Current;

        public bool MoveNext(int steps)
        {
            for (var i = 0; i < steps; i++)
            {
                HasContent = MoveNext();
                if (HasContent == false)
                {
                    break;
                }
            }
            return HasContent;
        }

        public bool SkipUntil(char c)
        {
            while (HasContent)
            {
                if (c == Enumerator.Current)
                {
                    break;
                }
                MoveNext();
            }
            return HasContent;
        }

        public char Current => Enumerator.Current;

        public bool Reset()
        {
            Index = -1;
            Enumerator.Reset();
            return MoveNext();
        }

        public void Dispose() => Enumerator.Dispose();
       
    }

    public class StringTokenBuilder
    {
        private StringBuilder token = new StringBuilder();
        private string str;
        private int index;
        private CharEnumerator enumerator;
        public bool HasContent { get; private set; }
        public const char NullChar = '\0';

        public bool Init(string str)
        {
            this.str = str;
            index = -1;
            enumerator = str.GetEnumerator();
            token.Clear();
            HasContent = enumerator.MoveNext();            
            return HasContent;
        }

        public char Current => enumerator.Current;

        public void BeginToken()
        {
            token.Clear();
        }

        public bool SkipUntil(char c)
        {
            if (HasContent)
            {
                
            }
            // ReSharper disable once AssignmentInConditionalExpression
            while (HasContent = enumerator.MoveNext() && enumerator.Current != NullChar)
            {
                
            }
        }

        public string GetToken() => token.ToString();


        private long number ReadInt(int radix)
        {
            //tries to mimic c style strtol

            var builder = new StringBuilder();
            var any = false;
            // ReSharper disable once AssignmentInConditionalExpression
            while (any = enumerator.MoveNext())
            {
                var c = enumerator.Current;
                if ((c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || (c >= '0' && c <= '9'))
                {
                    builder.Append(c);
                }
                else
                {
                    break;
                }
            }

            return ((builder.Length > 0 ? Convert.ToInt64(builder.ToString(), 16) : 0), any);
        }
    }
}

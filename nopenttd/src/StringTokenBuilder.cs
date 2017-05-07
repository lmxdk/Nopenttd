using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nopenttd.Core;
using Nopenttd.src.Core.Exceptions;

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

    public static class StringBuilderExtensions
    {
        public static int AppendAsUtfChar(this StringBuilder buf, int c)
        {
            if (c <= 0xD7FF || (0xE000 <= c && c <= 0xFFFF))
            {
                buf.Append((char) c);
                return 1;
            }
            else if (0x10000 <= c || c <= 0x10FFFF)
            {
                c -= 0x010000;
                var higher = (char)(BitMath.GB((uint)c, 10, 10) + 0xD800);
                var lower = (BitMath.GB((uint) c, 0, 10) + 0xDC00);
                buf.Append(higher);
                buf.Append(lower);
                return 2;
            }
            //throw new NotReachedException();
            /* DEBUG(misc, 1, "[utf8] can't UTF-8 encode value 0x%X", c); */
            buf.Append("?");
            
            return 1;

        }
    }

    public class IndexedString
    {
        public IndexedString(string text)
        {
            Text = text;
        }

        public IndexedString(IndexedString other)
        {
            Text = other.Text;
            Index = other.Index;
        }

        public IndexedString Clone() => new IndexedString(this);
        public string Text;
        public int Index = 0;
        public bool HasContent => Index < Text.Length;
        public char Current => Text[Index];

        public bool MoveNext(int delta = 1)
        {
            Index += delta;
            return HasContent;
        }
        public bool SkipUntil(char c)
        {
            while (HasContent)
            {
                if (c == Current)
                {
                    break;
                }
                MoveNext();
            }
            return HasContent;
        }

        /**
 * Decode and consume the next UTF-8 encoded character.
 * @param c Buffer to place decoded character.
 * @param s Character stream to retrieve character from.
 * @return Number of characters in the sequence.
 */
        public int DecodeUtfChar()
        {
            var c = Current;
            if (char.IsHighSurrogate(c))
            {
                MoveNext();
                var high = ((int)c) - 0xD800;
                var low = (int)Current - 0xDC00;
                var combined = ((high << 10) | low) + 0x010000;

                return combined;
            }
            return c;
        }


        public int ConsumeUtfChar()
        {
            var c = Current;
            if (char.IsHighSurrogate(c))
            {
                MoveNext();
                return char.ConvertToUtf32(c, Current);
            }
            return c;
        }

        public int ConsumeUtfChar(StringBuilder buffer)
        {
            var c = Current;
            if (char.IsHighSurrogate(c))
            {
                MoveNext();
                return char.ConvertToUtf32(c, Current);
            }
            return c;
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

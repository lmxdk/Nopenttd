using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NLog;

namespace Nopenttd
{
    public class StringParameters
    {
        private static readonly ILogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
        
        /// If not null, this instance references data from this parent instance.
        private StringParameters parent; /// Array with the actual data. 
        private ulong[] data;            /// Array with type information about the data. Can be null when no type information is needed. See #StringControlCode. 
        private char[] type;

        /// Current offset in the data/type arrays.
        public int offset;             /// Length of the data array. 

        /** Create a new StringParameters instance. */
        public StringParameters(ulong[] data, char[] type)
        {
            this.parent = null;
            this.data = data;
            this.type = type;
            this.offset = 0;
         }

        /** Create a new StringParameters instance. */
        //template<size_t Tnum_param>
            public StringParameters(long[] data) 
            {
                this.parent = null;
                this.data = data.Cast<ulong>().ToArray();
                this.type = null;
                this.offset = 0;
        }

        /**
         * Create a new StringParameters instance that can reference part of the data of
         * the given partent instance.
         */
        StringParameters(StringParameters parent, uint size)
        {
            this.parent = parent;
            data = parent.data; //was + parent.offset
            offset = parent.offset; //was 0
        
            Debug.Assert(data.Length <= parent.GetDataLeft());
            if (parent.type == null)
            {
                this.type = null;
            }
            else
            {
                this.type = parent.type; //was + parent.offset
            }
        }

        ~StringParameters()
        {
            if (this.parent != null)
            {
                this.parent.offset += this.data.Length;
            }
        }

        /** Read an int32 from the argument array. @see GetInt64. */
        public int GetInt32(char type = '\0') // 0
        {
            return (int)this.GetInt64(type);
        }

        /** Get a pointer to the current element in the data array. */
        public ulong GetDataPointer()
        {
            return this.data[this.offset];
        }

        /** Return the amount of elements which can still be read. */
        public int GetDataLeft()
        {
            return this.data.Length - this.offset;

        }

        /** Get a pointer to a specific element in the data array. */
        public ulong GetPointerToOffset(uint offset)
        {

            Debug.Assert(offset< this.data.Length);
            return this.data[offset];

        }

        /** Does this instance store information about the type of the parameters. */
        public bool HasTypeInformation()
        {
            return this.type != null;
        }

        /** Get the type of a specific element. */
        public char GetTypeAtOffset(uint offset)
        {
            Debug.Assert(offset< this.data.Length);
            Debug.Assert(this.HasTypeInformation());
            return this.type[offset];

        }

        public void SetParam(uint n, ulong v)
        {
            Debug.Assert(n < this.data.Length);
            this.data[n] = v;
        }

        public ulong GetParam(uint n)
        {
            Debug.Assert(n< this.data.Length);
            return this.data[n];
        }
    
        /** Reset the type array. */
        public void ClearTypeInformation()
        {
            Debug.Assert(this.type != null);
            this.type = new char[this.data.Length]; //set 0
        }


        /**
         * Read an int64 from the argument array. The offset is increased
         * so the next time GetInt64 is called the next value is read.
         */
        public long GetInt64(char type)
        {
            if (this.offset >= this.data.Length)
            {
                Log.Debug("Trying to read invalid string parameter");
                return 0;
            }
            if (this.type != null)
            {
                Debug.Assert(this.type[this.offset] == 0 || this.type[this.offset] == type);
                this.type[this.offset] = type;
            }
            return (long)this.data[this.offset++];
        }

        /**
         * Shift all data in the data array by the given amount to make
         * room for some extra parameters.
         */
        public void ShiftParameters(uint amount)
        {
            Debug.Assert(amount <= this.data.Length);
            Array.Copy(this.data, amount, this.data, 0, this.data.Length - amount);
        }
    }
}
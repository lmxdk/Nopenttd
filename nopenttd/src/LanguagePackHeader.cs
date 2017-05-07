using System;
using System.IO;
using Nopenttd.src;

namespace Nopenttd
{
    /** Header of a language file. */
    public class LanguagePackHeader
    {
        public const int HeaderByteSize = 4 + 4 + 32 * 2 + 32 * 2 + 16 * 2 + 2 * Language.TAB_COUNT + 2 * 8 + 2 * 8 + 2 * 8 + 2 + 1 + 1;

        /// Identifier for OpenTTD language files, big endian for "LANG"
        public const uint IDENT = 0x474E414C;

        /// 32-bits identifier
        public uint ident;

        /// 32-bits of auto generated version info which is basically a hash of strings.h
        public uint version;

        /// the international name of this language
        public string name; //[32];

        /// the localized name of this language
        public string own_name; //[32];

        /// the ISO code for the language (not country code)
        public string isocode; //[16];

        /// the offsets
        public ushort[] offsets = new ushort[Language.TAB_COUNT];

        /** Thousand separator used for anything not currencies */
        public string digit_group_separator; //[8];

        /** Thousand separator used for currencies */
        public string digit_group_separator_currency; //[8];

        /** Decimal separator */
        public string digit_decimal_separator; //[8];

        /// number of missing strings.
        public ushort missing;

        /// plural form index
        public byte plural_form;

        /// default direction of the text
        public byte text_dir;

        /**
         * Windows language ID:
         * Windows cannot and will not convert isocodes to something it can use to
         * determine whether a font can be used for the language or not. As a result
         * of that we need to pass the language id via strgen to OpenTTD to tell
         * what language it is in "Windows". The ID is the 'locale identifier' on:
         *   http://msdn.microsoft.com/en-us/library/ms776294.aspx
         */
        /// windows language id
        public ushort winlangid;

        /// newgrf language id
        public byte newgrflangid;

        /// the number of genders of this language
        public byte num_genders;

        /// the number of cases of this language
        public byte num_cases;

        /// pad header to be a multiple of 4
        //public byte pad = new byte[3];

        /// the genders used by this translation
        private string[] genders = new string[Language.MAX_NUM_GENDERS]; //[CASE_GENDER_LEN]; 

        /// the cases used by this translation
        private string[] cases = new string[Language.MAX_NUM_CASES]; //[CASE_GENDER_LEN];     

        /**
         * Get the index for the given gender.
         * @param gender_str The string representation of the gender.
         * @return The index of the gender, or MAX_NUM_GENDERS when the gender is unknown.
         */
        public byte GetGenderIndex(string gender_str)
        {
            for (byte i = 0; i < genders.Length; i++)
            {
                if (string.Equals(gender_str, genders[i], StringComparison.CurrentCultureIgnoreCase)) return i;
            }
            return (byte) genders.Length;
        }

        /**
         * Get the index for the given case.
         * @param case_str The string representation of the case.
         * @return The index of the case, or MAX_NUM_CASES when the case is unknown.
         */
        public byte GetCaseIndex(string case_str)
        {
            for (byte i = 0; i < cases.Length; i++)
            {
                if (string.Equals(case_str, cases[i], StringComparison.CurrentCultureIgnoreCase)) return i;
            }
            return (byte) cases.Length;
        }


        /**
         * Check whether the header is a valid header for OpenTTD.
         * @return true iff the header is deemed valid.
         */
        public bool IsValid()
        {
            return this.ident == IDENT &&
                   this.version == LANGUAGE_PACK_VERSION &&
                   this.plural_form < LANGUAGE_MAX_PLURAL &&
                   this.text_dir <= 1 &&
                   this.newgrflangid < StringConstants.MAX_LANG &&
                   this.num_genders < Language.MAX_NUM_GENDERS &&
                   this.num_cases < Language.MAX_NUM_CASES;
            //StrValid(this.name, lastof(this.name)) &&
            //StrValid(this.own_name, lastof(this.own_name)) &&
            //StrValid(this.isocode, lastof(this.isocode)) &&
            //StrValid(this.digit_group_separator, lastof(this.digit_group_separator)) &&
            //StrValid(this.digit_group_separator_currencyof(this.digit_group_separator_currency)) &&
            //StrValid(this.digit_decimal_separator, lastof(this.digit_decimal_separator));
        }


        /**
         * Reads the language file header and checks compatibility.
         * @param file the file to read
         * @param hdr  the place to write the header information to
         * @return true if and only if the language file is of a compatible version
         */
        public static bool ReadHeader(string file, LanguagePackHeader hdr)
        {

            var f = new FileInfo(file);
            if (f.Exists == false) { return false; }

            using (var stream = f.OpenRead())
            {
                //stream.Read(bytes, 0, ByteSize);
                using (var reader = new BinaryReader(stream))
                {
                    hdr.ident = reader.ReadUInt32();
                    hdr.version = reader.ReadUInt32();
                    hdr.name = new String(reader.ReadChars(32));
                    hdr.own_name = new String(reader.ReadChars(32));
                    hdr.isocode = new String(reader.ReadChars(16));

                    for (var i = 0; i < hdr.offsets.Length; i++)
                    {
                        hdr.offsets[i] = reader.ReadUInt16();
                    }

                    hdr.digit_group_separator = reader.ReadChars(8).ReadNullTerminatedString();
                    hdr.digit_group_separator_currency = reader.ReadChars(8).ReadNullTerminatedString();
                    hdr.digit_decimal_separator = reader.ReadChars(8).ReadNullTerminatedString();
                    hdr.missing = reader.ReadUInt16();
                    hdr.plural_form = reader.ReadByte();
                    hdr.text_dir = reader.ReadByte();
                    hdr.winlangid = reader.ReadUInt16();
                    hdr.newgrflangid = reader.ReadByte();
                    hdr.num_genders = reader.ReadByte();
                    hdr.num_cases = reader.ReadByte();

                    for (var i = 0; i < hdr.genders.Length; i++)
                    {
                        hdr.genders[i] = reader.ReadChars(Language.CASE_GENDER_LEN).ReadNullTerminatedString();
                    }
                    for (var i = 0; i < hdr.genders.Length; i++)
                    {
                        hdr.cases[i] = reader.ReadChars(Language.CASE_GENDER_LEN).ReadNullTerminatedString();
                    }
                }
            }
            
            return hdr.IsValid();
        }
    }
}
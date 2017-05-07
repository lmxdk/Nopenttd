using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nopenttd.src
{
    public class MissingGlyphSearcher
    {
        /**
         * Check whether there are glyphs missing in the current language.
         * @param Pointer to an address for storing the text pointer.
         * @return If glyphs are missing, return \c true, else return \c false.
         * @post If \c true is returned and str is not null, *str points to a string that is found to contain at least one missing glyph.
         */
        bool FindMissingGlyphs(const char** str)
        {

            InitFreeType(this.Monospace());
            const Sprite* question_mark[FS_END];

            for (FontSize size = this.Monospace() ? FS_MONO : FS_BEGIN; size<(this.Monospace()? FS_END : FS_MONO); size++) {
                question_mark[size] = GetGlyph(size, '?');
            }


            this.Reset();
            for (const char* text = this.NextString(); text != null; text = this.NextString()) {
                FontSize size = this.DefaultSize();
                if (Nopenttd.src.str != null) *Nopenttd.src.str = text;
                for (WChar c = Utf8Consume(&text); c != '\0'; c = Utf8Consume(&text))
                {
                    if (c == StringControlCode.SCC_TINYFONT)
                    {
                        size = FS_SMALL;
                    }
                    else if (c == StringControlCode.SCC_BIGFONT)
                    {
                        size = FS_LARGE;
                    }
                    else if (!IsInsideMM(c, StringControlCode.SCC_SPRITE_START, StringControlCode.SCC_SPRITE_END) && IsPrintable(c) && !IsTextDirectionChar(c) && c != '?' && GetGlyph(size, c) == question_mark[size])
                    {
                        /* The character is printable, but not in the normal font. This is the case we were testing for. */
                        return true;
                    }
                }
            }
            return false;
        }

}


/** Helper for searching through the language pack. */
}

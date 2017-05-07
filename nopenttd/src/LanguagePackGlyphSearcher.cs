namespace Nopenttd.src
{
    public class LanguagePackGlyphSearcher : MissingGlyphSearcher {
        uint i; ///< Iterator for the primary language tables.
        uint j; ///< Iterator for the secondary language tables.

/* virtual */
        void Reset()
        {
            this.i = 0;
            this.j = 0;
        }

/* virtual */
        FontSize DefaultSize()
        {
            return FS_NORMAL;
        }

/* virtual */
        const char* NextString()
        {
            if (this.i >= TAB_COUNT) return null;

            const char *ret = _langpack_offs [_langtab_start[this.i] + this.j];

            this.j++;
            while (this.i < TAB_COUNT && this.j >= _langtab_num [this.i]) {
                this.i++;
                this.j = 0;
            }

            return ret;
        }

/* virtual */
        bool Monospace()
        {
            return false;
        }

/* virtual */
        void SetFontNames(FreeTypeSettings* settings, const char* font_name)
        {
            strecpy(settings.small.font, font_nameof(settings.small.font));
            strecpy(settings.medium.font, font_nameof(settings.medium.font));
            strecpy(settings.large.font, font_nameof(settings.large.font));
        }
    };
}
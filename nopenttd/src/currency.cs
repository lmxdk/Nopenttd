/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file currency.cpp Support for different currencies. */

using Nopenttd;
using Nopenttd.Core;
using Nopenttd.src.Settings;

namespace Nopenttd
{


    /**
     * This enum gives the currencies a unique id which must be maintained for
     * savegame compatibility and in order to refer to them quickly, especially
     * for referencing the custom one.
     */
    public enum Currencies
    {
        /// British Pound
        CURRENCY_GBP,

        /// US Dollar    
        CURRENCY_USD,

        /// Euro    
        CURRENCY_EUR,

        /// Japanese Yen    
        CURRENCY_JPY,

        /// Austrian Schilling    
        CURRENCY_ATS,

        /// Belgian Franc    
        CURRENCY_BEF,

        /// Swiss Franc    
        CURRENCY_CHF,

        /// Czech Koruna    
        CURRENCY_CZK,

        /// Deutsche Mark    
        CURRENCY_DEM,

        /// Danish Krona    
        CURRENCY_DKK,

        /// Spanish Peseta    
        CURRENCY_ESP,

        /// Finish Markka    
        CURRENCY_FIM,

        /// French Franc    
        CURRENCY_FRF,

        /// Greek Drachma    
        CURRENCY_GRD,

        /// Hungarian Forint    
        CURRENCY_HUF,

        /// Icelandic Krona    
        CURRENCY_ISK,

        /// Italian Lira    
        CURRENCY_ITL,

        /// Dutch Gulden    
        CURRENCY_NLG,

        /// Norwegian Krone    
        CURRENCY_NOK,

        /// Polish Zloty    
        CURRENCY_PLN,

        /// Romenian Leu    
        CURRENCY_RON,

        /// Russian Rouble    
        CURRENCY_RUR,

        /// Slovenian Tolar    
        CURRENCY_SIT,

        /// Swedish Krona    
        CURRENCY_SEK,

        /// Turkish Lira    
        CURRENCY_YTL,

        /// Slovak Kornuna    
        CURRENCY_SKK,

        /// Brazilian Real    
        CURRENCY_BRL,

        /// Estonian Krooni    
        CURRENCY_EEK,

        /// Lithuanian Litas    
        CURRENCY_LTL,

        /// South Korean Won    
        CURRENCY_KRW,

        /// South African Rand    
        CURRENCY_ZAR,

        /// Custom currency    
        CURRENCY_CUSTOM,

        /// Georgian Lari    
        CURRENCY_GEL,

        /// Iranian Rial    
        CURRENCY_IRR,

        /// always the last item    
        CURRENCY_END,
    };



    /** Specification of a currency. */
    public struct CurrencySpec
    {
        public ushort rate;
        public string separator;//[8];
        /// %Year of switching to the Euro. May also be #CF_NOEURO or #CF_ISEURO.
        public Year to_euro;      
        public string prefix; // [16];
        public string suffix; // [16];
        /**
         * The currency symbol is represented by two possible values, prefix and suffix
         * Usage of one or the other is determined by #symbol_pos.
         * 0 = prefix
         * 1 = suffix
         * 2 = both : Special case only for custom currency.
         *            It is not a spec from Newgrf,
         *            rather a way to let users do what they want with custom currency
         */
        public byte symbol_pos;
        public StringID name;

        public CurrencySpec(ushort rate, string separator, Year toEuro, string prefix, string suffix, byte symbolPos, StringID name)
        {
            this.rate = rate;
            this.separator = separator;
            to_euro = toEuro;
            this.prefix = prefix;
            this.suffix = suffix;
            symbol_pos = symbolPos;
            this.name = name;
        }
    };

    public class Currency {
    public const int CF_NOEURO = 0;

    /// Currency never switches to the Euro (as far as known).
    public const int CF_ISEURO = 1;

    /// Currency _is_ the Euro.
    /*   exchange rate    prefix                         symbol_pos
	 *   |  separator        |           postfix             |
	 *   |   |   Euro year   |              |                | name
	 *   |   |    |          |              |                |  | */
    /** The original currency specifications. */
    public static readonly CurrencySpec[] origin_currency_specs = //new[(int)Currencies.CURRENCY_END]
    {
        new CurrencySpec(1, "", CF_NOEURO, "\xC2\xA3", "", 0, STR_GAME_OPTIONS_CURRENCY_GBP), /// british pound
        new CurrencySpec(2, "", CF_NOEURO, "$", "", 0, STR_GAME_OPTIONS_CURRENCY_USD), /// american dollar
        new CurrencySpec(2, "", CF_ISEURO, "\xE2\x82\xAC", "", 0, STR_GAME_OPTIONS_CURRENCY_EUR), /// euro
        new CurrencySpec(220, "", CF_NOEURO, "\xC2\xA5", "", 0, STR_GAME_OPTIONS_CURRENCY_JPY), /// japanese yen
        new CurrencySpec(27, "", 2002, "", NBSP "S.", 1, STR_GAME_OPTIONS_CURRENCY_ATS), /// austrian schilling
        new CurrencySpec(81, "", 2002, "BEF" NBSP, "", 0, STR_GAME_OPTIONS_CURRENCY_BEF), /// belgian franc
        new CurrencySpec(2, "", CF_NOEURO, "CHF" NBSP, "", 0, STR_GAME_OPTIONS_CURRENCY_CHF), /// swiss franc
        new CurrencySpec(41, "", CF_NOEURO, "", NBSP "K\xC4\x8D", 1, STR_GAME_OPTIONS_CURRENCY_CZK), /// czech koruna
        new CurrencySpec(4, "", 2002, "DM" NBSP, "", 0, STR_GAME_OPTIONS_CURRENCY_DEM), /// deutsche mark
        new CurrencySpec(11, "", CF_NOEURO, "", NBSP "kr", 1, STR_GAME_OPTIONS_CURRENCY_DKK), /// danish krone
        new CurrencySpec(333, "", 2002, "Pts" NBSP, "", 0, STR_GAME_OPTIONS_CURRENCY_ESP), /// spanish peseta
        new CurrencySpec(12, "", 2002, "", NBSP "mk", 1, STR_GAME_OPTIONS_CURRENCY_FIM), /// finnish markka
        new CurrencySpec(13, "", 2002, "FF" NBSP, "", 0, STR_GAME_OPTIONS_CURRENCY_FRF), /// french franc
        new CurrencySpec(681, "", 2002, "", "Dr.", 1, STR_GAME_OPTIONS_CURRENCY_GRD), /// greek drachma
        new CurrencySpec(378, "", CF_NOEURO, "", NBSP "Ft", 1, STR_GAME_OPTIONS_CURRENCY_HUF), /// hungarian forint
        new CurrencySpec(130, "", CF_NOEURO, "", NBSP "Kr", 1, STR_GAME_OPTIONS_CURRENCY_ISK), /// icelandic krona
        new CurrencySpec(3873, "", 2002, "", NBSP "L.", 1, STR_GAME_OPTIONS_CURRENCY_ITL), /// italian lira
        new CurrencySpec(4, "", 2002, "NLG" NBSP, "", 0, STR_GAME_OPTIONS_CURRENCY_NLG), /// dutch gulden
        new CurrencySpec(12, "", CF_NOEURO, "", NBSP "Kr", 1, STR_GAME_OPTIONS_CURRENCY_NOK), /// norwegian krone
        new CurrencySpec(6, "", CF_NOEURO, "", NBSP "z\xC5\x82", 1, STR_GAME_OPTIONS_CURRENCY_PLN), /// polish zloty
        new CurrencySpec(5, "", CF_NOEURO, "", NBSP "Lei", 1, STR_GAME_OPTIONS_CURRENCY_RON), /// romanian leu
        new CurrencySpec(50, "", CF_NOEURO, "", NBSP "p", 1, STR_GAME_OPTIONS_CURRENCY_RUR), /// russian rouble
        new CurrencySpec(479, "", 2007, "", NBSP "SIT", 1, STR_GAME_OPTIONS_CURRENCY_SIT), /// slovenian tolar
        new CurrencySpec(13, "", CF_NOEURO, "", NBSP "Kr", 1, STR_GAME_OPTIONS_CURRENCY_SEK), /// swedish krona
        new CurrencySpec(3, "", CF_NOEURO, "", NBSP "TL", 1, STR_GAME_OPTIONS_CURRENCY_TRY), /// turkish lira
        new CurrencySpec(60, "", 2009, "", NBSP "Sk", 1, STR_GAME_OPTIONS_CURRENCY_SKK), /// slovak koruna
        new CurrencySpec(4, "", CF_NOEURO, "R$" NBSP, "", 0, STR_GAME_OPTIONS_CURRENCY_BRL), /// brazil real
        new CurrencySpec(31, "", 2011, "", NBSP "EEK", 1, STR_GAME_OPTIONS_CURRENCY_EEK), /// estonian krooni
        new CurrencySpec(4, "", 2015, "", NBSP "Lt", 1, STR_GAME_OPTIONS_CURRENCY_LTL), /// lithuanian litas
        new CurrencySpec(1850, "", CF_NOEURO, "\xE2\x82\xA9", "", 0, STR_GAME_OPTIONS_CURRENCY_KRW), /// south korean won
        new CurrencySpec(13, "", CF_NOEURO, "R" NBSP, "", 0, STR_GAME_OPTIONS_CURRENCY_ZAR), /// south african rand
        new CurrencySpec(1, "", CF_NOEURO, "", "", 2, STR_GAME_OPTIONS_CURRENCY_CUSTOM), /// custom currency (add further languages below)
        new CurrencySpec(3, "", CF_NOEURO, "", NBSP "GEL", 1, STR_GAME_OPTIONS_CURRENCY_GEL), /// Georgian Lari
        new CurrencySpec(4901, "", CF_NOEURO, "", NBSP "Rls", 1, STR_GAME_OPTIONS_CURRENCY_IRR), /// Iranian Rial
    };

/** Array of currencies used by the system */
    public static CurrencySpec[] _currency_specs = new [Currencies.CURRENCY_END];

/**
 * This array represent the position of OpenTTD's currencies,
 * compared to TTDPatch's ones.
 * When a grf sends currencies, they are based on the order defined by TTDPatch.
 * So, we must reindex them to our own order.
 */
    public readonly byte[] TTDPatch_To_OTTDIndex =
    {
        (byte)Currencies.CURRENCY_GBP,
        (byte)Currencies.CURRENCY_USD,
        (byte)Currencies.CURRENCY_FRF,
        (byte)Currencies.CURRENCY_DEM,
        (byte)Currencies.CURRENCY_JPY,
        (byte)Currencies.CURRENCY_ESP,
        (byte)Currencies.CURRENCY_HUF,
        (byte)Currencies.CURRENCY_PLN,
        (byte)Currencies.CURRENCY_ATS,
        (byte)Currencies.CURRENCY_BEF,
        (byte)Currencies.CURRENCY_DKK,
        (byte)Currencies.CURRENCY_FIM,
        (byte)Currencies.CURRENCY_GRD,
        (byte)Currencies.CURRENCY_CHF,
        (byte)Currencies.CURRENCY_NLG,
        (byte)Currencies.CURRENCY_ITL,
        (byte)Currencies.CURRENCY_SEK,
        (byte)Currencies.CURRENCY_RUR,
        (byte)Currencies.CURRENCY_EUR,
    };

/**
 * Will return the ottd's index correspondence to
 * the ttdpatch's id.  If the id is bigger than the array,
 * it is a grf written for ottd, thus returning the same id.
 * Only called from newgrf.cpp
 * @param grfcurr_id currency id coming from newgrf
 * @return the corrected index
 */
    public static byte GetNewgrfCurrencyIdConverted(byte grfcurr_id)
    {
        return (grfcurr_id >= TTDPatch_To_OTTDIndex.Length) ? grfcurr_id : TTDPatch_To_OTTDIndex[grfcurr_id];
    }

/**
 * get a mask of the allowed currencies depending on the year
 * @return mask of currencies
 */
    public static ulong GetMaskOfAllowedCurrencies()
    {
        ulong mask = 0;
        uint i;

        for (i = 0; i < (int) Currencies.CURRENCY_END; i++)
        {
            Year to_euro = _currency_specs[i].to_euro;

            if (to_euro != CF_NOEURO && to_euro != CF_ISEURO && DateConstants._cur_year >= to_euro) continue;
            if (to_euro == CF_ISEURO && DateConstants._cur_year < 2000) continue;
            BitMath.SetBit(ref mask, i);
        }
        BitMath.SetBit(ref mask, Currencies.CURRENCY_CUSTOM); // always allow custom currency
        return mask;
    }

/**
 * Verify if the currency chosen by the user is about to be converted to Euro
 */
    public static void CheckSwitchToEuro()
    {
        if (_currency_specs[_settings_game.locale.currency].to_euro != CF_NOEURO &&
            _currency_specs[_settings_game.locale.currency].to_euro != CF_ISEURO &&
            DateConstants._cur_year >= _currency_specs[_settings_game.locale.currency].to_euro)
        {
            _settings_game.locale.currency = 2; // this is the index of euro above.
            AddNewsItem(STR_NEWS_EURO_INTRODUCTION, NT_ECONOMY, NF_NORMAL);
        }
    }



            /* XXX small hack, but makes the rest of the code a bit nicer to read */
//#define _custom_currency (_currency_specs[CURRENCY_CUSTOM])
//#define _currency ((const CurrencySpec*)&_currency_specs[GetGameSettings().locale.currency])

        /**
         * Will fill _currency_specs array with
         * default values from origin_currency_specs
         * Called only from newgrf.cpp and settings.cpp.
         * @param preserve_custom will not reset custom currency
         */
        public static void ResetCurrencies(bool preserve_custom)
    {
        for (uint i = 0; i < (uint)Currencies.CURRENCY_END; i++)
        {
            if (preserve_custom && i == (uint)Currencies.CURRENCY_CUSTOM) continue;
            _currency_specs[i] = origin_currency_specs[i];
        }
    }

    private static readonly StringID[] names = new StringID[(int) Currencies.CURRENCY_END + 1];

    /**
     * Build a list of currency names StringIDs to use in a dropdown list
     * @return Pointer to a (static) array of StringIDs
     */
    public static StringID BuildCurrencyDropdown()
    {
        /* Allow room for all currencies, plus a terminator entry */
        uint i;

        /* Add each name */
        for (i = 0; i < (uint)Currencies.CURRENCY_END; i++)
        {
            names[i] = _currency_specs[i].name;
        }
        /* Terminate the list */
        names[i] = StringConstants.INVALID_STRING_ID;

        return names;
    }
    }
}
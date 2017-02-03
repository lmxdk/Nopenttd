/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file news_type.h Types related to news. */

using System;

namespace Nopenttd
{

/** Constants in the message options window. */

    enum MessageOptionsSpace
    {
        /// Number of widgets needed for each news category, starting at widget #WID_MO_START_OPTION.
        MOS_WIDG_PER_SETTING = 4,

        /// Number of pixels between left edge of the window and the options buttons column.
        MOS_LEFT_EDGE = 6,

        /// Number of pixels between the buttons and the description columns. 
        MOS_COLUMN_SPACING = 4,

        /// Number of pixels between right edge of the window and the options descriptions column. 
        MOS_RIGHT_EDGE = 6,

        /// Additional space in the button with the option value (for better looks). 
        MOS_BUTTON_SPACE = 10,

        /// Number of vertical pixels between the categories and the global options.
        MOS_ABOVE_GLOBAL_SETTINGS = 6,

        /// Number of pixels between bottom edge of the window and bottom of the global options. 
        MOS_BOTTOM_EDGE = 6,
    }

/**
 * Type of news.
 */

    public enum NewsType
    {
        /// First vehicle arrived for company
        NT_ARRIVAL_COMPANY,

        /// First vehicle arrived for competitor
        NT_ARRIVAL_OTHER,

        /// An accident or disaster has occurred
        NT_ACCIDENT,

        /// Company info (new companies, bankruptcy messages)
        NT_COMPANY_INFO,

        /// Opening of industries
        NT_INDUSTRY_OPEN,

        /// Closing of industries
        NT_INDUSTRY_CLOSE,

        /// Economic changes (recession, industry up/dowm)
        NT_ECONOMY,

        /// Production changes of industry serviced by local company
        NT_INDUSTRY_COMPANY,

        /// Production changes of industry serviced by competitor(s)
        NT_INDUSTRY_OTHER,

        /// Other industry production changes
        NT_INDUSTRY_NOBODY,

        /// Bits of news about vehicles of the company
        NT_ADVICE,

        /// New vehicle has become available
        NT_NEW_VEHICLES,

        /// A type of cargo is (no longer) accepted
        NT_ACCEPTANCE,

        /// News about subsidies (announcements, expirations, acceptance)
        NT_SUBSIDIES,

        /// General news (from towns)
        NT_GENERAL,

        /// end-of-array marker
        NT_END,
    }

/**
 * References to objects in news.
 *
 * @warning
 * Be careful!
 * Vehicles are a special case, as news are kept when vehicles are autoreplaced/renewed.
 * You have to make sure, #ChangeVehicleNews catches the DParams of your message.
 * This is NOT ensured by the references.
 */

    public enum NewsReferenceType
    {
        /// Empty reference
        NR_NONE,

        /// Reference tile.     Scroll to tile when clicking on the news.
        NR_TILE,

        /// Reference vehicle.  Scroll to vehicle when clicking on the news. Delete news when vehicle is deleted.
        NR_VEHICLE,

        /// Reference station.  Scroll to station when clicking on the news. Delete news when station is deleted.
        NR_STATION,

        /// Reference industry. Scroll to industry when clicking on the news. Delete news when industry is deleted.
        NR_INDUSTRY,

        /// Reference town.     Scroll to town when clicking on the news.
        NR_TOWN,

        /// Reference engine.
        NR_ENGINE,
    };

/**
 * Various OR-able news-item flags.
 * @note #NF_INCOLOUR is set automatically if needed.
 */

    [Flags]
    public enum NewsFlag
    {
        /// News item is shown in colour (otherwise it is shown in black & white).
        NFB_INCOLOUR = 0,

        /// News item disables transparency in the viewport.
        NFB_NO_TRANSPARENT = 1,

        /// News item uses shaded colours.
        NFB_SHADE = 2,

        /// First bit for window layout.
        NFB_WINDOW_LAYOUT = 3,

        /// Number of bits for window layout.
        NFB_WINDOW_LAYOUT_COUNT = 3,

        /// String param 0 contains a vehicle ID. (special autoreplace behaviour)
        NFB_VEHICLE_PARAM0 = 6,

        /// Bit value for coloured news.
        NF_INCOLOUR = 1 << NFB_INCOLOUR,

        /// Bit value for disabling transparency.
        NF_NO_TRANSPARENT = 1 << NFB_NO_TRANSPARENT,

        /// Bit value for enabling shading.
        NF_SHADE = 1 << NFB_SHADE,

        /// Bit value for specifying that string param 0 contains a vehicle ID. (special autoreplace behaviour)
        NF_VEHICLE_PARAM0 = 1 << NFB_VEHICLE_PARAM0,

        /// Thin news item. (Newspaper with headline and viewport)
        NF_THIN = 0 << NFB_WINDOW_LAYOUT,

        /// Small news item. (Information window with text and viewport)
        NF_SMALL = 1 << NFB_WINDOW_LAYOUT,

        /// Normal news item. (Newspaper with text only)
        NF_NORMAL = 2 << NFB_WINDOW_LAYOUT,

        /// Vehicle news item. (new engine available)
        NF_VEHICLE = 3 << NFB_WINDOW_LAYOUT,

        /// Company news item. (Newspaper with face)
        NF_COMPANY = 4 << NFB_WINDOW_LAYOUT,
    }


/**
 * News display options
 */

    public enum NewsDisplay
    {
        /// Only show a reminder in the status bar
        ND_OFF,

        /// Show ticker
        ND_SUMMARY,

        /// Show newspaper
        ND_FULL,
    };

/**
 * Per-NewsType data
 */

    public struct NewsTypeData
    {
        readonly string name;

        /// Name
        readonly byte age;

        /// Maximum age of news items (in days)
        readonly SoundFx sound;

        /// Sound

        /**
         * Construct this entry.
         * @param name The name of the type.
         * @param age The maximum age for these messages.
         * @param sound The sound to play.
         */
        public NewsTypeData(string name, byte age, SoundFx sound)
        {
            this.name = name;
            this.age = age;
            this.sound = sound;

        }

        //NewsDisplay GetDisplay() const;
    }

/** Information about a single item of news. */

    public class NewsItem
    {
        /// Previous news item
        NewsItem prev;

        /// Next news item
        NewsItem next;

        /// Message text
        StringID string_id;

        /// Date of the news
        Date date;

        /// Type of the news
        NewsType type;

        /// NewsFlags bits @see NewsFlag
        NewsFlag flags;

        /// Type of ref1
        NewsReferenceType reftype1;

        /// Type of ref2
        NewsReferenceType reftype2;

        /// Reference 1 to some object: Used for a possible viewport, scrolling after clicking on the news, and for deleteing the news when the object is deleted.
        uint ref1;

        /// Reference 2 to some object: Used for scrolling after clicking on the news, and for deleteing the news when the object is deleted.
        uint ref2;

        /// Data to be freed when the news item has reached its end.
        //void *free_data;             

        //~NewsItem()
        //{
        //	free(this->free_data);
        //}

        ulong[] @params = new ulong[10]; /// Parameters for string resolving.
    };

/**
 * Data that needs to be stored for company news messages.
 * The problem with company news messages are the custom name
 * of the companies and the fact that the company data is reset,
 * resulting in wrong names and such.
 */

    public struct CompanyNewsInformation
    {
        string company_name;

        /// The name of the company
        string president_name;

        /// The name of the president
        string other_company_name;

        /// The name of the company taking over this one

        uint face;

        /// The face of the president
        byte colour; /// The colour related to the company

        //void FillData(ref Company c, ref Company other = null);
    }

}

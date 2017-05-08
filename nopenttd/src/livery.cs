/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file livery.h Functions/types related to livery colours. */

namespace Nopenttd
{

/** List of different livery schemes. */
    public enum LiveryScheme
    {
        LS_BEGIN = 0,
        LS_DEFAULT = 0,

        /* Rail vehicles */
        LS_STEAM,
        LS_DIESEL,
        LS_ELECTRIC,
        LS_MONORAIL,
        LS_MAGLEV,
        LS_DMU,
        LS_EMU,
        LS_PASSENGER_WAGON_STEAM,
        LS_PASSENGER_WAGON_DIESEL,
        LS_PASSENGER_WAGON_ELECTRIC,
        LS_PASSENGER_WAGON_MONORAIL,
        LS_PASSENGER_WAGON_MAGLEV,
        LS_FREIGHT_WAGON,

        /* Road vehicles */
        LS_BUS,
        LS_TRUCK,

        /* Ships */
        LS_PASSENGER_SHIP,
        LS_FREIGHT_SHIP,

        /* Aircraft */
        LS_HELICOPTER,
        LS_SMALL_PLANE,
        LS_LARGE_PLANE,

        /* Trams (appear on Road Vehicles tab) */
        LS_PASSENGER_TRAM,
        LS_FREIGHT_TRAM,

        LS_END
    };

//DECLARE_POSTFIX_INCREMENT(LiveryScheme)
    ///** Helper information for extract tool. */
//template <> struct EnumPropsT<LiveryScheme> : MakeEnumPropsT<LiveryScheme, byte, LS_BEGIN, LS_END, LS_END, 8> {};

/** List of different livery classes, used only by the livery GUI. */
    public enum LiveryClass
    {
        LC_OTHER,
        LC_RAIL,
        LC_ROAD,
        LC_SHIP,
        LC_AIRCRAFT,
        LC_END
    };

/** Information about a particular livery. */
    public struct Livery
    {
        /// Set if this livery should be used instead of the default livery.
        public bool in_use;

        /// First colour, for all vehicles.
        public byte colour1;

        /// Second colour, for vehicles with 2CC support.
        public byte colour2;


        /// Don't show the liveries at all
        public const byte LIT_NONE = 0;

        /// Show the liveries of your own company
        public const byte LIT_COMPANY = 1;

        /// Show the liveries of all companies
        public const byte LIT_ALL = 2;

        public void ResetCompanyLivery(Company c);
    };
}

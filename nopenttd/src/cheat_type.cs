/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file cheat_type.h Types related to cheating. */

namespace Nopenttd
{


/**
 * Info about each of the cheats.
 */

    public struct Cheat
    {
        /// has this cheat been used before?
        bool been_used;

        /// tells if the bool cheat is active or not
        bool value;
    }

/**
 * WARNING! Do _not_ remove entries in Cheats struct or change the order
 * of the existing ones! Would break downward compatibility.
 * Only add new entries at the end of the struct!
 */

    public struct Cheats
    {
        /// dynamite industries, objects
        Cheat magic_bulldozer;

        /// change to another company
        Cheat switch_company;

        /// get rich or poor
        Cheat money;

        /// allow tunnels that cross each other
        Cheat crossing_tunnels;

        /// empty cheat (build while in pause mode)
        Cheat dummy1;

        /// no jet will crash on small airports anymore
        Cheat no_jetcrash;

        /// empty cheat (change the climate of the map)
        Cheat dummy2;

        /// changes date ingame
        Cheat change_date;

        /// setup raw-material production in game
        Cheat setup_prod;

        /// empty cheat (enable running el-engines on normal rail)
        Cheat dummy3;

        /// edit the maximum heightlevel; this is a cheat because of the fact that it needs to reset NewGRF game state and doing so as a simple configuration breaks the expectation of many
        Cheat edit_max_hl;
    }

}

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */


/** @file clear_land.h Tables with sprites for clear land and fences. */

using Nopenttd.src;

namespace Nopenttd.Tables
{

    public static class ClearLand
    {
        public static readonly SpriteID[] _landscape_clear_sprites_rough = //[8]
        {
            Sprites.SPR_FLAT_ROUGH_LAND,
            Sprites.SPR_FLAT_ROUGH_LAND_1,
            Sprites.SPR_FLAT_ROUGH_LAND_2,
            Sprites.SPR_FLAT_ROUGH_LAND_3,
            Sprites.SPR_FLAT_ROUGH_LAND_4,
            Sprites.SPR_FLAT_ROUGH_LAND,
            Sprites.SPR_FLAT_ROUGH_LAND_1,
            Sprites.SPR_FLAT_ROUGH_LAND_2,
        };

        public static readonly byte[] _fence_mod_by_tileh_sw = //[32]
        {
            0, 2, 4, 0, 0, 2, 4, 0,
            0, 2, 4, 0, 0, 2, 4, 0,
            0, 2, 4, 0, 0, 2, 4, 4,
            0, 2, 4, 2, 0, 2, 4, 0,
        };

        public static readonly byte[] _fence_mod_by_tileh_se = //[32]
        {
            1, 1, 5, 5, 3, 3, 1, 1,
            1, 1, 5, 5, 3, 3, 1, 1,
            1, 1, 5, 5, 3, 3, 1, 5,
            1, 1, 5, 5, 3, 3, 3, 1,
        };

        public static readonly byte[] _fence_mod_by_tileh_ne = //[32]
        {
            0, 0, 0, 0, 4, 4, 4, 4,
            2, 2, 2, 2, 0, 0, 0, 0,
            0, 0, 0, 0, 4, 4, 4, 4,
            2, 2, 2, 2, 0, 2, 4, 0,
        };

        public static readonly byte[] _fence_mod_by_tileh_nw = //[32]
        {
            1, 5, 1, 5, 1, 5, 1, 5,
            3, 1, 3, 1, 3, 1, 3, 1,
            1, 5, 1, 5, 1, 5, 1, 5,
            3, 1, 3, 5, 3, 3, 3, 1,
        };


        public static readonly SpriteID[] _clear_land_fence_sprites = //[7]
        {
            Sprites.SPR_HEDGE_BUSHES,
            Sprites.SPR_HEDGE_BUSHES_WITH_GATE,
            Sprites.SPR_HEDGE_FENCE,
            Sprites.SPR_HEDGE_BLOOMBUSH_YELLOW,
            Sprites.SPR_HEDGE_BLOOMBUSH_RED,
            Sprites.SPR_HEDGE_STONE,
        };

        public static readonly SpriteID[] _clear_land_sprites_farmland = //[16] =
        {
            Sprites.SPR_FARMLAND_BARE,
            Sprites.SPR_FARMLAND_STATE_1,
            Sprites.SPR_FARMLAND_STATE_2,
            Sprites.SPR_FARMLAND_STATE_3,
            Sprites.SPR_FARMLAND_STATE_4,
            Sprites.SPR_FARMLAND_STATE_5,
            Sprites.SPR_FARMLAND_STATE_6,
            Sprites.SPR_FARMLAND_STATE_7,
            Sprites.SPR_FARMLAND_HAYPACKS,
        };

        public static readonly SpriteID[] _clear_land_sprites_snow_desert = //[8]
        {
            Sprites.SPR_FLAT_1_QUART_SNOW_DESERT_TILE,
            Sprites.SPR_FLAT_2_QUART_SNOW_DESERT_TILE,
            Sprites.SPR_FLAT_3_QUART_SNOW_DESERT_TILE,
            Sprites.SPR_FLAT_SNOW_DESERT_TILE,
        };
    }
}

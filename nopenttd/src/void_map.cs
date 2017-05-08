/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file void_map.h Map accessors for void tiles. */

using Nopenttd.Tiles;

namespace Nopenttd
{

    public static class VoidMap {
/**
 * Make a nice void tile ;)
 * @param t the tile to make void
 */
    public static void MakeVoid(this TileIndex t)
    {
        TileMap.SetTileType(t, TileType.MP_VOID);
        TileMap.SetTileHeight(t, 0);
        Map._m[t].m1 = 0;
        Map._m[t].m2 = 0;
        Map._m[t].m3 = 0;
        Map._m[t].m4 = 0;
        Map._m[t].m5 = 0;
        Map._me[t].m6 = 0;
        Map._me[t].m7 = 0;
    }

    }
}

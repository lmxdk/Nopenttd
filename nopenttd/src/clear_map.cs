/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file clearMap._map.h Map accessors for 'clear' tiles */

using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.src.Core.Exceptions;
using Nopenttd.Tiles;

namespace Nopenttd
{

/**
 * Ground types. Valid densities in comments after the enum.
 */
    public enum ClearGround
    {
        /// 0-3
        CLEAR_GRASS = 0,

        /// 3 
        CLEAR_ROUGH = 1,

        /// 3 
        CLEAR_ROCKS = 2,

        /// 3 
        CLEAR_FIELDS = 3,

        /// 0-3 
        CLEAR_SNOW = 4,

        /// 1,3 
        CLEAR_DESERT = 5,
    }

    public static class ClearMap
    {

/**
 * Test if a tile is covered with snow.
 * @param t the tile to check
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 * @return whether the tile is covered with snow.
 */
        public static bool IsSnowTile(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR));
            return BitMath.HasBit(Map._m[t].m3, 4);
        }

/**
 * Get the type of clear tile but never return CLEAR_SNOW.
 * @param t the tile to get the clear ground type of
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 * @return the ground type
 */
        public static ClearGround GetRawClearGround(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR));
            return (ClearGround) BitMath.GB(Map._m[t].m5, 2, 3);
        }

/**
 * Get the type of clear tile.
 * @param t the tile to get the clear ground type of
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 * @return the ground type
 */
        public static ClearGround GetClearGround(this TileIndex t)
        {
            if (IsSnowTile(t)) return ClearGround.CLEAR_SNOW;
            return GetRawClearGround(t);
        }

/**
 * Set the type of clear tile.
 * @param t  the tile to set the clear ground type of
 * @param ct the ground type
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 */
        public static bool IsClearGround(this TileIndex t, ClearGround ct)
        {
            return GetClearGround(t) == ct;
        }


/**
 * Get the density of a non-field clear tile.
 * @param t the tile to get the density of
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 * @return the density
 */
        public static uint GetClearDensity(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR));
            return BitMath.GB(Map._m[t].m5, 0, 2);
        }

/**
 * Increment the density of a non-field clear tile.
 * @param t the tile to increment the density of
 * @param d the amount to increment the density with
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 */
        public static void AddClearDensity(this TileIndex t, int d)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR)); // XXX incomplete
            Map._m[t].m5 += (byte)d;
        }

/**
 * Set the density of a non-field clear tile.
 * @param t the tile to set the density of
 * @param d the new density
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 */
        public static void SetClearDensity(this TileIndex t, uint d)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR));
            Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 0, 2, (byte)d);
        }


/**
 * Get the counter used to advance to the next clear density/field type.
 * @param t the tile to get the counter of
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 * @return the value of the counter
 */
        public static uint GetClearCounter(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR));
            return BitMath.GB(Map._m[t].m5, 5, 3);
        }

/**
 * Increments the counter used to advance to the next clear density/field type.
 * @param t the tile to increment the counter of
 * @param c the amount to increment the counter with
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 */
        public static void AddClearCounter(this TileIndex t, int c)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR)); // XXX incomplete
            Map._m[t].m5 += (byte)(c << 5);
        }

/**
 * Sets the counter used to advance to the next clear density/field type.
 * @param t the tile to set the counter of
 * @param c the amount to set the counter to
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 */
        public static void SetClearCounter(this TileIndex t, uint c)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR)); // XXX incomplete
            Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 5, 3, (byte)c);
        }


/**
 * Sets ground type and density in one go, also sets the counter to 0
 * @param t       the tile to set the ground type and density for
 * @param type    the new ground type of the tile
 * @param density the density of the ground tile
 * @pre TileMap.IsTileType(t, TileType.MP_CLEAR)
 */
        public static void SetClearGroundDensity(this TileIndex t, ClearGround type, uint density)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR)); // XXX incomplete
            Map._m[t].m5 = (byte)(0 << 5 | (byte)type << 2 | density);
        }


/**
 * Get the field type (production stage) of the field
 * @param t the field to get the type of
 * @pre GetClearGround(t) == CLEAR_FIELDS
 * @return the field type
 */
        public static uint GetFieldType(this TileIndex t)
        {
            Debug.Assert(GetClearGround(t) == ClearGround.CLEAR_FIELDS);
            return BitMath.GB(Map._m[t].m3, 0, 4);
        }

/**
 * Set the field type (production stage) of the field
 * @param t the field to get the type of
 * @param f the field type
 * @pre GetClearGround(t) == CLEAR_FIELDS
 */
        public static void SetFieldType(this TileIndex t, uint f)
        {
            Debug.Assert(GetClearGround(t) == ClearGround.CLEAR_FIELDS); // XXX incomplete
            Map._m[t].m3 = (byte) BitMath.SB(Map._m[t].m3, 0, 4, f);
        }

/**
 * Get the industry (farm) that made the field
 * @param t the field to get creating industry of
 * @pre GetClearGround(t) == CLEAR_FIELDS
 * @return the industry that made the field
 */
        public static IndustryID GetIndustryIndexOfField(this TileIndex t)
        {
            Debug.Assert(GetClearGround(t) == ClearGround.CLEAR_FIELDS);
            return(IndustryID) Map._m[t].m2;
        }

/**
 * Set the industry (farm) that made the field
 * @param t the field to get creating industry of
 * @param i the industry that made the field
 * @pre GetClearGround(t) == CLEAR_FIELDS
 */
        public static void SetIndustryIndexOfField(this TileIndex t, IndustryID i)
        {
            Debug.Assert(GetClearGround(t) == ClearGround.CLEAR_FIELDS);
            Map._m[t].m2 = i;
        }


/**
 * Is there a fence at the given border?
 * @param t the tile to check for fences
 * @param side the border to check
 * @pre IsClearGround(t, CLEAR_FIELDS)
 * @return 0 if there is no fence, otherwise the fence type
 */
        public static uint GetFence(this TileIndex t, DiagDirection side)
        {
            Debug.Assert(IsClearGround(t, ClearGround.CLEAR_FIELDS));
            switch (side)
            {
                default: throw new NotReachedException();
                case DiagDirection.DIAGDIR_SE: return BitMath.GB(Map._m[t].m4, 2, 3);
                case DiagDirection.DIAGDIR_SW: return BitMath.GB(Map._m[t].m4, 5, 3);
                case DiagDirection.DIAGDIR_NE: return BitMath.GB(Map._m[t].m3, 5, 3);
                case DiagDirection.DIAGDIR_NW: return BitMath.GB(Map._me[t].m6, 2, 3);
            }
        }

/**
 * Sets the type of fence (and whether there is one) for the given border.
 * @param t the tile to check for fences
 * @param side the border to check
 * @param h 0 if there is no fence, otherwise the fence type
 * @pre IsClearGround(t, CLEAR_FIELDS)
 */
        public static void SetFence(this TileIndex t, DiagDirection side, uint h)
        {
            Debug.Assert(IsClearGround(t, ClearGround.CLEAR_FIELDS));
            switch (side)
            {
                default: throw new NotReachedException();
                case DiagDirection.DIAGDIR_SE:
                    Map._m[t].m4 = (byte) BitMath.SB(Map._m[t].m4, 2, 3, h);
                    break;
                case DiagDirection.DIAGDIR_SW:
                    Map._m[t].m4 = (byte) BitMath.SB(Map._m[t].m4, 5, 3, h);
                    break;
                case DiagDirection.DIAGDIR_NE:
                    Map._m[t].m3 = (byte) BitMath.SB(Map._m[t].m3, 5, 3, h);
                    break;
                case DiagDirection.DIAGDIR_NW:
                    Map._me[t].m6 = (byte) BitMath.SB(Map._me[t].m6, 2, 3, h);
                    break;
            }
        }


/**
 * Make a clear tile.
 * @param t       the tile to make a clear tile
 * @param g       the type of ground
 * @param density the density of the grass/snow/desert etc
 */
        public static void MakeClear(this TileIndex t, ClearGround g, uint density)
        {
            TileMap.SetTileType(t, TileType.MP_CLEAR);
            Map._m[t].m1 = 0;
            TileMap.SetTileOwner(t, Owner.OWNER_NONE);
            Map._m[t].m2 = 0;
            Map._m[t].m3 = 0;
            Map._m[t].m4 = 0 << 5 | 0 << 2;
            SetClearGroundDensity(t, g, density); // Sets m5
            Map._me[t].m6 = 0;
            Map._me[t].m7 = 0;
        }


/**
 * Make a (farm) field tile.
 * @param t          the tile to make a farm field
 * @param field_type the 'growth' level of the field
 * @param industry   the industry this tile belongs to
 */
        public static void MakeField(this TileIndex t, uint field_type, IndustryID industry)
        {
            TileMap.SetTileType(t, TileType.MP_CLEAR);
            Map._m[t].m1 = 0;
            TileMap.SetTileOwner(t, Owner.OWNER_NONE);
            Map._m[t].m2 = industry;
            Map._m[t].m3 = (byte) field_type;
            Map._m[t].m4 = 0 << 5 | 0 << 2;
            SetClearGroundDensity(t, ClearGround.CLEAR_FIELDS, 3);
            Map._me[t].m6 = BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }

/**
 * Make a snow tile.
 * @param t the tile to make snowy
 * @param density The density of snowiness.
 * @pre GetClearGround(t) != CLEAR_SNOW
 */
        public static void MakeSnow(this TileIndex t, uint density = 0)
        {
            Debug.Assert(GetClearGround(t) != ClearGround.CLEAR_SNOW);
            Map._m[t].m3 = BitMath.SetBit(Map._m[t].m3, 4);
            if (GetRawClearGround(t) == ClearGround.CLEAR_FIELDS)
            {
                SetClearGroundDensity(t, ClearGround.CLEAR_GRASS, density);
            }
            else
            {
                SetClearDensity(t, density);
            }
        }

/**
 * Clear the snow from a tile and return it to its previous type.
 * @param t the tile to clear of snow
 * @pre GetClearGround(t) == CLEAR_SNOW
 */
        public static void ClearSnow(this TileIndex t)
        {
            Debug.Assert(GetClearGround(t) == ClearGround.CLEAR_SNOW);
            Map._m[t].m3 = BitMath.ClrBit(Map._m[t].m3, 4);
            SetClearDensity(t, 3);
        }
    }
}
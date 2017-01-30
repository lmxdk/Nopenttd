/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file water_map.h Map accessors for water tiles. */

using System;
using Nopenttd.Core;
using Nopenttd.src.Core.Exceptions;
using Nopenttd.Tiles;

namespace Nopenttd
{

/**
 * Bit field layout of m5 for water tiles.
 */

    public enum WaterTileTypeBitLayout
    {
        ///Start of the 'type' bitfield.
        WBL_TYPE_BEGIN = 4,

        ///Length of the 'type' bitfield.
        WBL_TYPE_COUNT = 4,

        ///Clear water or coast ('type' bitfield).
        WBL_TYPE_NORMAL = 0x0,

        ///Lock ('type' bitfield).
        WBL_TYPE_LOCK = 0x1,

        ///Depot ('type' bitfield).
        WBL_TYPE_DEPOT = 0x8,

        ///Flag for coast.
        WBL_COAST_FLAG = 0,

        ///Start of lock orientiation bitfield.
        WBL_LOCK_ORIENT_BEGIN = 0,

        ///Length of lock orientiation bitfield.
        WBL_LOCK_ORIENT_COUNT = 2,

        ///Start of lock part bitfield.
        WBL_LOCK_PART_BEGIN = 2,

        ///Length of lock part bitfield.
        WBL_LOCK_PART_COUNT = 2,

        ///Depot part flag.
        WBL_DEPOT_PART = 0,

        ///Depot axis flag.
        WBL_DEPOT_AXIS = 1,
    }

/** Available water tile types. */

    public enum WaterTileType
    {
        ///Plain water.
        WATER_TILE_CLEAR,

        ///Coast.
        WATER_TILE_COAST,

        ///Water lock.
        WATER_TILE_LOCK,

        ///Water Depot.
        WATER_TILE_DEPOT,
    }

/** classes of water (for #WATER_TILE_CLEAR water tile type). */

    public enum WaterClass
    {
        ///Sea.
        WATER_CLASS_SEA,

        ///Canal. 
        WATER_CLASS_CANAL,

        ///River. 
        WATER_CLASS_RIVER,

        ///Used for industry tiles on land (also for oilrig if newgrf says so). 
        WATER_CLASS_INVALID,
    }

/** Helper information for extract tool. */
//template <> struct EnumPropsT<WaterClass> : MakeEnumPropsT<WaterClass, byte, WATER_CLASS_SEA, WATER_CLASS_INVALID, WATER_CLASS_INVALID, 2> {};

/** Sections of the water depot. */

    public enum DepotPart
    {
        DEPOT_PART_NORTH = 0,

        ///Northern part of a depot.
        DEPOT_PART_SOUTH = 1,

        ///Southern part of a depot.
        DEPOT_PART_END = 2
    };

/** Sections of the water lock. */

    public enum LockPart
    {
        ///Middle part of a lock.
        LOCK_PART_MIDDLE = 0,

        ///Lower part of a lock.
        LOCK_PART_LOWER = 1,

        ///Upper part of a lock.
        LOCK_PART_UPPER = 2,
    };

    public class WaterMap
    {
/**
 * Get the water tile type at a tile.
 * @param t Water tile to query.
 * @return Water tile type at the tile.
 */
        //inline
        public static WaterTileType GetWaterTileType(TileIndex t)
        {
            if (TileMap.IsTileType(t, TileType.MP_WATER) == false)
                throw new ArgumentException(nameof(t), "Supplied tile must be of type MP_WATER");


            switch ((WaterTileTypeBitLayout)BitMath.GB(Map._m[t].m5, (byte)WaterTileTypeBitLayout.WBL_TYPE_BEGIN, (byte)WaterTileTypeBitLayout.WBL_TYPE_COUNT))
            {
                case WaterTileTypeBitLayout.WBL_TYPE_NORMAL:
                    return BitMath.HasBit(Map._m[t].m5, (byte)WaterTileTypeBitLayout.WBL_COAST_FLAG)
                        ? WaterTileType.WATER_TILE_COAST
                        : WaterTileType.WATER_TILE_CLEAR;
                case WaterTileTypeBitLayout.WBL_TYPE_LOCK:
                    return WaterTileType.WATER_TILE_LOCK;
                case WaterTileTypeBitLayout.WBL_TYPE_DEPOT:
                    return WaterTileType.WATER_TILE_DEPOT;
                default:
                    throw new NotReachedException();
            }
        }

        /**
         * Checks whether the tile has an waterclass associated.
         * You can then subsequently call GetWaterClass().
         * @param t Tile to query.
         * @return True if the tiletype has a waterclass.
         */
        //inline
        public static bool HasTileWaterClass(TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_WATER) || TileMap.IsTileType(t, TileType.MP_STATION) ||
                   TileMap.IsTileType(t, TileType.MP_INDUSTRY) || TileMap.IsTileType(t, TileType.MP_OBJECT);
        }

        /**
         * Get the water class at a tile.
         * @param t Water tile to query.
         * @pre IsTileType(t, MP_WATER) || IsTileType(t, MP_STATION) || IsTileType(t, MP_INDUSTRY) || IsTileType(t, MP_OBJECT)
         * @return Water class at the tile.
         */
        //inline
        public static WaterClass GetWaterClass(TileIndex t)
        {
            if (HasTileWaterClass(t) == false) throw new ArgumentException(nameof(t));
            return (WaterClass) BitMath.GB(Map._m[t].m1, 5, 2);
        }

        /**
         * Set the water class at a tile.
         * @param t  Water tile to change.
         * @param wc New water class.
         * @pre IsTileType(t, MP_WATER) || IsTileType(t, MP_STATION) || IsTileType(t, MP_INDUSTRY) || IsTileType(t, MP_OBJECT)
         */
        //inline
        public static void SetWaterClass(TileIndex t, WaterClass wc)
        {
            if (HasTileWaterClass(t) == false) throw new ArgumentException(nameof(t));
            BitMath.SB(ref Map._m[t].m1, 5, 2, (uint)wc);
        }

        /**
         * Tests if the tile was built on water.
         * @param t the tile to check
         * @pre IsTileType(t, MP_WATER) || IsTileType(t, MP_STATION) || IsTileType(t, MP_INDUSTRY) || IsTileType(t, MP_OBJECT)
         * @return true iff on water
         */
        //inline
        public static bool IsTileOnWater(TileIndex t)
        {
            return (GetWaterClass(t) != WaterClass.WATER_CLASS_INVALID);
        }

        /**
         * Is it a plain water tile?
         * @param t Water tile to query.
         * @return \c true if any type of clear water like ocean, river, or canal.
         * @pre IsTileType(t, MP_WATER)
         */
        //inline
        static bool IsWater(TileIndex t)
        {
            return GetWaterTileType(t) == WaterTileType.WATER_TILE_CLEAR;
        }

/**
 * Is it a sea water tile?
 * @param t Water tile to query.
 * @return \c true if it is a sea water tile.
 * @pre IsTileType(t, MP_WATER)
 */

        static bool IsSea(TileIndex t)
        {
            return IsWater(t) && GetWaterClass(t) == WaterClass.WATER_CLASS_SEA;
        }

/**
 * Is it a canal tile?
 * @param t Water tile to query.
 * @return \c true if it is a canal tile.
 * @pre IsTileType(t, MP_WATER)
 */
/*inline*/

        public static bool IsCanal(TileIndex t)
        {
            return IsWater(t) && GetWaterClass(t) == WaterClass.WATER_CLASS_CANAL;
        }

/**
 * Is it a river water tile?
 * @param t Water tile to query.
 * @return \c true if it is a river water tile.
 * @pre IsTileType(t, MP_WATER)
 */
/*inline*/

        public static bool IsRiver(TileIndex t)
        {
            return IsWater(t) && GetWaterClass(t) == WaterClass.WATER_CLASS_RIVER;
        }

/**
 * Is it a water tile with plain water?
 * @param t Tile to query.
 * @return \c true if it is a plain water tile.
 */
/*inline*/

        public static bool IsWaterTile(TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_WATER) && IsWater(t);
        }

/**
 * Is it a coast tile?
 * @param t Water tile to query.
 * @return \c true if it is a sea water tile.
 * @pre IsTileType(t, MP_WATER)
 */
/*inline*/

        public static bool IsCoast(TileIndex t)
        {
            return GetWaterTileType(t) == WaterTileType.WATER_TILE_COAST;
        }

/**
 * Is it a coast tile
 * @param t Tile to query.
 * @return \c true if it is a coast.
 */
/*inline*/

        public static bool IsCoastTile(TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_WATER) && IsCoast(t);
        }

/**
 * Is it a water tile with a ship depot on it?
 * @param t Water tile to query.
 * @return \c true if it is a ship depot tile.
 * @pre IsTileType(t, MP_WATER)
 */
/*inline*/

        public static bool IsShipDepot(TileIndex t)
        {
            return GetWaterTileType(t) == WaterTileType.WATER_TILE_DEPOT;
        }

/**
 * Is it a ship depot tile?
 * @param t Tile to query.
 * @return \c true if it is a ship depot tile.
 */
/*inline*/

        public static bool IsShipDepotTile(TileIndex t)
        {
            return TileMap.IsTileType(t, TileType.MP_WATER) && IsShipDepot(t);
        }

/**
 * Get the axis of the ship depot.
 * @param t Water tile to query.
 * @return Axis of the depot.
 * @pre IsShipDepotTile(t)
 */
/*inline*/

        public static Axis GetShipDepotAxis(TileIndex t)
        {
            if (IsShipDepotTile(t) == false) throw new ArgumentException(nameof(t));
            return (Axis) BitMath.GB(Map._m[t].m5, (byte) WaterTileTypeBitLayout.WBL_DEPOT_AXIS, 1);
        }

/**
 * Get the part of a ship depot.
 * @param t Water tile to query.
 * @return Part of the depot.
 * @pre IsShipDepotTile(t)
 */
/*inline*/

        public static DepotPart GetShipDepotPart(TileIndex t)
        {
            if (IsShipDepotTile(t) == false) throw new ArgumentException(nameof(t));
            return (DepotPart) BitMath.GB(Map._m[t].m5, (byte) WaterTileTypeBitLayout.WBL_DEPOT_PART, 1);
        }

/**
 * Get the direction of the ship depot.
 * @param t Water tile to query.
 * @return Direction of the depot.
 * @pre IsShipDepotTile(t)
 */
/*inline*/

        public static DiagDirection GetShipDepotDirection(TileIndex t)
        {
            return GetShipDepotAxis(t).XYNSToDiagDir((uint) GetShipDepotPart(t));
        }

/**
 * Get the other tile of the ship depot.
 * @param t Tile to query, containing one section of a ship depot.
 * @return Tile containing the other section of the depot.
 * @pre IsShipDepotTile(t)
 */
/*inline*/

        public static TileIndex GetOtherShipDepotTile(TileIndex t)
        {
            return
                (uint)
                (t +
                 (GetShipDepotPart(t) != DepotPart.DEPOT_PART_NORTH ? -1 : 1) *
                 (GetShipDepotAxis(t) != Axis.AXIS_X ? Map.TileDiffXY(0, 1) : Map.TileDiffXY(1, 0)));
        }

/**
 * Get the most northern tile of a ship depot.
 * @param t One of the tiles of the ship depot.
 * @return The northern tile of the depot.
 * @pre IsShipDepotTile(t)
 */
/*inline*/

        public static TileIndex GetShipDepotNorthTile(TileIndex t)
        {
            if (IsShipDepotTile(t) == false) throw new ArgumentException(nameof(t));
            TileIndex tile2 = GetOtherShipDepotTile(t);

            return t < tile2 ? t : tile2;
        }

/**
 * Is there a lock on a given water tile?
 * @param t Water tile to query.
 * @return \c true if it is a water lock tile.
 * @pre IsTileType(t, MP_WATER)
 */
/*inline*/

        public static bool IsLock(TileIndex t)
        {
            return GetWaterTileType(t) == WaterTileType.WATER_TILE_LOCK;
        }

/**
 * Get the direction of the water lock.
 * @param t Water tile to query.
 * @return Direction of the lock.
 * @pre IsTileType(t, MP_WATER) && IsLock(t)
 */
/*inline*/

        public static DiagDirection GetLockDirection(TileIndex t)
        {
            if (IsLock(t) == false) throw new ArgumentException(nameof(t));
            return
                (DiagDirection)
                BitMath.GB(Map._m[t].m5, (byte) WaterTileTypeBitLayout.WBL_LOCK_ORIENT_BEGIN,
                    (byte) WaterTileTypeBitLayout.WBL_LOCK_ORIENT_COUNT);
        }

/**
 * Get the part of a lock.
 * @param t Water tile to query.
 * @return The part.
 * @pre IsTileType(t, MP_WATER) && IsLock(t)
 */
/*inline*/

        public static byte GetLockPart(TileIndex t)
        {
            if (IsLock(t) == false) throw new ArgumentException(nameof(t));
            return
                (byte)
                BitMath.GB(Map._m[t].m5, (byte) WaterTileTypeBitLayout.WBL_LOCK_PART_BEGIN,
                    (byte) WaterTileTypeBitLayout.WBL_LOCK_PART_COUNT);
        }

/**
 * Get the random bits of the water tile.
 * @param t Water tile to query.
 * @return Random bits of the tile.
 * @pre IsTileType(t, MP_WATER)
 */
/*inline*/

        public static byte GetWaterTileRandomBits(TileIndex t)
        {
            if (TileMap.IsTileType(t, TileType.MP_WATER) == false) throw new ArgumentException(nameof(t));
            return Map._m[t].m4;
        }

/**
 * Checks whether the tile has water at the ground.
 * That is, it is either some plain water tile, or a object/industry/station/... with water under it.
 * @return true iff the tile has water at the ground.
 * @note Coast tiles are not considered waterish, even if there is water on a halftile.
 */
/*inline*/

        public static bool HasTileWaterGround(TileIndex t)
        {
            return HasTileWaterClass(t) && IsTileOnWater(t) && !IsCoastTile(t);
        }


/**
 * Helper function to make a coast tile.
 * @param t The tile to change into water
 */
/*inline*/

        public static void MakeShore(TileIndex t)
        {
            TileMap.SetTileType(t, TileType.MP_WATER);
            TileMap.SetTileOwner(t, Owner.OWNER_WATER);
            SetWaterClass(t, WaterClass.WATER_CLASS_SEA);
            Map._m[t].m2 = 0;
            Map._m[t].m3 = 0;
            Map._m[t].m4 = 0;
            Map._m[t].m5 = ((int) WaterTileTypeBitLayout.WBL_TYPE_NORMAL << (int) WaterTileTypeBitLayout.WBL_TYPE_BEGIN | 1 << (int) WaterTileTypeBitLayout.WBL_COAST_FLAG);
            BitMath.SB(ref Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }

/**
 * Helper function for making a watery tile.
 * @param t The tile to change into water
 * @param o The owner of the water
 * @param wc The class of water the tile has to be
 * @param random_bits Eventual random bits to be set for this tile
 */
/*inline*/

        public static void MakeWater(TileIndex t, Owner o, WaterClass wc, byte random_bits)
        {
            TileMap.SetTileType(t, TileType.MP_WATER);
            TileMap.SetTileOwner(t, o);
            SetWaterClass(t, wc);
            Map._m[t].m2 = 0;
            Map._m[t].m3 = 0;
            Map._m[t].m4 = random_bits;
            Map._m[t].m5 = (int) WaterTileTypeBitLayout.WBL_TYPE_NORMAL << (int) WaterTileTypeBitLayout.WBL_TYPE_BEGIN;
            BitMath.SB(ref Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }

/**
 * Make a sea tile.
 * @param t The tile to change into sea
 */
/*inline*/

        public static void MakeSea(TileIndex t)
        {
            MakeWater(t, Owner.OWNER_WATER, WaterClass.WATER_CLASS_SEA, 0);
        }

/**
 * Make a river tile
 * @param t The tile to change into river
 * @param random_bits Random bits to be set for this tile
 */
/*inline*/

        public static void MakeRiver(TileIndex t, byte random_bits)
        {
            MakeWater(t, Owner.OWNER_WATER, WaterClass.WATER_CLASS_RIVER, random_bits);
        }

/**
 * Make a canal tile
 * @param t The tile to change into canal
 * @param o The owner of the canal
 * @param random_bits Random bits to be set for this tile
 */
/*inline*/

        public static void MakeCanal(TileIndex t, Owner o, byte random_bits)
        {
            if (o == Owner.OWNER_WATER) throw new ArgumentException(nameof(o));
            MakeWater(t, o, WaterClass.WATER_CLASS_CANAL, random_bits);
        }

/**
 * Make a ship depot section.
 * @param t    Tile to place the ship depot section.
 * @param o    Owner of the depot.
 * @param did  Depot ID.
 * @param part Depot part (either #DEPOT_PART_NORTH or #DEPOT_PART_SOUTH).
 * @param a    Axis of the depot.
 * @param original_water_class Original water class.
 */
/*inline*/

        public static void MakeShipDepot(TileIndex t, Owner o, DepotID did, DepotPart part, Axis a,
            WaterClass original_water_class)
        {
            TileMap.SetTileType(t, TileType.MP_WATER);
            TileMap.SetTileOwner(t, o);
            SetWaterClass(t, original_water_class);
            Map._m[t].m2 = did;
            Map._m[t].m3 = 0;
            Map._m[t].m4 = 0;
            Map._m[t].m5 =
                (byte)
                ((int) WaterTileTypeBitLayout.WBL_TYPE_DEPOT << (int) WaterTileTypeBitLayout.WBL_TYPE_BEGIN |
                 (int) part << (int) WaterTileTypeBitLayout.WBL_DEPOT_PART |
                 (int) a << (int) WaterTileTypeBitLayout.WBL_DEPOT_AXIS);
            BitMath.SB(ref Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }

/**
 * Make a lock section.
 * @param t Tile to place the water lock section.
 * @param o Owner of the lock.
 * @param part Part to place.
 * @param dir Lock orientation
 * @param original_water_class Original water class.
 * @see MakeLock
 */
/*inline*/

        public static void MakeLockTile(TileIndex t, Owner o, LockPart part, DiagDirection dir,
            WaterClass original_water_class)
        {
            TileMap.SetTileType(t, TileType.MP_WATER);
            TileMap.SetTileOwner(t, o);
            SetWaterClass(t, original_water_class);
            Map._m[t].m2 = 0;
            Map._m[t].m3 = 0;
            Map._m[t].m4 = 0;
            Map._m[t].m5 =
                (byte)
                ((int) WaterTileTypeBitLayout.WBL_TYPE_LOCK << (int) WaterTileTypeBitLayout.WBL_TYPE_BEGIN |
                 (int) part << (int) WaterTileTypeBitLayout.WBL_LOCK_PART_BEGIN |
                 (int) dir << (int) WaterTileTypeBitLayout.WBL_LOCK_ORIENT_BEGIN);
            BitMath.SB(ref Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }

/**
 * Make a water lock.
 * @param t Tile to place the water lock section.
 * @param o Owner of the lock.
 * @param d Direction of the water lock.
 * @param wc_lower Original water class of the lower part.
 * @param wc_upper Original water class of the upper part.
 * @param wc_middle Original water class of the middle part.
 */
/*inline*/

        public static void MakeLock(TileIndex t, Owner o, DiagDirection d, WaterClass wc_lower, WaterClass wc_upper,
            WaterClass wc_middle)
        {
            TileIndexDiff delta = Map.TileOffsByDiagDir(d);

            /* Keep the current waterclass and owner for the tiles.
             * It allows to restore them after the lock is deleted */
            MakeLockTile(t, o, LockPart.LOCK_PART_MIDDLE, d, wc_middle);
            MakeLockTile((uint)(t - delta), IsWaterTile((uint)(t - delta)) ? TileMap.GetTileOwner((uint)(t - delta)) : o, LockPart.LOCK_PART_LOWER, d,wc_lower);
            MakeLockTile((uint)(t + delta), IsWaterTile((uint)(t + delta)) ? TileMap.GetTileOwner((uint)(t + delta)) : o, LockPart.LOCK_PART_UPPER, d,wc_upper);
        }

    }
}
/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file town_map.h Accessors for towns */

using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.Tiles;

namespace Nopenttd
{
    public static class TownMap
    {

/**
 * Get the index of which town this house/street is attached to.
 * @param t the tile
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE) or TileMap.IsTileType(t, TileType.MP_ROAD) but not a road depot
 * @return TownID
 */
        public static TownID GetTownIndex(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE) ||
                         (TileMap.IsTileType(t, TileType.MP_ROAD) && !t.IsRoadDepot()));
            return Map._m[t].m2;
        }

/**
 * Set the town index for a road or house tile.
 * @param t the tile
 * @param index the index of the town
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE) or TileMap.IsTileType(t, TileType.MP_ROAD) but not a road depot
 */
        public static void SetTownIndex(this TileIndex t, TownID index)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE) ||
                         (TileMap.IsTileType(t, TileType.MP_ROAD) && !t.IsRoadDepot()));
            Map._m[t].m2 = index;
        }

/**
 * Get the type of this house, which is an index into the house spec array
 * without doing any NewGRF related translations.
 * @param t the tile
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 * @return house type
 */
        public static HouseID GetCleanHouseType(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            return Map._m[t].m4 | (BitMath.GB(Map._m[t].m3, 6, 1) << 8);
        }

/**
 * Get the type of this house, which is an index into the house spec array
 * @param t the tile
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 * @return house type
 */
        public static HouseID GetHouseType(this TileIndex t)
        {
            return GetTranslatedHouseID(GetCleanHouseType(t));
        }

/**
 * Set the house type.
 * @param t the tile
 * @param house_id the new house type
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 */
        public static void SetHouseType(this TileIndex t, HouseID house_id)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            Map._m[t].m4 = BitMath.GB(house_id, 0, 8);
            Map._m[t].m3 = BitMath.SB(Map._m[t].m3, 6, 1, BitMath.GB(house_id, 8, 1));
        }

/**
 * Check if the lift of this animated house has a destination
 * @param t the tile
 * @return has destination
 */
        public static bool LiftHasDestination(this TileIndex t)
        {
            return BitMath.HasBit(Map._me[t].m7, 0);
        }

/**
 * Set the new destination of the lift for this animated house, and activate
 * the LiftHasDestination bit.
 * @param t the tile
 * @param dest new destination
 */
        public static void SetLiftDestination(this TileIndex t, byte dest)
        {
            Map._me[t].m7 = BitMath.SetBit(Map._me[t].m7, 0);
            Map._me[t].m7 = BitMath.SB(Map._me[t].m7, 1, 3, dest);
        }

/**
 * Get the current destination for this lift
 * @param t the tile
 * @return destination
 */
        public static byte GetLiftDestination(this TileIndex t)
        {
            return BitMath.GB(Map._me[t].m7, 1, 3);
        }

/**
 * Stop the lift of this animated house from moving.
 * Clears the first 4 bits of m7 at once, clearing the LiftHasDestination bit
 * and the destination.
 * @param t the tile
 */
        public static void HaltLift(this TileIndex t)
        {
            Map._me[t].m7 = BitMath.SB(Map._me[t].m7, 0, 4, 0);
        }

/**
 * Get the position of the lift on this animated house
 * @param t the tile
 * @return position, from 0 to 36
 */
        public static byte GetLiftPosition(this TileIndex t)
        {
            return BitMath.GB(Map._me[t].m6, 2, 6);
        }

/**
 * Set the position of the lift on this animated house
 * @param t the tile
 * @param pos position, from 0 to 36
 */
        public static void SetLiftPosition(this TileIndex t, byte pos)
        {
            Map._me[t].m6 = BitMath.SB(Map._me[t].m6, 2, 6, pos);
        }

/**
 * Get the completion of this house
 * @param t the tile
 * @return true if it is, false if it is not
 */
        public static bool IsHouseCompleted(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            return BitMath.HasBit(Map._m[t].m3, 7);
        }

/**
 * Mark this house as been completed
 * @param t the tile
 * @param status
 */
        public static void SetHouseCompleted(this TileIndex t, bool status)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            Map._m[t].m3 = BitMath.SB(Map._m[t].m3, 7, 1, !!status);
        }

/**
 * House Construction Scheme.
 *  Construction counter, for buildings under construction. Incremented on every
 *  periodic tile processing.
 *  On wraparound, the stage of building in is increased.
 *  GetHouseBuildingStage is taking care of the real stages,
 *  (as the sprite for the next phase of house building)
 *  (Get|Inc)HouseConstructionTick is simply a tick counter between the
 *  different stages
 */

/**
 * Gets the building stage of a house
 * Since the stage is used for determining what sprite to use,
 * if the house is complete (and that stage no longer is available),
 * fool the system by returning the TOWN_HOUSE_COMPLETE (3),
 * thus showing a beautiful complete house.
 * @param t the tile of the house to get the building stage of
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 * @return the building stage of the house
 */
        public static byte GetHouseBuildingStage(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            return IsHouseCompleted(t) ? (byte) TOWN_HOUSE_COMPLETED : BitMath.GB(Map._m[t].m5, 3, 2);
        }

/**
 * Gets the construction stage of a house
 * @param t the tile of the house to get the construction stage of
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 * @return the construction stage of the house
 */
        public static byte GetHouseConstructionTick(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            return IsHouseCompleted(t) ? 0 : BitMath.GB(Map._m[t].m5, 0, 3);
        }

/**
 * Sets the increment stage of a house
 * It is working with the whole counter + stage 5 bits, making it
 * easier to work:  the wraparound is automatic.
 * @param t the tile of the house to increment the construction stage of
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 */
        public static void IncHouseConstructionTick(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            Map._m[t].m5 = BitMath.AB(Map._m[t].m5, 0, 5, 1);

            if (BitMath.GB(Map._m[t].m5, 3, 2) == TOWN_HOUSE_COMPLETED)
            {
                /* House is now completed.
                 * Store the year of construction as well, for newgrf house purpose */
                SetHouseCompleted(t, true);
            }
        }

/**
 * Sets the age of the house to zero.
 * Needs to be called after the house is completed. During construction stages the map space is used otherwise.
 * @param t the tile of this house
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE) && IsHouseCompleted(t)
 */
        public static void ResetHouseAge(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE) && IsHouseCompleted(t));
            Map._m[t].m5 = 0;
        }

/**
 * Increments the age of the house.
 * @param t the tile of this house
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 */
        public static void IncrementHouseAge(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            if (IsHouseCompleted(t) && Map._m[t].m5 < 0xFF) Map._m[t].m5++;
        }

/**
 * Get the age of the house
 * @param t the tile of this house
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 * @return year
 */
        public static Year GetHouseAge(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            return IsHouseCompleted(t) ? Map._m[t].m5 : 0;
        }

/**
 * Set the random bits for this house.
 * This is required for newgrf house
 * @param t      the tile of this house
 * @param random the new random bits
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 */
        public static void SetHouseRandomBits(this TileIndex t, byte random)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            Map._m[t].m1 = random;
        }

/**
 * Get the random bits for this house.
 * This is required for newgrf house
 * @param t the tile of this house
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 * @return random bits
 */
        public static byte GetHouseRandomBits(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            return Map._m[t].m1;
        }

/**
 * Set the activated triggers bits for this house.
 * This is required for newgrf house
 * @param t        the tile of this house
 * @param triggers the activated triggers
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 */
        public static void SetHouseTriggers(this TileIndex t, byte triggers)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            BitMath.SB(Map._m[t].m3, 0, 5, triggers);
        }

/**
 * Get the already activated triggers bits for this house.
 * This is required for newgrf house
 * @param t the tile of this house
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 * @return triggers
 */
        public static byte GetHouseTriggers(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            return BitMath.GB(Map._m[t].m3, 0, 5);
        }

/**
 * Get the amount of time remaining before the tile loop processes this tile.
 * @param t the house tile
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 * @return time remaining
 */
        public static byte GetHouseProcessingTime(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            return BitMath.GB(Map._me[t].m6, 2, 6);
        }

/**
 * Set the amount of time remaining before the tile loop processes this tile.
 * @param t the house tile
 * @param time the time to be set
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 */
        public static void SetHouseProcessingTime(this TileIndex t, byte time)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            BitMath.SB(Map._me[t].m6, 2, 6, time);
        }

/**
 * Decrease the amount of time remaining before the tile loop processes this tile.
 * @param t the house tile
 * @pre TileMap.IsTileType(t, TileType.MP_HOUSE)
 */
        public static void DecHouseProcessingTime(this TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_HOUSE));
            Map._me[t].m6 -= 1 << 2;
        }

/**
 * Make the tile a house.
 * @param t tile index
 * @param tid Town index
 * @param counter of construction step
 * @param stage of construction (used for drawing)
 * @param type of house.  Index into house specs array
 * @param random_bits required for newgrf houses
 * @pre TileMap.IsTileType(t, MP_CLEAR)
 */
        public static void MakeHouseTile(this TileIndex t, ushort tid, byte counter, byte stage, HouseID type,
            byte random_bits)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_CLEAR));

            TileMap.SetTileType(t, TileType.MP_HOUSE);
            Map._m[t].m1 = random_bits;
            Map._m[t].m2 = tid;
            Map._m[t].m3 = 0;
            SetHouseType(t, type);
            SetHouseCompleted(t, stage == TOWN_HOUSE_COMPLETED);
            Map._m[t].m5 = IsHouseCompleted(t) ? 0 : (stage << 3 | counter);
            SetAnimationFrame(t, 0);
            SetHouseProcessingTime(t, HouseSpec.Get(type).processing_time);
        }

    }
}
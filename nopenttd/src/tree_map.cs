/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file tree_map.h Map accessors for tree tiles. */

using System.Diagnostics;
using Nopenttd.Core;
using Nopenttd.Tiles;

namespace Nopenttd
{
/**
 * List of tree types along all landscape types.
 *
 * This enumeration contains a list of the different tree types along
 * all landscape types. The values for the enumerations may be used for
 * offsets from the grfs files. These points to the start of
 * the tree list for a landscape. See the TREE_COUNT_* enumerations
 * for the amount of different trees for a specific landscape.
 */
    public enum TreeType
    {
        /// temperate tree
        TREE_TEMPERATE = 0x00,

        /// tree on a sub_arctic landscape
        TREE_SUB_ARCTIC = 0x0C,

        /// tree on the 'green part' on a sub-tropical map
        TREE_RAINFOREST = 0x14,

        /// a cactus for the 'desert part' on a sub-tropical map
        TREE_CACTUS = 0x1B,

        /// tree on a sub-tropical map, non-rainforest, non-desert
        TREE_SUB_TROPICAL = 0x1C,

        /// tree on a toyland map
        TREE_TOYLAND = 0x20,

        /// An invalid tree
        TREE_INVALID = 0xFF,
    };

/**
 * Enumeration for ground types of tiles with trees.
 *
 * This enumeration defines the ground types for tiles with trees on it.
 */
    public enum TreeGround
    {
        /// normal grass
        TREE_GROUND_GRASS = 0,

        /// some rough tile
        TREE_GROUND_ROUGH = 1,

        /// a desert or snow tile, depend on landscape
        TREE_GROUND_SNOW_DESERT = 2,

        /// shore
        TREE_GROUND_SHORE = 3,

        /// A snow tile that is rough underneath.
        TREE_GROUND_ROUGH_SNOW = 4,
    };

    public static class TreeMap
    {
        /* Counts the number of tree types for each landscape.
         *
         * This list contains the counts of different tree types for each landscape. This list contains
         * 5 entries instead of 4 (as there are only 4 landscape types) as the sub tropic landscape
         * has two types of area, one for normal trees and one only for cacti.
         */
        /// number of tree types on a temperate map.
        public const uint TREE_COUNT_TEMPERATE = TreeType.TREE_SUB_ARCTIC - TreeType.TREE_TEMPERATE;

        /// number of tree types on a sub arctic map.
        public const uint TREE_COUNT_SUB_ARCTIC = TreeType.TREE_RAINFOREST - TreeType.TREE_SUB_ARCTIC;

        /// number of tree types for the 'rainforest part' of a sub-tropic map.
        public const uint TREE_COUNT_RAINFOREST = TreeType.TREE_CACTUS - TreeType.TREE_RAINFOREST;

        /// number of tree types for the 'sub-tropic part' of a sub-tropic map.
        public const uint TREE_COUNT_SUB_TROPICAL = TreeType.TREE_TOYLAND - TreeType.TREE_SUB_TROPICAL;

        /// number of tree types on a toyland map.
        public const uint TREE_COUNT_TOYLAND = 9;



/**
 * Returns the treetype of a tile.
 *
 * This function returns the treetype of a given tile. As there are more
 * possible treetypes for a tile in a game as the enumeration #TreeType defines
 * this function may be return a value which isn't catch by an entry of the
 * enumeration #TreeType. But there is no problem known about it.
 *
 * @param t The tile to get the treetype from
 * @return The treetype of the given tile with trees
 * @pre Tile t must be of type TileType.MP_TREES)
 */
        public static TreeType GetTreeType(TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES));
            return (TreeType) Map._m[t].m3;
        }

/**
 * Returns the groundtype for tree tiles.
 *
 * This function returns the groundtype of a tile with trees.
 *
 * @param t The tile to get the groundtype from
 * @return The groundtype of the tile
 * @pre Tile must be of type MP_TREES
 */
        public static TreeGround GetTreeGround(TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES));
            return (TreeGround) BitMath.GB(Map._m[t].m2, 6, 3);
        }

/**
 * Returns the 'density' of a tile with trees.
 *
 * This function returns the density of a tile which got trees. Note
 * that this value doesn't count the number of trees on a tile, use
 * #GetTreeCount instead. This function instead returns some kind of
 * groundtype of the tile. As the map-array is finite in size and
 * the informations about the trees must be saved somehow other
 * informations about a tile must be saved somewhere encoded in the
 * tile. So this function returns the density of a tile for sub arctic
 * and sub tropical games. This means for sub arctic the type of snowline
 * (0 to 3 for all 4 types of snowtiles) and for sub tropical the value
 * 3 for a desert (and 0 for non-desert). The function name is not read as
 * "get the tree density of a tile" but "get the density of a tile which got trees".
 *
 * @param t The tile to get the 'density'
 * @pre Tile must be of type TileType.MP_TREES)
 * @see GetTreeCount
 */
        public static uint GetTreeDensity(TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES));
            return BitMath.GB(Map._m[t].m2, 4, 2);
        }

/**
 * Set the density and ground type of a tile with trees.
 *
 * This functions saves the ground type and the density which belongs to it
 * for a given tile.
 *
 * @param t The tile to set the density and ground type
 * @param g The ground type to save
 * @param d The density to save with
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static void SetTreeGroundDensity(TileIndex t, TreeGround g, uint d)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES)); // XXX incomplete
            Map._m[t].m2 = BitMath.SB(Map._m[t].m2, 4, 2, d);
            Map._m[t].m2 = BitMath.SB(Map._m[t].m2, 6, 3, g);
        }

/**
 * Returns the number of trees on a tile.
 *
 * This function returns the number of trees of a tile (1-4).
 * The tile must be contains at least one tree or be more specific: it must be
 * of type TileType.MP_TREES).
 *
 * @param t The index to get the number of trees
 * @return The number of trees (1-4)
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static uint GetTreeCount(TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES));
            return BitMath.GB(Map._m[t].m5, 6, 2) + 1;
        }

/**
 * Add a amount to the tree-count value of a tile with trees.
 *
 * This function add a value to the tree-count value of a tile. This
 * value may be negative to reduce the tree-counter. If the resulting
 * value reach 0 it doesn't get converted to a "normal" tile.
 *
 * @param t The tile to change the tree amount
 * @param c The value to add (or reduce) on the tree-count value
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static void AddTreeCount(TileIndex t, int c)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES)); // XXX incomplete
            Map._m[t].m5 += c << 6;
        }

/**
 * Returns the tree growth status.
 *
 * This function returns the tree growth status of a tile with trees.
 *
 * @param t The tile to get the tree growth status
 * @return The tree growth status
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static uint GetTreeGrowth(TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES));
            return BitMath.GB(Map._m[t].m5, 0, 3);
        }

/**
 * Add a value to the tree growth status.
 *
 * This function adds a value to the tree grow status of a tile.
 *
 * @param t The tile to add the value on
 * @param a The value to add on the tree growth status
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static void AddTreeGrowth(TileIndex t, int a)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES)); // XXX incomplete
            Map._m[t].m5 += a;
        }

/**
 * Sets the tree growth status of a tile.
 *
 * This function sets the tree growth status of a tile directly with
 * the given value.
 *
 * @param t The tile to change the tree growth status
 * @param g The new value
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static void SetTreeGrowth(TileIndex t, uint g)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES)); // XXX incomplete
            Map._m[t].m5 = BitMath.SB(Map._m[t].m5, 0, 3, g);
        }

/**
 * Get the tick counter of a tree tile.
 *
 * Returns the saved tick counter of a given tile.
 *
 * @param t The tile to get the counter value from
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static uint GetTreeCounter(TileIndex t)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES));
            return BitMath.GB(Map._m[t].m2, 0, 4);
        }

/**
 * Add a value on the tick counter of a tree-tile
 *
 * This function adds a value on the tick counter of a tree-tile.
 *
 * @param t The tile to add the value on
 * @param a The value to add on the tick counter
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static void AddTreeCounter(TileIndex t, int a)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES)); // XXX incomplete
            Map._m[t].m2 += a;
        }

/**
 * Set the tick counter for a tree-tile
 *
 * This function sets directly the tick counter for a tree-tile.
 *
 * @param t The tile to set the tick counter
 * @param c The new tick counter value
 * @pre Tile must be of type TileType.MP_TREES)
 */
        public static void SetTreeCounter(TileIndex t, uint c)
        {
            Debug.Assert(TileMap.IsTileType(t, TileType.MP_TREES)); // XXX incomplete
            Map._m[t].m2 = BitMath.SB(Map._m[t].m2, 0, 4, c);
        }

/**
 * Make a tree-tile.
 *
 * This functions change the tile to a tile with trees and all informations which belongs to it.
 *
 * @param t The tile to make a tree-tile from
 * @param type The type of the tree
 * @param count the number of trees
 * @param growth the growth status
 * @param ground the ground type
 * @param density the density (not the number of trees)
 */
        public static void MakeTree(TileIndex t, TreeType type, uint count, uint growth, TreeGround ground,
            uint density)
        {
            TileMap.SetTileType(t, TileType.MP_TREES);
            TileMap.SetTileOwner(t, Owner.OWNER_NONE);
            Map._m[t].m2 = (ushort) (ground << 6 | density << 4 | 0);
            Map._m[t].m3 = (byte) type;
            Map._m[t].m4 = 0 << 5 | 0 << 2;
            Map._m[t].m5 = (byte) (count << 6 | growth);
            BitMath.SB(Map._me[t].m6, 2, 4, 0);
            Map._me[t].m7 = 0;
        }
    }
}
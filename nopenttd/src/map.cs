/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file map.cpp Base functions related to the map and distances on them. */

using System;
using System.Reflection;
using NLog;
using NLog.Fluent;
using Nopenttd;
using Nopenttd.Core;
using Nopenttd.src.Core.Exceptions;
using Nopenttd.src.Settings;
using Nopenttd.Tiles;

namespace Nopenttd
{
    public class Map
    {
        private static readonly ILogger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);

        /// 2^_map_log_x == _map_size_x
        private static uint _map_log_x;

        /// 2^_map_log_y == _map_size_y
        private static uint _map_log_y;

        /// Size of the map along the X
        private static uint _map_size_x;

        /// Size of the map along the Y
        private static uint _map_size_y;

        /// The number of tiles on the map
        private static uint _map_size;

        /// _map_size - 1 (to mask the mapsize)
        private static uint _map_tile_mask;

        /// Tiles of the map
        public static Tile[] _m = null;

        /// Extended Tiles of the map 
        public static TileExtended[] _me = null;



        /**
         * 'Wraps' the given tile to it is within the map. It does
         * 'Wraps' the given tile to it is within the map. It does
         * this by masking the 'high' bits of.
         * @param x the tile to 'wrap'
         */
        public static uint TILE_MASK(uint x) => x & _map_tile_mask;

        /**
         * Logarithm of the map size along the X side.
         * @note try to avoid using this one
         * @return 2^"return value" == MapSizeX()
         */
        //inline
        public static uint MapLogX() => _map_log_x;

        /**
         * Logarithm of the map size along the y side.
         * @note try to avoid using this one
         * @return 2^"return value" == MapSizeY()
         */
        //inline
        public static uint MapLogY() => _map_log_y;

        /**
         * Get the size of the map along the X
         * @return the number of tiles along the X of the map
         */
        //inline 
        public static uint MapSizeX() => _map_size_x;


        /**
         * Get the size of the map along the Y
         * @return the number of tiles along the Y of the map
         */
        //inline 
        public static uint MapSizeY() => _map_size_y;

        /**
         * Get the size of the map
         * @return the number of tiles of the map
         */
        //inline
        public static uint MapSize() => _map_size;

        /**
         * Gets the maximum X coordinate within the map, including MP_VOID
         * @return the maximum X coordinate
         */
        //inline
        public static uint MapMaxX() => MapSizeX() - 1;

        /**
         * Gets the maximum Y coordinate within the map, including MP_VOID
         * @return the maximum Y coordinate
         */
        //inline
        public static uint MapMaxY() => MapSizeY() - 1;


        /**
         * (Re)allocates a map with the given dimension
         * @param size_x the width of the map along the NE/SW edge
         * @param size_y the 'height' of the map along the SE/NW edge
         */

        public static void AllocateMap(uint size_x, uint size_y)
        {
            /* Make sure that the map size is within the limits and that
             * size of both axes is a power of 2. */
            if (!MathFuncs.IsInsideMM((int) size_x, TileConstants.MIN_MAP_SIZE, TileConstants.MAX_MAP_SIZE + 1) ||
                !MathFuncs.IsInsideMM((int) size_y, TileConstants.MIN_MAP_SIZE, TileConstants.MAX_MAP_SIZE + 1) ||
                (size_x & (size_x - 1)) != 0 ||
                (size_y & (size_y - 1)) != 0)
            {
                throw new Exception("Invalid map size");
            }

            Log.Error($"Allocating map of size {size_x}x{size_y}");

            _map_log_x = BitMath.FindFirstBit(size_x);
            _map_log_y = BitMath.FindFirstBit(size_y);
            _map_size_x = size_x;
            _map_size_y = size_y;
            _map_size = size_x * size_y;
            _map_tile_mask = _map_size - 1;

            _m = new Tile[_map_size];
            _me = new TileExtended[_map_size];
        }


//#ifdef _DEBUG
//TileIndex TileAdd(TileIndex tile, TileIndexDiff add, const char *exp, const char *file, int line)
//{
//	int dx;
//	int dy;
//	uint x;
//	uint y;

//	dx = add & MapMaxX();
//	if (dx >= (int)MapSizeX() / 2) dx -= MapSizeX();
//	dy = (add - dx) / (int)MapSizeX();

//	x = TileX(tile) + dx;
//	y = TileY(tile) + dy;

//	if (x >= MapSizeX() || y >= MapSizeY()) {
//		char buf[512];

//		seprintf(buf, lastof(buf), "TILE_ADD(%s) when adding 0x%.4X and 0x%.4X failed", exp, tile, add);
//#if !defined(_MSC_VER) || defined(WINCE)
//		fprintf(stderr, "%s:%d %s\n", file, line, buf);
//#else
//		_assert(buf, (char*)file, line);
//#endif
//	}

//	assert(TileXY(x, y) == TILE_MASK(tile + add));

//	return TileXY(x, y);
//}
//#endif

/**
 * This function checks if we add addx/addy to tile, if we
 * do wrap around the edges. For example, tile = (10,2) and
 * addx = +3 and addy = -4. This function will now return
 * INVALID_TILE, because the y is wrapped. This is needed in
 * for example, farmland. When the tile is not wrapped,
 * the result will be tile + TileDiffXY(addx, addy)
 *
 * @param tile the 'starting' point of the adding
 * @param addx the amount of tiles in the X direction to add
 * @param addy the amount of tiles in the Y direction to add
 * @return translated tile, or INVALID_TILE when it would've wrapped.
 */

        public static TileIndex TileAddWrap(TileIndex tile, int addx, int addy)
        {
            uint x = (uint) (TileX(tile) + addx);
            uint y = (uint) (TileY(tile) + addy);

            /* Disallow void tiles at the north border. */
            if ((x == 0 || y == 0) && _settings_game.construction.freeform_edges) return TileConstants.INVALID_TILE;

            /* Are we about to wrap? */
            if (x >= MapMaxX() || y >= MapMaxY()) return TileConstants.INVALID_TILE;

            return TileXY(x, y);
        }

/** 'Lookup table' for tile offsets given a DiagDirection */

        public static readonly TileIndexDiffC[] _tileoffs_by_diagdir =
        {
            new TileIndexDiffC(-1, 0), /// DIAGDIR_NE
            new TileIndexDiffC(0, 1), /// DIAGDIR_SE
            new TileIndexDiffC(1, 0), /// DIAGDIR_SW
            new TileIndexDiffC(0, -1) /// DIAGDIR_NW
        };

/** 'Lookup table' for tile offsets given a Direction */

        public static readonly TileIndexDiffC[] _tileoffs_by_dir =
        {
            new TileIndexDiffC(-1, -1), /// DIR_N
            new TileIndexDiffC(-1, 0), /// DIR_NE
            new TileIndexDiffC(-1, 1), /// DIR_E
            new TileIndexDiffC(0, 1), /// DIR_SE
            new TileIndexDiffC(1, 1), /// DIR_S
            new TileIndexDiffC(1, 0), /// DIR_SW
            new TileIndexDiffC(1, -1), /// DIR_W
            new TileIndexDiffC(0, -1) /// DIR_NW
        };

/**
 * Gets the Manhattan distance between the two given tiles.
 * The Manhattan distance is the sum of the delta of both the
 * X and Y component.
 * Also known as L1-Norm
 * @param t0 the start tile
 * @param t1 the end tile
 * @return the distance
 */

        public static uint DistanceManhattan(TileIndex t0, TileIndex t1)
        {
            var dx = (uint) MathFuncs.Delta(TileX(t0), TileX(t1));
            var dy = (uint) MathFuncs.Delta(TileY(t0), TileY(t1));
            return dx + dy;
        }


/**
 * Gets the 'Square' distance between the two given tiles.
 * The 'Square' distance is the square of the shortest (straight line)
 * distance between the two tiles.
 * Also known as euclidian- or L2-Norm squared.
 * @param t0 the start tile
 * @param t1 the end tile
 * @return the distance
 */

        public static uint DistanceSquare(TileIndex t0, TileIndex t1)
        {
            int dx = (int) (TileX(t0) - TileX(t1));
            int dy = (int) (TileY(t0) - TileY(t1));
            return (uint) (dx * dx + dy * dy);
        }


/**
 * Gets the biggest distance component (x or y) between the two given tiles.
 * Also known as L-Infinity-Norm.
 * @param t0 the start tile
 * @param t1 the end tile
 * @return the distance
 */

        public static uint DistanceMax(TileIndex t0, TileIndex t1)
        {
            uint dx = MathFuncs.Delta(TileX(t0), TileX(t1));
            uint dy = MathFuncs.Delta(TileY(t0), TileY(t1));
            return Math.Max(dx, dy);
        }


/**
 * Gets the biggest distance component (x or y) between the two given tiles
 * plus the Manhattan distance, i.e. two times the biggest distance component
 * and once the smallest component.
 * @param t0 the start tile
 * @param t1 the end tile
 * @return the distance
 */

        public static uint DistanceMaxPlusManhattan(TileIndex t0, TileIndex t1)
        {
            uint dx = (uint) MathFuncs.Delta(TileX(t0), TileX(t1));
            uint dy = (uint) MathFuncs.Delta(TileY(t0), TileY(t1));
            return dx > dy ? 2 * dx + dy : 2 * dy + dx;
        }

/**
 * Param the minimum distance to an edge
 * @param tile the tile to get the distance from
 * @return the distance from the edge in tiles
 */

        public static uint DistanceFromEdge(TileIndex tile)
        {
            uint xl = TileX(tile);
            uint yl = TileY(tile);
            uint xh = MapSizeX() - 1 - xl;
            uint yh = MapSizeY() - 1 - yl;
            uint minl = Math.Min(xl, yl);
            uint minh = Math.Min(xh, yh);
            return Math.Min(minl, minh);
        }

/**
 * Gets the distance to the edge of the map in given direction.
 * @param tile the tile to get the distance from
 * @param dir the direction of interest
 * @return the distance from the edge in tiles
 */

        public static uint DistanceFromEdgeDir(TileIndex tile, DiagDirection dir)
        {
            switch (dir)
            {
                case DiagDirection.DIAGDIR_NE:
                    return (uint) (TileX(tile) - (_settings_game.construction.freeform_edges ? 1 : 0));
                case DiagDirection.DIAGDIR_NW:
                    return (uint) (TileY(tile) - (_settings_game.construction.freeform_edges ? 1 : 0));
                case DiagDirection.DIAGDIR_SW:
                    return MapMaxX() - TileX(tile) - 1;
                case DiagDirection.DIAGDIR_SE:
                    return MapMaxY() - TileY(tile) - 1;
                default:
                    throw new NotReachedException();
            }
        }


/**
 * A callback function type for searching tiles.
 *
 * @param tile The tile to test
 * @param user_data additional data for the callback function to use
 * @return A boolean value, depend on the definition of the function.
 */

        public delegate bool TestTileOnSearchProc<T>(ref TileIndex tile, T userData);

/**
 * Function performing a search around a center tile and going outward, thus in circle.
 * Although it really is a square search...
 * Every tile will be tested by means of the callback function proc,
 * which will determine if yes or no the given tile meets criteria of search.
 * @param tile to start the search from. Upon completion, it will return the tile matching the search
 * @param size: number of tiles per side of the desired search area
 * @param proc: callback testing function pointer.
 * @param user_data to be passed to the callback function. Depends on the implementation
 * @return result of the search
 * @pre proc != NULL
 * @pre size > 0
 */

        public static bool CircularTileSearch<T>(ref TileIndex tile, uint size, TestTileOnSearchProc<T> proc, T userData)
        {
            if (proc == null) throw new ArgumentNullException(nameof(proc));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Must be greater than zero");

            if (size % 2 == 1)
            {
                /* If the length of the side is uneven, the center has to be checked
                 * separately, as the pattern of uneven sides requires to go around the center */
                if (proc(ref tile, userData)) return true;

                /* If tile test is not successful, get one tile up,
                 * ready for a test in first circle around center tile */
                tile = TILE_ADD(tile, (uint) (int) TileOffsByDir(Direction.DIR_N));
                return CircularTileSearch(ref tile, size / 2, 1, 1, proc, userData);
            }
            else
            {
                return CircularTileSearch(ref tile, size / 2, 0, 0, proc, userData);
            }
        }

/**
 * Generalized circular search allowing for rectangles and a hole.
 * Function performing a search around a center rectangle and going outward.
 * The center rectangle is left out from the search. To do a rectangular search
 * without a hole, set either h or w to zero.
 * Every tile will be tested by means of the callback function proc,
 * which will determine if yes or no the given tile meets criteria of search.
 * @param tile to start the search from. Upon completion, it will return the tile matching the search.
 *  This tile should be directly north of the hole (if any).
 * @param radius How many tiles to search outwards. Note: This is a radius and thus different
 *                from the size parameter of the other CircularTileSearch function, which is a diameter.
 * @param w the width of the inner rectangle
 * @param h the height of the inner rectangle
 * @param proc callback testing function pointer.
 * @param user_data to be passed to the callback function. Depends on the implementation
 * @return result of the search
 * @pre proc != NULL
 * @pre radius > 0
 */

        public static bool CircularTileSearch<T>(ref TileIndex tile, uint radius, uint w, uint h, TestTileOnSearchProc<T> proc,
            T userData)
        {
            if (proc == null) throw new ArgumentNullException(nameof(proc));
            if (radius <= 0) throw new ArgumentOutOfRangeException(nameof(radius), "Must be greater than zero");

            uint x = TileX(tile) + w + 1;
            uint y = TileY(tile);

            uint[] extent = new uint[(int) DiagDirection.DIAGDIR_END] {w, h, w, h};

            for (uint n = 0; n < radius; n++)
            {
                for (var dir = (int) DiagDirection.DIAGDIR_BEGIN; dir < (int) DiagDirection.DIAGDIR_END; dir++)
                {
                    /* Is the tile within the map? */
                    for (uint j = extent[dir] + n * 2 + 1; j != 0; j--)
                    {
                        if (x < MapSizeX() && y < MapSizeY())
                        {
                            TileIndex t = TileXY(x, y);
                            /* Is the callback successful? */
                            if (proc(ref t, userData))
                            {
                                /* Stop the search */
                                tile = t;
                                return true;
                            }
                        }

                        /* Step to the next 'neighbour' in the circular line */
                        x += (uint) _tileoffs_by_diagdir[dir].x;
                        y += (uint) _tileoffs_by_diagdir[dir].y;
                    }
                }
                /* Jump to next circle to test */
                x += (uint) _tileoffs_by_dir[(int) Direction.DIR_W].x;
                y += (uint) _tileoffs_by_dir[(int) Direction.DIR_W].y;
            }

            tile = TileConstants.INVALID_TILE;
            return false;
        }

        private static readonly int[] ddx = new int[(int) DiagDirection.DIAGDIR_END] {-1, 1, 1, -1};
        private static readonly int[] ddy = new int[(int) DiagDirection.DIAGDIR_END] {1, 1, -1, -1};

/**
 * Finds the distance for the closest tile with water/land given a tile
 * @param tile  the tile to find the distance too
 * @param water whether to find water or land
 * @return distance to nearest water (max 0x7F) / land (max 0x1FF; 0x200 if there is no land)
 */

        uint GetClosestWaterDistance(TileIndex tile, bool water)
        {
            if (WaterMap.HasTileWaterGround(tile) == water) return 0;

            uint max_dist = (uint) (water ? 0x7F : 0x200);

            int x = (int) TileX(tile);
            int y = (int) TileY(tile);

            uint max_x = MapMaxX();
            uint max_y = MapMaxY();
            uint min_xy = (uint) (_settings_game.construction.freeform_edges ? 1 : 0);

            /* go in a 'spiral' with increasing manhattan distance in each iteration */
            for (uint dist = 1; dist < max_dist; dist++)
            {
                /* next 'diameter' */
                y--;

                /* going counter-clockwise around this square */
                for (var dir = (int) DiagDirection.DIAGDIR_BEGIN; dir < (int) DiagDirection.DIAGDIR_END; dir++)
                {

                    int dx = ddx[dir];
                    int dy = ddy[dir];

                    /* each side of this square has length 'dist' */
                    for (uint a = 0; a < dist; a++)
                    {
                        /* MP_VOID tiles are not checked (interval is [min; max) for IsInsideMM())*/
                        if (MathFuncs.IsInsideMM(x, min_xy, max_x) && MathFuncs.IsInsideMM(y, min_xy, max_y))
                        {
                            TileIndex t = TileXY((uint)x, (uint)y);
                            if (WaterMap.HasTileWaterGround(t) == water) return dist;
                        }
                        x += dx;
                        y += dy;
                    }
                }
            }

            if (!water)
            {
                /* no land found - is this a water-only map? */
                for (TileIndex t = 0; t < MapSize(); t++)
                {
                    if (!TileMap.IsTileType(t, TileType.MP_VOID) && !TileMap.IsTileType(t, TileType.MP_WATER)) return 0x1FF;
                }
            }

            return max_dist;
        }
















/**
 * Scales the given value by the map size, where the given value is
 * for a 256 by 256 map.
 * @param n the value to scale
 * @return the scaled size
 */
//inline
        public static uint ScaleByMapSize(uint n)
        {
            /* Subtract 12 from shift in order to prevent integer overflow
             * for large values of n. It's safe since the min mapsize is 64x64. */
            return MathFuncs.CeilDiv(n << (int) (MapLogX() + MapLogY() - 12), 1 << 4);
        }


/**
 * Scales the given value by the maps circumference, where the given
 * value is for a 256 by 256 map
 * @param n the value to scale
 * @return the scaled size
 */
//inline
        public static uint ScaleByMapSize1D(uint n)
        {
            /* Normal circumference for the X+Y is 256+256 = 1<<9
             * Note, not actually taking the full circumference into account,
             * just half of it. */
            return MathFuncs.CeilDiv((n << (int) MapLogX()) + (n << (int) MapLogY()), 1 << 9);
        }



/**
 * Returns the TileIndex of a coordinate.
 *
 * @param x The x coordinate of the tile
 * @param y The y coordinate of the tile
 * @return The TileIndex calculated by the coordinate
 */
//inline
        public static TileIndex TileXY(uint x, uint y)
        {
            return (y << (int) MapLogX()) + x;
        }

/**
 * Calculates an offset for the given coordinate(-offset).
 *
 * This function calculate an offset value which can be added to an
 * #TileIndex. The coordinates can be negative.
 *
 * @param x The offset in x direction
 * @param y The offset in y direction
 * @return The resulting offset value of the given coordinate
 * @see ToTileIndexDiff(TileIndexDiffC)
 */
//inline
        public static TileIndexDiff TileDiffXY(int x, int y)
        {
            /* Multiplication gives much better optimization on MSVC than shifting.
             * 0 << shift isn't optimized to 0 properly.
             * Typically x and y are constants, and then this doesn't result
             * in any actual multiplication in the assembly code.. */
            return (TileIndexDiff) (y * MapSizeX()) + x;
        }

/**
 * Get a tile from the virtual XY-coordinate.
 * @param x The virtual x coordinate of the tile.
 * @param y The virtual y coordinate of the tile.
 * @return The TileIndex calculated by the coordinate.
 */
//inline
        public static TileIndex TileVirtXY(uint x, uint y)
        {
            return (y >> 4 << (int) MapLogX()) + (x >> 4);
        }


/**
 * Get the X component of a tile
 * @param tile the tile to get the X component of
 * @return the X component
 */
//inline
        public static uint TileX(TileIndex tile)
        {
            return tile & MapMaxX();
        }

/**
 * Get the Y component of a tile
 * @param tile the tile to get the Y component of
 * @return the Y component
 */
//inline
        public static uint TileY(TileIndex tile)
        {
            return tile >> (int) MapLogX();
        }

/**
 * Return the offset between to tiles from a TileIndexDiffC struct.
 *
 * This function works like #TileDiffXY(int, int) and returns the
 * difference between two tiles.
 *
 * @param tidc The coordinate of the offset as TileIndexDiffC
 * @return The difference between two tiles.
 * @see TileDiffXY(int, int)
 */
//inline
        public static TileIndexDiff ToTileIndexDiff(TileIndexDiffC tidc)
        {
            return (tidc.y << (int) MapLogX()) + tidc.x;
        }

        /**
         * Adds to tiles together.
         *
         * @param x One tile
         * @param y Another tile to add
         * @return The resulting tile(index)
         */
        public static TileIndex TILE_ADD(uint x, uint y) => x + y;

        /**
         * Adds a given offset to a tile.
         *
         * @param tile The tile to add an offset on it
         * @param x The x offset to add to the tile
         * @param y The y offset to add to the tile
         */

        public static TileIndex TILE_ADDXY(uint tile, int x, int y)
        {
            return (TileIndex) (tile + TileDiffXY(x, y));
        }

/**
 * Returns the TileIndexDiffC offset from a DiagDirection.
 *
 * @param dir The given direction
 * @return The offset as TileIndexDiffC value
 */
//inline
        public static TileIndexDiffC TileIndexDiffCByDiagDir(DiagDirection dir)
        {
            if (dir.IsValidDiagDirection() == false) throw new ArgumentOutOfRangeException(nameof(dir));
            return _tileoffs_by_diagdir[(int) dir];
        }

/**
 * Returns the TileIndexDiffC offset from a Direction.
 *
 * @param dir The given direction
 * @return The offset as TileIndexDiffC value
 */
//inline
        public static TileIndexDiffC TileIndexDiffCByDir(Direction dir)
        {
            if (dir.IsValidDirection() == false) throw new ArgumentOutOfRangeException(nameof(dir));
            return _tileoffs_by_dir[(int) dir];
        }

/**
 * Add a TileIndexDiffC to a TileIndex and returns the new one.
 *
 * Returns tile + the diff given in diff. If the result tile would end up
 * outside of the map, INVALID_TILE is returned instead.
 *
 * @param tile The base tile to add the offset on
 * @param diff The offset to add on the tile
 * @return The resulting TileIndex
 */
//inline
        public static TileIndex AddTileIndexDiffCWrap(TileIndex tile, TileIndexDiffC diff)
        {
            var x = (uint) (TileX(tile) + diff.x);
            var y = (uint) (TileY(tile) + diff.y);
            /* Negative value will become big positive value after cast */
            if (x >= MapSizeX() || y >= MapSizeY())
            {
                return TileConstants.INVALID_TILE;
            }
            return TileXY(x, y);
        }

/**
 * Returns the diff between two tiles
 *
 * @param tile_a from tile
 * @param tile_b to tile
 * @return the difference between tila_a and tile_b
 */
//inline
        public static TileIndexDiffC TileIndexToTileIndexDiffC(TileIndex tile_a, TileIndex tile_b)
        {
            TileIndexDiffC difference;

            difference.x = (short) (TileX(tile_a) - TileX(tile_b));
            difference.y = (short) (TileY(tile_a) - TileY(tile_b));

            return difference;
        }

/**
 * Convert a DiagDirection to a TileIndexDiff
 *
 * @param dir The DiagDirection
 * @return The resulting TileIndexDiff
 * @see TileIndexDiffCByDiagDir
 */
//inline
        public static TileIndexDiff TileOffsByDiagDir(DiagDirection dir)
        {
            if (dir.IsValidDiagDirection() == false) throw new ArgumentOutOfRangeException(nameof(dir));
            return ToTileIndexDiff(_tileoffs_by_diagdir[(int) dir]);
        }

/**
 * Convert a Direction to a TileIndexDiff.
 *
 * @param dir The direction to convert from
 * @return The resulting TileIndexDiff
 */
//inline
        public static TileIndexDiff TileOffsByDir(Direction dir)
        {
            if (dir.IsValidDirection() == false) throw new ArgumentOutOfRangeException(nameof(dir));
            return ToTileIndexDiff(_tileoffs_by_dir[(int) dir]);
        }

/**
 * Adds a DiagDir to a tile.
 *
 * @param tile The current tile
 * @param dir The direction in which we want to step
 * @return the moved tile
 */
//inline
        public static TileIndex TileAddByDiagDir(TileIndex tile, DiagDirection dir)
        {
            return TILE_ADD(tile, (uint)TileOffsByDiagDir(dir).Difference);
        }

/**
 * Determines the DiagDirection to get from one tile to another.
 * The tiles do not necessarily have to be adjacent.
 * @param tile_from Origin tile
 * @param tile_to Destination tile
 * @return DiagDirection from tile_from towards tile_to, or INVALID_DIAGDIR if the tiles are not on an axis
 */
//inline
        public static DiagDirection DiagdirBetweenTiles(TileIndex tile_from, TileIndex tile_to)
        {
            int dx = (int) TileX(tile_to) - (int) TileX(tile_from);
            int dy = (int) TileY(tile_to) - (int) TileY(tile_from);
            if (dx == 0)
            {
                if (dy == 0)
                {
                    return DiagDirection.INVALID_DIAGDIR;
                }
                return (dy < 0 ? DiagDirection.DIAGDIR_NW : DiagDirection.DIAGDIR_SE);
            }
            else
            {
                if (dy != 0)
                {
                    return DiagDirection.INVALID_DIAGDIR;
                }
                return (dx < 0 ? DiagDirection.DIAGDIR_NE : DiagDirection.DIAGDIR_SW);
            }
        }



/**
 * Get a random tile out of a given seed.
 * @param r the random 'seed'
 * @return a valid tile
 */
//inline
        public static TileIndex RandomTileSeed(uint r) => TILE_MASK(r);

/**
 * Get a valid random tile.
 * @note a define so 'random' gets inserted in the place where it is actually
 *       called, thus making the random traces more explicit.
 * @return a valid tile
 */
        public static TileIndex RandomTile() => RandomTileSeed(Randomizer.Random());
    }
}
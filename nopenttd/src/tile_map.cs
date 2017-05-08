/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file tile_map.h Map writing/reading functions for tiles. */

using System;
using Nopenttd.Core;
using Nopenttd.src.Settings;
using Nopenttd.Slopes;
using Nopenttd.Tiles;

namespace Nopenttd
{
    public class TileMap
    {
/**
 * Returns the height of a tile
 *
 * This function returns the height of the northern corner of a tile.
 * This is saved in the global map-array. It does not take affect by
 * any slope-data of the tile.
 *
 * @param tile The tile to get the height from
 * @return the height of the tile
 * @pre tile < MapSize()
 */
/*inline*/

        public static uint TileHeight(TileIndex tile)
        {
            if (tile >= Map.MapSize()) throw new ArgumentOutOfRangeException(nameof(tile), "Must be less than MapSize");
            return Map._m[tile].height;
        }

/**
 * Sets the height of a tile.
 *
 * This function sets the height of the northern corner of a tile.
 *
 * @param tile The tile to change the height
 * @param height The new height value of the tile
 * @pre tile < MapSize()
 * @pre heigth <= MAX_TILE_HEIGHT
 */
/*inline*/

        public static void SetTileHeight(TileIndex tile, uint height)
        {
            if (tile >= Map.MapSize())
                throw new ArgumentOutOfRangeException(nameof(tile), $"Must be less than {Map.MapSize()}");
            if (height > TileConstants.MAX_TILE_HEIGHT)
                throw new ArgumentOutOfRangeException(nameof(height),
                    $"Must be less than or equal to {TileConstants.MAX_TILE_HEIGHT}");
            Map._m[tile].height = (byte) height;
        }

/**
 * Returns the height of a tile in pixels.
 *
 * This function returns the height of the northern corner of a tile in pixels.
 *
 * @param tile The tile to get the height
 * @return The height of the tile in pixel
 */
/*inline*/

        public static uint TilePixelHeight(TileIndex tile)
        {
            return TileHeight(tile) * TileConstants.TILE_HEIGHT;
        }

/**
 * Returns the tile height for a coordinate outside map.  Such a height is
 * needed for painting the area outside map using completely black tiles.
 * The idea is descending to heightlevel 0 as fast as possible.
 * @param x The X-coordinate (same unit as TileX).
 * @param y The Y-coordinate (same unit as TileY).
 * @return The height in pixels in the same unit as TilePixelHeight.
 */
/*inline*/

        public static uint TilePixelHeightOutsideMap(int x, int y)
        {
            return TileHeightOutsideMap(x, y) * TileConstants.TILE_HEIGHT;
        }

/**
 * Get the tiletype of a given tile.
 *
 * @param tile The tile to get the TileType
 * @return The tiletype of the tile
 * @pre tile < MapSize()
 */
/*inline*/

        public static TileType GetTileType(TileIndex tile)
        {
            if (tile >= Map.MapSize())
                throw new ArgumentOutOfRangeException(nameof(tile), $"Must be less than {Map.MapSize()}");
            return (TileType) BitMath.GB(Map._m[tile].type, 4, 4);
        }

/**
 * Check if a tile is within the map (not a border)
 *
 * @param tile The tile to check
 * @return Whether the tile is in the interior of the map
 * @pre tile < MapSize()
 */
/*inline*/

        public static bool IsInnerTile(TileIndex tile)
        {
            if (tile >= Map.MapSize())
                throw new ArgumentOutOfRangeException(nameof(tile), $"Must be less than {Map.MapSize()}");

            uint x = Map.TileX(tile);
            uint y = Map.TileY(tile);

            return x < Map.MapMaxX() && y < Map.MapMaxY() &&
                   ((x > 0 && y > 0) || !_settings_game.construction.freeform_edges);
        }

/**
 * Set the type of a tile
 *
 * This functions sets the type of a tile. If the type
 * MP_VOID is selected the tile must be at the south-west or
 * south-east edges of the map and vice versa.
 *
 * @param tile The tile to save the new type
 * @param type The type to save
 * @pre tile < MapSize()
 * @pre type MP_VOID <=> tile is on the south-east or south-west edge.
 */
/*inline*/

        public static void SetTileType(TileIndex tile, TileType type)
        {
            if (tile >= Map.MapSize())
                throw new ArgumentOutOfRangeException(nameof(tile), $"Must be less than {Map.MapSize()}");
            /* VOID tiles (and no others) are exactly allowed at the lower left and right
             * edges of the map. If _settings_game.construction.freeform_edges is true,
             * the upper edges of the map are also VOID tiles. */
            if (IsInnerTile(tile) != (type != TileType.MP_VOID)) throw new ArgumentException(nameof(tile));
            BitMath.SB(ref Map._m[tile].type, 4, 4, (uint) type);
        }

/**
 * Checks if a tile is a give tiletype.
 *
 * This function checks if a tile got the given tiletype.
 *
 * @param tile The tile to check
 * @param type The type to check against
 * @return true If the type matches against the type of the tile
 */
/*inline*/

        public static bool IsTileType(TileIndex tile, TileType type)
        {
            return GetTileType(tile) == type;
        }

/**
 * Checks if a tile is valid
 *
 * @param tile The tile to check
 * @return True if the tile is on the map and not one of MP_VOID.
 */
/*inline*/

        public static bool IsValidTile(TileIndex tile)
        {
            return tile < Map.MapSize() && !IsTileType(tile, TileType.MP_VOID);
        }

/**
 * Returns the owner of a tile
 *
 * This function returns the owner of a tile. This cannot used
 * for tiles which type is one of MP_HOUSE, MP_VOID and MP_INDUSTRY
 * as no company owned any of these buildings.
 *
 * @param tile The tile to check
 * @return The owner of the tile
 * @pre IsValidTile(tile)
 * @pre The type of the tile must not be MP_HOUSE and MP_INDUSTRY
 */
/*inline*/

        public static Owner GetTileOwner(TileIndex tile)
        {
            if (IsValidTile(tile) == false) throw new ArgumentException(nameof(tile), "Invalid tile");
            if (IsTileType(tile, TileType.MP_HOUSE)) throw new ArgumentException(nameof(tile));
            if (IsTileType(tile, TileType.MP_INDUSTRY)) throw new ArgumentException(nameof(tile));

            return (Owner) BitMath.GB(Map._m[tile].m1, 0, 5);
        }

/**
 * Sets the owner of a tile
 *
 * This function sets the owner status of a tile. Note that you cannot
 * set a owner for tiles of type MP_HOUSE, MP_VOID and MP_INDUSTRY.
 *
 * @param tile The tile to change the owner status.
 * @param owner The new owner.
 * @pre IsValidTile(tile)
 * @pre The type of the tile must not be MP_HOUSE and MP_INDUSTRY
 */
/*inline*/

        public static void SetTileOwner(TileIndex tile, Owner owner)
        {
            if (IsValidTile(tile) == false) throw new ArgumentException(nameof(tile), "Invalid tile");
            if (IsTileType(tile, TileType.MP_HOUSE)) throw new ArgumentException(nameof(tile));
            if (IsTileType(tile, TileType.MP_INDUSTRY)) throw new ArgumentException(nameof(tile));

            BitMath.SB(ref Map._m[tile].m1, 0, 5, (uint)owner);
        }

/**
 * Checks if a tile belongs to the given owner
 *
 * @param tile The tile to check
 * @param owner The owner to check against
 * @return True if a tile belongs the the given owner
 */
/*inline*/

        public static bool IsTileOwner(TileIndex tile, Owner owner)
        {
            return GetTileOwner(tile) == owner;
        }

/**
 * Set the tropic zone
 * @param tile the tile to set the zone of
 * @param type the new type
 * @pre tile < MapSize()
 */
/*inline*/

        public static void SetTropicZone(TileIndex tile, TropicZone type)
        {
            if (tile >= Map.MapSize())
                throw new ArgumentOutOfRangeException(nameof(tile), "Must be less than MapSize()");
            if (IsTileType(tile, TileType.MP_VOID) && type != TropicZone.TROPICZONE_NORMAL)
                throw new ArgumentException(nameof(tile));
            BitMath.SB(ref Map._m[tile].type, 0, 2, (uint) type);
        }

/**
 * Get the tropic zone
 * @param tile the tile to get the zone of
 * @pre tile < MapSize()
 * @return the zone type
 */
/*inline*/

        public static TropicZone GetTropicZone(TileIndex tile)
        {
            if (tile >= Map.MapSize())
                throw new ArgumentOutOfRangeException(nameof(tile), "Must be less than MapSize()");
            return (TropicZone) BitMath.GB(Map._m[tile].type, 0, 2);
        }

/**
 * Get the current animation frame
 * @param t the tile
 * @pre IsTileType(t, MP_HOUSE) || IsTileType(t, MP_OBJECT) || IsTileType(t, MP_INDUSTRY) ||IsTileType(t, MP_STATION)
 * @return frame number
 */
/*inline*/

        public static byte GetAnimationFrame(TileIndex t)
        {
            if (IsAnimatable(t) == false) throw new ArgumentException(nameof(t), "Tile must be animatable");
            return Map._me[t].m7;
        }

/**
 * Set a new animation frame
 * @param t the tile
 * @param frame the new frame number
 * @pre IsTileType(t, MP_HOUSE) || IsTileType(t, MP_OBJECT) || IsTileType(t, MP_INDUSTRY) ||IsTileType(t, MP_STATION)
 */
/*inline*/

        public static void SetAnimationFrame(TileIndex t, byte frame)
        {
            if (IsAnimatable(t) == false) throw new ArgumentException(nameof(t), "Tile must be animatable");
            Map._me[t].m7 = frame;
        }

        private static bool IsAnimatable(TileIndex t)
        {
            return IsTileType(t, TileType.MP_HOUSE) || IsTileType(t, TileType.MP_OBJECT) ||
                   IsTileType(t, TileType.MP_INDUSTRY) || IsTileType(t, TileType.MP_STATION);
        }

/**
 * Return the slope of a given tile
 * @param tile Tile to compute slope of
 * @param h    If not \c NULL, pointer to storage of z height
 * @return Slope of the tile, except for the HALFTILE part
 */
/*inline*/

        public static Slope GetTilePixelSlope(TileIndex tile, ref int h)
        {
            Slope s = GetTileSlope(tile, ref h);
            //if (h != NULL) *
            h *= (int) TileConstants.TILE_HEIGHT;
            return s;
        }

/**
 * Get bottom height of the tile
 * @param tile Tile to compute height of
 * @return Minimum height of the tile
 */
/*inline*/

        public static int GetTilePixelZ(TileIndex tile)
        {
            return GetTileZ(tile) * (int) TileConstants.TILE_HEIGHT;
        }

/**
 * Get top height of the tile
 * @param t Tile to compute height of
 * @return Maximum height of the tile
 */
/*inline*/

        public static int GetTileMaxPixelZ(TileIndex tile)
        {
            return GetTileMaxZ(tile) * (int) TileConstants.TILE_HEIGHT;
        }

/**
 * Calculate a hash value from a tile position
 *
 * @param x The X coordinate
 * @param y The Y coordinate
 * @return The hash of the tile
 */
/*inline*/

        public static uint TileHash(uint x, uint y)
        {
            uint hash = x >> 4;
            hash ^= x >> 6;
            hash ^= y >> 4;
            hash -= y >> 6;
            return hash;
        }

/**
 * Get the last two bits of the TileHash
 *  from a tile position.
 *
 * @see TileHash()
 * @param x The X coordinate
 * @param y The Y coordinate
 * @return The last two bits from hash of the tile
 */
/*inline*/

        public static uint TileHash2Bit(uint x, uint y)
        {
            return BitMath.GB(TileHash(x, y), 0, 2);
        }












/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file tile_map.cpp Global tile accessors. */

/**
 * Returns the tile height for a coordinate outside map.  Such a height is
 * needed for painting the area outside map using completely black tiles.
 * The idea is descending to heightlevel 0 as fast as possible.
 * @param x The X-coordinate (same unit as TileX).
 * @param y The Y-coordinate (same unit as TileY).
 * @return The height in the same unit as TileHeight.
 */

        private static uint TileHeightOutsideMap(int x, int y)
        {
            /* In all cases: Descend to heightlevel 0 as fast as possible.
             * So: If we are at the 0-side of the map (x<0 or y<0), we must
             * subtract the distance to coordinate 0 from the heightlevel at
             * coordinate 0.
             * In other words: Subtract e.g. -x. If we are at the MapMax
             * side of the map, we also need to subtract the distance to
             * the edge of map, e.g. Map.MapMaxX - x.
             *
             * NOTE: Assuming constant heightlevel outside map would be
             * simpler here. However, then we run into painting problems,
             * since whenever a heightlevel change at the map border occurs,
             * we would need to repaint anything outside map.
             * In contrast, by doing it this way, we can localize this change,
             * which means we may assume constant heightlevel for all tiles
             * at more than <heightlevel at map border> distance from the
             * map border.
             */
            if (x < 0)
            {
                if (y < 0)
                {
                    return (uint) Math.Max((int) TileHeight(Map.TileXY(0, 0)) - (-x) - (-y), 0);
                }
                else if (y < (int) Map.MapMaxY())
                {
                    return (uint) Math.Max((int) TileHeight(Map.TileXY(0, (uint) y)) - (-x), 0);
                }
                else
                {
                    return
                        (uint)
                        Math.Max((int) TileHeight(Map.TileXY(0, Map.MapMaxY())) - (-x) - (y - (int) Map.MapMaxY()), 0);
                }
            }
            else if (x < (int) Map.MapMaxX())
            {
                if (y < 0)
                {
                    return (uint) Math.Max((int) TileHeight(Map.TileXY((uint) x, 0)) - (-y), 0);
                }
                else if (y < (int) Map.MapMaxY())
                {
                    return TileHeight(Map.TileXY((uint) x, (uint) y));
                }
                else
                {
                    return
                        (uint)
                        Math.Max((int) TileHeight(Map.TileXY((uint) x, Map.MapMaxY())) - (y - (int) Map.MapMaxY()), 0);
                }
            }
            else
            {
                if (y < 0)
                {
                    return
                        (uint) Math.Max((int) TileHeight(Map.TileXY(Map.MapMaxX(), 0)) - (x - Map.MapMaxX()) - (-y), 0);
                }
                else if (y < (int) Map.MapMaxY())
                {
                    return
                        (uint) Math.Max((int) TileHeight(Map.TileXY(Map.MapMaxX(), (uint) y)) - (x - Map.MapMaxX()), 0);
                }
                else
                {
                    return
                        (uint)
                        Math.Max(
                            (int) TileHeight(Map.TileXY(Map.MapMaxX(), Map.MapMaxY())) - (x - Map.MapMaxX()) -
                            (y - Map.MapMaxY()), 0);
                }
            }
        }

/**
 * Get a tile's slope given the heigh of its four corners.
 * @param hnorth  The height at the northern corner in the same unit as TileHeight.
 * @param hwest   The height at the western corner in the same unit as TileHeight.
 * @param heast   The height at the eastern corner in the same unit as TileHeight.
 * @param hsouth  The height at the southern corner in the same unit as TileHeight.
 * @param [out] h The lowest height of the four corners.
 * @return The slope.
 */

        static Slope GetTileSlopeGivenHeight(int hnorth, int hwest, int heast, int hsouth, ref int h)
        {
            /* Due to the fact that tiles must connect with each other without leaving gaps, the
             * biggest difference in height between any corner and 'Math.Min' is between 0, 1, or 2.
             *
             * Also, there is at most 1 corner with height difference of 2.
             */
            int hminnw = Math.Min(hnorth, hwest);
            int hmines = Math.Min(heast, hsouth);
            int hmin = Math.Min(hminnw, hmines);

            //if (h != NULL) *
            h = hmin;

            int hmaxnw = Math.Max(hnorth, hwest);
            int hmaxes = Math.Max(heast, hsouth);
            int hmax = Math.Max(hmaxnw, hmaxes);

            Slope r = Slope.SLOPE_FLAT;

            if (hnorth != hmin) r |= Slope.SLOPE_N;
            if (hwest != hmin) r |= Slope.SLOPE_W;
            if (heast != hmin) r |= Slope.SLOPE_E;
            if (hsouth != hmin) r |= Slope.SLOPE_S;

            if (hmax - hmin == 2) r |= Slope.SLOPE_STEEP;

            return r;
        }

        /**
         * Return the slope of a given tile inside the map.
         * @param tile Tile to compute slope of
         * @param h    If not \c NULL, pointer to storage of z height
         * @return Slope of the tile, except for the HALFTILE part
         */

        private static Slope GetTileSlope(TileIndex tile, ref int h)
        {
            if (tile >= Map.MapSize())
                throw new ArgumentOutOfRangeException(nameof(tile), $"Must be less than {Map.MapSize()}");

            uint x = Map.TileX(tile);
            uint y = Map.TileY(tile);
            if (x == Map.MapMaxX() || y == Map.MapMaxY())
            {
                //if (h != NULL) *
                h = (int) TileHeight(tile);
                return Slope.SLOPE_FLAT;
            }

            int hnorth = (int) TileHeight(tile); // Height of the North corner.
            int hwest = (int) TileHeight((uint) (tile + Map.TileDiffXY(1, 0))); // Height of the West corner.
            int heast = (int) TileHeight((uint) (tile + Map.TileDiffXY(0, 1))); // Height of the East corner.
            int hsouth = (int) TileHeight((uint) (tile + Map.TileDiffXY(1, 1))); // Height of the South corner.

            return GetTileSlopeGivenHeight(hnorth, hwest, heast, hsouth, ref h);
        }

        /**
         * Return the slope of a given tile outside the map.
         *
         * @param tile Tile outside the map to compute slope of.
         * @param h    If not \c NULL, pointer to storage of z height.
         * @return Slope of the tile outside map, except for the HALFTILE part.
         */

        private static Slope GetTilePixelSlopeOutsideMap(int x, int y, ref int h)
        {
            int hnorth = (int) TileHeightOutsideMap(x, y); // N corner.
            int hwest = (int) TileHeightOutsideMap(x + 1, y); // W corner.
            int heast = (int) TileHeightOutsideMap(x, y + 1); // E corner.
            int hsouth = (int) TileHeightOutsideMap(x + 1, y + 1); // S corner.

            Slope s = GetTileSlopeGivenHeight(hnorth, hwest, heast, hsouth, ref h);
            //if (h != NULL) *
            h *= (int) TileConstants.TILE_HEIGHT;
            return s;
        }

        /**
         * Check if a given tile is flat
         * @param tile Tile to check
         * @param h If not \c NULL, pointer to storage of z height (only if tile is flat)
         * @return Whether the tile is flat
         */

        private static bool IsTileFlat(TileIndex tile, ref int h)
        {
            if (tile >= Map.MapSize())
                throw new ArgumentOutOfRangeException(nameof(tile), $"Must be less than {Map.MapSize()}");

            if (!IsInnerTile(tile))
            {
                //if (h != NULL) *
                h = (int) TileHeight(tile);
                return true;
            }

            uint z = TileHeight(tile);
            if (TileHeight((uint) (tile + Map.TileDiffXY(1, 0))) != z) return false;
            if (TileHeight((uint) (tile + Map.TileDiffXY(0, 1))) != z) return false;
            if (TileHeight((uint) (tile + Map.TileDiffXY(1, 1))) != z) return false;

            //if (h != NULL) *
            h = (int) z;
            return true;
        }

        /**
         * Get bottom height of the tile
         * @param tile Tile to compute height of
         * @return Minimum height of the tile
         */

        public static int GetTileZ(TileIndex tile)
        {
            if (Map.TileX(tile) == Map.MapMaxX() || Map.TileY(tile) == Map.MapMaxY()) return 0;

            int h = (int) TileHeight(tile); // N corner
            h = Math.Min(h, (int) TileHeight((uint) (tile + Map.TileDiffXY(1, 0)))); // W corner
            h = Math.Min(h, (int) TileHeight((uint) (tile + Map.TileDiffXY(0, 1)))); // E corner
            h = Math.Min(h, (int) TileHeight((uint) (tile + Map.TileDiffXY(1, 1)))); // S corner

            return h;
        }

/**
 * Get bottom height of the tile outside map.
 *
 * @param tile Tile outside the map to compute height of.
 * @return Minimum height of the tile outside the map.
 */

        private static int GetTilePixelZOutsideMap(int x, int y)
        {
            uint h = TileHeightOutsideMap(x, y); // N corner.
            h = Math.Min(h, TileHeightOutsideMap(x + 1, y)); // W corner.
            h = Math.Min(h, TileHeightOutsideMap(x, y + 1)); // E corner.
            h = Math.Min(h, TileHeightOutsideMap(x + 1, y + 1)); // S corner

            return (int) (h * TileConstants.TILE_HEIGHT);
        }

/**
 * Get top height of the tile inside the map.
 * @param t Tile to compute height of
 * @return Maximum height of the tile
 */

        private static int GetTileMaxZ(TileIndex t)
        {
            if (Map.TileX(t) == Map.MapMaxX() || Map.TileY(t) == Map.MapMaxY())
                return (int) TileHeightOutsideMap((int) Map.TileX(t), (int) Map.TileY(t));

            int h = (int) TileHeight(t); // N corner
            h = Math.Max(h, (int) TileHeight((uint) (t + Map.TileDiffXY(1, 0)))); // W corner
            h = Math.Max(h, (int) TileHeight((uint) (t + Map.TileDiffXY(0, 1)))); // E corner
            h = Math.Max(h, (int) TileHeight((uint) (t + Map.TileDiffXY(1, 1)))); // S corner

            return h;
        }

        /**
         * Get top height of the tile outside the map.
         *
         * @see Detailed description in header.
         *
         * @param tile Tile outside to compute height of.
         * @return Maximum height of the tile.
         */

        private static int GetTileMaxPixelZOutsideMap(int x, int y)
        {
            uint h = TileHeightOutsideMap(x, y);
            h = Math.Max(h, TileHeightOutsideMap(x + 1, y));
            h = Math.Max(h, TileHeightOutsideMap(x, y + 1));
            h = Math.Max(h, TileHeightOutsideMap(x + 1, y + 1));

            return (int) (h * TileConstants.TILE_HEIGHT);
        }
    }
}


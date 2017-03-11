/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file tilematrix_type.hpp Template for storing a value per area of the map. */


//#include "core/alloc_func.hpp"
//#include "tilearea_type.h"

/**
 * A simple matrix that stores one value per N*N square of the map.
 * Storage is only allocated for the part of the map that has values
 * assigned.
 *
 * @note No constructor is called for newly allocated values, you
 *       have to do this yourself if needed.
 * @tparam T The type of the stored items.
 * @tparam N Grid size.
 */
//template <typename T, uint N>

using System;
using Nopenttd.Tiles;

namespace Nopenttd
{
    public class TileMatrix<T>
    {

        private const uint N = 128; //TODO REPLACE
        /** Allocates space for a new tile in the matrix.
         * @param tile Tile to add.
         */

        public void AllocateStorage(TileIndex tile)
        {
            uint old_left = Map.TileX(this.area.tile) / N;
            uint old_top = Map.TileY(this.area.tile) / N;
            uint old_w = this.area.w / N;
            uint old_h = this.area.h / N;

            /* Add the square the tile is in to the tile area. We do this
             * by adding top-left and bottom-right of the square. */
            uint grid_x = (Map.TileX(tile) / N) * N;
            uint grid_y = (Map.TileY(tile) / N) * N;
            this.area.Add(Map.TileXY(grid_x, grid_y));
            this.area.Add(Map.TileXY(grid_x + N - 1, grid_y + N - 1));

            /* Allocate new storage. */
            var newData = new T[this.area.w / N * this.area.h / N];

            if (old_w > 0)
            {
                /* Copy old data if present. */
                uint offs_x = old_left - Map.TileX(this.area.tile) / N;
                uint offs_y = old_top - Map.TileY(this.area.tile) / N;

                for (uint row = 0; row < old_h; row++)
                {
                    var sourceIndex = row * old_w;
                    var destinationIndex = (row + offs_y) * this.area.w / N + offs_x;

                    Array.Copy(this.data, sourceIndex, newData, destinationIndex, old_w);
                }
            }

            this.data = newData;
        }

        public const uint GRID = N;

        /// Area covered by the matrix.
        public TileArea area;

        /// Pointer to data array.
        public T[] data;

        public TileMatrix()
        {
            area = new TileArea(TileConstants.INVALID_TILE, 0, 0);
            data = null;
        }

        /**
         * Get the total covered area.
         * @return The area covered by the matrix.
         */
        //const TileArea& GetArea() //const
        public TileArea GetArea() //const
        {
            return this.area;
        }

        /**
         * Get the area of the matrix square that contains a specific tile.
         * @param The tile to get the map area for.
         * @param extend Extend the area by this many squares on all sides.
         * @return Tile area containing the tile.
         */

        static TileArea GetAreaForTile(TileIndex tile, uint extend = 0)
        {
            uint tile_x = (Map.TileX(tile) / N) * N;
            uint tile_y = (Map.TileY(tile) / N) * N;
            uint w = N, h = N;

            w += Math.Min(extend * N, tile_x);
            h += Math.Min(extend * N, tile_y);

            tile_x -= Math.Min(extend * N, tile_x);
            tile_y -= Math.Min(extend * N, tile_y);

            w += Math.Min(extend * N, Map.MapSizeX() - tile_x - w);
            h += Math.Min(extend * N, Map.MapSizeY() - tile_y - h);

            return new TileArea(Map.TileXY(tile_x, tile_y), (byte)w, (byte)h);
        }

        /**
         * Extend the coverage area to include a tile.
         * @param tile The tile to include.
         */

        void Add(TileIndex tile)
        {
            if (!this.area.Contains(tile))
            {
                this.AllocateStorage(tile);
            }
        }

        /**
         * Get the value associated to a tile index.
         * @param tile The tile to get the value for.
         * @return Pointer to the value.
         */

        T Get(TileIndex tile)
        {
            this.Add(tile);

            tile -= this.area.tile;
            uint x = Map.TileX(tile) / N;
            uint y = Map.TileY(tile) / N;

            return this.data[y * this.area.w / N + x];
        }

        /** Array access operator, see #Get. */
        //inline
        public T this[TileIndex tile] => this.Get(tile);
    }

}
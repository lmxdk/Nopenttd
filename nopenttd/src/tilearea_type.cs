/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file tilearea_type.h Type for storing the 'area' of something uses on the map. */

using System;
using System.Collections;
using System.Collections.Generic;
using Nopenttd.Core;
using Nopenttd.Tiles;

namespace Nopenttd
{


/** Represents the covered area of e.g. a rail station */

    public struct TileArea
    {
        //OrthogonalTileArea {

        internal TileIndex tile;

        /// The base tile of the area
        internal ushort w;

        /// The width of the area
        internal ushort h;

        /// The height of the area

        /**
         * Construct this tile area with some set values
         * @param tile the base tile
         * @param w the width
         * @param h the height
         */
        public TileArea(TileIndex? tile = null, byte w = 0, byte h = 0)
        {
            this.tile = tile ?? TileConstants.INVALID_TILE;
            this.w = w;
            this.h = h;
        }

        /**
         * Clears the 'tile area', i.e. make the tile invalid.
         */

        public void Clear()
        {
            tile = TileConstants.INVALID_TILE;
            w = 0;
            h = 0;
        }


        /**
         * Get the center tile.
         * @return The tile at the center, or just north of it.
         */

        public TileIndex GetCenterTile()
        {
            return Map.TILE_ADDXY(tile, w / 2, h / 2);
        }


        /**
         * Construct this tile area based on two points.
         * @param start the start of the area
         * @param end   the end of the area
         */
        public TileArea(TileIndex start, TileIndex end)
        {

            if (start >= Map.MapSize()) throw new ArgumentOutOfRangeException(nameof(start), "start must be less than MapSize");
            if (end >= Map.MapSize()) throw new ArgumentOutOfRangeException(nameof(end), "end must be less than MapSize");

	uint sx = Map.TileX(start);
        uint sy = Map.TileY(start);
        uint ex = Map.TileX(end);
        uint ey = Map.TileY(end);

	if (sx > ex) MemFuncs.Swap(ref sx, ref ex);
	if (sy > ey) MemFuncs.Swap(ref sy, ref ey);


    tile = Map.TileXY(sx, sy);
	w    = (ushort)(ex - sx + 1);
	h    = (ushort)(ey - sy + 1);
}

    /**
     * Add a single tile to a tile area; enlarge if needed.
     * @param to_add The tile to add
     */
    public void Add(TileIndex to_add)
    {
        if (tile == TileConstants.INVALID_TILE)
        {
            tile = to_add;
            w = 1;
            h = 1;
            return;
        }

        uint sx = Map.TileX(tile);
        uint sy = Map.TileY(tile);
        uint ex = sx + w - 1;
        uint ey = sy + h - 1;

        uint ax = Map.TileX(to_add);
        uint ay = Map.TileY(to_add);

        sx = Math.Min(ax, sx);
        sy = Math.Min(ay, sy);
        ex = Math.Max(ax, ex);
        ey = Math.Max(ay, ey);

        tile = Map.TileXY(sx, sy);
        w = (ushort)(ex - sx + 1);
        h = (ushort)(ey - sy + 1);
    }

    /**
     * Does this tile area intersect with another?
     * @param ta the other tile area to check against.
     * @return true if they intersect.
     */
    public bool Intersects(ref TileArea ta) 
{
	if (ta.w == 0 || w == 0) return false;
    if (ta.w == 0 || ta.h == 0 || w == 0 || h == 0) throw new InvalidOperationException("neither the supplied or this area contains anything");

    uint left1   = Map.TileX(tile);
	uint top1    = Map.TileY(tile);
	uint right1  = left1 + w - 1;
	uint bottom1 = top1  + h - 1;

	uint left2   = Map.TileX(ta.tile);
	uint top2    = Map.TileY(ta.tile);
	uint right2  = left2 + ta.w - 1;
	uint bottom2 = top2  + ta.h - 1;

	return !(
			left2   > right1  ||
			right2  < left1   ||
			top2    > bottom1 ||
			bottom2 < top1
		);
    }

    /**
     * Does this tile area contain a tile?
     * @param tile Tile to test for.
     * @return True if the tile is inside the area.
     */
    public bool Contains(TileIndex tile)
    {
        if (w == 0) return false;
        if (w == 0 || h == 0) throw new InvalidOperationException("This has no width or height");

        uint left   = Map.TileX(this.tile);
	uint top    = Map.TileY(this.tile);
	uint tile_x = Map.TileX(tile);
	uint tile_y = Map.TileY(tile);

	return MathFuncs.IsInsideBS((int)tile_x, left, w) && MathFuncs.IsInsideBS((int)tile_y, top, h);
    }

    /**
     * Clamp the tile area to map borders.
     */
    public void ClampToMap()
    {
        if (tile >= Map.MapSize()) throw new InvalidOperationException("Tile is outside of map");
        w = Math.Min(w, (ushort)(Map.MapSizeX() - Map.TileX(tile)));
        h = Math.Min(h, (ushort)(Map.MapSizeY() - Map.TileY(tile)));
    }

};

/** Represents a diagonal tile area. */

    public struct DiagonalTileArea
    {

        internal TileIndex tile;

        /// Base tile of the area
        internal short a;

        /// Extent in diagonal "x" direction (may be negative to signify the area stretches to the left)
        internal short b;

        /// Extent in diagonal "y" direction (may be negative to signify the area stretches upwards)

        /**
         * Construct this tile area with some set values.
         * @param tile The base tile.
         * @param a The "x" extent.
         * @param b The "y" estent.
         */
        public DiagonalTileArea(TileIndex? tile = null, byte a = 0, byte b = 0)
        {
            this.tile = tile ?? TileConstants.INVALID_TILE;
            this.a = a;
            this.b = b;
        }

        /**
         * Clears the TileArea by making the tile invalid and setting a and b to 0.
         */

        public void Clear()
        {
            tile = TileConstants.INVALID_TILE;
            a = 0;
            b = 0;
        }
        public DiagonalTileArea(TileIndex start, TileIndex end)
        {
            tile = start;
            if (start >= Map.MapSize()) throw new ArgumentOutOfRangeException(nameof(start), "start must be less than MapSize");
            if (end >= Map.MapSize()) throw new ArgumentOutOfRangeException(nameof(end), "end must be less than MapSize");

            /* Unfortunately we can't find a new base and make all a and b positive because
             * the new base might be a "flattened" corner where there actually is no single
             * tile. If we try anyway the result is either inaccurate ("one off" half of the
             * time) or the code gets much more complex;
             *
             * We also need to increment/decrement a and b here to have one-past-end semantics
             * for a and b, just the way the orthogonal tile area does it for w and h. */

            a = (short)(Map.TileY(end) + Map.TileX(end) - Map.TileY(start) - Map.TileX(start));
            b = (short)(Map.TileY(end) - Map.TileX(end) - Map.TileY(start) + Map.TileX(start));
            if (a > 0)
            {
                a++;
            }
            else
            {
                a--;
            }

            if (b > 0)
            {
                b++;
            }
            else
            {
                b--;
            }
        }

        /**
         * Does this tile area contain a tile?
         * @param tile Tile to test for.
         * @return True if the tile is inside the area.
         */
        public bool Contains(TileIndex tile)
        {

    int a = (int)(Map.TileY(tile) + Map.TileX(tile));
        int b = (int)(Map.TileY(tile) - Map.TileX(tile));

        int start_a = (int)(Map.TileY(this.tile) + Map.TileX(this.tile));
        int start_b = (int)(Map.TileY(this.tile) - Map.TileX(this.tile));

        int end_a = start_a + this.a;
        int end_b = start_b + this.b;

	/* Swap if necessary, preserving the "one past end" semantics. */
	if (start_a > end_a) {
		int tmp = start_a;
        start_a = end_a + 1;
		end_a = tmp + 1;
	}
	if (start_b > end_b) {
		int tmp = start_b;
    start_b = end_b + 1;
		end_b = tmp + 1;
	}

	return (a >= start_a && a<end_a && b >= start_b && b<end_b);
}
    }

    /** Shorthand for the much more common orthogonal tile area. */
    //typedef OrthogonalTileArea TileArea;

    ///** Base class for tile iterators. */
    ////TODO implement Iterator
    public abstract class TileIterator : IEnumerator<TileIndex>
    {
        /// The current tile we are at.
        protected TileIndex tile;

        private IEnumerator<TileIndex> _enumeratorImplementation;

        /**
         * Initialise the iterator starting at this tile.
         * @param tile The tile we start iterating from.
         */

        protected TileIterator(TileIndex? tile)
        {
            this.tile = tile ?? TileConstants.INVALID_TILE;
        }


        /**
         * Get the tile we are currently at.
         * @return The tile we are at, or INVALID_TILE when we're done.
         */
        //inline
        //public operator TileIndex()
        //{
        //    return this->tile;
        //}

        /**
         * Move ourselves to the next tile in the rectangle on the map.
         */
        //public abstract TileIterator operator++();

        /**
         * Allocate a new iterator that is a copy of this one.
         */
        //public abstract TileIterator Clone();
        public void Dispose()
        {

        }

        public abstract bool MoveNext();

        public abstract void Reset();

        public TileIndex Current => tile;
        object IEnumerator.Current => tile;
    };

    /** Iterator to iterate over a tile area (rectangle) of the map. */

    public class OrthogonalTileIterator : TileIterator
    {
        private int w;

        /// The width of the iterated area.
        private int x;

        /// The current 'x' position in the rectangle.
        private int y;

        /// The current 'y' position in the rectangle.

        /**
         * Construct the iterator.
         * @param ta Area, i.e. begin point and width/height of to-be-iterated area.
         */
        public OrthogonalTileIterator(TileArea ta) : base(ta.tile)
        {
            if (ta.w == 0 || ta.h == 0)
            {
                tile = TileConstants.INVALID_TILE;
            }

            w = ta.w;
            x = ta.w;
            y = ta.h;
        }

        /**
         * Construct the iterator.
         * @param corner1 Tile from where to begin iterating.
         * @param corner2 Tile where to end the iterating.
         */

        public OrthogonalTileIterator(TileIndex corner1, TileIndex corner2) : this(new TileArea(corner1, corner2))
        {

        }

        /**
         * Move ourselves to the next tile in the rectangle on the map.
         */
        //inline
        public override bool MoveNext()
        {
            if (tile == TileConstants.INVALID_TILE)
            {
                return false;
            }

            if (--x > 0)
            {
                tile++;
                return true;
            }
            else if (--y > 0)
            {
                x = w;
                tile += (TileIndex) (uint) (Map.TileDiffXY(1, 1).Difference - w);
                return true;
            }
            else
            {
                tile = TileConstants.INVALID_TILE;
                return false;
            }
        }

        public override void Reset()
        {
            throw new System.NotImplementedException();
        }
    }

/** Iterator to iterate over a diagonal area of the map. */

    public class DiagonalTileIterator : TileIterator
    {

        private uint base_x;

        /// The base tile x coordinate from where the iterating happens.
        private uint base_y;

        /// The base tile y coordinate from where the iterating happens.
        private int a_cur;

        /// The current (rotated) x coordinate of the iteration.
        private int b_cur;

        /// The current (rotated) y coordinate of the iteration.
        private int a_max;

        /// The (rotated) x coordinate of the end of the iteration.
        private int b_max;

        /// The (rotated) y coordinate of the end of the iteration.

        /**
         * Construct the iterator.
         * @param ta Area, i.e. begin point and (diagonal) width/height of to-be-iterated area.
         */
        public DiagonalTileIterator(DiagonalTileArea ta) : base(ta.tile)
        {
            base_x = Map.TileX(ta.tile);
            base_y = Map.TileY(ta.tile);
            a_cur = 0;
            b_cur = 0;
            a_max = ta.a;
            b_max = ta.b;

        }

        /**
         * Construct the iterator.
         * @param corner1 Tile from where to begin iterating.
         * @param corner2 Tile where to end the iterating.
         */

        DiagonalTileIterator(TileIndex corner1, TileIndex corner2) : this(new DiagonalTileArea(corner1, corner2))
        {

        }

        //virtual TileIterator *Clone() const
        //{
        //	return new DiagonalTileIterator(*this);
        //}

        /**
 * Create a diagonal tile area from two corners.
 * @param start First corner of the area.
 * @param end Second corner of the area.
 */


        /**
         * Move ourselves to the next tile in the rectangle on the map.
         */
        public override bool MoveNext()
        {
            if (tile != TileConstants.INVALID_TILE)
            {
                return false;
            }

    /* Determine the next tile, while clipping at map borders */
    bool new_line = false;
    do
    {
        /* Iterate using the rotated coordinates. */
        if (a_max == 1 || a_max == -1)
        {
            /* Special case: Every second column has zero length, skip them completely */
            a_cur = 0;
            if (b_max > 0)
            {
                b_cur = Math.Min(b_cur + 2, b_max);
            }
            else
            {
                b_cur = Math.Max(b_cur - 2, b_max);
            }
        }
        else
        {
            /* Every column has at least one tile to process */
            if (a_max > 0)
            {
                a_cur += 2;
                new_line = a_cur >= a_max;
            }
            else
            {
                a_cur -= 2;
                new_line = a_cur <= a_max;
            }
            if (new_line)
            {
                /* offset of initial a_cur: one tile in the same direction as a_max
				 * every second line.
				 */
                a_cur = Math.Abs(a_cur) % 2 != 0 ? 0 : (a_max > 0 ? 1 : -1);

                if (b_max > 0)
                {
                    ++b_cur;
                }
                else
                {
                    --b_cur;
                }
            }
        }

        /* And convert the coordinates back once we've gone to the next tile. */
        uint x = (uint)(base_x + (a_cur - b_cur) / 2);
        uint y = (uint)(base_y + (b_cur + a_cur) / 2);
        /* Prevent wrapping around the map's borders. */
        tile = x >= Map.MapSizeX() || y >= Map.MapSizeY() ? TileConstants.INVALID_TILE : Map.TileXY(x, y);
    } while (tile > Map.MapSize() && b_max != b_cur);

    if (b_max == b_cur) tile = TileConstants.INVALID_TILE;
    
    return tile != TileConstants.INVALID_TILE;
}

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }

/**
 * A loop which iterates over the tiles of a TileArea.
 * @param var The name of the variable which contains the current tile.
 *            This variable will be allocated in this \c for of this loop.
 * @param ta  The tile area to search over.
 */
//#TILE_AREA_LOOP(var, ta) for (OrthogonalTileIterator var(ta); var != INVALID_TILE; ++var)

}
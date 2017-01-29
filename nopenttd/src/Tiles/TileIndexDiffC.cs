namespace Nopenttd.Tiles
{
    /**
 * A pair-construct of a TileIndexDiff.
 *
 * This can be used to save the difference between to
 * tiles as a pair of x and y value.
 */
    public struct TileIndexDiffC {
        public short x;        /// The x value of the coordinate
        public short y;        /// The y value of the coordinate

        public TileIndexDiffC(short x, short y)
        {
            this.x = x;
            this.y = y;
        }
    };
}
namespace Nopenttd.Tiles
{
    /**
     * An offset value between to tiles.
     *
     * This value is used for the difference between
     * to tiles. It can be added to a tileindex to get
     * the resulting tileindex of the start tile applied
     * with this saved difference.
     *
     * @see TileDiffXY(int, int)
     */
    public struct TileIndexDiff
    {
        public int Difference { get; set; }


        public TileIndexDiff(int i)
        {
            Difference = i;
        }

        public static implicit operator int(TileIndexDiff d)
        {
            return d.Difference;
        }

        public static implicit operator TileIndexDiff(int i)
        {
            return new TileIndexDiff(i);
        }
    }
}
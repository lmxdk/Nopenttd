/**
 * The index/ID of a Tile.
 */
//typedef TileIndex;

namespace Nopenttd.Tiles
{
    public struct TileIndex
    {
        public uint Index { get; set; }

        public TileIndex(uint i)
        {
            Index = i;
        }

        public static implicit operator uint(TileIndex t)
        {
            return t.Index;
        }

        public static implicit operator TileIndex(uint i)
        {
            return new TileIndex(i);
        }
    }
}
namespace Nopenttd.Tiles
{
    /**
         * Data that is stored per tile. Also used Tile for this.
         * Look at docs/landscape.html for the exact meaning of the members.
         */
    public struct TileExtended {
        public byte m6; /// General purpose
        public byte m7; /// Primarily used for newgrf support
    }
}
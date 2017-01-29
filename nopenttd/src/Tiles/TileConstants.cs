
namespace Nopenttd.Tiles
{
    public class TileConstants
    {
        /// Tile size in world coordinates.
        public const uint TILE_SIZE = 16;

        /// For masking in/out the inner-tile world coordinate units.
        public const uint TILE_UNIT_MASK = TILE_SIZE - 1;

        /// Pixel distance between tile columns/rows in #ZOOM_LVL_BASE.
        public const uint TILE_PIXELS = 32;

        /// Height of a height level in world coordinate AND in pixels in #ZOOM_LVL_BASE.
        public const uint TILE_HEIGHT = 8;

        /// Maximum height of a building in pixels in #ZOOM_LVL_BASE. (Also applies to "bridge buildings" on the bridge floor.)
        public const uint MAX_BUILDING_PIXELS = 200;

        /// Maximum allowed tile height
        public const uint MAX_TILE_HEIGHT = 255;

        /// Lower bound of maximum allowed heightlevel (in the construction settings)
        public const uint MIN_MAX_HEIGHTLEVEL = 15;

        /// Default maximum allowed heightlevel (in the construction settings)
        public const uint DEF_MAX_HEIGHTLEVEL = 30;

        /// Upper bound of maximum allowed heightlevel (in the construction settings)
        public const uint MAX_MAX_HEIGHTLEVEL = MAX_TILE_HEIGHT;

        /// Minimum snowline height
        public const uint MIN_SNOWLINE_HEIGHT = 2;

        /// Default snowline height
        public const uint DEF_SNOWLINE_HEIGHT = 15;

        /// Maximum allowed snowline height
        public const uint MAX_SNOWLINE_HEIGHT = (MAX_TILE_HEIGHT - 2);



        /**
         * The very nice invalid tile marker
         */
        public static readonly TileIndex INVALID_TILE = 0xffff;




        /** Minimal and maximal map width and height */
        public const uint MIN_MAP_SIZE_BITS = 6;                      ///< Minimal size of map is equal to 2 ^ MIN_MAP_SIZE_BITS
        public const uint MAX_MAP_SIZE_BITS = 12;                     ///< Maximal size of map is equal to 2 ^ MAX_MAP_SIZE_BITS
        public const uint MIN_MAP_SIZE = 1 << (int)MIN_MAP_SIZE_BITS; ///< Minimal map size = 64
        public const uint MAX_MAP_SIZE = 1 << (int)MAX_MAP_SIZE_BITS; ///< Maximal map size = 4096

        /**
         * Approximation of the length of a straight track, relative to a diagonal
         * track (ie the size of a tile side).
         *
         * #defined instead of const so it can
         * stay integer. (no runtime float operations) Is this needed?
         * Watch out! There are _no_ brackets around here, to prevent intermediate
         * rounding! Be careful when using this!
         * This value should be sqrt(2)/2 ~ 0.7071
         */
         public const float STRAIGHT_TRACK_LENGTH = 7071/10000;
    }
}
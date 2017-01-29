namespace Nopenttd.Tiles
{

    /**
     * Additional infos of a tile on a tropic game.
     *
     * The tropiczone is not modified during gameplay. It mainly affects tree growth. (desert tiles are visible though)
     *
     * In randomly generated maps:
     *  TROPICZONE_DESERT: Generated everywhere, if there is neither water nor mountains (TileHeight >= 4) in a certain distance from the tile.
     *  TROPICZONE_RAINFOREST: Generated everywhere, if there is no desert in a certain distance from the tile.
     *  TROPICZONE_NORMAL: Everywhere else, i.e. between desert and rainforest and on sea (if you clear the water).
     *
     * In scenarios:
     *  TROPICZONE_NORMAL: Default value.
     *  TROPICZONE_DESERT: Placed manually.
     *  TROPICZONE_RAINFOREST: Placed if you plant certain rainforest-trees.
     */
    public enum TropicZone
    {
        TROPICZONE_NORMAL = 0,

        /// Normal tropiczone
        TROPICZONE_DESERT = 1,

        /// Tile is desert
        TROPICZONE_RAINFOREST = 2 /// Rainforest tile
    }
}
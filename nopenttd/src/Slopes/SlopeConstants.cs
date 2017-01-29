namespace Nopenttd.Slopes
{
    public static class SlopeConstants
    {
        /** Constant bitset with safe slopes for building a level crossing. */

        public const uint VALID_LEVEL_CROSSING_SLOPES =
            (uint)
            ((1 << (int) Slope.SLOPE_SEN) | (1 << (int) Slope.SLOPE_ENW) | (1 << (int) Slope.SLOPE_NWS) |
             (1 << (int) Slope.SLOPE_NS) | (1 << (int) Slope.SLOPE_WSE) | (1 << (int) Slope.SLOPE_EW) |
             (1 << (int) Slope.SLOPE_FLAT));
    }
}
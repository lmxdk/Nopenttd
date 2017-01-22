namespace Nopenttd.Core.Geometry
{

    /** Dimensions (a width and height) of a rectangle in 2D */
    public struct Dimension {
        uint width;
        uint height;

        /** @file geometry_func.cpp Geometry functions. */
        /**
         * Compute bounding box of both dimensions.
         * @param d1 First dimension.
         * @param d2 Second dimension.
         * @return The bounding box of both dimensions, the smallest dimension that surrounds both arguments.
         */
        public static Dimension maxdim(ref Dimension d1, ref Dimension d2)
        {
            var dim = new Dimension();
            dim.width = max(d1.width, d2.width);
            dim.height = max(d1.height, d2.height);
            return dim;
        }
    };
}
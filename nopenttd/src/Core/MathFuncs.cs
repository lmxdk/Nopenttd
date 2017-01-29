/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file math_func.cpp Math functions. */

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Nopenttd.Core
{
    public class MathFuncs
    {
/**
 * Compute least common multiple (lcm) of arguments \a a and \a b, the smallest
 * integer value that is a multiple of both \a a and \a b.
 * @param a First number.
 * @param b second number.
 * @return Least common multiple of values \a a and \a b.
 *
 * @note This function only works for non-negative values of \a a and \a b.
 */

        public static int LeastCommonMultiple(int a, int b)
        {
            if (a == 0 || b == 0) return 0; // By definition.
            if (a == 1 || a == b) return b;
            if (b == 1) return a;

            return a * b / GreatestCommonDivisor(a, b);
        }

/**
 * Compute greatest common divisor (gcd) of \a a and \a b.
 * @param a First number.
 * @param b second number.
 * @return Greatest common divisor of \a a and \a b.
 */

        public static int GreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                int t = b;
                b = a % b;
                a = t;
            }
            return a;

        }

/**
 * Deterministic approximate division.
 * Cancels out division errors stemming from the integer nature of the division over multiple runs.
 * @param a Dividend.
 * @param b Divisor.
 * @return a/b or (a/b)+1.
 */

        public static int DivideApprox(int a, int b)
        {
            int random_like = ((a + b) * (a - b)) % b;

            int remainder = a % b;

            int ret = a / b;

            if (System.Math.Abs(random_like) < System.Math.Abs(remainder))
            {
                ret += ((a < 0) ^ (b < 0)) ? -1 : 1;
            }

            return ret;
        }

        /**
         * Compute the integer square root.
         * @param num Radicand.
         * @return Rounded integer square root.
         * @note Algorithm taken from http://en.wikipedia.org/wiki/Methods_of_computing_square_roots
         */

        public static uint IntSqrt(uint num)
        {
            uint res = 0;
            uint bit = (uint)(1UL << 30); // Second to top bit number.

            /* 'bit' starts at the highest power of four <= the argument. */
            while (bit > num) bit >>= 2;

            while (bit != 0)
            {
                if (num >= res + bit)
                {
                    num -= res + bit;
                    res = (res >> 1) + bit;
                }
                else
                {
                    res >>= 1;
                }
                bit >>= 2;
            }

            /* Arithmetic rounding to nearest integer. */
            if (num > res) res++;

            return res;
        }

        /* $Id$ */

        /*
         * This file is part of OpenTTD.
         * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
         * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
         * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
         */

        /** @file math_func.hpp Integer math functions */

        /**
         * Return the smallest multiple of n equal or greater than x
         *
         * @note n must be a power of 2
         * @param x The min value
         * @param n The base of the number we are searching
         * @return The smallest multiple of n equal or greater than x
         */

        public static int Align(int x, uint n)
        {
            Debug.Assert((n & (n - 1)) == 0 && n != 0);
            n--;
            return (int) ((x + n) & ~(int) n);
        }

        /**
         * Return the smallest multiple of n equal or greater than x
         * Applies to pointers only
         *
         * @note n must be a power of 2
         * @param x The min value
         * @param n The base of the number we are searching
         * @return The smallest multiple of n equal or greater than x
         * @see Align()
         */
        //        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //        template<typename T> 
        //static inline T * AlignPtr(T* x, uint n)
        //    {
        //        assert_compile(sizeof(size_t) == sizeof(void*));
        //        return (T*)Align((size_t)x, n);
        //    }

        /**
         * Clamp a value between an interval.
         *
         * This function returns a value which is between the given interval of
         * min and max. If the given value is in this interval the value itself
         * is returned otherwise the border of the interval is returned, according
         * which side of the interval was 'left'.
         *
         * @note The min value must be less or equal of max or you get some
         *       unexpected results.
         * @param a The value to clamp/truncate.
         * @param min The minimum of the interval.
         * @param max the maximum of the interval.
         * @returns A value between min and max which is closest to a.
         * @see ClampU(uint, uint, uint)
         * @see Clamp(int, int, int)
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int a, int min, int max)
        {
            Debug.Assert(min <= max);
            if (a <= min) return min;
            if (a >= max) return max;
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long a, long min, long max)
        {
            Debug.Assert(min <= max);
            if (a <= min) return min;
            if (a >= max) return max;
            return a;
        }

        /**
         * Clamp an integer between an interval.
         *
         * This function returns a value which is between the given interval of
         * min and max. If the given value is in this interval the value itself
         * is returned otherwise the border of the interval is returned, according
         * which side of the interval was 'left'.
         *
         * @note The min value must be less or equal of max or you get some
         *       unexpected results.
         * @param a The value to clamp/truncate.
         * @param min The minimum of the interval.
         * @param max the maximum of the interval.
         * @returns A value between min and max which is closest to a.
         * @see ClampU(uint, uint, uint)
         */
        //static inline int Clamp(const int a, const int min, const int max)
        //{
        //    return Clamp<int>(a, min, max);
        //}

        /**
         * Clamp an unsigned integer between an interval.
         *
         * This function returns a value which is between the given interval of
         * min and max. If the given value is in this interval the value itself
         * is returned otherwise the border of the interval is returned, according
         * which side of the interval was 'left'.
         *
         * @note The min value must be less or equal of max or you get some
         *       unexpected results.
         * @param a The value to clamp/truncate.
         * @param min The minimum of the interval.
         * @param max the maximum of the interval.
         * @returns A value between min and max which is closest to a.
         * @see Clamp(int, int, int)
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ClampU(uint a, uint min, uint max)
        {
            Debug.Assert(min <= max);
            if (a <= min) return min;
            if (a >= max) return max;
            return a;
        }

        /**
         * Reduce a signed 64-bit int to a signed 32-bit one
         *
         * This function clamps a 64-bit integer to a 32-bit integer.
         * If the 64-bit value is smaller than the smallest 32-bit integer
         * value 0x80000000 this value is returned (the left one bit is the sign bit).
         * If the 64-bit value is greater than the greatest 32-bit integer value 0x7FFFFFFF
         * this value is returned. In all other cases the 64-bit value 'fits' in a
         * 32-bits integer field and so the value is casted to int32 and returned.
         *
         * @param a The 64-bit value to clamps
         * @return The 64-bit value reduced to a 32-bit value
         * @see Clamp(int, int, int)
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ClampToI32(long a)
        {
            return (int) Clamp(a, int.MinValue, int.MaxValue);
        }

        /**
         * Reduce an unsigned 64-bit int to an unsigned 16-bit one
         *
         * @param a The 64-bit value to clamp
         * @return The 64-bit value reduced to a 16-bit value
         * @see ClampU(uint, uint, uint)
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ClampToU16(ulong a)
        {
            /* MSVC thinks, in its infinite wisdom, that int min(int, int) is a better
             * match for min(uint64, uint) than uint64 min(uint64, uint64). As such we
             * need to cast the UINT16_MAX to prevent MSVC from displaying its
             * infinite loads of warnings. */
            return (ushort) System.Math.Min(a, ushort.MaxValue);
        }

        /**
         * Returns the (absolute) difference between two (scalar) variables
         *
         * @param a The first scalar
         * @param b The second scalar
         * @return The absolute difference between the given scalars
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Delta(int a, int b)
        {
            return (a < b) ? b - a : a - b;
        }

        public static uint Delta(uint a, uint b)
        {
            return (a < b) ? b - a : a - b;
        }

        /**
         * Checks if a value is between a window started at some base point.
         *
         * This function checks if the value x is between the value of base
         * and base+size. If x equals base this returns true. If x equals
         * base+size this returns false.
         *
         * @param x The value to check
         * @param base The base value of the interval
         * @param size The size of the interval
         * @return True if the value is in the interval, false else.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInsideBS(int x, uint @base, uint size)
        {
            return (uint) (x - @base) < size;
        }

        /**
         * Checks if a value is in an interval.
         *
         * Returns true if a value is in the interval of [min, max).
         *
         * @param x The value to check
         * @param min The minimum of the interval
         * @param max The maximum of the interval
         * @see IsInsideBS()
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInsideMM(int x, uint min, uint max)
        {
            return (uint) (x - min) < (max - min);
        }

        /**
         * Converts a "fract" value 0..255 to "percent" value 0..100
         * @param i value to convert, range 0..255
         * @return value in range 0..100
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToPercent8(uint i)
        {
            Debug.Assert(i < 256);
            return i * 101 >> 8;
        }

        /**
         * Converts a "fract" value 0..65535 to "percent" value 0..100
         * @param i value to convert, range 0..65535
         * @return value in range 0..100
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToPercent16(uint i)
        {
            Debug.Assert(i < 65536);
            return i * 101 >> 16;
        }

        /**
         * Computes ceil(a / b) for non-negative a and b.
         * @param a Numerator
         * @param b Denominator
         * @return Quotient, rounded up
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CeilDiv(uint a, uint b)
        {
            return (a + b - 1) / b;
        }

        /**
         * Computes ceil(a / b) * b for non-negative a and b.
         * @param a Numerator
         * @param b Denominator
         * @return a rounded up to the nearest multiple of b.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Ceil(uint a, uint b)
        {
            return CeilDiv(a, b) * b;
        }

        /**
         * Computes round(a / b) for signed a and unsigned b.
         * @param a Numerator
         * @param b Denominator
         * @return Quotient, rounded to nearest
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundDivSU(int a, uint b)
        {
            if (a > 0)
            {
                /* 0.5 is rounded to 1 */
                return (a + (int) b / 2) / (int) b;
            }
            else
            {
                /* -0.5 is rounded to 0 */
                return (a - ((int) b - 1) / 2) / (int) b;
            }
        }
    }
}
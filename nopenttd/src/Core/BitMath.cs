/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file bitmath_func.hpp Functions related to bit mathematics. */

using System;
using System.Runtime.CompilerServices;

namespace Nopenttd.Core
{
    /*Linqpad helper
     * var types = new [] {"int", "uint", "long", "ulong", "short", "ushort", "byte", "sbyte"};

	foreach (var type in types)
	{
		$@"method".Dump();;
	}
     */
    public class BitMath
    {
        /**
         * Fetch \a n bits from \a x, started at bit \a s.
         *
         * This function can be used to fetch \a n bits from the value \a x. The
         * \a s value set the start position to read. The start position is
         * count from the LSB and starts at \c 0. The result starts at a
         * LSB, as this isn't just an and-bitmask but also some
         * bit-shifting operations. GB(0xFF, 2, 1) will so
         * return 0x01 (0000 0001) instead of
         * 0x04 (0000 0100).
         *
         * @param x The value to read some bits.
         * @param s The start position to read some bits.
         * @param n The number of bits to read.
         * @pre n < sizeof(T) * 8
         * @pre s + n <= sizeof(T) * 8
         * @return The selected bits, aligned to a LSB.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint GB(uint x, byte s, byte n)
{
	return (x >> s) & (((uint)1 << n) - 1);
}
        public static int GB(int x, byte s, byte n)
        {
            return (x >> s) & ((1 << n) - 1);
        }
        public static ushort GB(ushort x, byte s, byte n)
        {
            return (ushort)((x >> s) & ((1 << n) - 1));
        }
        public static short GB(short x, byte s, byte n)
        {
            return (short)((x >> s) & ((1 << n) - 1));
        }
        public static ulong GB(ulong x, byte s, byte n)
        {
            return (ulong)((x >> s) & (ulong)((1L << n) - 1L));
        }
        public static long GB(long x, byte s, byte n)
        {
            return (long)((x >> s) & ((1 << n) - 1));
        }

        public static byte GB(byte x, byte s, byte n)
        {
            return (byte)((x >> s) & ((1 << n) - 1));
        }
        public static sbyte GB(sbyte x, byte s, byte n)
        {
            return (sbyte)((x >> s) & ((1 << n) - 1));
        }

        /**
         * Set \a n bits in \a x starting at bit \a s to \a d
         *
         * This function sets \a n bits from \a x which started as bit \a s to the value of
         * \a d. The parameters \a x, \a s and \a n works the same as the parameters of
         * #GB. The result is saved in \a x again. Unused bits in the window
         * provided by n are set to 0 if the value of \a d isn't "big" enough.
         * This is not a bug, its a feature.
         *
         * @note Parameter \a x must be a variable as the result is saved there.
         * @note To avoid unexpected results the value of \a d should not use more
         *       space as the provided space of \a n bits (log2)
         * @param x The variable to change some bits
         * @param s The start position for the new bits
         * @param n The size/window for the new bits
         * @param d The actually new bits to save in the defined position.
         * @pre n < sizeof(T) * 8
         * @pre s + n <= sizeof(T) * 8
         * @return The new value of \a x
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint SB(uint x, byte s, byte n, uint d)
{
	x &= (uint)(~((((uint)1U << n) - 1) << s));
	x |= (uint)(d << s);
	return x;
}
        public static int SB(int x, byte s, byte n, int d)
        {
            x &= (int)(~((((int)1U << n) - 1) << s));
            x |= (int)(d << s);
            return x;
        }
        public static ushort SB(ushort x, byte s, byte n, ushort d)
        {
            x &= (ushort)(~((((ushort)1U << n) - 1) << s));
            x |= (ushort)(d << s);
            return x;
        }
        public static short SB(short x, byte s, byte n, short d)
        {
            x &= (short)(~((((short)1U << n) - 1) << s));
            x |= (short)(d << s);
            return x;
        }

        public static ulong SB(ulong x, byte s, byte n, ulong d)
        {
            x &= (ulong)(~((((ulong)1U << n) - 1) << s));
            x |= (ulong)(d << s);
            return x;
        }
        public static long SB(long x, byte s, byte n, long d)
        {
            x &= (long)(~((((long)1U << n) - 1) << s));
            x |= (long)(d << s);
            return x;
        }
        public static sbyte SB(sbyte x, byte s, byte n, sbyte d)
        {
            x &= (sbyte)(~((((sbyte)1U << n) - 1) << s));
            x |= (sbyte)(d << s);
            return x;
        }

        public static byte SB(byte x, byte s, byte n, byte d)
        {
            x &= (byte)(~((((byte)1U << n) - 1) << s));
            x |= (byte)(d << s);
            return x;
        }

        /**
         * Add \a i to \a n bits of \a x starting at bit \a s.
         *
         * This adds the value of \a i on \a n bits of \a x starting at bit \a s. The parameters \a x,
         * \a s, \a i are similar to #GB. Besides, \ a x must be a variable as the result are
         * saved there. An overflow does not affect the following bits of the given
         * bit window and is simply ignored.
         *
         * @note Parameter x must be a variable as the result is saved there.
         * @param x The variable to add some bits at some position
         * @param s The start position of the addition
         * @param n The size/window for the addition
         * @pre n < sizeof(T) * 8
         * @pre s + n <= sizeof(T) * 8
         * @param i The value to add at the given start position in the given window.
         * @return The new value of \a x
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AB(int x, byte s, byte n, int i)
        {
            int mask = ((((int)1U << n) - 1) << s);
            x = (int)((x & ~mask) | ((x + (i << s)) & mask));
            return x;
        }
        public static uint AB(uint x, byte s, byte n, uint i)
        {
            uint mask = ((((uint)1U << n) - 1) << s);
            x = (uint)((x & ~mask) | ((x + (i << s)) & mask));
            return x;
        }
        public static long AB(long x, byte s, byte n, long i)
        {
            long mask = ((((long)1U << n) - 1) << s);
            x = (long)((x & ~mask) | ((x + (i << s)) & mask));
            return x;
        }
        public static ulong AB(ulong x, byte s, byte n, ulong i)
        {
            ulong mask = ((((ulong)1U << n) - 1) << s);
            x = (ulong)((x & ~mask) | ((x + (i << s)) & mask));
            return x;
        }
        public static short AB(short x, byte s, byte n, short i)
        {
            short mask = (short)(((1 << n) - 1) << s);
            x = (short)((x & ~mask) | ((x + (i << s)) & mask));
            return x;
        }
        public static ushort AB(ushort x, byte s, byte n, ushort i)
        {
            ushort mask = (ushort)(((1 << n) - 1) << s);
            x = (ushort)((x & ~mask) | ((x + (i << s)) & mask));
            return x;
        }
        public static byte AB(byte x, byte s, byte n, byte i)
        {
            byte mask = (byte)(((1 << n) - 1) << s);
            x = (byte)((x & ~mask) | ((x + (i << s)) & mask));
            return x;
        }
        public static sbyte AB(sbyte x, byte s, byte n, sbyte i)
        {
            sbyte mask = (sbyte)(((1 << n) - 1) << s);
            x = (sbyte)((x & ~mask) | ((x + (i << s)) & mask));
            return x;
        }



        /**
         * Checks if a bit in a value is set.
         *
         * This function checks if a bit inside a value is set or not.
         * The \a y value specific the position of the bit, started at the
         * LSB and count from \c 0.
         *
         * @param x The value to check
         * @param y The position of the bit to check, started from the LSB
         * @pre y < sizeof(T) * 8
         * @return True if the bit is set, false else.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasBit(int x, byte y)
        {
            return (x & ((int)1U << y)) != 0;
        }
        public static bool HasBit(uint x, byte y)
        {
            return (x & ((uint)1U << y)) != 0;
        }
        public static bool HasBit(long x, byte y)
        {
            return (x & ((long)1U << y)) != 0;
        }
        public static bool HasBit(ulong x, byte y)
        {
            return (x & ((ulong)1U << y)) != 0;
        }
        public static bool HasBit(short x, byte y)
        {
            return (x & ((short)1U << y)) != 0;
        }
        public static bool HasBit(ushort x, byte y)
        {
            return (x & ((ushort)1U << y)) != 0;
        }
        public static bool HasBit(byte x, byte y)
        {
            return (x & ((byte)1U << y)) != 0;
        }
        public static bool HasBit(sbyte x, byte y)
        {
            return (x & ((sbyte)1U << y)) != 0;
        }



        /**
         * Set a bit in a variable.
         *
         * This function sets a bit in a variable. The variable is changed
         * and the value is also returned. Parameter y defines the bit and
         * starts at the LSB with 0.
         *
         * @param x The variable to set a bit
         * @param y The bit position to set
         * @pre y < sizeof(T) * 
         * @return The new value of the old value with the bit set
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SetBit(int x, byte y)
        {
            return (int)(x | ((int)1U << y));
        }
        public static uint SetBit(uint x, byte y)
        {
            return (uint)(x | ((uint)1U << y));
        }
        public static long SetBit(long x, byte y)
        {
            return (long)(x | ((long)1U << y));
        }
        public static ulong SetBit(ulong x, byte y)
        {
            return (ulong)(x | ((ulong)1U << y));
        }
        public static short SetBit(short x, byte y)
        {
            return (short)(x | ((short)1U << y));
        }
        public static ushort SetBit(ushort x, byte y)
        {
            return (ushort)(x | ((ushort)1U << y));
        }
        public static byte SetBit(byte x, byte y)
        {
            return (byte)(x | ((byte)1U << y));
        }
        public static sbyte SetBit(sbyte x, byte y)
        {
            return (sbyte)(x | ((sbyte)1U << y));
        }




        /**
         * Sets several bits in a variable.
         *
         * This macro sets several bits in a variable. The bits to set are provided
         * by a value. The new value is also returned.
         *
         * @param x The variable to set some bits
         * @param y The value with set bits for setting them in the variable
         * @return The new value of x
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SetBits(int x, int y)
        {
            return x |= y;
        }
        public static uint SetBits(uint x, uint y)
        {
            return x |= y;
        }
        public static long SetBits(long x, long y)
        {
            return x |= y;
        }
        public static ulong SetBits(ulong x, ulong y)
        {
            return x |= y;
        }
        public static short SetBits(short x, short y)
        {
            return x |= y;
        }
        public static ushort SetBits(ushort x, ushort y)
        {
            return x |= y;
        }
        public static byte SetBits(byte x, byte y)
        {
            return x |= y;
        }
        public static sbyte SetBits(sbyte x, sbyte y)
        {
            return x |= y;
        }

        /**
         * Clears a bit in a variable.
         *
         * This function clears a bit in a variable. The variable is
         * changed and the value is also returned. Parameter y defines the bit
         * to clear and starts at the LSB with 0.
         *
         * @param x The variable to clear the bit
         * @param y The bit position to clear
         * @pre y < sizeof(T) * 8
         * @return The new value of the old value with the bit cleared
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClrBit(int x, byte y)
        {
            return (int)(x & ~((int)1 << y));
        }
        public static uint ClrBit(uint x, byte y)
        {
            return (uint)(x & ~((uint)1 << y));
        }
        public static long ClrBit(long x, byte y)
        {
            return (long)(x & ~((long)1 << y));
        }
        public static ulong ClrBit(ulong x, byte y)
        {
            return (ulong)(x & ~((ulong)1 << y));
        }
        public static short ClrBit(short x, byte y)
        {
            return (short)(x & ~((short)1 << y));
        }
        public static ushort ClrBit(ushort x, byte y)
        {
            return (ushort)(x & ~((ushort)1 << y));
        }
        public static byte ClrBit(byte x, byte y)
        {
            return (byte)(x & ~((byte)1 << y));
        }
        public static sbyte ClrBit(sbyte x, byte y)
        {
            return (sbyte)(x & ~((sbyte)1 << y));
        }



        /**
         * Clears several bits in a variable.
         *
         * This macro clears several bits in a variable. The bits to clear are
         * provided by a value. The new value is also returned.
         *
         * @param x The variable to clear some bits
         * @param y The value with set bits for clearing them in the variable
         * @return The new value of x
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClearBits(int x, int y)
        {
            return (int)(x & ~y);
        }
        public static uint ClearBits(uint x, uint y)
        {
            return (uint)(x & ~y);
        }
        public static long ClearBits(long x, long y)
        {
            return (long)(x & ~y);
        }
        public static ulong ClearBits(ulong x, ulong y)
        {
            return (ulong)(x & ~y);
        }
        public static short ClearBits(short x, short y)
        {
            return (short)(x & ~y);
        }
        public static ushort ClearBits(ushort x, ushort y)
        {
            return (ushort)(x & ~y);
        }
        public static byte ClearBits(byte x, byte y)
        {
            return (byte)(x & ~y);
        }
        public static sbyte ClearBits(sbyte x, sbyte y)
        {
            return (sbyte)(x & ~y);
        }

        /**
         * Toggles a bit in a variable.
         *
         * This function toggles a bit in a variable. The variable is
         * changed and the value is also returned. Parameter y defines the bit
         * to toggle and starts at the LSB with 0.
         *
         * @param x The variable to toggle the bit
         * @param y The bit position to toggle
         * @pre y < sizeof(T) * 8
         * @return The new value of the old value with the bit toggled
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToggleBit(int x, byte y)
        {
            return (int)(x ^ ((int)1 << y));
        }
        public static uint ToggleBit(uint x, byte y)
        {
            return (uint)(x ^ ((uint)1 << y));
        }
        public static long ToggleBit(long x, byte y)
        {
            return (long)(x ^ ((long)1 << y));
        }
        public static ulong ToggleBit(ulong x, byte y)
        {
            return (ulong)(x ^ ((ulong)1 << y));
        }
        public static short ToggleBit(short x, byte y)
        {
            return (short)(x ^ ((short)1 << y));
        }
        public static ushort ToggleBit(ushort x, byte y)
        {
            return (ushort)(x ^ ((ushort)1 << y));
        }
        public static byte ToggleBit(byte x, byte y)
        {
            return (byte)(x ^ ((byte)1 << y));
        }
        public static sbyte ToggleBit(sbyte x, byte y)
        {
            return (sbyte)(x ^ ((sbyte)1 << y));
        }




        /**
         * Returns the first non-zero bit in a 6-bit value (from right).
         *
         * Returns the position of the first bit that is not zero, counted from the
         * LSB. Ie, 110100 returns 2, 000001 returns 0, etc. When x == 0 returns
         * 0.
         *
         * @param x The 6-bit value to check the first zero-bit
         * @return The first position of a bit started from the LSB or 0 if x is 0.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte FIND_FIRST_BIT(int x) => _ffb_64[x];

        /**
         * Finds the position of the first non-zero bit in an integer.
         *
         * This function returns the position of the first bit set in the
         * integer. It does only check the bits of the bitmask
         * 0x3F3F (0011111100111111) and checks only the
         * bits of the bitmask 0x3F00 if and only if the
         * lower part 0x00FF is 0. This results the bits at 0x00C0 must
         * be also zero to check the bits at 0x3F00.
         *
         * @param value The value to check the first bits
         * @return The position of the first bit which is set
         * @see FIND_FIRST_BIT
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte FindFirstBit2x64(int value)
{
	if ((value & 0xFF) == 0) {
		return (byte)(FIND_FIRST_BIT((value >> 8) & 0x3F) + 8);
	} else {
		return FIND_FIRST_BIT(value & 0x3F);
	}
}

        /**
         * Clear the first bit in an integer.
         *
         * This function returns a value where the first bit (from LSB)
         * is cleared.
         * So, 110100 returns 110000, 000001 returns 000000, etc.
         *
         * @param value The value to clear the first bit
         * @return The new value with the first bit cleared
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static int KillFirstBit(int value)
        {
            return (int)(value & (value - 1));
        }
        public static uint KillFirstBit(uint value)
        {
            return (uint)(value & (value - 1));
        }
        public static long KillFirstBit(long value)
        {
            return (long)(value & (value - 1));
        }
        public static ulong KillFirstBit(ulong value)
        {
            return (ulong)(value & (value - 1));
        }
        public static short KillFirstBit(short value)
        {
            return (short)(value & (value - 1));
        }
        public static ushort KillFirstBit(ushort value)
        {
            return (ushort)(value & (value - 1));
        }
        public static byte KillFirstBit(byte value)
        {
            return (byte)(value & (value - 1));
        }
        public static sbyte KillFirstBit(sbyte value)
        {
            return (sbyte)(value & (value - 1));
        }



        /**
         * Counts the number of set bits in a variable.
         *
         * @param value the value to count the number of bits in.
         * @return the number of bits.
         */

        /* This loop is only called once for every bit set by clearing the lowest
 * bit in each loop. The number of bits is therefore equal to the number of
 * times the loop was called. It was found at the following website:
 * http://graphics.stanford.edu/~seander/bithacks.html */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static uint CountBits(int value)
        {
            uint num;

            for (num = 0; value != 0; num++)
            {
                value &= (int)(value - 1);
            }

            return num;
        }

        public static uint CountBits(uint value)
        {
            uint num;

            for (num = 0; value != 0; num++)
            {
                value &= (uint)(value - 1);
            }

            return num;
        }

        public static uint CountBits(long value)
        {
            uint num;

            for (num = 0; value != 0; num++)
            {
                value &= (long)(value - 1);
            }

            return num;
        }

        public static uint CountBits(ulong value)
        {
            uint num;

            for (num = 0; value != 0; num++)
            {
                value &= (ulong)(value - 1);
            }

            return num;
        }

        public static uint CountBits(short value)
        {
            uint num;

            for (num = 0; value != 0; num++)
            {
                value &= (short)(value - 1);
            }

            return num;
        }

        public static uint CountBits(ushort value)
        {
            uint num;

            for (num = 0; value != 0; num++)
            {
                value &= (ushort)(value - 1);
            }

            return num;
        }

        public static uint CountBits(byte value)
        {
            uint num;

            for (num = 0; value != 0; num++)
            {
                value &= (byte)(value - 1);
            }

            return num;
        }

        public static uint CountBits(sbyte value)
        {
            uint num;

            for (num = 0; value != 0; num++)
            {
                value &= (sbyte)(value - 1);
            }

            return num;
        }



        /**
         * Test whether \a value has exactly 1 bit set
         *
         * @param value the value to test.
         * @return does \a value have exactly 1 bit set?
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static bool HasExactlyOneBit(int value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        public static bool HasExactlyOneBit(uint value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        public static bool HasExactlyOneBit(long value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        public static bool HasExactlyOneBit(ulong value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        public static bool HasExactlyOneBit(short value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        public static bool HasExactlyOneBit(ushort value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        public static bool HasExactlyOneBit(byte value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }
        public static bool HasExactlyOneBit(sbyte value)
        {
            return value != 0 && (value & (value - 1)) == 0;
        }



        /**
         * Test whether \a value has at most 1 bit set
         *
         * @param value the value to test.
         * @return does \a value have at most 1 bit set?
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static bool HasAtMostOneBit(int value)
        {
            return (value & (value - 1)) == 0;
        }
        public static bool HasAtMostOneBit(uint value)
        {
            return (value & (value - 1)) == 0;
        }
        public static bool HasAtMostOneBit(long value)
        {
            return (value & (value - 1)) == 0;
        }
        public static bool HasAtMostOneBit(ulong value)
        {
            return (value & (value - 1)) == 0;
        }
        public static bool HasAtMostOneBit(short value)
        {
            return (value & (value - 1)) == 0;
        }
        public static bool HasAtMostOneBit(ushort value)
        {
            return (value & (value - 1)) == 0;
        }
        public static bool HasAtMostOneBit(byte value)
        {
            return (value & (value - 1)) == 0;
        }
        public static bool HasAtMostOneBit(sbyte value)
        {
            return (value & (value - 1)) == 0;
        }



        /**
         * ROtate \a x Left by \a n
         *
         * @note Assumes a byte has 8 bits
         * @param x The value which we want to rotate
         * @param n The number how many we want to rotate
         * @pre n < sizeof(T) * 8
         * @return A bit rotated number
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static int ROL(int x, byte n)
        {
            return (int)(x << n | x >> (sizeof(int) * 8 - n));
        }
        public static uint ROL(uint x, byte n)
        {
            return (uint)(x << n | x >> (sizeof(uint) * 8 - n));
        }
        public static long ROL(long x, byte n)
        {
            return (long)(x << n | x >> (sizeof(long) * 8 - n));
        }
        public static ulong ROL(ulong x, byte n)
        {
            return (ulong)(x << n | x >> (sizeof(ulong) * 8 - n));
        }
        public static short ROL(short x, byte n)
        {
            return (short)(x << n | x >> (sizeof(short) * 8 - n));
        }
        public static ushort ROL(ushort x, byte n)
        {
            return (ushort)(x << n | x >> (sizeof(ushort) * 8 - n));
        }
        public static byte ROL(byte x, byte n)
        {
            return (byte)(x << n | x >> (sizeof(byte) * 8 - n));
        }
        public static sbyte ROL(sbyte x, byte n)
        {
            return (sbyte)(x << n | x >> (sizeof(sbyte) * 8 - n));
        }





        /**
         * ROtate \a x Right by \a n
         *
         * @note Assumes a byte has 8 bits
         * @param x The value which we want to rotate
         * @param n The number how many we want to rotate
         * @pre n < sizeof(T) * 8
         * @return A bit rotated number
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ROR(int x, byte n)
        {
            return (int)(x >> n | x << (sizeof(int) * 8 - n));
        }
        public static uint ROR(uint x, byte n)
        {
            return (uint)(x >> n | x << (sizeof(uint) * 8 - n));
        }
        public static long ROR(long x, byte n)
        {
            return (long)(x >> n | x << (sizeof(long) * 8 - n));
        }
        public static ulong ROR(ulong x, byte n)
        {
            return (ulong)(x >> n | x << (sizeof(ulong) * 8 - n));
        }
        public static short ROR(short x, byte n)
        {
            return (short)(x >> n | x << (sizeof(short) * 8 - n));
        }
        public static ushort ROR(ushort x, byte n)
        {
            return (ushort)(x >> n | x << (sizeof(ushort) * 8 - n));
        }
        public static byte ROR(byte x, byte n)
        {
            return (byte)(x >> n | x << (sizeof(byte) * 8 - n));
        }
        public static sbyte ROR(sbyte x, byte n)
        {
            return (sbyte)(x >> n | x << (sizeof(sbyte) * 8 - n));
        }
        

        /**
         * Do an operation for each set bit in a value.
         *
         * This macros is used to do an operation for each set
         * bit in a variable. The second parameter is a
         * variable that is used as the bit position counter.
         * The fourth parameter is an expression of the bits
         * we need to iterate over. This expression will be
         * evaluated once.
         *
         * @param Tbitpos_type Type of the position counter variable.
         * @param bitpos_var   The position counter variable.
         * @param Tbitset_type Type of the bitset value.
         * @param bitset_value The bitset value which we check for bits.
         *
         * @see FOR_EACH_SET_BIT
         */
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //TODO FOR_EACH_SET_BIT_EX
        //#define FOR_EACH_SET_BIT_EX(Tbitpos_type, bitpos_var, Tbitset_type, bitset_value) \
        //	for (                                                                           \
        //		Tbitset_type ___FESBE_bits = (bitpos_var = (Tbitpos_type)0, bitset_value);    \
        //		___FESBE_bits != (Tbitset_type)0;                                             \
        //		___FESBE_bits = (Tbitset_type)(___FESBE_bits >> 1), bitpos_var++              \
        //	)                                                                               \
        //		if ((___FESBE_bits & 1) != 0)

        /**
         * Do an operation for each set set bit in a value.
         *
         * This macros is used to do an operation for each set
         * bit in a variable. The first parameter is a variable
         * that is used as the bit position counter.
         * The second parameter is an expression of the bits
         * we need to iterate over. This expression will be
         * evaluated once.
         *
         * @param bitpos_var   The position counter variable.
         * @param bitset_value The value which we check for set bits.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FOR_EACH_SET_BIT(uint bitpos_var, uint bitset_value, Action action)
        {
            for (uint bits = bitpos_var = 0;
                bits != (uint) 0;
                bits = (uint) (bits >> 1))
            {
                bitpos_var++;
                if ((bits & 1) != 0)
                {
                    action();
                } 
            }
        }

        /**
         * Perform a 32 bits endianness bitswap on x.
         * @param x the variable to bitswap
         * @return the bitswapped value.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint BSWAP32(uint x)
	{
		return ((x >> 24) & 0xFF) | ((x >> 8) & 0xFF00) | ((x << 8) & 0xFF0000) | ((x << 24) & 0xFF000000);
	}

        /**
         * Perform a 16 bits endianness bitswap on x.
         * @param x the variable to bitswap
         * @return the bitswapped value.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort BSWAP16(ushort x)
	{
		return (ushort)((x >> 8) | (x << 8));
	}


        //FROM bitmath_func.cpp
        /** Lookup table to check which bit is set in a 6 bit variable */
        static readonly byte[] _ffb_64 = new byte[64] {
    0,  0,  1,  0,  2,  0,  1,  0,
    3,  0,  1,  0,  2,  0,  1,  0,
    4,  0,  1,  0,  2,  0,  1,  0,
    3,  0,  1,  0,  2,  0,  1,  0,
    5,  0,  1,  0,  2,  0,  1,  0,
    3,  0,  1,  0,  2,  0,  1,  0,
    4,  0,  1,  0,  2,  0,  1,  0,
    3,  0,  1,  0,  2,  0,  1,  0,
};

        /**
         * Search the first set bit in a 32 bit variable.
         *
         * This algorithm is a static implementation of a log
         * congruence search algorithm. It checks the first half
         * if there is a bit set search there further. And this
         * way further. If no bit is set return 0.
         *
         * @param x The value to search
         * @return The position of the first bit set
         */
        public static byte FindFirstBit(uint x)
        {
            if (x == 0) return 0;
            /* The macro FIND_FIRST_BIT is better to use when your x is
              not more than 128. */

            byte pos = 0;

            if ((x & 0x0000ffff) == 0) { x >>= 16; pos += 16; }
            if ((x & 0x000000ff) == 0) { x >>= 8; pos += 8; }
            if ((x & 0x0000000f) == 0) { x >>= 4; pos += 4; }
            if ((x & 0x00000003) == 0) { x >>= 2; pos += 2; }
            if ((x & 0x00000001) == 0) { pos += 1; }

            return pos;
        }

        /**
         * Search the last set bit in a 64 bit variable.
         *
         * This algorithm is a static implementation of a log
         * congruence search algorithm. It checks the second half
         * if there is a bit set search there further. And this
         * way further. If no bit is set return 0.
         *
         * @param x The value to search
         * @return The position of the last bit set
         */
        public static byte FindLastBit(long x)
        {
            if (x == 0) return 0;

            byte pos = 0;

            //if ((x & 0xffffffff00000000) != 0) { x >>= 32; pos += 32; }
            if ((x & ~0x00000000ffffffff) != 0) { x >>= 32; pos += 32; }
            if ((x & 0x00000000ffff0000) != 0) { x >>= 16; pos += 16; }
            if ((x & 0x000000000000ff00) != 0) { x >>= 8; pos += 8; }
            if ((x & 0x00000000000000f0) != 0) { x >>= 4; pos += 4; }
            if ((x & 0x000000000000000c) != 0) { x >>= 2; pos += 2; }
            if ((x & 0x0000000000000002) != 0) { pos += 1; }

            return pos;
        }

    }
}
/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file random_func.hpp Pseudo random number generator. */

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Nopenttd.Core
{

/**
 * Structure to encapsulate the pseudo random number generators.
 */

    struct Randomizer
    {
        /** The state of the randomizer */
        private uint stateS;
        private uint stateT;

        ///Random used in the game state calculations
        private static Randomizer _random;

        ///Random used every else where is does not (directly) influence the game state
        private static Randomizer _interactive_random;


        /**
         * Saves the current seeds
         * @param storage Storage for saving
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SaveRandomSeeds(SavedRandomSeeds storage)
        {
            storage.random = _random;
            storage.interactive_random = _interactive_random;
        }

        /**
         * Restores previously saved seeds
         * @param storage Storage where SaveRandomSeeds() stored th seeds
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RestoreRandomSeeds(SavedRandomSeeds storage)
        {
            _random = storage.random;
            _interactive_random = storage.interactive_random;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Random()
        {
            return _random.Next();
        }

        /**
         * Pick a random number between 0 and \a limit - 1, inclusive. That means 0
         * can be returned and \a limit - 1 can be returned, but \a limit can not be
         * returned.
         * @param limit Limit for the range to be picked from.
         * @return A random number in [0,\a limit).
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RandomRange(uint limit)
        {
            return _random.Next(limit);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint InteractiveRandom()
        {
            return _interactive_random.Next();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint InteractiveRandomRange(uint limit)
        {
            return _interactive_random.Next(limit);
        }

        /**
         * Checks if a given randomize-number is below a given probability.
         *
         * This function is used to check if the given probability by the fraction of (a/b)
         * is greater than low 16 bits of the given randomize-number r.
         *
         * Do not use this function twice on the same random 16 bits as it will yield
         * the same result. One can use a random number for two calls to Chance16I,
         * where one call sends the low 16 bits and the other the high 16 bits.
         *
         * @param a The numerator of the fraction
         * @param b The denominator of the fraction, must of course not be null
         * @param r The given randomize-number
         * @return True if the probability given by r is less or equal to (a/b)
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance16I(uint a, uint b, uint r)
        {
            Debug.Assert(b != 0);
            return (((ushort) r * b + b / 2) >> 16) < a;
        }

        /**
         * Flips a coin with given probability.
         *
         * This function returns true with (a/b) probability.
         *
         * @see Chance16I()
         * @param a The nominator of the fraction
         * @param b The denominator of the fraction
         * @return True with (a/b) probability
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance16(uint a, uint b)
        {
            return Chance16I(a, b, Random());
        }

        /**
         * Flips a coin with a given probability and saves the randomize-number in a variable.
         *
         * This function uses the same parameters as Chance16. The third parameter
         * must be a variable the randomize-number from Random() is saved in.
         *
         * The low 16 bits of r will already be used and can therefore not be passed to
         * Chance16I. One can only send the high 16 bits to Chance16I.
         *
         * @see Chance16I()
         * @param a The numerator of the fraction
         * @param b The denominator of the fraction
         * @param r The variable to save the randomize-number from Random()
         * @return True in (a/b) percent
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance16R(uint a, uint b, ref uint r)
        {
            r = Random();
            return Chance16I(a, b, r);
        }



        /* $Id$ */

        /*
         * This file is part of OpenTTD.
         * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
         * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
         * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
         */

        /** @file random_func.cpp Implementation of the pseudo random generator. */
       

        /**
         * Generate the next pseudo random number
         * @return the random number
         */
        public uint Next()
        {
            uint s = stateS;
            uint t = stateT;

            stateS = s + BitMath.RotateRight(t ^ 0x1234567F, 7) + 1;
            return stateT = BitMath.RotateRight(s, 3) - 1;
        }

        /**
         * Generate the next pseudo random number scaled to \a limit, excluding \a limit
         * itself.
         * @param limit Limit of the range to be generated from.
         * @return Random number in [0,\a limit)
         */
        public uint Next(uint limit)
        {
            return (uint)((ulong)Next() * (ulong)limit) >> 32;
        }

        /**
         * (Re)set the state of the random number generator.
         * @param seed the new state
         */
        public void SetSeed(uint seed)
        {
            stateS = seed;
            stateT = seed;
        }

        /**
         * (Re)set the state of the random number generators.
         * @param seed the new state
         */
        public void SetRandomSeed(uint seed)
        {
            _random.SetSeed(seed);
            _interactive_random.SetSeed(seed * 0x1234567);
        }
    }
}
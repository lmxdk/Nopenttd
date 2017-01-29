/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file sort_func.hpp Functions related to sorting operations. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Nopenttd.Core
{

    public static class Sort
    {
        /**
         * Type safe qsort()
         *
         * @note Use this sort for irregular sorted data.
         *
         * @param base Pointer to the first element of the array to be sorted.
         * @param num Number of elements in the array pointed by base.
         * @param comparator Function that compares two elements.
         * @param desc Sort descending.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QSortT<T>(T[] arr, uint num, IComparer<T> comparer, bool desc = false)
        {
            if (num < 2)
            {
                return;
            }

            Array.Sort(arr, comparer);

            if (desc)
            {
                Array.Reverse(arr);
            }
        }

        /**
         * Type safe Gnome Sort.
         *
         * This is a slightly modified Gnome search. The basic
         * Gnome search tries to sort already sorted list parts.
         * The modification skips these.
         *
         * @note Use this sort for presorted / regular sorted data.
         *
         * @param base Pointer to the first element of the array to be sorted.
         * @param num Number of elements in the array pointed by base.
         * @param comparator Function that compares two elements.
         * @param desc Sort descending.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GSortT<T>(T[] arr, uint num, IComparer<T> comparer, bool desc = false)
        {
            if (num < 2)
            {
                return;
            }
            
            Debug.Assert(arr != null);
            Debug.Assert(comparer != null);

            uint a = 0;
            uint b = 1;
            uint offset = 0;

            while (num > 1)
            {
                int diff = comparer.Compare(arr[a], arr[b]);
                if ((!desc && diff <= 0) || (desc && diff >= 0))
                {
                    if (offset != 0)
                    {
                        /* Jump back to the last direction switch point */
                        a += offset;
                        b += offset;
                        offset = 0;
                        continue;
                    }

                    a++;
                    b++;
                    num--;
                }
                else
                {
                    arr.Swap(a, b);

                    if (a == 0)
                    {
                        continue;
                    }

                    a--;
                    b--;
                    offset++;
                }
            }
        }
    }
}
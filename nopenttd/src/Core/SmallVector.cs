/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file smallvec_type.hpp Simple vector class that allows allocating an item without the need to copy this->data needlessly. */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Nopenttd.Core
{ 

/**
 * Simple vector template class.
 *
 * @note There are no asserts in the class so you have
 *       to care about that you grab an item which is
 *       inside the list.
 *
 * @tparam T The type of the items stored
 * @tparam S The steps of allocation
 */
 //template <typename T, uint S>
    public class SmallVector<T>
    {

        protected T[] data = null;

        ///< The pointer to the first item
        protected uint items = 0;

        ///< The number of items stored
        protected uint capacity = 0;

        ///< The available space for storing items

        public SmallVector()
        {
        }

        /**
         * Copy constructor.
         * @param other The other vector to copy.
         */

        public SmallVector(SmallVector<T> other)
        {
            Assign(other);
        }

        /**
         * Assign items from other vector.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Assign(SmallVector<T> other)
        {
            if (Object.ReferenceEquals(other, this))
            {
                return;
            }

            Clear();
            if (other.Length() > 0)
            {
                EnsureCapacity(other.Length());
                Array.Copy(other.data, data, other.Length());
            }
        }

        /**
         * Remove all items from the list.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Clear()
        {
            /* In fact we just reset the item counter avoiding the need to
             * probably reallocate the same amount of memory the list was
             * previously using. */
            items = 0;
        }

        /**
         * Remove all items from the list and free allocated memory.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            items = 0;
            capacity = 0;
            data = null;
        }

        /**
         * Compact the list down to the smallest block size boundary.
         */

        public void Compact()
        {
            if (newCapacity >= capacity)
            {
                return;
            }

            capacity = newCapacity;
            data = ReallocT(data, capacity);
        }

        /**
         * Append an item and return it.
         * @param to_add the number of items to append
         * @return pointer to newly allocated item
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(T item)
        {
            EnsureCapacity(1);
            data[items++] = item;        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(uint toAdd = 1)
        {
            uint newCount = items + toAdd;
            if (newCount > capacity)
            {
                capacity = newCount;
                var oldData = data;
                data = new T[capacity];

                if (items > 0)
                {
                    Array.Copy(oldData, data, items);
                }
            }
        }

        /**
         * Set the size of the vector, effectively truncating items from the end or appending uninitialised ones.
         * @param num_items Target size.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resize(uint numItems)
        {
            var oldItems = items;    
            capacity = items = capacity;
            var oldData = data;
            data = new T[capacity];

            if (items > 0)
            {
                Array.Copy(oldData, data, Math.Min(items, oldItems));
            }
        }


        /**
         * Search for the first occurrence of an item.
         * The '!=' operator of T is used for comparison.
         * @param item Item to search for
         * @return The position of the item, or -1 when not present
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int FindIndex(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < items; i++)
            {
                if (comparer.Equals(data[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        /**
         * Tests whether a item is present in the vector.
         * The '!=' operator of T is used for comparison.
         * @param item Item to test for
         * @return true iff the item is present
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item)
        {
            return FindIndex(item) != -1;
        }

        /**
         * Removes given item from this vector
         * @param item item to remove
         * @note it has to be pointer to item in this map. It is overwritten by the last item.
         * NOTE LMK changed from taken T item to uint index
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Erase(uint index)
        {
            Debug.Assert(index >= 0 && index < data.Length);
            data[index] = data[--items];
        }

        /**
         * Remove items from the vector while preserving the order of other items.
         * @param pos First item to remove.
         * @param count Number of consecutive items to remove.
         */

        public void ErasePreservingOrder(uint pos, uint count = 1)
        {
            if (count == 0) return;
            Debug.Assert(pos < items);
            Debug.Assert(pos + count <= items);
            items -= count;
            uint to_move = items - pos;
            if (to_move > 0)
            {
                Array.Copy(data, pos + count, data, pos, count);
            }
        }

        /**
         * Tests whether a item is present in the vector, and appends it to the end if not.
         * The '!=' operator of T is used for comparison.
         * @param item Item to test for
         * @return true iff the item is was already present
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Include(T item)
        {
            var isMember = Contains(item);
            if (isMember == false)
            {
                Append(item);
            }
            return isMember;
        }

        /**
         * Get the number of items in the list.
         *
         * @return The number of items in the list.
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint Length() => items;

/**
 * Get the pointer to the first item (const)
 *
 * @return the pointer to the first item
 */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Begin()
        {
            return data[0];
        }

        /**
         * Get the pointer to item "number" (const)
         *
         * @param index the position of the item
         * @return the pointer to the item
         */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get(uint index)
        {
            /* Allow access to the 'first invalid' item */
            Debug.Assert(index <= items);
            return data[index];
        }

        /**
         * Get item "number"
         *
         * @param index the position of the item
         * @return the item
         */
        public T this[uint index]
        {
            get
            {
                Debug.Assert(index <= items);
                return data[index];
            }
        }
    }
};

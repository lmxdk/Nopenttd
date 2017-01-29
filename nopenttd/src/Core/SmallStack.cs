/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file smallstack_type.hpp Minimal stack that uses a pool to avoid pointers and doesn't allocate any heap memory if there is only one valid item. */

/**
 * A simplified pool which stores values instead of pointers and doesn't
 * redefine operator new/delete. It also never zeroes memory and always reuses
 * it.
 */
//template<typename TItem, typename uint, uint Tgrowth_step, uint max_size>

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Nopenttd.Core;

public class SimplePool<TItem> {

	private uint growthStep;
	private uint maxSize;
    private uint first_unused;
    private uint first_free;
    private readonly Mutex mutex;
    private SmallVector<SimplePoolPoolItem> data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SimplePool()
    {
        first_unused = 0;
        first_free = 0;
        mutex = new Mutex();
    }
	~SimplePool() { delete this->mutex; }

    /**
	 * Get the mutex. We don't lock the mutex in the pool methods as the
	 * SmallStack isn't necessarily in a consistent state after each method.
	 * @return Mutex.
	 */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mutex GetMutex() { return mutex; }

    /**
	 * Get the item at position index.
	 * @return Item at index.
	 */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TItem Get(uint index) { return data[index].value; }

    /**
	 * Create a new item and return its index.
	 * @return Index of new item.
	 */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Create()
	{
		uint index = FindFirstFree();
		if (index < maxSize) {
			data[index].valid = true;
			first_free = index + 1;
			first_unused = Math.Max(first_unused, first_free);
		}
		return index;
	}

    /**
	 * Destroy (or rather invalidate) the item at the given index.
	 * @param index Index of item to be destroyed.
	 */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Destroy(uint index)
	{
		data[index].valid = false;
		first_free = Math.Min(first_free, index);
	}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private uint FindFirstFree()
	{
		uint index = first_free;
		for (; index < first_unused; index++)
        {
		    if (!data[index].valid)
		    {
		        return index;
		    }
		}

		if (index >= data.Length() && index < maxSize)
        {
			data.Resize(index + 1);
		}
		return index;
	}

	struct SimplePoolPoolItem : IPoolItem<TItem>
	{
	    public TItem value { get; set; }
		public bool valid;
	}
}

interface IPoolItem<TItem>
{
    TItem value;
    bool valid;
}

/**
 * Base class for SmallStack. We cannot add this into SmallStack itself as
 * certain compilers don't like it.
 */
//template <typename TItem, typename uint>

/**
	 * SmallStack item that can be kept in a pool.
	 */
public struct PooledSmallStack<TItem>
{
    public TItem value;
    public bool valid;
    public uint branch_count; /// Number of branches in the tree structure this item is parent of
};

public class SmallStackPool<TItem> : SmallStack<PooledSmallStack<TItem>>
{
}

/**
 * Minimal stack that uses a pool to avoid pointers. It has some peculiar
 * properties that make it useful for passing around lists of IDs but not much
 * else:
 * 1. It always includes an invalid item as bottom.
 * 2. It doesn't have a deep copy operation but uses smart pointers instead.
 *    Every copy is thus implicitly shared.
 * 3. Its items are immutable.
 * 4. Due to 2. and 3. memory management can be done by "branch counting".
 *    Whenever you copy a smallstack, the first item on the heap increases its
 *    branch_count, signifying that there are multiple items "in front" of it.
 *    When deleting a stack items are deleted up to the point where
 *    branch_count > 0.
 * 5. You can choose your own index type, so that you can align it with your
 *    value type. E.G. value types of 16 bits length like to be combined with
 *    index types of the same length.
 * 6. All accesses to the underlying pool are guarded by a mutex and atomic in
 *    the sense that the mutex stays locked until the pool has reacquired a
 *    consistent state. This means that even though a common data structure is
 *    used the SmallStack is still reentrant.
 * @tparam TItem Value type to be used.
 * @tparam uint Index type to use for the pool.
 * @tparam Tinvalid Invalid item to keep at the bottom of each stack.
 * @tparam Tgrowth_step Growth step for pool.
 * @tparam max_size Maximum size for pool.
 */
//template <typename TItem, typename uint, TItem Tinvalid, uint Tgrowth_step, uint max_size>
public class SmallStack<TItem> {

    public TItem value;
    public bool valid;
    private uint maxSize;

    /**
	 * Constructor for a stack with one or two items in it.
	 * @param value Initial item. If not missing or Tinvalid there will be Tinvalid below it.
	 */

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SmallStack(TItem value, uint maxSize) //maxsize var ikke med
    {
        this.value = value;
        this.maxSize = maxSize;

    }

    /**
	 * Remove the head of stack and all other items members that are unique to it.
	 */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    ~SmallStack()
	{
		/* Pop() locks the mutex and after each pop the pool is consistent.*/
	    while (next != maxSize)
	    {
	        Pop();
	    }
	}

    /**
	 * Shallow copy the stack, marking the first item as branched.
	 * @param other Stack to copy from
	 */

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SmallStack(SmallStack other)
    {
        this.value = other.value;
        Branch();
    }

    //   /**
    // * Shallow copy the stack, marking the first item as branched.
    // * @param other Stack to copy from
    // * @return This smallstack.
    // */
    //   [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //   SmallStack operator=(const SmallStack &other)
    //{
    //	if (this == &other) return *this;
    //	while (this->next != max_size) this->Pop();
    //	this->next = other.next;
    //	this->value = other.value;
    //	/* Deleting and branching are independent operations, so it's fine to
    //	 * acquire separate locks for them. */
    //	this->Branch();
    //	return *this;
    //}

    /**
	 * Pushes a new item onto the stack if there is still space in the
	 * underlying pool. Otherwise the topmost item's value gets overwritten.
	 * @param item Item to be pushed.
	 */
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Push(TItem item)
	{
		if (value != Tinvalid) {
			ThreadMutexLocker lock(_pool.GetMutex());
			uint new_item = _pool.Create();
			if (new_item != max_size) {
				PooledSmallStack &pushed = _pool.Get(new_item);
				pushed.value = this->value;
				pushed.next = this->next;
				pushed.branch_count = 0;
				this->next = new_item;
			}
		}
		this->value = item;
	}

	/**
	 * Pop an item from the stack.
	 * @return Current top of stack.
	 */
	inline TItem Pop()
	{
		TItem ret = this->value;
		if (this->next == max_size) {
			this->value = Tinvalid;
		} else {
			ThreadMutexLocker lock(_pool.GetMutex());
			PooledSmallStack &popped = _pool.Get(this->next);
			this->value = popped.value;
			if (popped.branch_count == 0) {
				_pool.Destroy(this->next);
			} else {
				--popped.branch_count;
				/* We can't use Branch() here as we already have the mutex.*/
				if (popped.next != max_size) {
					++(_pool.Get(popped.next).branch_count);
				}
			}
			/* Accessing popped here is no problem as the pool will only set
			 * the validity flag, not actually delete the item, on Destroy().
			 * It's impossible for another thread to acquire the same item in
			 * the mean time because of the mutex. */
			this->next = popped.next;
		}
		return ret;
	}

	/**
	 * Check if the stack is empty.
	 * @return If the stack is empty.
	 */
	inline bool IsEmpty() const
	{
		return this->value == Tinvalid && this->next == max_size;
	}

	/**
	 * Check if the given item is contained in the stack.
	 * @param item Item to look for.
	 * @return If the item is in the stack.
	 */
	inline bool Contains(const TItem &item) const
	{
		if (item == Tinvalid || item == this->value) return true;
		if (this->next != max_size) {
			ThreadMutexLocker lock(_pool.GetMutex());
			const SmallStack *in_list = this;
			do {
				in_list = static_cast<const SmallStack *>(
						static_cast<const Item *>(&_pool.Get(in_list->next)));
				if (in_list->value == item) return true;
			} while (in_list->next != max_size);
		}
		return false;
	}

protected:
	static SmallStackPool _pool;

	/**
	 * Create a branch in the pool if necessary.
	 */
	inline void Branch()
	{
		if (this->next != max_size) {
			ThreadMutexLocker lock(_pool.GetMutex());
			++(_pool.Get(this->next).branch_count);
		}
	}
};

#endif

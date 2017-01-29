using System.Runtime.CompilerServices;

namespace Nopenttd.Core
{
    /**
 * Base class for SmallStack. We cannot add this into SmallStack itself as
 * certain compilers don't like it.
 */
    //template <typename TItem, typename uint>

    public struct SmallStackItem<TItem>
    {
        public uint next; /// Pool index of next item.
        public TItem value; /// Value of current item.

        /**
         * Create a new item.
         * @param value Value of the item.
         * @param next Next item in the stack.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SmallStackItem(TItem value, uint next)
        {
            this.next = next;
            this.value = value;
        }
    }
}
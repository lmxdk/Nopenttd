using System.Runtime.CompilerServices;

namespace Nopenttd.Core
{
    /**
     * Simple vector template class, with automatic free.
     *
     * @note There are no asserts in the class so you have
     *       to care about that you grab an item which is
     *       inside the list.
     *
     * @param T The type of the items stored, must be a pointer
     * @param S The steps of allocation
     */
    //TODO DELETE
    public class AutoFreeSmallVector<T> : SmallVector<T> where T : new() {

        ~AutoFreeSmallVector()
        {
            Clear();
        }

        /**
         * Remove all items from the list.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Clear()
        {
            for (uint i = 0; i < items; i++)
            {
                data[i] = default(T);
            }

            items = 0;
        }
    };
}
// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using System.Collections.Generic;

namespace AdventOfCode
{
    /// <summary>
    /// Contains extensions of the List class
    /// </summary>
    internal static class ListExtensions
    {
        /// <summary>
        /// Swaps two elements in a list
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list instance</param>
        /// <param name="a">The index of the first item</param>
        /// <param name="b">The index of the second item</param>
        public static void Swap<T>(this List<T> list, int a, int b)
        {
            T tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }
    }
}

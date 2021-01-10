// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using System.Collections.Generic;

namespace AdventOfCode.Puzzles
{
    /// <summary>
    /// Contains common utility functions
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Determines if the two matrices are equal
        /// </summary>
        /// <param name="a">The first matrix to test</param>
        /// <param name="b">The second matrix to test</param>
        /// <returns>true if every member of the matrix is equal; otherwise, false</returns>
        public static bool AreMatricesEqual<T>(T[,] a, T[,] b)
        {
            for (int i = 0; i <= a.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= a.GetUpperBound(1); j++)
                {
                    if (!a[i,j].Equals(b[i,j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Builds all permutations of the items in the specified list
        /// </summary>
        /// <typeparam name="T">The type contained in the list</typeparam>
        /// <param name="list">The list to permute</param>
        /// <returns>The list of all permutations of the original list</returns>
        public static List<List<T>> BuildPermutations<T>(List<T> list)
        {
            var orders = new List<List<T>>();
            BuildPermutations(list, orders, 0, list.Count - 1);
            return orders;
        }

        private static void BuildPermutations<T>(List<T> list, List<List<T>> orders, int k, int m)
        {
            if (k == m)
            {
                orders.Add(new List<T>(list));
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    list.Swap(k, i);
                    BuildPermutations(list, orders, k + 1, m);
                    list.Swap(k, i);
                }
            }
        }

        /// <summary>
        /// Flips the specified matrix about the horizontal (first dimension) axis
        /// </summary>
        /// <typeparam name="T">The type of the items in the matrix</typeparam>
        /// <param name="matrix">The matrix to flip</param>
        /// <returns>The flipped matrix</returns>
        public static T[,] Flip<T>(T[,] matrix)
        {
            int n = matrix.GetUpperBound(0) + 1;
            var flipped = new T[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    flipped[i, j] = matrix[i, n - j - 1];
                }
            }
            return flipped;
        }

        /// <summary>
        /// Flops the specified matrix about the vertical (second dimension) axis
        /// </summary>
        /// <typeparam name="T">The type of the items in the matrix</typeparam>
        /// <param name="matrix">The matrix to flop</param>
        /// <returns>The flopped matrix</returns>
        public static T[,] Flop<T>(T[,] matrix)
        {
            int n = matrix.GetUpperBound(0) + 1;
            var flipped = new T[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    flipped[i, j] = matrix[n - i - 1, j];
                }
            }
            return flipped;
        }

        /// <summary>
        /// Rotates the specified matrix 90° clockwise
        /// </summary>
        /// <typeparam name="T">The type of the items in the matrix</typeparam>
        /// <param name="matrix">The matrix to rotate</param>
        /// <returns>The rotated matrix</returns>
        public static T[,] RotateClockwise<T>(T[,] matrix)
        {
            int n = matrix.GetUpperBound(0) + 1;
            var rotated = new T[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    rotated[i, j] = matrix[n - j - 1, i];
                }
            }
            return rotated;
        }


    }
}

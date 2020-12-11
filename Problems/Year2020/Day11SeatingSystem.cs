using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AdventOfCode.Problems.Year2020
{
    class Day11SeatingSystem : ProblemBase<int>
    {
        public Day11SeatingSystem(ILogger logger) : base(logger, "Seating System", 2020, 11) { }

        protected override int ExecutePart1()
        {
            bool?[,] matrix = ParseLayout();
            matrix = Stabilize(matrix, ApplyAdjacencyRules);
            return CountOccupiedSeats(matrix);
        }

        protected override int ExecutePart2()
        {
            bool?[,] matrix = ParseLayout();
            matrix = Stabilize(matrix, ApplyLineOfSightRules);
            return CountOccupiedSeats(matrix);
        }

        private static bool?[,] Stabilize(bool?[,] matrix, ApplyRulesDelegate applyRules)
        {
            bool?[,] oldMatrix;
            do
            {
                oldMatrix = (bool?[,])matrix.Clone();
                matrix = applyRules(matrix);
            } while (!AreMatricesEqual(oldMatrix, matrix));
            return matrix;
        }

        private static bool AreMatricesEqual(bool?[,] a, bool?[,] b)
        {
            for (int i = 0; i <= a.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= a.GetUpperBound(1); j++)
                {
                    if (a[i,j] != b[i,j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private delegate bool?[,] ApplyRulesDelegate(bool?[,] matrix);

        private static bool?[,] ApplyAdjacencyRules(bool?[,] matrix)
        {
            var newMatrix = (bool?[,])matrix.Clone();
            for (int i = 0; i <= matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= matrix.GetUpperBound(1); j++)
                {
                    if (matrix[i,j].HasValue && matrix[i,j].Value &&
                        CountAdjacentOccupiedSeats(matrix, i, j) >= 4)
                    {
                        newMatrix[i, j] = false;
                    }
                    else if (matrix[i,j].HasValue && 
                        CountAdjacentOccupiedSeats(matrix, i, j) == 0)
                    {
                        newMatrix[i, j] = true;
                    }
                }
            }
            return newMatrix;
        }

        private static int CountAdjacentOccupiedSeats(bool?[,] matrix, int row, int col)
        {
            int occupied = 0;
            for (int i = row - 1; i <= row + 1; i++)
            {
                if (i < 0 || i > matrix.GetUpperBound(0))
                {
                    continue;
                }
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (j < 0 || j > matrix.GetUpperBound(1) || (i == row && j == col))
                    {
                        continue;
                    }
                    if (matrix[i,j].HasValue && matrix[i,j].Value)
                    {
                        occupied++;
                    }
                }
            }
            return occupied;
        }

        private static bool?[,] ApplyLineOfSightRules(bool?[,] matrix)
        {
            var newMatrix = (bool?[,])matrix.Clone();
            for (int i = 0; i <= matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= matrix.GetUpperBound(1); j++)
                {
                    if (matrix[i,j].HasValue && matrix[i,j].Value &&
                        CountLineOfSightOccupiedSeats(matrix, i, j) >= 5)
                    {
                        newMatrix[i, j] = false;
                    }
                    else if (matrix[i,j].HasValue && 
                        CountLineOfSightOccupiedSeats(matrix, i, j) == 0)
                    {
                        newMatrix[i, j] = true;
                    }
                }
            }
            return newMatrix;
        }

        private static int CountLineOfSightOccupiedSeats(bool?[,] matrix, int row, int col)
        {
            int occupied = 0;

            foreach (var (deltaRow, deltaCol) in LineOfSightDeltas)
            {
                if (IsLineOfSightOccupied(matrix, row, col, deltaRow, deltaCol))
                {
                    occupied++;
                }
            }

            return occupied;
        }

        private static readonly List<(int deltaRow, int deltaCol)> LineOfSightDeltas =
            new List<(int deltaRow, int deltaCol)>()
            {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1), (0, 1),
                (1, -1), (1, 0), (1, 1)
            };

        private static bool IsLineOfSightOccupied(bool?[,] matrix, int row, int col, int deltaRow, int deltaCol)
        {
            bool occupied = false;
            row += deltaRow;
            col += deltaCol;
            while (row >= 0 && row <= matrix.GetUpperBound(0) &&
                   col >= 0 && col <= matrix.GetUpperBound(1)) 
            {
                if (matrix[row, col].HasValue)
                {
                    occupied = matrix[row, col].Value;
                    break;
                }
                row += deltaRow;
                col += deltaCol;
            }

            return occupied;
        }

        private static int CountOccupiedSeats(bool?[,] matrix)
        {
            int occupied = 0;
            for (int i = 0; i <= matrix.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= matrix.GetUpperBound(1); j++)
                {
                    if (matrix[i,j].HasValue && matrix[i,j].Value)
                    {
                        occupied++;
                    }
                }
            }
            return occupied;
        }

        private static bool?[,] ParseLayout()
        {
            bool?[,] matrix = new bool?[seatLayouts.Length, seatLayouts[0].Length];
            for (int i = 0; i < seatLayouts.Length; i++)
            { 
                for (int j = 0; j < seatLayouts[i].Length; j++)
                {
                    if (seatLayouts[i][j] == 'L')
                    {
                        matrix[i,j] = false;
                    }
                }
            }
            return matrix;
        }

        #region Data

        private static readonly string[] seatLayouts =
        {
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLL.LLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLL..LLLL.LLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLL.LLLLLLLL.L",
            "LLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "......L.LL.....L.L..L.....L.L..LL.L.LL.L...L..L..............L..L......LL.....LL...L.L.....",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLLLLLLL..LLLLLLLLLLLL.LLLLLLLL.LLLLLLLL..LLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLL.LLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL..LLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "...L..L....LL.L.L......LLLLL.....LL....L.......L.LL.L.L.L..LL...LLLLLL.LL........L.L.......",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            ".LLLLLLLL.LLLL..LLLL.LLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLL.LLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LL.LLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLL.LL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.L.LL.LLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLLL.LLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "......L.L..L.L...LLL.....L.L..L......LL...L..L..L.L......L.L..L....LLLL....L..L....L.L..L.L",
            "LLLLLL.LL.LLLLLLLLLL.LLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLL.LLL.LLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLL.LLLL.LLLLLLLLL..LLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLL.LLL.LLLLLLLLLLL.LLL.LL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLL.LL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLL.LLLL.LLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLL.LLLLLLLL.LLLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.L.LL.LLLLL.LLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            ".LLL......L........L.....L..LL.L..L..L.L....L.L............L...........LLL..L..LL....LL..L.",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLL.LLLLLLLL.LLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLL..LLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLL.L.LLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            ".......LL..L.LLL.L....L..L....L......L..L...LL...L.L...LL....L.L........L...L...L.L.......L",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLL..LLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLL.LLLLLLLLLLLL.LL.LLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            ".L..LLL.L....LL.L..L..LL..L.LL.L......L........LLL.....L..LL...L.LL.LL.....L....L..LL.L....",
            "LLLLLLLLLLL.LL.LLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLL.LLLLLL.LLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLL.LLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLL.LLLLLLL..LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "L..L.LL....L..LL.L.....L.....L...L..L..L...L....L.L.L..LL.L......L...L......L....L.....L.L.",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLL.LLLL.LLLLLL.LLLLLLLLLLLLLLL..LLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.L.LLL.LLLLLL.LLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLL.L.LLLLL.LLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            ".L..L.L...LLL.LL.L.L...L........LLLLL.L..L......L.....L....L.LL.L.LLL...LL..LL.LL..LL.....L",
            "LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLL..LLLLLLL.LLLLLLLL",
            "..L.L..L...L..L.L........L.L.LLL..L.L...LLL..L..........L........L....LLLL.LL...L......LL..",
            "LLLLLLLLL.LLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLL.LLLLLL.LLLLLLLL.LLLLLLLL..LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLLLL.LLL",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLL.LL.LLLLLLLLLLLLLLLLLL.LLLLLLL.LLLLLLLL",
            "....LL.L..L...LL......L.....L..LL.L.......LL...L....LLLLL..L..L.LLL.......LL..LL...LL.....L",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLL.L.LLLLLL",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLL.L.LLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLL.LLLLLLL.",
            ".L.LL..L.LLL.L...L.......L..LLL.....L...LL...LLL...L.LL............L.L....LL.....L.L..LL..L",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLL.LLLL.LLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLLLLL.LLLLL",
            "LLLLLLLLL.LLLL.LLLLL.L.L.LLLLLLLLLLL.LLLL.L.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLLLLLLL.L.LLLLLLLL.LLLLLLLLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLL.LLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLL..L....L.LL..L.L.LLL.LL..L.......L.LL..L..L....L..L...L...L.L..L.L.L.L.L.L.LLL.L.....L.L",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLL.LL.LLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLL.LLLLLL.LL.LLLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLL.LLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLL.LLLLLLLLLLLLLLL.LLLL.LLLLLL.LLLLLLLLLL.LLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            ".L..L..LL.L..L..L.L.L.LL..L....L.....L.LL..........L..L...LLL.L....LL...LL...L.LL.L.....LL.",
            "LLLL.LLLL.LLLL.LLLLLLLLLLLLLLLLLLLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLL",
            "LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLLLLLLL.LLLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL",
            ".LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLL.LLLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLLLLLLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLLLLLLLLL",
            "LLLLLLLLLL.LLL.LLLLL.LLLLLL.LLLLLLLLLLLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL.LL.L.LLLLLL.LL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLLL",
            "LLLLLLLLL.LLLL..LLLL.LLLLLL.LLLLLLLL.LL.L.LLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLLLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLL.LLLLL.LLLLLL.LLLLLLLL.LLLLLLLLLLL.LLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLL.LLLLLLLL",
            "LLLLLLLLL.LLLLLLLLLL.LLLLLL.LLLLL.LLLLLLL.LL.LLL.LLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLL..LLLLLLLL",
            "LLLLLLLLLLLLLL.LLLLL.LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL.LLLLLLLL.LLLLLLLLL.LLLLLLLLLL.LLLLL"
        };

        #endregion Data
    }
}

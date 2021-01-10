// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AdventOfCode.Puzzles.Year2020
{
    /// <summary>
    /// Day 11: Seating System
    /// https://adventofcode.com/2020/day/11
    /// </summary>
    [TestClass]
    public class Day11SeatingSystem
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(37, ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(2275, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(26, ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(2121, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="seatLayout">The layout of the seats</param>
        /// <returns>The number of occupied seats</returns>
        private static int ExecutePart1(string[] seatLayout)
        {
            bool?[,] matrix = ParseLayout(seatLayout);
            matrix = Stabilize(matrix, ApplyAdjacencyRules);
            return CountOccupiedSeats(matrix);
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="seatLayout">The layout of the seats</param>
        /// <returns>The number of occupied seats</returns>
        private static int ExecutePart2(string[] seatLayout)
        {
            bool?[,] matrix = ParseLayout(seatLayout);
            matrix = Stabilize(matrix, ApplyLineOfSightRules);
            return CountOccupiedSeats(matrix);
        }

        /// <summary>
        /// Applies changes to the matrix until it no longer changes
        /// </summary>
        /// <param name="matrix">The seating matrix to modify</param>
        /// <param name="applyRules">The set of rules to apply</param>
        /// <returns>The stabilized seating matrix</returns>
        private static bool?[,] Stabilize(bool?[,] matrix, ApplyRulesDelegate applyRules)
        {
            bool?[,] oldMatrix;
            do
            {
                oldMatrix = (bool?[,])matrix.Clone();
                matrix = applyRules(matrix);
            } while (!Utils.AreMatricesEqual(oldMatrix, matrix));
            return matrix;
        }

        /// <summary>
        /// Defines a method that can apply rules to a seating matrix
        /// </summary>
        /// <param name="matrix">The seating matrix to modify</param>
        /// <returns>The modified seating matrix</returns>
        private delegate bool?[,] ApplyRulesDelegate(bool?[,] matrix);

        /// <summary>
        /// Uses adjacent seats for rules
        /// </summary>
        /// <param name="matrix">The seating matrix to modify</param>
        /// <returns>The modified seating matrix</returns>
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

        /// <summary>
        /// Counts the number of occupied seats around the specified seat
        /// </summary>
        /// <param name="matrix">The seating matrix</param>
        /// <param name="row">The row of the seat</param>
        /// <param name="col">The column of the seat</param>
        /// <returns>The number of occupied adjacent seats</returns>
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

        /// <summary>
        /// Uses line of sight for rules
        /// </summary>
        /// <param name="matrix">The matrix to modify</param>
        /// <returns>The modified matrix</returns>
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

        /// <summary>
        /// Counts the number of occupied seats in line-of-sight around the specified seat
        /// </summary>
        /// <param name="matrix">The seating matrix</param>
        /// <param name="row">The row of the seat</param>
        /// <param name="col">The column of the seat</param>
        /// <returns>The number of occupied line-of-sight seats</returns>
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

        /// <summary>
        /// The list of row/col offsets that constitute a line of sight
        /// </summary>
        private static readonly List<(int deltaRow, int deltaCol)> LineOfSightDeltas =
            new List<(int deltaRow, int deltaCol)>()
            {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1), (0, 1),
                (1, -1), (1, 0), (1, 1)
            };

        /// <summary>
        /// Determines if the seat at the end of a line of sight is occupied
        /// </summary>
        /// <param name="matrix">The seating matrix</param>
        /// <param name="row">The row of the seat</param>
        /// <param name="col">The column of the seat</param>
        /// <param name="deltaRow">The line-of-sight row delta</param>
        /// <param name="deltaCol">The line-of-sight column delta</param>
        /// <returns>true if the seat is occupied; otherwise, false</returns>
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

        /// <summary>
        /// Counts the number of occupied seats
        /// </summary>
        /// <param name="matrix">The seating matrix to count</param>
        /// <returns>The number of occupied seats</returns>
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

        /// <summary>
        /// Parses the seating layout
        /// </summary>
        /// <param name="seatLayout">The seating layout definition</param>
        /// <returns>The parsed seating layout</returns>
        private static bool?[,] ParseLayout(string[] seatLayout)
        {
            bool?[,] matrix = new bool?[seatLayout.Length, seatLayout[0].Length];
            for (int i = 0; i < seatLayout.Length; i++)
            { 
                for (int j = 0; j < seatLayout[i].Length; j++)
                {
                    if (seatLayout[i][j] == 'L')
                    {
                        matrix[i,j] = false;
                    }
                }
            }
            return matrix;
        }

        #region Data

        private static readonly string[] sampleInput =
        {
            "L.LL.LL.LL",
            "LLLLLLL.LL",
            "L.L.L..L..",
            "LLLL.LL.LL",
            "L.LL.LL.LL",
            "L.LLLLL.LL",
            "..L.L.....",
            "LLLLLLLLLL",
            "L.LLLLLL.L",
            "L.LLLLL.LL"
        };

        private static readonly string[] puzzleInput =
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

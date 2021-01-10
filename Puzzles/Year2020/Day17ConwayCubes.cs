// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

//#define PRINT

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Puzzles.Year2020
{
    /// <summary>
    /// Day 17: Conway Cubes
    /// https://adventofcode.com/2020/day/17
    /// </summary>
    [TestClass]
    public class Day17ConwayCubes
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(112, ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(372, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(848, ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(1896, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="initialState">The initial state of the conway cubes</param>
        /// <returns>The number of active cubes after 6 rounds</returns>
        private static int ExecutePart1(string[] initialState)
        {
            var grid = LoadInitialState(initialState);
            grid = ApplyRules(grid, 6, false);
            return GetActiveCount(grid);
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="initialState">The initial state of the conway cubes</param>
        /// <returns>The number of active cubes after 6 rounds</returns>
        private static int ExecutePart2(string[] initialState)
        {
            var grid = LoadInitialState(initialState);
            grid = ApplyRules(grid, 6, true);
            return GetActiveCount(grid);
        }

        /// <summary>
        /// Applies the on/off rules to the grid
        /// </summary>
        /// <param name="grid">The grid to apply the rules to</param>
        /// <param name="numCycles">The number of cycles of rules to apply</param>
        /// <param name="useW">Whether or not to use the 4th dimension</param>
        /// <returns>The modified grid</returns>
        private static Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> 
            ApplyRules(
            Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> grid, 
            int numCycles, bool useW)
        {
            int startW = useW ? grid.Keys.Min() - 1 : 0;
            int endW = useW ? grid.Keys.Max() + 1 : 0;
            int startZ = grid[0].Keys.Min() - 1;
            int endZ = grid[0].Keys.Max() + 1;
            int start = grid[0][0].Keys.Min() - 1;
            int end = grid[0][0].Keys.Max() + 1;

            var newGrid = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>>();
            for (int w = startW; w <= endW; w++)
            {
                for (int z = startZ; z <= endZ; z++)
                {
                    for (int y = start; y <= end; y++)
                    {
                        for (int x = start; x <= end; x++)
                        {
                            bool currValue = GetCoordinateValue(grid, x, y, z, w);
                            int neighborsActive = GetNeighborActiveCount(grid, x, y, z, w);
                            if (currValue)
                            {
                                SetCoordinateValue(newGrid, x, y, z, w, neighborsActive == 2 || neighborsActive == 3);
                            }
                            else if (!currValue && neighborsActive == 3)
                            {
                                SetCoordinateValue(newGrid, x, y, z, w, true);
                            }
                            else
                            {
                                SetCoordinateValue(newGrid, x, y, z, w, currValue);
                            }
                        }
                    }
                }
            }

            PrintGrid(newGrid);
            if (numCycles > 1)
            {
                newGrid = ApplyRules(newGrid, numCycles - 1, useW);
            }
            return newGrid;
        }

        /// <summary>
        /// Gets the value for a coordinate
        /// </summary>
        /// <param name="grid">The grid containing the cubes</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        /// <param name="w">The w coordinate</param>
        /// <returns>The value of the coordinate</returns>
        private static bool GetCoordinateValue(
            Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> grid,
            int x, int y, int z, int w)
        {
            if (!grid.TryGetValue(w, out Dictionary<int, Dictionary<int, Dictionary<int, bool>>> wCube))
            {
                wCube = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();
                grid[w] = wCube;
            }
            if (!wCube.TryGetValue(z, out Dictionary<int, Dictionary<int, bool>> zPlane))
            {
                zPlane = new Dictionary<int, Dictionary<int, bool>>();
                wCube[z] = zPlane;
            }
            if (!zPlane.TryGetValue(y, out Dictionary<int, bool> yLine))
            {
                yLine = new Dictionary<int, bool>();
                zPlane[y] = yLine;
            }
            _ = yLine.TryGetValue(x, out bool value);
            return value;
        }

        /// <summary>
        /// Sets the value of a coordinate
        /// </summary>
        /// <param name="grid">The grid containing the cubes</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        /// <param name="w">The w coordinate</param>
        /// <param name="value">The value to set</param>
        private static void SetCoordinateValue(
            Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> grid, int x, 
            int y, int z, int w, bool value)
        {
            if (!grid.TryGetValue(w, out Dictionary<int, Dictionary<int, Dictionary<int, bool>>> wCube))
            {
                wCube = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();
                grid[w] = wCube;
            }
            if (!wCube.TryGetValue(z, out Dictionary<int, Dictionary<int, bool>> zPlane))
            {
                zPlane = new Dictionary<int, Dictionary<int, bool>>();
                wCube[z] = zPlane;
            }
            if (!zPlane.TryGetValue(y, out Dictionary<int, bool> yLine))
            {
                yLine = new Dictionary<int, bool>();
                zPlane[y] = yLine;
            }
            yLine[x] = value;
        }

        /// <summary>
        /// Gets the number of active cubes neighboring the specified cube
        /// </summary>
        /// <param name="grid">The grid containing the cubes</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        /// <param name="w">The w coordinate</param>
        /// <returns>The number of active neighbors</returns>
        private static int GetNeighborActiveCount(
            Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> grid,
            int x, int y, int z, int w)
        {
            int activeCount = 0;
            for (int h = w - 1; h <= w + 1; h++)
            {
                for (int i = z - 1; i <= z + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        for (int k = x - 1; k <= x + 1; k++)
                        {
                            if ((x != k || y != j || z != i || w != h) && 
                                GetCoordinateValue(grid, k, j, i, h))
                            {
                                activeCount++;
                            }
                        }
                    }
                }
            }
            return activeCount;
        }

        /// <summary>
        /// Gets the total number of active cubes in the grid
        /// </summary>
        /// <param name="grid">The grid to count</param>
        /// <returns>The number of active cubes</returns>
        private static int GetActiveCount(
            Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> grid)
        {
            int activeCount = 0;
            foreach (var wCube in grid)
            {
                foreach (var zPlane in wCube.Value)
                {
                    foreach (var yLine in zPlane.Value)
                    {
                        foreach (var xPos in yLine.Value)
                        {
                            if (xPos.Value)
                            {
                                activeCount++;
                            }
                        }
                    }
                }
            }
            return activeCount;
        }

        /// <summary>
        /// Loads the initial state of the grid
        /// </summary>
        /// <param name="initialState">The initial grid state definition</param>
        /// <returns>The grid</returns>
        private static Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> 
            LoadInitialState(string[] initialState)
        {
            var grid = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>>();
            int startIndex = initialState.Length / -2;
            int y = startIndex;
            foreach (string row in initialState)
            {
                int x = startIndex;
                foreach (char col in row)
                {
                    SetCoordinateValue(grid, x, y, 0, 0, col == '#');
                    x++;
                }
                y++;
            }
            PrintGrid(grid);
            return grid;
        }

        /// <summary>
        /// Prints out the grid to the console
        /// </summary>
        /// <param name="grid">The grid to print</param>
        private static void PrintGrid(
            Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> grid)
        {
#if PRINT
            int startW = grid.Keys.Min();
            int endW = grid.Keys.Max();
            int startZ = grid[0].Keys.Min();
            int endZ = grid[0].Keys.Max();
            int start = grid[0][0].Keys.Min();
            int end = grid[0][0].Keys.Max();

            var row = new System.Text.StringBuilder();
            for (int w = startW; w <= endW; w++)
            {
                for (int z = startZ; z <= endZ; z++)
                {
                    System.Console.WriteLine("");
                    System.Console.WriteLine($"z={z}, w={w}");
                    for (int y = start; y <= end; y++)
                    {
                        row.Clear();
                        for (int x = start; x <= end; x++)
                        {
                            row.Append(GetCoordinateValue(grid, x, y, z, w) ? '#' : '.');
                        }
                        System.Console.WriteLine(row.ToString());
                    }
                }
            }
#endif
        }

        #region Data

        private static readonly string[] sampleInput =
        {
            ".#.",
            "..#",
            "###"
        };

        private static readonly string[] puzzleInput =
        {
            ".##.####",
            ".#.....#",
            "#.###.##",
            "#####.##",
            "#...##.#",
            "#######.",
            "##.#####",
            ".##...#."
        };

        #endregion Data
    }
}

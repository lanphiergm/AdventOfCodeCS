using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Problems.Year2020
{
    class Day17ConwayCubes : ProblemBase<int>
    {
        public Day17ConwayCubes(ILogger logger) : base(logger, "Conway Cubes", 2020, 17) { }

        private Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>> grid =
            new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, bool>>>>();

        protected override int ExecutePart1()
        {
            LoadInitialState();
            ApplyRules(6, false);
            return GetActiveCount();
        }

        protected override int ExecutePart2()
        {
            LoadInitialState();
            ApplyRules(6, true);
            return GetActiveCount();
        }

        private void ApplyRules(int numCycles, bool useW)
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
                            bool currValue = GetCoordinateValue(x, y, z, w);
                            int neighborsActive = GetNeighborActiveCount(x, y, z, w);
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

            grid = newGrid;
            PrintGrid();
            if (numCycles > 1)
            {
                ApplyRules(numCycles - 1, useW);
            }
        }

        private bool GetCoordinateValue(int x, int y, int z, int w)
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

        private int GetNeighborActiveCount(int x, int y, int z, int w)
        {
            int activeCount = 0;
            for (int h = w - 1; h <= w; h++)
            {
                for (int i = z - 1; i <= z + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        for (int k = x - 1; k <= x + 1; k++)
                        {
                            if ((x != k || y != j || z != i || w != h) && GetCoordinateValue(k, j, i, h))
                            {
                                activeCount++;
                            }
                        }
                    }
                }
            }
            return activeCount;
        }

        private int GetActiveCount()
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

        private void LoadInitialState()
        {
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
            PrintGrid();
        }

        private void PrintGrid()
        {
            int startW = grid.Keys.Min();
            int endW = grid.Keys.Max();
            int startZ = grid[0].Keys.Min();
            int endZ = grid[0].Keys.Max();
            int start = grid[0][0].Keys.Min();
            int end = grid[0][0].Keys.Max();

            var row = new StringBuilder();
            for (int w = startW; w <= endW; w++)
            {
                for (int z = startZ; z <= endZ; z++)
                {
                    Logger.LogInformation("");
                    Logger.LogInformation($"z={z}, w={w}");
                    for (int y = start; y <= end; y++)
                    {
                        row.Clear();
                        for (int x = start; x <= end; x++)
                        {
                            row.Append(GetCoordinateValue(x, y, z, w) ? '#' : '.');
                        }
                        Logger.LogInformation(row.ToString());
                    }
                }
            }
        }

        private static readonly string[] initialState =
        {
            ".#.",
            "..#",
            "###",
        };

        private static readonly string[] realinitialState =
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
    }
}

// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Puzzles.Year2020
{
    /// <summary>
    /// Day 20: Jurassic Jigsaw
    /// https://adventofcode.com/2020/day/20
    /// </summary>
    [TestClass]
    public class Day20JurassicJigsaw
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(20899048083289L, ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(45079100979683L, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(273, ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(1946, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="tileData">The raw tile data</param>
        /// <returns>The product of the IDs of the corner tiles</returns>
        private static long ExecutePart1(string[] tileData)
        {
            var tiles = Initialize(tileData);
            var corners = tiles.Where(t => t.IsCorner);
            Assert.AreEqual(4, corners.Count());
            long cornerProduct = 1;
            foreach (var tile in corners)
            {
                cornerProduct *= tile.Id;
            }
            return cornerProduct;
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="tileData">The raw tile data</param>
        /// <returns>The number of waves in the image, not counting sea monsters</returns>
        private static int ExecutePart2(string[] tileData)
        {
            var tiles = Initialize(tileData);
            var image = AssembleImage(tiles);
            int seaMonsterCount = 0;
            bool[,] waves = null;
            // There are only sea monsters in one orientation of the image. These loops will 
            // orient the image into all 8 possible positions and break out when we get a non-zero
            // count
            for (int flip = 0; flip < 2; flip++)
            {
                for (int rotate = 0; rotate < 4; rotate++)
                {
                    (seaMonsterCount, waves) = CountSeaMonsters(image);
                    if (seaMonsterCount > 0)
                    {
                        break;
                    }
                    image = Utils.RotateClockwise(image);
                }
                if (seaMonsterCount > 0)
                {
                    break;
                }
                image = Utils.Flip(image);
            }

            // The sea monsters have been removed from this image so we just need to sum up the
            // true values to get the wave count
            int waveCount = 0;
            for (int i = 0; i <= waves.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= waves.GetUpperBound(1); j++)
                {
                    if (waves[i, j])
                    {
                        waveCount++;
                    }
                }
            }
            return waveCount;
        }

        /// <summary>
        /// Assembles the tiles into a single grid with the borders removed
        /// </summary>
        /// <param name="tiles">The tiles with edges matched</param>
        /// <returns>The assembled image</returns>
        private static bool[,] AssembleImage(List<Tile> tiles)
        {
            int tileGridSize = (int)Math.Sqrt(tiles.Count);
            var image = new bool[tileGridSize * 8, tileGridSize * 8];
            Tile rowStart = tiles.First(t => t.IsCorner);
            while (rowStart.Top.Neighbor != null || rowStart.Left.Neighbor != null)
            {
                rowStart.RotateClockwise();
            }

            Tile currTile;
            for (int yTile = tileGridSize - 1; yTile >= 0; yTile--)
            {
                currTile = rowStart;
                for (int xTile = 0; xTile < tileGridSize; xTile++)
                {
                    //Copy this tile's data
                    for (int y = 1; y < 9; y++)
                    {
                        for (int x = 1; x < 9; x++)
                        {
                            image[xTile * 8 + x - 1, yTile * 8 + y - 1] = currTile.Pixels[x, y];
                        }
                    }

                    //Prepare the next tile to the right
                    var nextTile = currTile.Right.Neighbor?.Tile;
                    if (nextTile != null)
                    {
                        while (!ReferenceEquals(currTile.Right, nextTile.Left.Neighbor))
                        {
                            nextTile.RotateClockwise();
                        }
                        if (nextTile.Left.IsInverted)
                        {
                            nextTile.FlipY();
                        }
                    }
                    currTile = nextTile;
                }

                //Prepare the next tile down
                var nextRow = rowStart.Bottom.Neighbor?.Tile;
                if (nextRow != null)
                {
                    while (!ReferenceEquals(rowStart.Bottom, nextRow.Top.Neighbor))
                    {
                        nextRow.RotateClockwise();
                    }
                    if (rowStart.Bottom.IsInverted)
                    {
                        nextRow.FlipX();
                    }
                    Assert.IsNull(nextRow.Left.Neighbor);
                }
                rowStart = nextRow;
            }

            return image;
        }

        /// <summary>
        /// Counts the number of sea monsters in the image and returns the resulting image when
        /// the sea monsters have been removed
        /// </summary>
        /// <param name="image">The image to search</param>
        /// <returns>The number of sea monsters and the image without any sea monsters</returns>
        private static (int count, bool[,] waves)  CountSeaMonsters(bool[,] image)
        {
            int count = 0;
            for (int y = 0; y < image.GetUpperBound(1) - 2; y++)
            {
                for (int x = 0; x < image.GetUpperBound(0) - 18; x++)
                {
                    bool found = true;
                    foreach (var (xOffset, yOffset) in seaMonsterOffsets)
                    {
                        if (!image[x + xOffset, y + yOffset])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        count++;
                        foreach (var (xOffset, yOffset) in seaMonsterOffsets)
                        {
                            image[x + xOffset, y + yOffset] = false;
                        }
                    }
                }
            }
            return (count, image);
        }

        /// <summary>
        /// Matches up the edges of the tiles
        /// </summary>
        /// <param name="tiles"></param>
        private static void MatchEdges(List<Tile> tiles)
        {
            // Go through each tile
            for (int i = 0; i < tiles.Count - 1; i++)
            {
                // Go through each edge of the current tile that hasn't been matched yet
                foreach (Edge edge in tiles[i].Edges.Where(e => e.Neighbor == null))
                {
                    // Go through each tile after this one
                    for (int j = i + 1; j < tiles.Count; j++)
                    {
                        // Go through each edge of the following tile that hasn't been matched yet
                        foreach (Edge otherEdge in tiles[j].Edges.Where(e => e.Neighbor == null))
                        {
                            if (edge == otherEdge)
                            {
                                edge.Neighbor = otherEdge;
                                otherEdge.Neighbor = edge;
                                break;
                            }
                        }
                        if (edge.Neighbor != null)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// A tile of the overall image
        /// </summary>
        private class Tile
        {
            private const int TOP = 0;
            private const int RIGHT = 1;
            private const int BOTTOM = 2;
            private const int LEFT = 3;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="id">The ID of the tile</param>
            public Tile(int id)
            {
                Id = id;
            }

            /// <summary>
            /// The ID of the tile
            /// </summary>
            public int Id { get; }

            /// <summary>
            /// The pixels in the image
            /// </summary>
            public bool[,] Pixels { get; private set; } = new bool[10, 10];

            /// <summary>
            /// The edges of the tile
            /// </summary>
            public Edge[] Edges { get; } = new Edge[4];

            /// <summary>
            /// The top edge
            /// </summary>
            public Edge Top => Edges[TOP];

            /// <summary>
            /// The right edge
            /// </summary>
            public Edge Right => Edges[RIGHT];

            /// <summary>
            /// The bottom edge
            /// </summary>
            public Edge Bottom => Edges[BOTTOM];

            /// <summary>
            /// The left edge
            /// </summary>
            public Edge Left => Edges[LEFT];

            /// <summary>
            /// Whether or not this tile is a corner
            /// </summary>
            public bool IsCorner => Edges.Where(e => e.Neighbor == null).Count() == 2;

            /// <summary>
            /// Sets the edges from the pixels
            /// </summary>
            public void InitializeEdges()
            {
                var top = new Edge(this);
                var right = new Edge(this);
                var bottom = new Edge(this);
                var left = new Edge(this);
                for (int i = 0; i < 10; i++)
                {
                    top.Pixels[i] = Pixels[i, 9];
                    right.Pixels[i] = Pixels[9, 9 - i];
                    bottom.Pixels[i] = Pixels[9 - i, 0];
                    left.Pixels[i] = Pixels[0, i];
                }
                Edges[TOP] = top;
                Edges[RIGHT] = right;
                Edges[BOTTOM] = bottom;
                Edges[LEFT] = left;
            }

            /// <summary>
            /// Flips the X values of the image (about the Y axis)
            /// </summary>
            public void FlipX()
            {
                Pixels = Utils.Flop(Pixels);
                Edge temp = Edges[LEFT];
                Edges[LEFT] = Edges[RIGHT];
                Edges[RIGHT] = temp;

                foreach (var edge in Edges)
                {
                    edge.Reverse();
                }
            }

            /// <summary>
            /// Flips the Y values of the image (about the X axis)
            /// </summary>
            public void FlipY()
            {
                Pixels = Utils.Flip(Pixels);
                Edge temp = Edges[TOP];
                Edges[TOP] = Edges[BOTTOM];
                Edges[BOTTOM] = temp;

                foreach (var edge in Edges)
                {
                    edge.Reverse();
                }
            }

            /// <summary>
            /// Rotates the image clockwise 90°
            /// </summary>
            public void RotateClockwise()
            {
                Pixels = Utils.RotateClockwise(Pixels);
                Edge temp = Edges[TOP];
                Edges[TOP] = Edges[LEFT];
                Edges[LEFT] = Edges[BOTTOM];
                Edges[BOTTOM] = Edges[RIGHT];
                Edges[RIGHT] = temp;
            }

        }

        /// <summary>
        /// An edge of a tile
        /// </summary>
        private class Edge
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="tile">The tile to which this edge belongs</param>
            public Edge(Tile tile)
            {
                Tile = tile;
            }

            /// <summary>
            /// The pixels of this edge
            /// </summary>
            public bool[] Pixels { get; private set; } = new bool[10];

            /// <summary>
            /// The neighboring edge to this edge
            /// </summary>
            public Edge Neighbor { get; set; }

            /// <summary>
            /// The tile to which this edge belongs
            /// </summary>
            public Tile Tile { get; }

            /// <summary>
            /// Whether or not this tile and its neighbor are inverted
            /// </summary>
            public bool IsInverted => Neighbor != null && Enumerable.SequenceEqual(Pixels, Neighbor.Pixels);

            /// <summary>
            /// Reverses the direction of the edge
            /// </summary>
            public void Reverse()
            {
                Pixels = Pixels.Reverse().ToArray();
            }

            /// <summary>
            /// Equality operator
            /// </summary>
            /// <param name="a">The left edge to compare</param>
            /// <param name="b">The right edge to compare</param>
            /// <returns>true if the edges match in either direction; otherwise, false</returns>
            public static bool operator ==(Edge a, Edge b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }
                else if (a is null || b is null)
                {
                    return false;
                }

                return Enumerable.SequenceEqual(a.Pixels, b.Pixels) ||
                       Enumerable.SequenceEqual(a.Pixels, b.Pixels.Reverse());
            }

            /// <summary>
            /// Inequality operator
            /// </summary>
            /// <param name="a">The left edge to compare</param>
            /// <param name="b">The right edge to compare</param>
            /// <returns>false if the edges match in either direction; otherwise, true</returns>
            public static bool operator !=(Edge a, Edge b) => !(a == b);

            /// <summary>
            /// Determines whether the specified object is equal to the current object
            /// </summary>
            /// <param name="obj">The object to compare with the current object</param>
            /// <returns>true if the specified object is equal to the current object; otherwise, false</returns>
            public override bool Equals(object obj) => obj is Edge other && this == other;

            /// <summary>
            /// Serves as the default hash function
            /// </summary>
            /// <returns>A hash code for the current object</returns>
            public override int GetHashCode() => Pixels.GetHashCode();
        }

        /// <summary>
        /// Initializes the tiles from the raw tile data
        /// </summary>
        /// <param name="tileData">The raw tile data</param>
        /// <returns>The initialized list of tiles with edges matched</returns>
        private static List<Tile> Initialize(string[] tileData)
        {
            var tiles = new List<Tile>();
            for (int row = 0; row < tileData.Length; row += 12)
            {
                var tile = new Tile(int.Parse(tileData[row][5..^1]));
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (tileData[row + i + 1][j] == '#')
                        {
                            tile.Pixels[j, 9 - i] = true;
                        }
                    }
                }
                tile.InitializeEdges();
                tiles.Add(tile);
            }
            MatchEdges(tiles);
            return tiles;
        }

        /// <summary>
        /// Prints an image to the console for debugging
        /// </summary>
        /// <param name="image">The image to print</param>
        private static void PrintImage(bool[,] image)
        {
            for (int i = image.GetUpperBound(1); i >= 0; i--)
            {
                var row = new StringBuilder();
                for (int j = 0; j <= image.GetUpperBound(0); j++)
                {
                    row.Append(image[j, i] ? '#' : '.');
                }
                Console.WriteLine(row.ToString());
            }
            Console.WriteLine(string.Empty);
        }

        #region Data

        private static readonly (int xOffset, int yOffset)[] seaMonsterOffsets =
        {
            (18, 2),
            (0, 1), (5, 1), (6, 1), (11, 1), (12, 1), (17, 1), (18, 1), (19, 1),
            (1, 0), (4, 0), (7, 0), (10, 0), (13, 0), (16, 0)
        };

        private static readonly string[] sampleInput =
        {
            "Tile 2311:",
            "..##.#..#.",
            "##..#.....",
            "#...##..#.",
            "####.#...#",
            "##.##.###.",
            "##...#.###",
            ".#.#.#..##",
            "..#....#..",
            "###...#.#.",
            "..###..###",
            "",
            "Tile 1951:",
            "#.##...##.",
            "#.####...#",
            ".....#..##",
            "#...######",
            ".##.#....#",
            ".###.#####",
            "###.##.##.",
            ".###....#.",
            "..#.#..#.#",
            "#...##.#..",
            "",
            "Tile 1171:",
            "####...##.",
            "#..##.#..#",
            "##.#..#.#.",
            ".###.####.",
            "..###.####",
            ".##....##.",
            ".#...####.",
            "#.##.####.",
            "####..#...",
            ".....##...",
            "",
            "Tile 1427:",
            "###.##.#..",
            ".#..#.##..",
            ".#.##.#..#",
            "#.#.#.##.#",
            "....#...##",
            "...##..##.",
            "...#.#####",
            ".#.####.#.",
            "..#..###.#",
            "..##.#..#.",
            "",
            "Tile 1489:",
            "##.#.#....",
            "..##...#..",
            ".##..##...",
            "..#...#...",
            "#####...#.",
            "#..#.#.#.#",
            "...#.#.#..",
            "##.#...##.",
            "..##.##.##",
            "###.##.#..",
            "",
            "Tile 2473:",
            "#....####.",
            "#..#.##...",
            "#.##..#...",
            "######.#.#",
            ".#...#.#.#",
            ".#########",
            ".###.#..#.",
            "########.#",
            "##...##.#.",
            "..###.#.#.",
            "",
            "Tile 2971:",
            "..#.#....#",
            "#...###...",
            "#.#.###...",
            "##.##..#..",
            ".#####..##",
            ".#..####.#",
            "#..#.#..#.",
            "..####.###",
            "..#.#.###.",
            "...#.#.#.#",
            "",
            "Tile 2729:",
            "...#.#.#.#",
            "####.#....",
            "..#.#.....",
            "....#..#.#",
            ".##..##.#.",
            ".#.####...",
            "####.#.#..",
            "##.####...",
            "##..#.##..",
            "#.##...##.",
            "",
            "Tile 3079:",
            "#.#.#####.",
            ".#..######",
            "..#.......",
            "######....",
            "####.#..#.",
            ".#...#.##.",
            "#.#####.##",
            "..#.###...",
            "..#.......",
            "..#.###..."
        };

        private static readonly string[] puzzleInput =
        {
            "Tile 3209:",
            ".##.####..",
            "....#....#",
            "#.##.#....",
            "#.....#...",
            "#..#......",
            ".##.##..##",
            "##.#......",
            "#.######..",
            "....##..#.",
            "....######",
            "",
            "Tile 1409:",
            ".##.####..",
            "...#...#.#",
            "#....#.#..",
            "#...##.##.",
            ".###....#.",
            "..#....#..",
            "##...#....",
            ".....#....",
            "......#..#",
            ".#.#.#####",
            "",
            "Tile 1039:",
            "##...#.##.",
            "..#.##..##",
            "#......#.#",
            "#.#.#....#",
            "####...#.#",
            "#..#.....#",
            "#.....#...",
            ".#..#...##",
            "..#....##.",
            "#.#.#.####",
            "",
            "Tile 3037:",
            "#..##.#.##",
            "##.......#",
            ".#......##",
            ".........#",
            "..........",
            "...#.#....",
            "#...####.#",
            "...#..#.##",
            ".........#",
            "#..###.#..",
            "",
            "Tile 1427:",
            "#..#.#..#.",
            "......##..",
            "#.......##",
            "..#..#....",
            "#...###.#.",
            ".......#.#",
            ".#.......#",
            ".#...##...",
            "......#...",
            ".#.#.#....",
            "",
            "Tile 2879:",
            "###..#.#..",
            "#....##...",
            "..#..#....",
            "#.....#...",
            ".#.......#",
            "#......#.#",
            ".#.#.....#",
            "#.....#..#",
            "..........",
            "####.##...",
            "",
            "Tile 3617:",
            "...#.#..#.",
            "#...#....#",
            "...#.#...#",
            "#.....#..#",
            "#........#",
            "......#..#",
            "#.......##",
            "....#....#",
            ".##......#",
            "##.####..#",
            "",
            "Tile 1801:",
            "..###...##",
            "#.###.....",
            ".###....##",
            "#...#.....",
            "#........#",
            ".#.##...##",
            "#.#..#...#",
            "###.#....#",
            "#.#......#",
            "##.##.##.#",
            "",
            "Tile 3221:",
            ".##.###.##",
            "#..##....#",
            "#.#.#.#.#.",
            "#...#.#...",
            "#.....##..",
            "......#..#",
            "..........",
            "#...#.##..",
            "...#...#..",
            ".###...##.",
            "",
            "Tile 1987:",
            ".##.###.##",
            ".##....###",
            "#..#.#.##.",
            ".#..##...#",
            ".....##..#",
            ".......#..",
            "#......##.",
            "#.#.....##",
            "......##.#",
            "###...#...",
            "",
            "Tile 2917:",
            "#.#..#..##",
            "......##..",
            ".#........",
            "##.......#",
            ".#.......#",
            "..#.#.#...",
            "...#.#...#",
            "...##..##.",
            "........##",
            ".#.#.#.#.#",
            "",
            "Tile 1297:",
            "#..#.##...",
            "...##..#.#",
            "....#...##",
            "##.#.#.##.",
            "#.......##",
            "#.........",
            "####......",
            "#.........",
            "#.........",
            ".#....###.",
            "",
            "Tile 1613:",
            "#...####..",
            ".....#.###",
            ".....#.##.",
            "#.#......#",
            "...##...##",
            "#.........",
            ".##..#....",
            "....#.....",
            "..........",
            "..##.##.##",
            "",
            "Tile 1481:",
            "#.##..##..",
            "#.#..#.#.#",
            "..........",
            "##..#..#..",
            "..##....#.",
            "..#...#.#.",
            "#..#......",
            "##.....#..",
            "...#.....#",
            ".#.####..#",
            "",
            "Tile 3779:",
            "##.#......",
            "...#.....#",
            "...#......",
            "##.##.#..#",
            "##......##",
            "#..#...#..",
            "....#.#.##",
            "#......#.#",
            ".......#.#",
            "#...###..#",
            "",
            "Tile 3121:",
            ".#.#..#.##",
            "#..#.#.#..",
            "....#.....",
            "#..#.#.#..",
            ".......###",
            "#.#......#",
            "#.....#..#",
            "#.#.#....#",
            "...#.#....",
            ".##..#.##.",
            "",
            "Tile 1913:",
            "##..##...#",
            ".......##.",
            "##..#..#..",
            "##..#....#",
            "..........",
            "#.#...#..#",
            ".........#",
            ".......###",
            "......#.##",
            ".#...##.#.",
            "",
            "Tile 3929:",
            ".#...#####",
            ".#.......#",
            "#...#.....",
            "..#.##....",
            "###..##...",
            "..#.....#.",
            "......#...",
            "#........#",
            ".........#",
            "##..##....",
            "",
            "Tile 3947:",
            "#...#.#..#",
            "......#...",
            "#.#...#...",
            "........#.",
            "..#.#.....",
            "#....#.##.",
            "..........",
            "##..#.....",
            "##....#.##",
            "##.###.###",
            "",
            "Tile 2663:",
            ".#.#.#.##.",
            "....###...",
            "#..#......",
            "#...#.####",
            "##......#.",
            "#.###.#..#",
            "#..#...#.#",
            "..#......#",
            "##......#.",
            "....##..##",
            "",
            "Tile 3499:",
            "########.#",
            "....#..#.#",
            ".#.##....#",
            "......#...",
            ".#..#..#..",
            "#..#.....#",
            "###...##..",
            "#..##.....",
            "#.#...#..#",
            "#..###.##.",
            "",
            "Tile 3877:",
            "..######.#",
            "..#.#....#",
            ".#........",
            "##..##..##",
            ".#.#...#.#",
            ".....#.###",
            ".........#",
            "#........#",
            ".....#....",
            "#..#.###.#",
            "",
            "Tile 2251:",
            "#.#.......",
            "..##...#.#",
            "#..#..##.#",
            ".#...#..#.",
            "##........",
            "##.##.###.",
            "..#...#...",
            "..#......#",
            "#..#..#...",
            "##..#.#...",
            "",
            "Tile 2357:",
            ".....#.#..",
            ".#.#......",
            "#...###...",
            ".#..##.#.#",
            "#........#",
            "...#....#.",
            "#.........",
            ".##.##.#.#",
            "###...####",
            "...##....#",
            "",
            "Tile 2381:",
            "...####.##",
            "#.......#.",
            "......##..",
            "..#.##.#..",
            "#..###....",
            "##.#..#...",
            ".##..#....",
            "##..#..#..",
            "#....#..#.",
            "#.#...#.#.",
            "",
            "Tile 1693:",
            "###.......",
            "#...####.#",
            "##.##.....",
            "#..#.....#",
            "#.#......#",
            "...#.#....",
            "....#.##.#",
            "..#.#.....",
            "..#.#...#.",
            ".##..###..",
            "",
            "Tile 3697:",
            "....#.#..#",
            "#.........",
            "#.#...#...",
            "..#....##.",
            "#....##.#.",
            "#........#",
            ".#...#....",
            "#...#..#..",
            "#.........",
            "###....#..",
            "",
            "Tile 3307:",
            "####..##.#",
            "........#.",
            "#........#",
            "#..##.##..",
            "#...##..#.",
            "...##.##..",
            "#.....#..#",
            "##.......#",
            "......#.##",
            "##.#..#...",
            "",
            "Tile 1823:",
            "#.##...##.",
            "......#...",
            "#.........",
            "#.........",
            "#.##....##",
            "##..##.#.#",
            ".#........",
            ".#....####",
            "#......#..",
            "#..#..##..",
            "",
            "Tile 2837:",
            ".#...#.#.#",
            "..##.....#",
            "##....##.#",
            "#.#..##.##",
            "###.#....#",
            ".#....#...",
            "..#...#...",
            ".#.#......",
            "#...#.....",
            "#....###..",
            "",
            "Tile 1483:",
            "##.##...#.",
            ".##..##...",
            ".##...#.#.",
            "#...#....#",
            "#..##..#..",
            "#.......##",
            ".....#...#",
            "..###.#..#",
            "#...##.#.#",
            "#...#.####",
            "",
            "Tile 3889:",
            ".#.#.##.#.",
            "...###.#.#",
            "#.....##.#",
            "......#...",
            ".#..#.##.#",
            "##....#...",
            ".###.....#",
            "#..#...#.#",
            "##....#...",
            "#.#..##..#",
            "",
            "Tile 2677:",
            ".......###",
            "#.#..#..#.",
            ".#...##..#",
            ".........#",
            "........##",
            ".......#.#",
            "...#...#.#",
            ".......#..",
            "..#...####",
            "..#..####.",
            "",
            "Tile 1021:",
            "#...######",
            "..#.#.....",
            "........##",
            "#....#..##",
            "#....##.#.",
            "..#......#",
            "#..#.#.#.#",
            "#.....#...",
            ".#.#......",
            ".####.###.",
            "",
            "Tile 3067:",
            "#####..#.#",
            "##..#.##..",
            "#.###...#.",
            "###...#..#",
            "#......#..",
            "###......#",
            "#.....#...",
            ".....#..##",
            "#...#..###",
            "..#.#..#..",
            "",
            "Tile 1559:",
            "..#...#.##",
            "#.......##",
            "....#....#",
            "..##.#..##",
            "##..#..#.#",
            "##.....##.",
            "....#..##.",
            "#.#...##..",
            "....#..#..",
            "#..#.....#",
            "",
            "Tile 3917:",
            "#.######..",
            ".#......#.",
            "#...#...##",
            ".......#..",
            "##......#.",
            "#.......#.",
            "#..##..###",
            "....##...#",
            ".....#....",
            "###...#...",
            "",
            "Tile 3823:",
            "...####.##",
            "....#..#..",
            "......#.#.",
            "......#...",
            "...#..#...",
            "#..##.#...",
            "..#......#",
            "#...#..###",
            ".#.......#",
            ".###..#.#.",
            "",
            "Tile 1307:",
            "...#.....#",
            "##.#.....#",
            "###......#",
            ".....###.#",
            "#..##.##..",
            "#.........",
            "..##.....#",
            "#....#..#.",
            "#.#.#....#",
            "#.##...#.#",
            "",
            "Tile 1949:",
            "....##.#.#",
            "#...#.....",
            "....#.#.#.",
            "#...#.....",
            "#..#......",
            "........##",
            ".#..#.#.##",
            ".....#...#",
            "..........",
            "..##....##",
            "",
            "Tile 1571:",
            "#..#####.#",
            ".....##.##",
            "..#.#..#..",
            "#..#.#....",
            ".....#...#",
            "...#.....#",
            ".#.....##.",
            "##...#...#",
            ".....#...#",
            ".#.#...###",
            "",
            "Tile 2777:",
            "#.###.#..#",
            "#....#..##",
            "#........#",
            "#....#..#.",
            "#.#.......",
            ".#.#...#.#",
            "..#.##....",
            "##........",
            ".....##...",
            "..#.###.##",
            "",
            "Tile 3019:",
            ".##.#####.",
            "...#..##..",
            "...#....##",
            ".........#",
            "#.........",
            "#....#...#",
            "#.........",
            ".......#.#",
            "..##.....#",
            ".##...#...",
            "",
            "Tile 2579:",
            "###.######",
            "##.....#.#",
            ".#....##.#",
            "#.#...#..#",
            "#...##....",
            "....#....#",
            "#.#...#..#",
            "##.......#",
            "###......#",
            "#..##.####",
            "",
            "Tile 2887:",
            "..##..###.",
            "####.##.##",
            ".##..#...#",
            ".#....#...",
            "..#.....##",
            "#..#......",
            "....##...#",
            "..........",
            "..##...#..",
            ".#..##....",
            "",
            "Tile 2287:",
            "..#.####..",
            "##..#.....",
            "....##..#.",
            ".....##...",
            "##..###.##",
            "#....##.#.",
            "##....#...",
            "..........",
            "...#......",
            ".#.#..#.##",
            "",
            "Tile 1637:",
            "###.#...#.",
            "#......###",
            "....###..#",
            "..#..#...#",
            "....#..#.#",
            "....#.....",
            ".........#",
            "#....#.#.#",
            "...###...#",
            ".##..#.#.#",
            "",
            "Tile 1901:",
            ".##.......",
            ".#........",
            "##..##....",
            "......#.#.",
            "....#..#..",
            "#.#...##.#",
            ".#.....#.#",
            "..........",
            "#..#..#..#",
            ".#.##....#",
            "",
            "Tile 2957:",
            "##.###..#.",
            "..###..#.#",
            "#...#.#...",
            ".....#.###",
            "#........#",
            "......#...",
            ".#......##",
            "##...#..##",
            ".#.#.#...#",
            "..###.##..",
            "",
            "Tile 2699:",
            "..#.#.###.",
            ".#...#.#.#",
            "#.....##..",
            "#...#.#..#",
            ".....#..##",
            ".....###..",
            "#..#......",
            "##.#.....#",
            "##.#..#..#",
            "#..#...#..",
            "",
            "Tile 2081:",
            "..#...#...",
            "....#.....",
            "#.#.#.....",
            "..##.##..#",
            "...#...#.#",
            ".####.....",
            ".##..#..#.",
            "#.....##..",
            ".........#",
            "#..##.###.",
            "",
            "Tile 2383:",
            ".####.###.",
            "...##....#",
            "...#....#.",
            "...#.....#",
            "##......#.",
            "...#......",
            "##.#.#...#",
            "##.#...#.#",
            "........##",
            ".##.##.#.#",
            "",
            "Tile 3407:",
            "##..#.###.",
            "#..#..#...",
            "#.......#.",
            ".#.......#",
            "###...#..#",
            "........##",
            "#..###...#",
            "#.....##.#",
            "..##......",
            ".####.....",
            "",
            "Tile 2503:",
            ".###..#...",
            ".......###",
            "##....#.#.",
            ".##.....#.",
            ".#.......#",
            "##.#..#...",
            ".#.......#",
            "....######",
            "...#..#...",
            "...######.",
            "",
            "Tile 1249:",
            "##.#.#####",
            "..#.......",
            ".#.......#",
            "..#.......",
            ".......#..",
            "###....#..",
            "...#.....#",
            ".......###",
            "#.....#..#",
            "..##...##.",
            "",
            "Tile 3541:",
            "..##.#####",
            ".....#....",
            "..##...#.#",
            "..........",
            ".#....##..",
            "..........",
            "..#...##.#",
            "##.#..#.#.",
            "......#..#",
            "..#..#...#",
            "",
            "Tile 1447:",
            ".#...####.",
            "#.#..#...#",
            ".....#.#.#",
            ".....#....",
            ".....#..#.",
            "#.....#..#",
            "..##...#.#",
            "#.##......",
            "..#...#.#.",
            ".#.##.#..#",
            "",
            "Tile 2969:",
            "#..###.#..",
            "#.#..#....",
            "#........#",
            "#..#.#.#..",
            ".#.......#",
            "..#..#.#.#",
            "#........#",
            ".........#",
            "#...#.#...",
            ".###......",
            "",
            "Tile 1621:",
            ".#.#.#....",
            ".#..#....#",
            "....##...#",
            "#.#....#..",
            "..........",
            "##.#..##.#",
            "#.#...#..#",
            "#...#.....",
            ".##....#..",
            "#..#.##.##",
            "",
            "Tile 2129:",
            "######..#.",
            "....#....#",
            "#........#",
            "#.....##..",
            "##.#...#.#",
            "##.##....#",
            "#.........",
            "##..##....",
            "#.#...#..#",
            ".#.....##.",
            "",
            "Tile 1741:",
            ".....#....",
            "#......#..",
            "........#.",
            "#....#..#.",
            ".......#.#",
            "#......#.#",
            "#...##..##",
            "##.....#..",
            "..##.#..#.",
            "....#...#.",
            "",
            "Tile 1549:",
            "#.##.#...#",
            "#.........",
            ".#.....#.#",
            ".#.#.#....",
            ".....#....",
            "......#.##",
            "......####",
            "#.#.#..#..",
            "#...#.....",
            "..#..##..#",
            "",
            "Tile 1063:",
            "####.#####",
            "#........#",
            "##........",
            "##........",
            "........#.",
            "#.#....#..",
            "....#.....",
            ".##.###...",
            "#.........",
            "##.##.#.##",
            "",
            "Tile 3931:",
            ".##......#",
            ".#.......#",
            "...#....#.",
            "#...#.....",
            "....#...#.",
            "..#...##.#",
            "#.#.#..#.#",
            "..........",
            "#..#....#.",
            ".#..#.#...",
            "",
            "Tile 1367:",
            ".##.#.....",
            "##.#......",
            "###....#..",
            ".....#...#",
            "##...###..",
            "#.#....#..",
            "...##....#",
            "#..#...#..",
            "..#...#...",
            "#..#....##",
            "",
            "Tile 1783:",
            "#.#####...",
            "......##.#",
            "#....#.###",
            "..#......#",
            "#.........",
            "###......#",
            "##........",
            "##....#...",
            "#.#.#..#.#",
            "#####.#...",
            "",
            "Tile 1777:",
            "#.....####",
            "#.##..#...",
            ".##.#..##.",
            "..#.#.#...",
            "#....#...#",
            "#......#..",
            "..#.#.#...",
            "#.#.......",
            "#.......##",
            "#.##..##..",
            "",
            "Tile 1487:",
            "...###.###",
            "..........",
            "##.......#",
            "#.#.......",
            "#....#..#.",
            "..#..#....",
            "#.#..#..##",
            ".#.##.#...",
            "........#.",
            "#.####.#..",
            "",
            "Tile 2039:",
            ".#.#######",
            "..#...##..",
            "..#..#...#",
            "##......##",
            "#......#.#",
            "#...##...#",
            "...#...#..",
            "#.##.#....",
            "....####.#",
            ".#..#..##.",
            "",
            "Tile 2833:",
            ".##.#.#..#",
            ".....#.#.#",
            "#.....####",
            "#...#....#",
            "#.......##",
            "#......###",
            "......##..",
            "##.#..##..",
            "##.##..#.#",
            "#.#...#.##",
            "",
            "Tile 3301:",
            "..#.####.#",
            ".#.#...##.",
            "#.#.##.#..",
            ".##.#.....",
            "..#......#",
            "..........",
            "#..##.#.#.",
            "#.......#.",
            "..........",
            ".###...###",
            "",
            "Tile 2963:",
            "..##.##.#.",
            "##....#.##",
            "#.#.#.....",
            "##.......#",
            "....#.....",
            ".....#....",
            "#.#...#...",
            ".#.......#",
            "....#....#",
            "..##..#.##",
            "",
            "Tile 2477:",
            "###.#..###",
            "......#...",
            "#.##.#..##",
            "###...#..#",
            "##....#...",
            "...##...#.",
            "..#....#..",
            "#.#....###",
            "#.#.#.#...",
            "..##.....#",
            "",
            "Tile 2659:",
            "####..##..",
            "..#.##....",
            "#......#..",
            "...#...###",
            "#....###.#",
            ".........#",
            "#.#.......",
            "#.#.#..#.#",
            "##.....#.#",
            "#...#..###",
            "",
            "Tile 1051:",
            "###.#.#..#",
            "#.#..#....",
            "#.......#.",
            "#...##...#",
            "#....#...#",
            ".....#....",
            "##..##..#.",
            "..#......#",
            "#.........",
            "..#####.#.",
            "",
            "Tile 3637:",
            ".#...###.#",
            "#.#.#.....",
            "..#..#.###",
            "...##.....",
            "#..#....##",
            ".........#",
            "#.......#.",
            "..##.#..#.",
            "##......##",
            "#######.##",
            "",
            "Tile 1697:",
            "##.###.#..",
            "....#...##",
            "#.......##",
            "....#....#",
            "#.....#...",
            "#.##......",
            "##.......#",
            "#....#..##",
            "#.........",
            "...#...##.",
            "",
            "Tile 1499:",
            "#...####.#",
            "..#.##.#.#",
            ".##.....##",
            ".##....#..",
            ".##......#",
            ".#......##",
            "#..##.###.",
            "#.#....#..",
            ".##.#.#.##",
            ".#.#.#.#..",
            "",
            "Tile 2543:",
            "#.##.#.##.",
            ".....#....",
            "...#.#...#",
            "#...#..#.#",
            "..##....#.",
            "##..#..#.#",
            "...#......",
            "#.##......",
            "###...####",
            "#..####.#.",
            "",
            "Tile 2269:",
            "###.#..###",
            "..#..#...#",
            "#..#.##..#",
            "#......##.",
            ".#..#.....",
            "#.....#...",
            "#..#.#....",
            ".#......##",
            ".###..#...",
            ".###......",
            "",
            "Tile 2029:",
            "##.###....",
            ".#.....##.",
            ".........#",
            "#.#......#",
            "##.#....#.",
            ".##.#.....",
            "#.........",
            "#...#....#",
            ".........#",
            "...###.##.",
            "",
            "Tile 2141:",
            "#.#.#..#.#",
            "..##.....#",
            "#.........",
            ".##......#",
            "#.#..#.#..",
            "#........#",
            "##..##.#..",
            ".#........",
            "#.#.....##",
            ".....##...",
            "",
            "Tile 1601:",
            ".......##.",
            "##.#.##.##",
            "####...#..",
            "#.##...###",
            "##...#...#",
            "#......#..",
            "#..#....##",
            "##......#.",
            "..##...#.#",
            "#..###..##",
            "",
            "Tile 1049:",
            "..#.##.#.#",
            "##........",
            ".#......#.",
            "#...###..#",
            "........#.",
            "....#.#...",
            ".###..###.",
            ".#....#...",
            "#....#...#",
            "#....#..#.",
            "",
            "Tile 1543:",
            "###...#..#",
            ".........#",
            "#.....##.#",
            ".#...#...#",
            "....#.#...",
            "...#.....#",
            "#..#.....#",
            "..#.......",
            "#......#..",
            "..#..###..",
            "",
            "Tile 2069:",
            "##..#.#.##",
            "#...#....#",
            "#...##....",
            "#..#......",
            "#...#....#",
            "....#....#",
            ".........#",
            "#.....###.",
            "...#.....#",
            ".##.#.#...",
            "",
            "Tile 2687:",
            "#.#..#....",
            "#.#..#....",
            "#....#....",
            "..........",
            ".....#...#",
            "#........#",
            "....#.....",
            "...##...#.",
            "#.......#.",
            "##....#.##",
            "",
            "Tile 3691:",
            ".######.#.",
            "..#..##..#",
            "#.....#...",
            "......#...",
            "#.##.#..##",
            "##....#..#",
            "....#....#",
            ".##..#....",
            ".........#",
            ".#..#.#..#",
            "",
            "Tile 2521:",
            "#..#.###..",
            "#.#.......",
            "##..##..##",
            ".....#.#..",
            ".....#..##",
            ".###....#.",
            "#.....##.#",
            "#.....##.#",
            "#.#.#...##",
            "###.#...#.",
            "",
            "Tile 1093:",
            "#..#######",
            "#......#.#",
            "..........",
            "..#....#.#",
            "##.##....#",
            "..#..#...#",
            ".....##...",
            "#.....#..#",
            "#.#.#.#..#",
            "#..##.####",
            "",
            "Tile 2341:",
            ".####.#.#.",
            "#.#...#..#",
            ".....#...#",
            "#....#..#.",
            ".........#",
            "#......###",
            "..#.#.....",
            "#.......#.",
            "#.#.#.....",
            "######....",
            "",
            "Tile 2903:",
            "#.#.###.##",
            ".#...#....",
            "........#.",
            "..#.......",
            "#......##.",
            ".#...#...#",
            ".#...#....",
            "#.##.....#",
            "..........",
            "....#..#..",
            "",
            "Tile 3413:",
            "...#.####.",
            "#...#...##",
            ".....#.#.#",
            ".#.#..#...",
            ".#.......#",
            "#.......#.",
            "#....##...",
            "..#....#..",
            "......##..",
            "....#.####",
            "",
            "Tile 2213:",
            "###...####",
            "..#....#..",
            "..##...#..",
            "...#.#....",
            "#..##.#.#.",
            "..#..##...",
            ".##..##...",
            "........##",
            ".#..#.....",
            "##..#.####",
            "",
            "Tile 1931:",
            "#.#.##.###",
            ".........#",
            "........##",
            "..........",
            ".#.....##.",
            "#.#.#....#",
            "#.....#..#",
            ".......#..",
            ".#........",
            "#.......#.",
            "",
            "Tile 1373:",
            ".###.#..##",
            ".#.....#.#",
            ".....###..",
            "....#...#.",
            "..........",
            "##..#....#",
            "#.........",
            "...#...##.",
            ".......#.#",
            "......####",
            "",
            "Tile 1229:",
            "#..#..#.##",
            "..........",
            ".#........",
            ".......##.",
            ".......#.#",
            "#..##....#",
            "#.#....#.#",
            ".#.....#.#",
            ".........#",
            ".##..###.#",
            "",
            "Tile 1361:",
            "...#.....#",
            ".#......#.",
            "#.#.#....#",
            "#........#",
            "......#.##",
            "#.....##..",
            "#.......#.",
            "........#.",
            "#...#.#.##",
            "##.#.#.###",
            "",
            "Tile 1031:",
            "###.#.#.#.",
            "#...##....",
            "#..#.#...#",
            "#...#..#.#",
            ".#.....#..",
            "......#..#",
            ".###...##.",
            "..#.#....#",
            "...#...#.#",
            "##...#.##.",
            "",
            "Tile 1451:",
            "..#.##.#.#",
            "....#..#..",
            ".#......#.",
            ".......##.",
            "#.#.#.....",
            ".......#.#",
            "##..#...##",
            "......#...",
            ".#........",
            ".##...#.##",
            "",
            "Tile 3943:",
            "..#.#.##.#",
            ".........#",
            "#........#",
            "#..#..#..#",
            ".#.#......",
            ".##...#...",
            "##....#..#",
            "..#.#.....",
            "..#.......",
            "#..##.###.",
            "",
            "Tile 2393:",
            "...###..#.",
            ".........#",
            ".....#..##",
            ".##....###",
            "..........",
            "#.........",
            ".#.###...#",
            "..##.....#",
            ".#.####..#",
            ".#.#.#..##",
            "",
            "Tile 1531:",
            "..#.#####.",
            "#........#",
            "...##..#..",
            "#..#.....#",
            "#........#",
            "##...#.#..",
            ".#......##",
            "..........",
            "#.#.#.##.#",
            "#....##.##",
            "",
            "Tile 3923:",
            "##...#....",
            "......##.#",
            "...#..#...",
            ".....#....",
            ".....#..##",
            "#..######.",
            ".........#",
            "#......###",
            "##.....#.#",
            "#..#.#...#",
            "",
            "Tile 1471:",
            ".#.#.#.#.#",
            "##...#...#",
            "..#.......",
            "#.........",
            ".....#..##",
            "#..##.#.#.",
            "#........#",
            "#...#.....",
            "#...#....#",
            ".#####.##.",
            "",
            "Tile 1123:",
            "#..###...#",
            "#.##.##.##",
            "##.....#..",
            "#.......##",
            ".......#.#",
            ".....#.###",
            "......#..#",
            ".....#...#",
            "#.#....#..",
            "##.#.#..##",
            "",
            "Tile 1907:",
            "###.#....#",
            "#..#..#.##",
            "...###....",
            "#....#...#",
            ".....#.#.#",
            "#....#....",
            "#.........",
            ".#......##",
            "#........#",
            "###...###.",
            "",
            "Tile 2371:",
            "##.....###",
            "..#...#.#.",
            "#.......##",
            "#.###..#..",
            ".......##.",
            "#..##.#..#",
            ".#..#..#.#",
            "##......##",
            ".#.....#.#",
            ".######.#.",
            "",
            "Tile 1117:",
            "####..#..#",
            "...##....#",
            "#.........",
            "#.#..#...#",
            "#..##....#",
            "#....#....",
            ".#..###.##",
            ".#.#..#.##",
            "#........#",
            "....#..#.#",
            "",
            "Tile 1553:",
            "########.#",
            "##.....#.#",
            "..#..#.#..",
            "..#..#....",
            "###......#",
            ".#..#.#...",
            "..#.......",
            "........##",
            "........#.",
            "#.#.......",
            "",
            "Tile 3709:",
            "##..#...##",
            "...#.#....",
            "#........#",
            "...#..#...",
            "#.......##",
            "#..#.##...",
            ".........#",
            "..#.....##",
            "#....#...#",
            ".###.#..#.",
            "",
            "Tile 2221:",
            "#.####..##",
            "....#.#...",
            ".........#",
            ".....#..#.",
            "....#...#.",
            "#..##.#...",
            "#..##.....",
            "#.#..#..##",
            ".#...#....",
            "##.#.#.##.",
            "",
            "Tile 1303:",
            ".#..######",
            "..###....#",
            "#.........",
            "###....#.#",
            "..#......#",
            "##...#....",
            "..........",
            "#.....#...",
            "#........#",
            "#.#.#.###.",
            "",
            "Tile 3391:",
            "##..##...#",
            "#..#.....#",
            ".......##.",
            "#..#......",
            "....#.....",
            "#.##..#..#",
            "..###..#.#",
            "#.......##",
            ".......#..",
            "#..#.#.#.#",
            "",
            "Tile 1433:",
            "##..#..##.",
            "#.........",
            "..........",
            "....#.....",
            ".......#..",
            "##..#....#",
            "#.....#..#",
            "..........",
            "..#...#.##",
            "..#.#...#.",
            "",
            "Tile 1163:",
            ".###.##.##",
            "#.........",
            "#.......#.",
            ".......#.#",
            ".........#",
            "##....#..#",
            "#......##.",
            "#....#.#.#",
            ".........#",
            ".#...#..#.",
            "",
            "Tile 2113:",
            "###..###.#",
            "...#..#...",
            "#...##...#",
            ".........#",
            "...#.#....",
            ".#....#...",
            "..#.##....",
            "....#.....",
            "#..##.#..#",
            "##.#...##.",
            "",
            "Tile 3023:",
            ".####.####",
            "#....#....",
            ".#...#..#.",
            "##......#.",
            "#......#..",
            ".........#",
            ".......#..",
            "#....#.###",
            "#..#..##..",
            ".#.##.##..",
            "",
            "Tile 1103:",
            "......#.##",
            "#..#..#...",
            "..#.#..#.#",
            ".#..#....#",
            "..##...##.",
            "##.##.....",
            ".#..#.....",
            "#.#...#..#",
            "..#...#.#.",
            "#.#####.#.",
            "",
            "Tile 3469:",
            "..##.#....",
            "....##..#.",
            "..........",
            "#........#",
            "##....#..#",
            ".........#",
            "#..#.#.#..",
            ".........#",
            ".#..#..###",
            "##.#..#...",
            "",
            "Tile 3671:",
            "..#.....##",
            ".....#....",
            ".#......#.",
            ".#...#.#..",
            ".#........",
            "#.##......",
            "#.#...#.#.",
            "#..#....##",
            "..#.......",
            "###.######",
            "",
            "Tile 1213:",
            "#..#......",
            "...#.....#",
            "#.......##",
            "....##....",
            "....#.#..#",
            "#.#......#",
            "#.........",
            "#..#..#...",
            "#....#....",
            ".#.##.###.",
            "",
            "Tile 2063:",
            ".##...##.#",
            ".......#..",
            "#........#",
            "..#...#...",
            ".###.....#",
            "#........#",
            "#..##.#.#.",
            ".....#....",
            "......#.#.",
            ".####...#.",
            "",
            "Tile 1753:",
            "#.##.##.##",
            "###.#.#..#",
            "#........#",
            "##....#..#",
            "....#.....",
            "#....#.##.",
            "..#..#....",
            "...#.#....",
            ".....#..##",
            "##.##....#",
            "",
            "Tile 2309:",
            "#..#..#.##",
            "#.......##",
            "#..#......",
            ".......#.#",
            "#..#.....#",
            "##.#.#...#",
            ".#.#....#.",
            "#....#....",
            ".##.......",
            "#..#....#.",
            "",
            "Tile 3767:",
            "#......#..",
            ".#.#..#.#.",
            "#........#",
            "..#.#.....",
            "#....#...#",
            ".#..#..#..",
            "#.....#..#",
            "#.........",
            "..#....##.",
            ".##.#.###.",
            "",
            "Tile 1321:",
            "#.####...#",
            "#.#...#..#",
            "##........",
            ".......#.#",
            ".#....#.#.",
            ".#..##....",
            "#.#....#.#",
            "........##",
            "..#.##...#",
            "#..#.#.#.#",
            "",
            "Tile 2731:",
            "..#.#..###",
            "#..#......",
            "..#....###",
            "...#..#..#",
            "..........",
            "..#.....##",
            "#..###....",
            "..........",
            "#...#.....",
            "#......#..",
            "",
            "Tile 2803:",
            ".#.#.##.#.",
            "##..##...#",
            "...#..#..#",
            "..#.#..#.#",
            "#.#..##..#",
            "##.#..#..#",
            "...###.###",
            "##.#..#...",
            "..#.......",
            ".......#..",
            "",
            "Tile 3191:",
            ".###..#.#.",
            "###.#...#.",
            "........##",
            "....##...#",
            ".....#....",
            ".#.......#",
            "..........",
            "..#...#...",
            "..###.....",
            "##.....#.#",
            "",
            "Tile 2753:",
            "..#.#####.",
            "....##..#.",
            ".........#",
            "....##....",
            "#..#...##.",
            "#.....#..#",
            "..........",
            ".##.......",
            "#.....#..#",
            ".##..#.##.",
            "",
            "Tile 2789:",
            "##.#...#..",
            "#.....##..",
            ".....###..",
            "#.#.....##",
            "#.#...##.#",
            ".........#",
            "##.......#",
            "##......##",
            "#..#..###.",
            "..###....#",
            "",
            "Tile 2719:",
            "##.###.#.#",
            ".........#",
            "#....##...",
            "...#..#...",
            "##.......#",
            "....##...#",
            "..#...#..#",
            "##.#.....#",
            "##....#..#",
            ".#####...#",
            "",
            "Tile 2143:",
            ".######..#",
            "#......#..",
            "#.......##",
            "....#....#",
            "#...#.....",
            ".........#",
            "....#.#..#",
            ".......#.#",
            "..........",
            "#.##....##",
            "",
            "Tile 2411:",
            "..#.#..##.",
            "##...##..#",
            "...###...#",
            "..#......#",
            "#.###.....",
            ".........#",
            "..#.##....",
            "#..#....##",
            "#..#..#..#",
            "....##...#",
            "",
            "Tile 1187:",
            "##.##.....",
            "##..#....#",
            "..#..##...",
            "##.#......",
            ".#..##....",
            "#....#....",
            "..........",
            "##.......#",
            "#..##..#.#",
            "#..#.####.",
            "",
            "Tile 3049:",
            "..#..#.#.#",
            ".....#....",
            "#........#",
            "#...#..#..",
            ".........#",
            "..#...#.#.",
            "#.#....#..",
            ".....#....",
            ".......#..",
            ".#.#.#.##.",
            "",
            "Tile 1997:",
            ".##.#.###.",
            ".....##...",
            "....###..#",
            "#....#...#",
            "#.....#..#",
            "#...#..#.#",
            "....##...#",
            ".##..#....",
            "#...#.....",
            "#.#.###.#.",
            "",
            "Tile 3181:",
            "...#..#..#",
            "..#..#..##",
            "........#.",
            "..........",
            "#......#.#",
            "##........",
            "##.....#..",
            "#.....##.#",
            ".#..#.....",
            "##.....###",
            "",
            "Tile 3371:",
            ".#....###.",
            "#.....#...",
            "##........",
            "#.#.......",
            "#........#",
            ".#.......#",
            "#........#",
            "...#.....#",
            "..#.##.#..",
            "...#..####",
            "",
            "Tile 3491:",
            ".#.##...#.",
            "#..#..##..",
            "..#.####.#",
            "...#......",
            "#...#....#",
            "#.........",
            ".....#..##",
            "#...##....",
            "##.....#..",
            ".####.....",
            "",
            "Tile 2999:",
            "#..#.##.##",
            ".#.#.....#",
            "#.#.......",
            "...#.....#",
            ".........#",
            "###...#..#",
            "..#......#",
            "#...#.#...",
            "..........",
            ".#....####",
            "",
            "Tile 1423:",
            ".....###.#",
            "##.#......",
            "#......#..",
            "##...#...#",
            "#.#.#..#..",
            "#.##......",
            ".....#...#",
            ".#.......#",
            "......#..#",
            "#.#..#####",
            "",
            "Tile 3739:",
            ".#...###.#",
            "..#...#...",
            ".#.#.#....",
            "....#.....",
            "..........",
            ".#.#...###",
            "#.........",
            "#...###...",
            "#....#..##",
            "#..#.....#"
        };

        #endregion Data
    }
}

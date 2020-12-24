using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems.Year2020
{
    /// <summary>
    /// For this solution, I used a cubic hexagonal grid as described here:
    /// https://www.redblobgames.com/grids/hexagons/
    /// </summary>
    class Day24LobbyLayout : ProblemBase<int>
    {
        public Day24LobbyLayout(ILogger logger) : base(logger, "Lobby Layout", 2020, 24) { }

        protected override int ExecutePart1()
        {
            var grid = ComputeDay0Grid();
            return grid.Where(t => t.Value).Count();
        }

        protected override int ExecutePart2()
        {
            var grid = ComputeDay0Grid();
            int maxX = grid.Max(t => t.Key.x) + 1;
            int maxY = grid.Max(t => t.Key.y) + 1;
            int maxZ = grid.Max(t => t.Key.z) + 1;
            int minX = grid.Min(t => t.Key.x) - 1;
            int minY = grid.Min(t => t.Key.y) - 1;
            int minZ = grid.Min(t => t.Key.z) - 1;
            for (int i = 0; i < 100; i++)
            {
                var newGrid = new Dictionary<(int x, int y, int z), bool>(grid);
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        for (int z = minZ; z <= maxZ; z++)
                        {
                            var key = (x, y, z);
                            int adjacentCount = GetAdjacentCount(grid, x, y, z);
                            _ = grid.TryGetValue(key, out bool value);
                            if (value)
                            {
                                newGrid[key] = adjacentCount != 0 && adjacentCount <= 2;
                            }
                            else if (!value && adjacentCount == 2)
                            {
                                newGrid[key] = true;
                                maxX = Math.Max(maxX, x + 1);
                                maxY = Math.Max(maxY, y + 1);
                                maxZ = Math.Max(maxZ, z + 1);
                                minX = Math.Min(minX, x - 1);
                                minY = Math.Min(minY, y - 1);
                                minZ = Math.Min(minZ, z - 1);
                            }
                        }
                    }
                }
                grid = newGrid;
            }
            return grid.Where(t => t.Value).Count();
        }

        private static int GetAdjacentCount(Dictionary<(int x, int y, int z), bool> grid,
            int x, int y, int z)
        {
            int count = 0;
            foreach (var (xOffset, yOffset, zOffset) in adjacentOffsets)
            {
                _ = grid.TryGetValue((x + xOffset, y + yOffset, z + zOffset), out bool value);
                if (value)
                {
                    count++;
                }
            }
            return count;
        }

        private static readonly List<(int xOffset, int yOffset, int zOffset)> adjacentOffsets =
            new List<(int xOffset, int yOffset, int zOffset)>()
            {
                (0, 1, -1), (1, 0, -1),
                (-1, 1, 0), (1, -1, 0),
                (-1, 0, 1), (0, -1, 1)
            };

        private static Dictionary<(int x, int y, int z), bool> ComputeDay0Grid()
        {
            var grid = new Dictionary<(int x, int y, int z), bool>();
            foreach (string tile in tileDirections)
            {
                int x = 0;
                int y = 0;
                int z = 0;
                var matches = directionRegex.Matches(tile);
                foreach (Match match in matches)
                {
                    switch (match.Groups[1].Value)
                    {
                        case "nw":
                            y += 1;
                            z -= 1;
                            break;
                        case "ne":
                            x += 1;
                            z -= 1;
                            break;
                        case "e":
                            x += 1;
                            y -= 1;
                            break;
                        case "se":
                            z += 1;
                            y -= 1;
                            break;
                        case "sw":
                            z += 1;
                            x -= 1;
                            break;
                        case "w":
                            y += 1;
                            x -= 1;
                            break;
                    }
                }
                _ = grid.TryGetValue((x, y, z), out bool existingValue);
                grid[(x, y, z)] = !existingValue;
            }
            return grid;
        }

        private static readonly Regex directionRegex = new Regex(@"(se|sw|ne|nw|e|w)");

        #region Data

        private static readonly string[] tileDirections =
        {
            "neeneneneswneneee",
            "eeeswneseseeseeeseeeeesesee",
            "swswsweswwswswswnw",
            "neseewwswwneswnewnewwswswwswwse",
            "wswsweenweesenweeeeeenweeee",
            "neswwwswwwnewwwwwwwwsewwswsww",
            "neseeeswesweenweeeeeeeeee",
            "neeeneneeneeswneeneneneene",
            "ewwsenwwenwnwswnwswwnewwwsewne",
            "sesesesesesewseseweeeeneweseeww",
            "neeeswnwneswneneneweeneeneswnwnesw",
            "neseswnwnenenewneneesenenenenenenenenenene",
            "eneeeeseseeseesewseseseeeseee",
            "enenewseeenewnweseewnwneseweesw",
            "sesesesesenwseseseseseseseswseseee",
            "nenenenenwneeesweseneeeeeenenenwe",
            "neeenenwseswsenewseswenwewsww",
            "neeenesweswwneeneeeeeeeeenwe",
            "swseneswewwsewsweeeseeswwsesww",
            "nenwwnwnwswwnwwnwnwnwnwnweswswnenww",
            "swswseseswnwswswswseswseeswswswswnwswsesenw",
            "nwnewwswnwesewnwwwsenwwnewewe",
            "newwsewwsenwswwswswswswseswwnwwwsw",
            "swswsweswseswswwswswnwseswswswswswswswsw",
            "nenwnwnwnenwnwnwnenenwnenwneswnesenwenw",
            "nwswnwwnwnwwnwnenwwse",
            "neesweeeesewewneese",
            "nwneweswwwswswwswwswswsewsw",
            "wwwwsenwwnwneswsewsewnenewnwwseswne",
            "eeeeeneneneneweneeeeeweswnene",
            "enenwneneeneswnewnweewseswneswnew",
            "swsesesesesenweswseseseswswseswswsesese",
            "nweeeneweewsw",
            "nwswswneswswswneswsewswswswswsesw",
            "swenwwnenwnwnwswnwsesenenw",
            "swswswnwneswswswseswswswswswswswseswse",
            "sweseseseeseseweenese",
            "wnwnwnwnwnenwnwswnwnwenwsenwenwnwnene",
            "eeenweeeeeeeeeeesweeee",
            "nesenwswnwseswneewnenwenwsenwenwewnw",
            "eeeeeewesenweeseeeeswe",
            "swwwswneswsewwswsw",
            "nwnenwnenwnenwnenesenenwnwnwnwnwnwwnw",
            "newnesewnwnenwneneenwnenenwnenenenenesw",
            "eeeeeeeeeeenweeeseeeesw",
            "swnwnwwnwwwnwwewnwwsewwwwnwnew",
            "nwnwnwenenwswnwnwnwnwwnwnenwswewnwnw",
            "swseenwnwswnwnenesenenwnwneeswneenwwnw",
            "enwwseeswwenwswswesw",
            "enwseseeeeeeenweeeseeseswseee",
            "eseswseseswseneenwwneeswwwse",
            "swswswwesenwswswswswswswswswswsenesesw",
            "neswenwswswswnwnwsweswnweswswsweswsw",
            "newsenenwswesewenesewwseeesenwesew",
            "seswswswswswswneseseswweswseswsewseswnw",
            "wnwwwwnwnwnwwnenwwewnwnwswnwnww",
            "newwwwwwsenwwwewwwnwwwww",
            "seseseeeswseseswseswswwsewseseswswse",
            "eenenwswnenwneswnesenwnenenenwnwnenenewsw",
            "swnwewswwewnwnwswwwnwnwewwwse",
            "nwenewneneneewnene",
            "wwwwwswwswenwwsewwwwwwswsww",
            "enenewswnwneneneneseneneneeneneeneene",
            "wnwswswnewwwswnewwwsewwseswnew",
            "nenenwwsenweswnwswswnwswnwnenwnwne",
            "seeenwesesesewseseeseseeesesesese",
            "eeneeeeeeeneenwenweswsweeene",
            "wneneneswseneneneneneneeneenenwsenwnene",
            "seseseswsenewesesesenwneseseeseseesese",
            "wsesenesenwsesweseswnwsesweseseesenwsese",
            "wwwwnwwnewewwswsenwseseewnwnenwnw",
            "wnenenenenenenenenwnenenwnenenesenenene",
            "neeewnwwneneesweneeweseswnwneswsw",
            "neesewwneenwnwwwwseswwnwwwwswww",
            "esweeeeeeneneenwe",
            "eseeseeeeenwswseeseneeseseseswee",
            "neneenewnweneseswnenenwneneenwwseswne",
            "senwwnesenwswnwneeewswnenwnwwnwnwsene",
            "eeeeseeeeeneewwneeweneeenene",
            "nwnenesenwwwseswseswnwwesesenwwnene",
            "swseneswswswswswsewswnwneseswseneeswsesw",
            "nwnwwnwswnwnwwnwnwenenwswwnwnwnwnwenw",
            "seswswseeesesenwnweeseeeneeenewese",
            "nwsenenesenwwnenenwnwneswnenenenenenene",
            "swsewnwwwseswwwsewneneswneenene",
            "nwenenwnwnenwsenwnenwnwswnwnenenenwnenwnw",
            "swwneneneneneenweeeneenesweenesenesw",
            "swnwnwswnwwwswsenewsewswsewswswseswsww",
            "enenenenesenenenenenenenenenenwswnenewne",
            "newewesesewseseseseeeseseeesesese",
            "nenwwwnwsewswwwwewsewwwwwsew",
            "enwewnwwwwnwwnwnwnwnwnwnwwwnwnwswnw",
            "neweeswesweeswewneseewnw",
            "swswswswsewesewswswswswneseswswswseswse",
            "swneeewseneeeeeswneewwneseenwe",
            "wwwwwwwwwneewwsewwwwweww",
            "eeeesweneenweeeneneeweeenee",
            "wnenwneseeenwseeneneewneseswwnene",
            "wwwnwewnwnwnwnwnwewneenwnwnwswswswnw",
            "enenenenwesesewnwneeeswneneeneenenene",
            "nwnewseneseenwwwseswneswseneeeswnew",
            "swsewwsweseswneseseeseswneseneswswse",
            "swswswwswswnewsesesweswswswswseneswnesww",
            "enwwnwwnwwnewnwswswseeswnenwwnwwnwse",
            "sesenwnenwnwnenwnwnwswnwnenwnwnenenwnwnwsw",
            "seeneswswwswwswswwwwswwwswnenesw",
            "swwwwswneseswwwwswwnwswswswswwswsew",
            "swseeenenwnwswenwneneenwnwnenwnwsewsw",
            "wswswswwwswwwswswswneswswswseswswsw",
            "neswnewnenenwnenewenenwneneeenenenw",
            "eswneneneneneenenwneneeeneenenewe",
            "enenenenenenwneneeneeeeeeenese",
            "seeseewswwseseneesenwneseseswsewswsw",
            "ewwewswwswwswswwewwnwswwsww",
            "swwswswswswswswswswswne",
            "nwnenwnenenenwnwnwswnwne",
            "nwnenwnwswnwwewnwnwswnwsenwwnwnwnwnw",
            "nwnwnenwnwnwnwwnwnwnwswnwswnwnwewnwnw",
            "eseneseeswenwneeneneneneewnenwnenene",
            "swwnwwswwswswwswwswswswsewwwwswne",
            "swsewsenenweseesweee",
            "nwnwnwnwsenwnwnwnenenwnewnwnwnenwnwnwnw",
            "nwsweeeseseeeeseeeeseneeswene",
            "neenwneneswnwnesewewswneswnwnwneneswe",
            "esenenwnwnewseneswnwsewswwnwsenwsewswse",
            "nwwwweneeewwwesesenwneneswswswnw",
            "nwnwsenwnewseswnwesenewnenenwnenenenwe",
            "wnwwnwnenwnwsenwnewwnwwsewwnwwwwe",
            "wwwewwswwswwwswwwwswsw",
            "swswwswswseswswswswseswswswswnesw",
            "wwwwwwwwwwswwewwnwwwww",
            "nwnenwnwnwnwnwsenenwnwnwnwnwnw",
            "swnwswswswswwseeeswswswswswswsewnwnwsw",
            "nenwnenwnenewnwnenwnwnenwnwseenenwnenwne",
            "seewseeseeeeseseeeweneseseseese",
            "seenesesesesesesesewseseseseesesesese",
            "nenenenenwneneneenweneneneesenenenesene",
            "swswneneeseswwswnwseswswwswswseswsesw",
            "nwswsenwneenwnwnwnwsenwnwwsenwenewnwsw",
            "nweseseseswswswswswswswswswsw",
            "seseswseseseswsenwseseseseesesesesesese",
            "wwwwwwsenwwnewewnwwsewswnesww",
            "nwseswswnwseswswsewseseeseswswesesesesw",
            "seswseseenwswseseseenwswseswseesewswse",
            "nwwnwnwswnwwnwnwnwnwesenwnwnwwwwnww",
            "neneswseneneneswwnenweneeneneenesenww",
            "swswswswneseswswseswswswwswswsesweswswse",
            "swneswseswswswswswswswswswseswswseseswsene",
            "swswneneswswwneswwwswswswwswnweswsene",
            "nenwnwnwwnwnwnwnwnwnwnwnwnwenwnwnwswnwnw",
            "wwneswwwwwwwwewwwnwwwsewww",
            "neneeswnwneneeneeee",
            "swesweswswnesweswsesewwnenwnwnewnwnee",
            "nwwwwewwwwwwwnwwseswwwswwe",
            "sweenwseeseneweseseeewnwseeseenenw",
            "eeeeeenweseeneeeeeeeenesw",
            "nwnwnwswnwnwnwswnwnwnwnweeenwnwswnenw",
            "eneeneeeeweeeeeneeeeneewsee",
            "swnenwnweneneenwnwnwnwswnwnenenenwnwnwnw",
            "nenwnenwnwnenwnenesenenwnwnwnenw",
            "neeswnweswswseewwnwwweswwwwwne",
            "nwseeeeeeweseeeeseeeeeeee",
            "sweseneneseeseeseeneeeeeewsweew",
            "wnwwnewswwwswwwwewwwwnwww",
            "wswnwsweswwneseswnwswswwseswsweswnwwsw",
            "nweswneeweeeeswsenwew",
            "neeswneneenenweeneneneeneseenesenwe",
            "nwnwnwnwnwnwnwnwnwnwnenenwnwnwnwsenwnwse",
            "seswseseswseseswseseseseswsesenw",
            "nwnenwewnesenwnwnwnwnewnenwnenwne",
            "swswswswesweswswswwswswswswswswwswsw",
            "nwnweeeeeeeeeseeeeeeeese",
            "wswnwnwswswswwwwswswswswswwwseswswse",
            "seneswwseswsesewsenwsenenesenwseseese",
            "neneneswneswenenweeneenenenenwnwswnene",
            "newwwnwwwwnwsewwwwseswwwwse",
            "swsenwenwnwnwnwnewnwswenwwesw",
            "newwnwnwnewsewwswwwwnwnwnwswww",
            "eseswsenenesesenwwseswseseseeseesese",
            "nwwenwenwsenwnewnenenwnwwnweneswnwne",
            "neswneneseswseeseseswswsenesenwswswsww",
            "neneeeswseneswnwnwwenenenwseswwwe",
            "nwswseseseeeseseseseesesesesese",
            "wswwnwswswnenwneswnweswseenwewwwse",
            "swseseswseneswswseswswseseswswnwswseswswsw",
            "sesesenwseswseseseseseeesesesesesesese",
            "nenenenenenenewwneeeneneseneeneneene",
            "wnwswwewswnwnwsenenwesesweeswsene",
            "enenenwneneneeneeeneeseswneenenenene",
            "seenwseseeneeswwneseeeseswwesenww",
            "seseswnwwsewnewswnwseswneswswswesweenw",
            "nenenenenwnwnenenwwnwenwnenw",
            "nwwwwnwneeeeneneneseswsenewesenw",
            "swswswneneseswwwwswswswswswnwswswswsw",
            "nesesenwswswseswnenwsewswseseseseswnese",
            "neeeenenweswenweneneeneeneeeeese",
            "seeweneseeneseseseswseewsenenwe",
            "seseseseseseseseseseseseseseswsenwsw",
            "wswswswswsweswwwwseweswwwswswne",
            "nwseeswneswneswwnenwseswenewseswwsw",
            "neeweenweeeeneeeeeesweeeee",
            "swwswwswwneweswwswswwwswsewwsw",
            "sesweswwswenwweswswesenwnwnwnweswse",
            "nwneenenwnewenesw",
            "wnwseewwenwwwwesewwwnwewwnw",
            "eeeneesenenenweswene",
            "seenwseseseeseseewsweesesesewsese",
            "wswsweswswseseseseseseseseswswsesesenwse",
            "wweeeeeneeseeeeeeeeesese",
            "swseswseseseeswsenenwseswseseseseseswsw",
            "wwwewwwwwnwswwwnwnwnwwwewnw",
            "wwnwnenwseneeswswswewewswswswwenw",
            "neswswswswswseseswseseseseswseesenwswsw",
            "neeswnewnwwswsenwnwnenwnwsenesewene",
            "nenwseeswwswseswsesesweseswswswswseswsw",
            "nwwwwseneneswseseneeseswswnwnwseene",
            "seesesesesenwseweseeseseeseseswseese",
            "ewwwswswwsewnwwwwswnewneeswsw",
            "eeneneneneneneeneenenewnenenenesewne",
            "enweeswnwsweseneeneenwsenewnwswswne",
            "sesenwseewsesenweseeeseseeeseeenee",
            "nwnwnwnenwnwnwswnwnwnwnenwnwenwnenwswnwnwnw",
            "seswnwsenwnenwnwnenwnenwnwnenw",
            "nenwnwnwsenwnwnwnwnwnwnwnesenwswnwnwnwnwne",
            "swnwnenwnenenwnwnwnwnenwweenwwenwnwnwnw",
            "senwnwnwwnwwwwnwwnwnwnwswnenwnwnwnewnw",
            "neswnenwswsesewnwnewsewwnewnesenewnwnw",
            "wwwsenwnewwwwewwnwwwwwsww",
            "enwneeneneswswnesw",
            "enweesesewsenesesw",
            "eeswewneesenenweneneneneneeseee",
            "nwnwseneswnwnwwewwwwsenwwwnwww",
            "swwneswneswwwswsweeswswswwswswseswnw",
            "ewwwwwsewswwwwwwenwwwneww",
            "swneswseeswswswwswnwsewwswswswswwswwsw",
            "sweswwswseneseeewswwnenwnwneewwsw",
            "swwswswseswswswwswswswswswswswwswswne",
            "nwwwswwwwwwswwweweswneswwew",
            "neneneswsenenenwnenenwneswnweweenwsww",
            "senwswnewwseseeseenewseseneeeswene",
            "nenwsewwsenewswseenesewsenenewnwe",
            "swswswswwwwwewswwnewwswwwwww",
            "swsweswsenwnweenenwe",
            "wnwsesesesewseswnesenesenwswsesesesesesese",
            "eeewseeeseeweeeseeewwenee",
            "eeeeeneeeeeeeweseeewwe",
            "neneneneenenwneneneneneneneneneneneswne",
            "seeseeseesesesenweseeeswneenwnwee",
            "seeeseenwswneeeewswewneseeew",
            "nwseenwnwnwnwnwswnwnewwseenwnwnwsenw",
            "esweeeweweneeseeneseee",
            "ewswnwswswwnewswwsweseswweswswww",
            "sweseeneswwwwnwnenwsenwwsweseswe",
            "eweeseseeswnwneneeeneew",
            "swnweseeeeeneweeeeeeweene",
            "wwnwwweswwnenwwnwnwswwwewwsw",
            "neneeeweenewneneeeeneeeneswe",
            "swwwesewnwsenenwnewnwneseenwwswnew",
            "senwnwnenwnwnewnwneneswwneeneneenewse",
            "nwwwnwwwwwnwwwwnwsewnwwwsew",
            "wwwswwswwwswwwwseneneeswwwww",
            "neswswseswswnwseswseseswswwseswsesesesese",
            "nwsewnwnwwnewnwnwnenwneenwnwseneenw",
            "seseseseseseseesenewsesesesenesewsese",
            "newwnwnwnwwnwwwnwsenwnwnwwwsenww",
            "swswenwnewswswswneweweswswswswnwsesese",
            "seeseswseseseseesesesesesewseseswnwse",
            "nenewneneneswnenwsenenenenewneenenenee",
            "sewseneseswneseneseeewswnwswsewswwsw",
            "eeswwswswswswswswswswswsenwswswnwwswsw",
            "seneenwswseseeeneeeeeswswsenewe",
            "nenenwnwnenwenenenwnewnwwsenenenwnwne",
            "nenwenwswenwnenwnwsenwwnwenwnwswnwnw",
            "wwwsenwwswnwwwewenewwwwwnwnw",
            "nwnwnwnesenwnwnwsenwnwnwswnwnwnwnwnwnwnw",
            "swswswswseswsweswswnwswwswseswneswswswwsw",
            "eswnenwneenwewnwnwnww",
            "seseseseeseneseseseseseseweeseseseswnw",
            "enwwnwnwswnwnwnwnwwwnwnwnwwnw",
            "swsewseneswneewwwwnwnwnwnwwseswsesww",
            "nenesenwnenenenenenenenenenewneneswnee",
            "nwweswwsweswswswneswswnwswswswswww",
            "neeneeneneneeneenenenenenenewsenene",
            "nwswswswsweswwswswswwswewwwswswsw",
            "swwswwwwwswwseswswswnwweneswswsw",
            "wswnwneeneswnwwswwnwwsenenewwnwww",
            "nenenenenwwenwnwnenwenenww",
            "seseneseseswwneseeseewseseesesenwse",
            "nwenwsenwnewneswnwnenwnwnwneenwnwnwnenw",
            "nwnwnenwnwnwnwnweswnenwsenenwnwnwnenwnwwne",
            "nwnenwenwswnwenwwswnenwnenwnwnwnwewnee",
            "wwnwenwnwwnwwwnenwnwwnwnwnwesww",
            "neseeneeeneeeeewneneneneeenee",
            "swseseswnwneeseseneswswwswswnwswse",
            "swnwswseswswwswswswneswsw",
            "swwweeeeswnwneneeewnweeeeee",
            "enenwnenwsenwswnwnwnenenwnewnesenwnwsenwnw",
            "swewswwneneseeeenwneenenwnenwseee",
            "seseseseseenwseneseseeseswseeseswnwse",
            "seneeeseseeeseswnwswsesesenwwwseenw",
            "wwnewwwwwwwsenewwwwwwswwnww",
            "nwswwwswswwnwsweswswsweswswsesweswsw",
            "swswnwnwnwnwwenwnwnwenenwnenw",
            "seeseseseseweseseeeseseesee",
            "wnwnwnwsewnwnwnwnw",
            "nwnenenwnwneswnwnwnwnwnwnwnwneenwnwwnw",
            "sweeseneweseneswenwsesenweswsenwnee",
            "wswnwnweseseewenenenwenenesweene",
            "nwnesenwnwseesewneswwnesweswnwnwsenww",
            "sewneswwwnwewnenwnwnwwwswnwwnwswe",
            "eeesweseneeweenweeswenweneewe",
            "neneneeneeeeneneeesweeenwenene",
            "eeneneeeneeeenwneeswweneenenee",
            "sesweeneeseeeweeeenweeeese",
            "swnwswneewwwnwwsewwnwnwwnwnwwenwnw",
            "eeeeesweseeeseswenwnwenwnwee",
            "sewwnewwswwwwswnesewswsw",
            "nesewseewseswsesewwswweseseneswswe",
            "newneenwsewnenenenenenwne",
            "sewsenwseeseesewneswwwnwsenweswnene",
            "swswswwwwwswwwswneww",
            "wnewwwwwwwwwwwwwwwswsw",
            "swwwnewwwwswsenwnwwwnenwseenww",
            "neeenenwweneeneeneneneneneeneneswse",
            "nwsenwnwnwnwnwswwnenenwsewnwswnwenwse",
            "nwesenwnewwnwwwnwwwwnwwswnwnww",
            "neswwsenwswnwwwswswseswswwsw",
            "neneeneswneneneenewnewseenenenenene",
            "seswseswsesesenesesesesesese",
            "wwwnwwwwweswewswnw",
            "seneseswsesewseseneesewwseswwswsenese",
            "enenwnwnwwnwswnwnwnwswnwenwnwswenwnw",
            "neswwsenwswsweneswswnwswseswnesenwnesw",
            "senwwnweswseswneswneewneseesesesesene",
            "nwenwwnesenwnwwswenwnenwswnenwnenwse",
            "weneeseeneeswweswnweswenwewswnw",
            "eswnwewwnwnwswwswwswene",
            "swwnwneseseseneneseswewswwsenewseee",
            "seswwnwwwswseeswwswnwwsewwenene",
            "neeneneneneneenenenenewsenenenwswnene",
            "eesesweeeeeeeneenweeweseww",
            "seswswswnwswswsweswswnwswswswseswswswsesw",
            "nenwnwnwnenenwnwnwnwwnwsenesenwnenwnew",
            "neneswneneneenesenwnenenwnenwwnewnesene",
            "wnwnwewnwwswsenewenenwnwswwwwwwnw",
            "weneswnenwnenwnenenenenwnenesewnwnene",
            "eneneewswneenenwswnenweeeeneesee",
            "neswseseswseswwseseseseswnwnwnwswnwsesw",
            "nenenenenenwenenewnwnwwsenwnenenenene",
            "weseswnewnenwwenwnwnwewnenenwsenw",
            "swenwnewewwwnwneswewwsewnwsewnw",
            "seseseseneswseseswseswseseseseswnesewsesese",
            "nwnwnwnenwnwnwnwnenwnwenwnwnwnwnwnwnwsww",
            "nwesweeeneeeeseeeeeneeeeee",
            "esweseswnenwsenwnw",
            "swneswwswswswewswwnwnewswsenenenwsw",
            "enenwnwswnwnwnwsenenwnwnenenwnwnenwnwsenw",
            "neswneswswswsenwseseneswseseneswseswswse",
            "ewneneenwnenwneneswnwnw",
            "swsenwsenesweswwnwsenesewswswwseswswsesw",
            "swwswwswwwswwswswwswnwswswswe",
            "nwwnewwewwwwswneswwswswseswswsw",
            "seeseswwnesesenwsenwswweseswenwnese",
            "nwnenwneneswswwnenesenenwnenenwnwnenenene",
            "nenwnenwnwswnwnenwnewneneneneneneneenwne",
            "eeeenenenenwneneeeeeeneewsese",
            "neseswsesesesewsenewsesenwseneswnesesw",
            "eswnwswsweeswnwnwnwwswwwswwesesesw",
            "wnwwsenwwwenwswswswsewwenewnwwnw",
            "seweeeeeeeneeeeseenwswnenwee",
            "nwsenwwwwwwwwwwwwnwwwneww",
            "swneswswswswwswsweswwwswnenwwswseswsw",
            "swneeeeeeesweenwneneeeeeee",
            "neewsewswseswswsenwswswneswwswenesw",
            "wnwnwsewwswnwwwwnwnwnwnenw",
            "neseseeseswsesesewswsenwswseseswseswswnw",
            "nwenenwnwnwnwnwnwnwwswnwnwnwnwnwsenwe",
            "neneeewnewnweseenenwswwnesewsee",
            "sesewseswneswswseseswnwswswneswneswswe",
            "seseseswseswsesesesesesesesenesesewsesenwse",
            "enwwwswswwwnwsesewwewwwnwew",
            "nwsenwnweseswnwnenwnwnwnwnwnwwwswnwnw",
            "nwseseeneseesweseseseseseseseeswnwsese",
            "seswswseswneseswswneswswswseswswswswswswnw",
            "swswsenwseswswseswswswseseneseseseswneswsw",
            "seseeseswswswswswseswnwswswnwsweswsesw",
            "wneseswseseneseesenenwwnesewseswswese",
            "neneswnenwnenenenenwneesenwnenenenwnwnene",
            "sweneseweseeeeeseeeeeenweeee",
            "enwnwnwnwnwnewenwsenenenwnenenwnwswne",
            "weeeeneeeseseeeeseseesewewee",
            "wsenwnwsewwwwwwwnwwnwnewnwww",
            "nwnwewnwnewnwsewnwwnwnwnwnwwwwwse",
            "swswswwwwswwswswsewnewwwswwneww",
            "nenwswewwswneswwswswwweswswwww",
            "neneneswenenenwneneweewnwnenenenene",
            "swswseneseseseseswseswsese",
            "nwnesenwnenenewneseswwwnwenenwseene",
            "wsesewseseseseseseesesesesesenesesese",
            "seseweswsesesesesesesese",
            "nwnwnwnenwsenwnwnwneswnenwnenwenwnenwnw",
            "seseswewseneseseseseseseswswsesewnese",
            "seswseswsesenesesesesesenwswseseseswsese",
            "sweenwseeeeeeeeenweeeeeeswe",
            "eenwwnewswsesewewswswseneswnewww",
            "wwwwwwwwwnwwsewww",
            "wnwnenwswenwnwnwnwse",
            "swneseneneswneneesewwnenene",
            "wwswewwwwwwwwnwwnwwwswwswwse",
            "sesesewseeseswsewseswnesesesesesesene",
            "swwwwwwwwwswwwwnewwwsenesw",
            "swswseswswnwswswswseseeswsw",
            "newwswsenwnewwsewseseswenwneeswwnw",
            "nwsweswswswswswswnwswswswseswswswswsesw",
            "nwswneseswseswsenewenewwneseewsenw",
            "sewseneseneseseseseesewseseneswsesesese",
            "wnwnwenwnwnwnwnwnwnwnwsenwnwneswnwnwne",
            "sewneewwsesenwnwnewwswwwwneswwsw",
            "wwwnwsewwwwswwwwwnwwwwwe",
            "ewneenwseneneewswneswneenenwenee",
            "swwswswswswswswneswswseswswweswswswswswsw",
            "nwswnwswnenenwswneneneneeneene",
            "esenenenwneneneswenenewneneneenenenenene",
            "swswsweseewweswwsesw",
            "enwewwnwnwsenwneseswwesewnewenew",
            "eeeseseseeneesweeeeeee",
            "ewenwwwswwseswnwswnwwswsweswwww",
            "wnwweeneenewnenenwnewswneeesew",
            "nenwnenwnenenwswnenwnewnwnenwenwnwnene",
            "neneneseneeeneneesewneneeew",
            "neneneneneneneenenwneneneneswneenesenene",
            "nenwnenenwnwnwenenwnwneneneenewswnenenw",
            "swwwswwwwewswswswwwswnwseneswswww",
            "neenenewneneseneneesenewenenenenwnwnese",
            "wnwwweswswswwswweswswwswwswwwwnw",
            "wswwwewwswwswwwwwwnwwwsw",
            "senwesewenwseseswesesesenwswnwsesee",
            "nwnwnwnwnwneneswnwsenenwnenwsenenwnwnw",
            "seewwwwwnwnwwwwnwneswwwnwswnww",
            "seeweseeeseeeeseneeswseeseene",
            "enesweeweeeneesweneseeeeneee",
            "senwseweswneeesweeneseweeesenesew",
            "swswseswseswswsweneswswswwenwwneswsw",
            "swswswwewnwswswswswewwswswswswswswsw",
            "swswswseswsesewseswseenwseswseswnwnesese",
            "ewswswswswswneswswnwwenwsesenwswwsee",
            "swnenwweesenwswnewneneneswnwnwnwswnenw",
            "nwnwnwnwnenwswnenwnenwenwnwwnwnenenwnwne",
            "neneseenewnenenenewnenenenesenenenenenee",
            "nwnwnwswnenwnwenwsenenwnwnenwnwnenenwnwnene",
            "nenenenenewnenewsenenenenesenenenenene",
            "swswswswseseesesesenwswswsesenwseseseese",
            "neneneweeeenesesewneweneeeneene",
            "wwnwewswsewwnewwnewwwww",
            "seseeeeseeeeeesewseee",
            "senesesesweseswenwnesesweseneeseswe",
            "wweeswswnwswwsesesewesenwseewee",
            "nwwnwwnwnwnwnwwwnwnwswenwwnwnw",
            "wwswnewwwwwwwwwwsewwwww",
            "neneeneneneneneneeneenenesenenenewne",
            "neweseeeeseeeeeeeswseeseesee",
            "nweneneswnwnwnwnenesenesewnwnwneenwnwnw",
            "neneseswnenenwnwswwnwnenwnewseneseneneenw",
            "nenenewesewseweeeswswneswneseee",
            "swswseseeseseeeseesesesenwsesesesenwsese",
            "seneeneenweeneneenesweeewneee",
            "wwwewsenewwswneeneeswseswswenw",
            "nwnwwnewsenewswnesenwwwnwenwwsenwnw",
            "seeenwneewnweswnweewseewseese",
            "sewnwswwwwnewwnewnwwwsewwww",
            "swsenwnwewnwnwnwnwnwnwwnwnw",
            "enwesweeseeneswneeseewsewwnee",
            "wnwweneswswwsenwwwnwwwwnwnwww",
            "wwwswwwwwwnewswwwweswwww",
            "swswnewswswsweswswwwswseswswwwsw",
            "wwwnwwewnwwwwnwwwwnwwwnwsew",
            "nwnwnwnwnwsenwwnwsewnwnwnwnwnwnenwnwnw",
            "nesenweswnenenesenenenwneneeeneenwne",
            "sweeeseeeeesenweseeseseeese",
            "eseswwswseswseswneseseseseseswswseswwswsw",
            "nwenwnwsenwnwnenwnwnwnwnwnwsenwnwnwwne",
            "wewwwwwnewwwwewwnwwswww",
            "wnwwwswswneeswswswswswseseseesenwesw",
            "seweeeenweeeeesweeeeee",
            "nenenenenwsewnenenwneneswnenenenenenenene",
            "neesewneswneeeneneneneeenwwwneene",
            "swseswneswseswwesesweswswswnwseswnwswswe",
            "newnwwwseswswwswwswnwswwwsewswwne",
            "swswswwswswswswswsenwseswnenewswwswsw",
            "neeneenenewneeweeeswneseeeeee",
            "newsewnenwnewsenesewnwee",
            "nwnwswswseseneswswnweseswsenwneseweswsw",
            "seswswwwwwswwneswswwenewswne",
            "eneeenwwnwwwwwswnwnwswnwnwwww",
            "nwnwnwnwnwnwnwwewnwsww",
            "seeswneseseseeseesesesewenwseenwwse",
            "nwnwwswnwwwwewnwnwwwnwnwwewnwew",
            "eeseseseeeseneseeeseseeneseseswwsese",
            "swwswswweswwwnwswneswwswswswwww",
            "wnwswwwwwnwnwwwsenenwewswwwnenw",
            "sewswseseswnwnwsweenweswwseswswswnenwne",
            "wwnwwwnwnwwwwwnenwwnewswnwwnwwse",
            "seweswwneeswsenwswnwwswswswnwseswwsw",
            "nwnenenwnwnwneswnenenenenwenwnenwnenwneswne",
            "sewsenwnenwnwwwnenwnewwsenwwnwwnwse",
            "seseeseseenweneseweseswswswnesesenw",
            "swwneswwwwswwnewwswwseswswwsww",
            "seesweswnwseseseswnwnwswswnwse",
            "eesesweseeeseeeeeeenwsweene",
            "swseswswswseswswswswswswswwswswenwswsesw",
            "swwswswsweswwwnwswswswnwswwsewswsw",
            "nwnwwwnwwnwnwwnwwneenwwwwnwsesee",
            "weeenwseseseseeseenesesee",
            "nwnwwseenwwwwnwwnwwnwnenwsenwnww",
            "eeeeseseeeeswseeseseswneseeseeenw",
            "eeeeeneesweeeesweeenenweeee",
            "eweeseneeseesesenesweeseewsewne",
            "neneneswswenenwwnenewsweneswenwswsenw",
            "wwswnwsewneseswswneesenewswwwsenwsw",
            "seswswsweswnweswesweswswwnwnwsesese",
            "weneeeneeeeewseeeeeeeneee",
            "nwwnewwswwswswswwwswswwewswwwe",
            "seseesewseneeseesesweswsenwwsenwsese",
            "wenwseeswwnwewwneswnwenwwnwnwse",
            "wwwewswwwwwwwewnwwwwwnww",
            "seseswsesweweeesenweeseeeeseesenw",
            "swseneswneseswwswswswswseneseswswswswsesww",
            "esenwseseeeseseseesesesesenwwnwseene",
            "neeneneneeneeenenenwwswe",
            "wnewwswswswwwswswswswsewswwswwsw",
            "eswnwesenesweeeneesweeneseeesw",
            "nenwneneneeswneneneswnwnenene",
            "swswswswswswswneseswswenwwnweswswswsw",
            "eeeeneeeneeewneneeeeeeenwesw",
            "neneeesweseneneenwseenwnee",
            "nwnenenenwswewnwnwnwnwnwnweswseswnenw",
            "wneseseseseseseswseneswseseswsesesesese",
            "wswsweseseswswneswwswneswneswswnwswsw",
            "swnwnweneneswsewnenwnw",
            "nwnwnwnwnewnwnenwnenwnwnwnwnwnesenenesene",
            "nenenenenenwnenenwneenwswnenwsenenenenw",
            "swnwswnweesenwswswswsenwswseseeswswsesw",
            "ewswnwseswsenweenwsenesesewsee",
            "nwnewseewsewnwnwnesewwnwsesenwwswnw",
            "nwnwnenwnenwneneneenwwnenenenwswnwenenw",
            "nwwwswwwwwwnwwwswwenwnwwwe",
            "wnwsenesenwswsewswneswswnwsewswwwwsw"
        };

#endregion Data
    }
}

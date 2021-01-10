// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Year2015
{
    /// <summary>
    /// Day 15: Science for Hungry People
    /// https://adventofcode.com/2015/day/15
    /// </summary>
    [TestClass]
    public class Day15ScienceForHungryPeople
    {
        /// <summary>
        /// Part 1 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part1_SampleInput()
        {
            Assert.AreEqual(62842880L, ExecutePart1(sampleInput));
        }

        /// <summary>
        /// Part 1 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part1_PuzzleInput()
        {
            Assert.AreEqual(222870L, ExecutePart1(puzzleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using sample input
        /// </summary>
        [TestMethod]
        public void Part2_SampleInput()
        {
            Assert.AreEqual(57600000L, ExecutePart2(sampleInput));
        }

        /// <summary>
        /// Part 2 of the puzzle using my actual puzzle input
        /// </summary>
        [TestMethod]
        public void Part2_PuzzleInput()
        {
            Assert.AreEqual(117936L, ExecutePart2(puzzleInput));
        }

        /// <summary>
        /// Executes part 1 of the puzzle
        /// </summary>
        /// <param name="ingredientDefinitions">The ingredient definitions</param>
        /// <returns>The high score of the best combination</returns>
        private static long ExecutePart1(string[] ingredientDefinitions)
        {
            return GetHighScore(ingredientDefinitions);
        }

        /// <summary>
        /// Executes part 2 of the puzzle
        /// </summary>
        /// <param name="ingredientDefinitions">The ingredient definitions</param>
        /// <returns>The high score of the best combination with 500 calories</returns>
        private static long ExecutePart2(string[] ingredientDefinitions)
        {
            return GetHighScore(ingredientDefinitions, 500);
        }

        /// <summary>
        /// Computes the high score of all amounts of ingredient combinations
        /// </summary>
        /// <param name="ingredientDefinitions">The ingredient definitions</param>
        /// <param name="calorieTarget">The target number of calories for the cookie. Use null to
        /// denote no calorie target</param>
        /// <returns></returns>
        private static long GetHighScore(string[] ingredientDefinitions, int? calorieTarget = null)
        {
            var ingredients = ParseIngredients(ingredientDefinitions);
            var amounts = BuildAmounts(100, ingredients.Count);
            long maxScore = 0;

            // Go through each possible combination of amounts
            foreach (var amount in amounts)
            {
                // Multiply the ingredient qualities times the amount to determine the overall
                // values for the combination
                int capacity = 0;
                int durability = 0;
                int flavor = 0;
                int texture = 0;
                int calories = 0;
                for (int i = 0; i < ingredients.Count; i++)
                {
                    capacity += ingredients[i].Capacity * amount[i];
                    durability += ingredients[i].Durability * amount[i];
                    flavor += ingredients[i].Flavor * amount[i];
                    texture += ingredients[i].Texture * amount[i];
                    calories += ingredients[i].Calories * amount[i];
                }

                // Skip this amount if it doesn't meet the calorie target
                if (calorieTarget != null && calories != calorieTarget)
                {
                    continue;
                }

                // Compute the score for this combination, zeroing out negative values
                long score = Math.Max(capacity, 0) * Math.Max(durability, 0) * 
                             Math.Max(flavor, 0) * Math.Max(texture, 0);
                maxScore = Math.Max(score, maxScore);
            }
            return maxScore;
        }

        /// <summary>
        /// Recursively creates a list of all possible combinations of ingredient amounts
        /// </summary>
        /// <param name="amountRemaining">The total amount of ingredients remaining to be 
        /// added</param>
        /// <param name="ingredientsRemaining">The number of ingredients remaining to be 
        /// added</param>
        /// <returns>The list of ingredient combination amounts</returns>
        private static List<List<int>> BuildAmounts(int amountRemaining, int ingredientsRemaining)
        {
            var amounts = new List<List<int>>();
            if (ingredientsRemaining == 1)
            {
                // We're at the end of the list of ingredients so just use what is left
                amounts.Add(new List<int>() { amountRemaining });
            }
            else
            {
                // Go through every possible value for this ingredient
                for (int i = 1; i <= amountRemaining - ingredientsRemaining + 1; i++)
                {
                    // Build a list of all possible values for the remaining ingredients
                    var subAmounts = BuildAmounts(amountRemaining - i, ingredientsRemaining - 1);
                    foreach (var subAmount in subAmounts)
                    {
                        // Add the current value to each of the remaining values
                        amounts.Add(new List<int>(subAmount.Prepend(i)));
                    }
                }
            }
            return amounts;
        }

        /// <summary>
        /// Parses the ingredient definitions into objects
        /// </summary>
        /// <param name="ingredientDefinitions">The ingredient definitions</param>
        /// <returns>The list of Ingredient objects</returns>
        private static List<Ingredient> ParseIngredients(string[] ingredientDefinitions)
        {
            var ingredients = new List<Ingredient>();
            foreach (string definition in ingredientDefinitions)
            {
                ingredients.Add(new Ingredient(definition));
            }
            return ingredients;
        }
        private static readonly Regex ingredientRegex = new Regex("(.*): capacity ([-]?[0-9]*), durability ([-]?[0-9]*), flavor ([-]?[0-9]*), texture ([-]?[0-9]*), calories ([0-9])");

        /// <summary>
        /// Represents the characteristics of an ingredient
        /// </summary>
        private class Ingredient
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="definition">The definition for the ingredient</param>
            public Ingredient(string definition)
            {
                var match = ingredientRegex.Match(definition);
                Name = match.Groups[1].Value;
                Capacity = int.Parse(match.Groups[2].Value);
                Durability = int.Parse(match.Groups[3].Value);
                Flavor = int.Parse(match.Groups[4].Value);
                Texture = int.Parse(match.Groups[5].Value);
                Calories = int.Parse(match.Groups[6].Value);
            }

            /// <summary>
            /// The name of the ingredient
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// How well the ingredient helps the cookie absorb milk
            /// </summary>
            public int Capacity { get; }

            /// <summary>
            /// How well the ingredient keeps the cookie intact when full of milk
            /// </summary>
            public int Durability { get; }

            /// <summary>
            /// How tasty the ingredient makes the cookie
            /// </summary>
            public int Flavor { get; }

            /// <summary>
            /// How the ingredient improves the feel of the cookie
            /// </summary>
            public int Texture { get; }

            /// <summary>
            /// How many calories the ingredient adds to the cookie
            /// </summary>
            public int Calories { get; }
        }

        #region Data

        private static readonly string[] sampleInput =
        {
            "Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8",
            "Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3"
        };

        private static readonly string[] puzzleInput =
        {
            "Sugar: capacity 3, durability 0, flavor 0, texture -3, calories 2",
            "Sprinkles: capacity -3, durability 3, flavor 0, texture 0, calories 9",
            "Candy: capacity -1, durability 0, flavor 4, texture 0, calories 1",
            "Chocolate: capacity 0, durability 0, flavor -2, texture 2, calories 8"
        };

        #endregion Data
    }
}

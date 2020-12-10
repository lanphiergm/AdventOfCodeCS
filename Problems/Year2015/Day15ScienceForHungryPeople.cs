using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Problems.Year2015
{
    class Day15ScienceForHungryPeople : ProblemBase<long>
    {
        public Day15ScienceForHungryPeople(ILogger logger) : base(logger, "Science for Hungry People", 2015, 15) { }

        protected override long ExecutePart1()
        {
            return GetHighScore();
        }

        protected override long ExecutePart2()
        {
            return GetHighScore(500);
        }

        private static long GetHighScore(int? calorieTotal = null)
        {
            var ingredients = ParseIngredients();
            var amounts = BuildAmounts(100, ingredients.Count);
            long maxScore = 0;
            foreach (var amount in amounts)
            {
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
                if (calorieTotal != null && calories != calorieTotal)
                {
                    continue;
                }
                long score = Math.Max(capacity, 0) * Math.Max(durability, 0) * 
                             Math.Max(flavor, 0) * Math.Max(texture, 0);
                maxScore = Math.Max(score, maxScore);
            }
            return maxScore;
        }

        private static List<Ingredient> ParseIngredients()
        {
            var ingredients = new List<Ingredient>();
            foreach (string definition in ingredientDefinitions)
            {
                ingredients.Add(new Ingredient(definition));
            }
            return ingredients;
        }

        private static List<List<int>> BuildAmounts(int amountRemaining, int ingredientsRemaining)
        {
            var amounts = new List<List<int>>();
            if (ingredientsRemaining == 1)
            {
                amounts.Add(new List<int>() { amountRemaining });
            }
            else
            {
                for (int i = 1; i <= amountRemaining - ingredientsRemaining + 1; i++)
                {
                    var subAmounts = BuildAmounts(amountRemaining - i, ingredientsRemaining - 1);
                    foreach (var subAmount in subAmounts)
                    {
                        amounts.Add(new List<int>(subAmount.Prepend(i)));
                    }
                }
            }
            return amounts;
        }

        private class Ingredient
        {
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

            public string Name { get; }
            public int Capacity { get; }
            public int Durability { get; }
            public int Flavor { get; }
            public int Texture { get; }
            public int Calories { get; }
        }

        private static readonly Regex ingredientRegex = new Regex("(.*): capacity ([-]?[0-9]*), durability ([-]?[0-9]*), flavor ([-]?[0-9]*), texture ([-]?[0-9]*), calories ([0-9])");

        #region Data

        private static readonly string[] ingredientDefinitions =
        {
            "Sugar: capacity 3, durability 0, flavor 0, texture -3, calories 2",
            "Sprinkles: capacity -3, durability 3, flavor 0, texture 0, calories 9",
            "Candy: capacity -1, durability 0, flavor 4, texture 0, calories 1",
            "Chocolate: capacity 0, durability 0, flavor -2, texture 2, calories 8"
        };

        #endregion Data
    }
}

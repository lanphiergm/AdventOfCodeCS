// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using System;
using System.Reflection;

namespace AdventOfCode
{
    /// <summary>
    /// Application startup object
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The application entry point
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <remarks>
        /// This method is used for profiling individual tests that are not performing well.
        /// </remarks>
        static void Main(string[] args)
        {
            // Set the puzzle and part to run here
            var instance = new Puzzles.Year2020.Day22CrabCombat();
            PuzzlePart part = instance.Part2_PuzzleInput;

            // Log what is being run and invoke it
            Console.WriteLine($"Executing {instance.GetType().FullName} {part.GetMethodInfo().Name}");
            part.Invoke();
        }

        /// <summary>
        /// Defines the part of a puzzle that is being executed
        /// </summary>
        private delegate void PuzzlePart();
    }
}

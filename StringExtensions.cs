// ############################################################################
// # Galen Lanphier                                                           #
// # https://github.com/lanphiergm/AdventOfCodeCS                             #
// # MIT License. See LICENSE file                                            #
// ############################################################################

using System.Text.RegularExpressions;

namespace AdventOfCode
{
    /// <summary>
    /// Contains extensions of the string class
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Replaces only the first occurance of oldValue with newValue
        /// </summary>
        /// <param name="instance">The string to use as input</param>
        /// <param name="oldValue">The old value to find</param>
        /// <param name="newValue">The new value to replace with</param>
        /// <returns>The modified string</returns>
        public static string ReplaceOnce(this string instance, string oldValue, string newValue)
        {
            var regex = new Regex(Regex.Escape(oldValue));
            return regex.Replace(instance, newValue, 1);
        }
    }
}

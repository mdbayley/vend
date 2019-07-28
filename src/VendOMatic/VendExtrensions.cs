using System;
using System.Collections.Generic;
using System.Linq;

namespace VendOMatic
{
    public static class VendExtrensions
    {
        /// <summary>
        /// This extension method iterates across all possible permutations recursively to find the first set of 
        /// available Denominations that sum to the required value, favouring higher Denominations.
        /// </summary>
        public static bool TryMakeChange(this IList<Denomination> available, decimal value, out IList<Denomination> change)
        {
            if (value == 0) // No calc required
            {
                change = new List<Denomination>();
                return true;
            }
            if (available == null || value < 0M) // Invalid inputs, could throw here
            {
                change = null;
                return false;
            }

            // Need a descending-order list accessible by index, for recursion
            var denominations = available.OrderByDescending(denomination => denomination.Value).ToList();

            // Create output collection
            var result = new List<Denomination>();

            // Start recursion (use recursion to ensure you can rewind the iterations to cover all possibilities)
            if (CheckForChange(denominations, 0, value, result))
            {
                // Success
                change = result;
                return true;
            }

            // Not possible
            change = null;
            return false;
        }

        private static bool CheckForChange(IReadOnlyList<Denomination> denominations, int index, decimal required, ICollection<Denomination> change)
        {
            // Which denomination? (Processed in descending Value)
            var denomination = denominations[index];

            // No need to iterate across entire Count, only what we have or what we need.
            var start = Math.Min(denomination.Count, (int)(required / denomination.Value));

            // Iterate in descending count inlc. zero, use most of highest values first
            for (var count = start; count >= 0; count--)
            {
                // What is left after this count of Value
                var remaining = required - (count * denomination.Value);

                // This completes the required change successfully
                if (remaining == 0)
                {
                    if (count > 0) // Just keeps the response neat
                    {
                        change.Add(new Denomination{Value = denomination.Value, Count = count});
                    }
                    return true;
                }

                // Too much! (This should actually not be possible based on start/count above)
                if (remaining < 0) return false;

                // No more denominations in this recursive iteration
                if (index + 1 >= denominations.Count) continue;

                // Recursive call to allow rewinding the stack (ensure all possible permutations are evaluated)
                if (CheckForChange(denominations, index + 1, remaining, change))
                {
                    // Required change found, populate with successful denomination counts as unwinding
                    if (count > 0)
                    {
                        change.Add(new Denomination { Value = denomination.Value, Count = count });
                    }
                    return true;
                }
            }

            // No match evaliuated for this (or lower) denominations
            return false;
        }
    }
}

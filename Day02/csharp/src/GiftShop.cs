using System.Linq;

namespace AdventOfCode.Day02
{
    /// <summary>
    /// Configuration constants for the gift shop validation.
    /// </summary>
    public static class Configuration
    {
        public const string DEFAULT_INPUT_PATH = "../puzzle_input";
        public const string TEST_INPUT_PATH = "../puzzle_input_small";
    }

    /// <summary>
    /// Represents the validation result for a single product ID.
    /// </summary>
    public struct ProductValidationResult
    {
        public bool IsValidPartOne { get; init; }
        public bool IsValidPartTwo { get; init; }
        public long ProductID { get; init; }
    }

    /// <summary>
    /// Tracks validation statistics for both parts of the problem.
    /// </summary>
    public class ValidationStatistics
    {
        public int ValidCountPartOne { get; set; }
        public int InvalidCountPartOne { get; set; }
        public long InvalidSumPartOne { get; set; }

        public int ValidCountPartTwo { get; set; }
        public int InvalidCountPartTwo { get; set; }
        public long InvalidSumPartTwo { get; set; }

        public void UpdateFromResult(ProductValidationResult result)
        {
            if (result.IsValidPartOne)
                ValidCountPartOne++;
            else
            {
                InvalidCountPartOne++;
                InvalidSumPartOne += result.ProductID;
            }

            if (result.IsValidPartTwo)
                ValidCountPartTwo++;
            else
            {
                InvalidCountPartTwo++;
                InvalidSumPartTwo += result.ProductID;
            }
        }
    }

    /// <summary>
    /// Validates product IDs according to gift shop criteria.
    /// </summary>
    public class GiftShop
    {
        private readonly ValidationStatistics _statistics = new();
        private readonly bool _enableDebugLogging;

        /// <summary>
        /// Initializes a new instance of the GiftShop class.
        /// </summary>
        /// <param name="enableDebugLogging">Whether to enable debug logging for invalid product IDs</param>
        public GiftShop(bool enableDebugLogging = false)
        {
            _enableDebugLogging = enableDebugLogging;
        }

        /// <summary>
        /// Processes the puzzle input and validates all product ID ranges.
        /// </summary>
        public void ProcessInput()
        {
            var productIDRanges = PuzzleInputManager.GetIDs();
            System.Console.WriteLine($"Processing {productIDRanges.Count()} Product ID ranges...");

            foreach (var range in productIDRanges)
            {
                ProcessProductIDRange(range);
            }
        }

        /// <summary>
        /// Outputs the results for Part One of the puzzle.
        /// </summary>
        public void PartOne()
        {
            System.Console.WriteLine("Part One:");
            System.Console.WriteLine($"Total Valid Product IDs: {_statistics.ValidCountPartOne}");
            System.Console.WriteLine($"Total Invalid Product IDs: {_statistics.InvalidCountPartOne}");
            System.Console.WriteLine($"Answer - Sum of Invalid Product IDs: {_statistics.InvalidSumPartOne}");
        }

        /// <summary>
        /// Outputs the results for Part Two of the puzzle.
        /// </summary>
        public void PartTwo()
        {
            System.Console.WriteLine("Part Two:");
            System.Console.WriteLine($"Total Valid Product IDs: {_statistics.ValidCountPartTwo}");
            System.Console.WriteLine($"Total Invalid Product IDs: {_statistics.InvalidCountPartTwo}");
            System.Console.WriteLine($"Answer - Sum of Invalid Product IDs: {_statistics.InvalidSumPartTwo}");
        }

        /// <summary>
        /// Processes all product IDs within the specified range.
        /// </summary>
        /// <param name="range">The range of product IDs to validate</param>
        private void ProcessProductIDRange(ProductIDRange range)
        {
            for (long productID = range.StartID; productID <= range.EndID; productID++)
            {
                var result = ValidateProductID(productID);
                _statistics.UpdateFromResult(result);

                if (_enableDebugLogging && (!result.IsValidPartOne || !result.IsValidPartTwo))
                {
                    System.Console.WriteLine($"Invalid Product ID: {productID}");
                }
            }
        }

        /// <summary>
        /// Validates a single product ID against both part one and part two criteria.
        /// </summary>
        /// <param name="productID">The product ID to validate</param>
        /// <returns>Validation result containing both part one and part two results</returns>
        private ProductValidationResult ValidateProductID(long productID)
        {
            string productIDString = productID.ToString();

            return new ProductValidationResult
            {
                ProductID = productID,
                IsValidPartOne = IsValidProductIDPartOne(productIDString),
                IsValidPartTwo = IsValidProductIDPartTwo(productIDString)
            };
        }

        /// <summary>
        /// Validates a product ID according to Part One criteria.
        /// A product ID is valid if it does not consist of two identical halves.
        /// </summary>
        /// <param name="productID">The product ID string to validate</param>
        /// <returns>True if the product ID is valid, false otherwise</returns>
        private static bool IsValidProductIDPartOne(string productID)
        {
            int length = productID.Length;

            // Odd length cannot be made of two equal halves
            if (length % 2 != 0)
                return true;

            int halfLength = length / 2;
            string firstHalf = productID.Substring(0, halfLength);
            string secondHalf = productID.Substring(halfLength);

            return firstHalf != secondHalf;
        }

        /// <summary>
        /// Validates a product ID according to Part Two criteria.
        /// A product ID is valid if it does not consist of any repeating sequence.
        /// </summary>
        /// <param name="productID">The product ID string to validate</param>
        /// <returns>True if the product ID is valid, false otherwise</returns>
        private static bool IsValidProductIDPartTwo(string productID)
        {
            int length = productID.Length;

            // Check for repeating sequences of length 1 to length/2
            for (int sequenceLength = 1; sequenceLength <= length / 2; sequenceLength++)
            {
                // Sequence length must divide evenly into total length
                if (length % sequenceLength != 0)
                    continue;

                string pattern = productID.Substring(0, sequenceLength);
                int repeatCount = length / sequenceLength;
                string reconstructed = string.Concat(Enumerable.Repeat(pattern, repeatCount));

                if (reconstructed == productID)
                    return false; // Found a repeating sequence that makes up the entire product ID
            }

            return true;
        }

    }

    /// <summary>
    /// Entry point for the Advent of Code Day 2 solution.
    /// </summary>
    public static class Solution
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments. Use --debug to enable debug logging.</param>
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Day 2 - Gift Shop");
            System.Console.WriteLine("-----------------------");

            // Enable debug logging with --debug flag
            bool enableDebug = args.Length > 0 && args[0] == "--debug";

            var giftShop = new GiftShop(enableDebug);
            giftShop.ProcessInput();
            giftShop.PartOne();
            giftShop.PartTwo();
        }
    }
}
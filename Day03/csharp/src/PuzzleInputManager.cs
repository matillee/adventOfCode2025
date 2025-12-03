
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Day03
{
    /// <summary>
    /// Represents a bank of batteries with their joltage values.
    /// </summary>
    public class BatteryBank
    {
        /// <summary>
        /// Gets the string representation of battery joltages.
        /// </summary>
        public string BatteryJoltages { get; private set; }

        private readonly bool _enableDebugLogging;

        /// <summary>
        /// Initializes a new instance of the BatteryBank class.
        /// </summary>
        /// <param name="line">String containing battery joltage digits</param>
        /// <param name="enableDebugLogging">Whether to enable debug logging output</param>
        /// <exception cref="System.ArgumentException">Thrown when input is invalid</exception>
        public BatteryBank(string line, bool enableDebugLogging = false)
        {
            _enableDebugLogging = enableDebugLogging;
            if (string.IsNullOrWhiteSpace(line))
                throw new System.ArgumentException("Input line cannot be null or whitespace", nameof(line));
            if (line.Trim().Any(c => !char.IsDigit(c)))
                throw new System.ArgumentException("Input line contains invalid characters", nameof(line));
            BatteryJoltages = line.Trim();
        }

        /// <summary>
        /// Finds the maximum possible two-digit number by selecting two digits from the battery joltages
        /// where the second digit appears after the first digit in the sequence.
        /// </summary>
        /// <returns>The maximum two-digit number that can be formed</returns>
        public long getMaxJoltagePartOne()
        {
            return getMaxJoltage(2);
        }

        /// <summary>
        /// Finds the maximum possible twelve-digit number where each subsequent digit appears after the 
        /// previous digit in the sequence.
        /// </summary>
        /// <returns>The maximum twelve-digit number that can be formed</returns>
        public long getMaxJoltagePartTwo()
        {
            return getMaxJoltage(12);
        }

        /// <summary>
        /// Finds the maximum possible number by selecting a specified number of digits
        /// where each subsequent digit appears after the previous digit in the sequence.
        /// Uses a greedy algorithm to select the largest possible digit at each step.
        /// </summary>
        /// <param name="numberOfBatteriesToTurnOn">Number of digits to select</param>
        /// <returns>The maximum number that can be formed</returns>
        public long getMaxJoltage(int numberOfBatteriesToTurnOn)
        {
            if (numberOfBatteriesToTurnOn <= 0 || numberOfBatteriesToTurnOn > BatteryJoltages.Length)
                return 0;

            string result = "";
            int currentPosition = 0;

            for (int digitIndex = 0; digitIndex < numberOfBatteriesToTurnOn; digitIndex++)
            {
                // Calculate how many positions we need to reserve for remaining digits
                int remainingDigitsNeeded = numberOfBatteriesToTurnOn - digitIndex - 1;

                // Calculate the latest position we can start searching from
                // to ensure we have enough positions left for the remaining digits
                int searchEndPosition = BatteryJoltages.Length - remainingDigitsNeeded;

                if (_enableDebugLogging)
                    System.Console.WriteLine($"Digit {digitIndex + 1}: Searching from position {currentPosition} to {searchEndPosition - 1}");

                // Find the maximum digit in the valid search range
                char maxDigit = '0';
                int maxDigitPosition = -1;

                for (int searchPosition = currentPosition; searchPosition < searchEndPosition; searchPosition++)
                {
                    if (BatteryJoltages[searchPosition] > maxDigit)
                    {
                        maxDigit = BatteryJoltages[searchPosition];
                        maxDigitPosition = searchPosition;
                    }
                }

                if (maxDigitPosition == -1)
                {
                    // This should not happen with correct bounds, but handle gracefully
                    if (_enableDebugLogging)
                        System.Console.WriteLine($"Warning: No valid digit found at position {digitIndex}");
                    break;
                }

                if (_enableDebugLogging)
                    System.Console.WriteLine($"Selected digit '{maxDigit}' at position {maxDigitPosition}");

                result += maxDigit;
                currentPosition = maxDigitPosition + 1; // Next search starts after the selected digit
            }

            if (_enableDebugLogging)
                System.Console.WriteLine($"Final result: {result}");
            return result.Length > 0 ? long.Parse(result) : 0;
        }
    }

    /// <summary>
    /// Manages loading and parsing puzzle input files for battery banks.
    /// </summary>
    public class PuzzleInputManager
    {
        /// <summary>
        /// Loads and parses battery bank data from the specified file.
        /// </summary>
        /// <param name="path">Path to the input file (defaults to puzzle input)</param>
        /// <param name="enableDebugLogging">Whether to enable debug logging for battery banks</param>
        /// <returns>Collection of parsed battery bank data</returns>
        public static IEnumerable<BatteryBank> GetBatteryBanks(string path = Configuration.DEFAULT_INPUT_PATH, bool enableDebugLogging = false)
        {
            var input = GetInput(path);
            return input.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries)
                       .Select(line => new BatteryBank(line, enableDebugLogging));
        }

        /// <summary>
        /// Reads the entire contents of the specified input file.
        /// </summary>
        /// <param name="path">Path to the input file</param>
        /// <returns>Raw file contents as string</returns>
        /// <exception cref="FileNotFoundException">Thrown when the file does not exist</exception>
        public static string GetInput(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Input file not found: {path}");
            }
            return ReadFile(path, Encoding.UTF8);
        }

        private static string ReadFile(string path, Encoding encoding)
        {
            string result;
            using (StreamReader streamReader = new StreamReader(path, encoding))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

    }
}
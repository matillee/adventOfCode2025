
namespace AdventOfCode.Day03
{
    /// <summary>
    /// Provides configuration constants for the Day 3 puzzle solution.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Default path to the main puzzle input file.
        /// </summary>
        public const string DEFAULT_INPUT_PATH = "../puzzle_input";

        /// <summary>
        /// Path to the test input file for debugging and validation.
        /// </summary>
        public const string TEST_INPUT_PATH = "../puzzle_input_small";
    }
    /// <summary>
    /// Represents the lobby with its floor layout.
    /// </summary>
    public class Lobby
    {
        /// <summary>
        /// Indicates whether debug logging is enabled for this lobby instance.
        /// </summary>
        private readonly bool enableDebugLogging;

        /// <summary>
        /// Accumulates the sum of maximum joltages for part one of the puzzle.
        /// </summary>
        private long sumOfMaxJoltagePartOne = 0;

        /// <summary>
        /// Accumulates the sum of maximum joltages for part two of the puzzle.
        /// </summary>
        private long sumOfMaxJoltagePartTwo = 0;

        /// <summary>
        /// Initializes a new instance of the Lobby class.
        /// </summary>
        /// <param name="enableDebugLogging">Whether to enable debug logging output during processing</param>
        public Lobby(bool enableDebugLogging = false)
        {
            this.enableDebugLogging = enableDebugLogging;
        }

        /// <summary>
        /// Processes all battery banks from the input file and calculates maximum joltages for both parts.
        /// Updates the internal sum variables with the accumulated results.
        /// </summary>
        public void ProcessInput()
        {
            var batteryBanks = PuzzleInputManager.GetBatteryBanks(Configuration.TEST_INPUT_PATH);
            foreach (var bank in batteryBanks)
            {
                if (enableDebugLogging)
                {
                    System.Console.WriteLine($"Processing Battery Bank: {bank}");
                }
                sumOfMaxJoltagePartOne += bank.getMaxJoltagePartOne();
                sumOfMaxJoltagePartTwo += bank.getMaxJoltagePartTwo();
            }
        }

        /// <summary>
        /// Outputs the result for part one of the puzzle (sum of two-digit maximum joltages).
        /// </summary>
        public void PartOne()
        {
            System.Console.WriteLine($"Part One: {sumOfMaxJoltagePartOne}");
        }

        /// <summary>
        /// Outputs the result for part two of the puzzle (sum of twelve-digit maximum joltages).
        /// </summary>
        public void PartTwo()
        {
            System.Console.WriteLine($"Part Two: {sumOfMaxJoltagePartTwo}");
        }

    }

    /// <summary>
    /// Entry point class for the Day 3 Advent of Code solution.
    /// </summary>
    public class Solution
    {
        /// <summary>
        /// Main entry point for the Day 3 solution. Processes command line arguments
        /// and executes both parts of the puzzle.
        /// </summary>
        /// <param name="args">Command line arguments. Pass "--debug" to enable debug logging</param>
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Day 3 - Lobby");
            System.Console.WriteLine("-----------------------");

            // Enable debug logging with --debug flag
            bool enableDebug = args.Length > 0 && args[0] == "--debug";

            var lobby = new Lobby(enableDebug);
            lobby.ProcessInput();
            lobby.PartOne();
            lobby.PartTwo();
        }
    }
}
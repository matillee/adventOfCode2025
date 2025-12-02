
using System.Collections.Generic;

namespace AdventOfCode.Day01
{
    /// <summary>
    /// Configuration constants for the dial simulation.
    /// </summary>
    public static class Configuration
    {
        public const int DIAL_POSITIONS = 100;
        public const int INITIAL_DIAL_POSITION = 50;
        public const string DEFAULT_INPUT_PATH = "../puzzle_input";
        public const string TEST_INPUT_PATH = "../puzzle_input_small";
    }

    /// <summary>
    /// Represents the result of a dial movement operation.
    /// </summary>
    public struct DialMovementResult
    {
        public int OldPosition { get; init; }
        public int NewPosition { get; init; }
        public int ZeroCrossings { get; init; }
        public bool LandsOnZero { get; init; }
    }

    /// <summary>
    /// Handles dial movement calculations and zero crossing detection.
    /// </summary>
    public class DialProcessor
    {
        /// <summary>
        /// Processes a single movement on the dial and calculates the result.
        /// </summary>
        /// <param name="currentPosition">The current position on the dial (0-99)</param>
        /// <param name="stepValue">The number of steps to move (positive=right, negative=left)</param>
        /// <returns>The movement result including new position and zero crossings</returns>
        public static DialMovementResult ProcessMovement(int currentPosition, int stepValue)
        {
            int oldPosition = currentPosition;
            int positionChange = oldPosition + stepValue;

            int newPosition = positionChange % Configuration.DIAL_POSITIONS;
            if (newPosition < 0)
                newPosition += Configuration.DIAL_POSITIONS;

            bool landsOnZero = newPosition == 0;
            int zeroCrossings = CalculateZeroCrossingsAndLandings(positionChange, stepValue, oldPosition, newPosition);

            return new DialMovementResult
            {
                OldPosition = oldPosition,
                NewPosition = newPosition,
                ZeroCrossings = zeroCrossings,
                LandsOnZero = landsOnZero
            };
        }

        /// <summary>
        /// Calculates the number of times the dial crosses or lands on position zero during movement.
        /// This method handles three scenarios: negative movement (left), positive movement (right),
        /// and single-rotation movements that land exactly on zero.
        /// </summary>
        /// <param name="positionChange">The raw position change (oldPosition + stepValue) before modulo wrapping</param>
        /// <param name="oldPosition">The starting position on the dial (0-99)</param>
        /// <param name="newPosition">The final position on the dial after modulo wrapping (0-99)</param>
        /// <returns>The total number of zero crossings and landings for this movement</returns>
        /// <remarks>
        /// Examples:
        /// - Moving from position 50 by +250 steps: crosses zero at positions 100, 200, and lands on 300→0 = 3 total
        /// - Moving from position 10 by -60 steps: wraps through zero once = 1 total
        /// - Moving from position 25 by -25 steps: lands exactly on zero = 1 total
        /// </remarks>
        public static int CalculateZeroCrossingsAndLandings(int positionChange, int stepValue, int oldPosition, int newPosition)
        {
            int numberOfZeroCrossingsAndLandings = 0;

            // Case 1: Moving in negative direction (leftward rotation)
            if (positionChange < 0)
            {
                // Calculate how many complete rotations we make going backwards
                // Formula: (-positionChange - 1) / DIAL_POSITIONS + 1 gives us the number of zero crossings
                // Example: position 20, move -120 → positionChange = -100 → (100-1)/100 + 1 = 1 crossing
                numberOfZeroCrossingsAndLandings = (-positionChange - 1) / Configuration.DIAL_POSITIONS + 1;

                // Special case: If we start at zero and move left, we don't cross zero immediately
                // because we're already there - we only cross it after completing a full rotation
                if (oldPosition == 0)
                    numberOfZeroCrossingsAndLandings -= 1;
            }
            // Case 2: Moving in positive direction (rightward rotation)
            else if (positionChange >= Configuration.DIAL_POSITIONS)
            {
                // Calculate how many times we cross zero during forward movement
                // Each complete rotation (100 positions) crosses zero once
                // Example: position 50, move +250 → positionChange = 300 → 300/100 = 3 crossings
                numberOfZeroCrossingsAndLandings = positionChange / Configuration.DIAL_POSITIONS;

                // If we land exactly on zero after the movement, we need to avoid double-counting
                // the final landing, since it will be added back in the final check below
                if (newPosition == 0)
                    numberOfZeroCrossingsAndLandings -= 1;
            }

            // Final check: Count landings on zero exactly:
            // - Example 1: position 25, move -25 steps → lands exactly on zero (positionChange = 0)
            // - Example 2: position 90, move +10 steps → wraps once to land on zero (positionChange = 100)
            // We only count this if there was actual movement (stepValue != 0) to avoid counting
            // stationary positions as zero crossings. Using stepValue instead of positionChange to
            // ensure we don't face issues where oldPosition + stepValue cancel each other out.
            if (newPosition == 0 && stepValue != 0)
                numberOfZeroCrossingsAndLandings += 1;

            return numberOfZeroCrossingsAndLandings;
        }
    }

    /// <summary>
    /// Main class for processing dial instructions and tracking zero crossings.
    /// </summary>
    public class SecretEntrance
    {
        private int numberOfTimesDialStaysAtZero = 0;
        private int numberOfTimesDialPointsAtZero = 0;
        private int currentDialPosition = Configuration.INITIAL_DIAL_POSITION;
        private readonly bool enableDebugLogging;

        /// <summary>
        /// Initializes a new instance of the SecretEntrance class.
        /// </summary>
        /// <param name="enableDebugLogging">Whether to enable debug logging output</param>
        public SecretEntrance(bool enableDebugLogging = false)
        {
            this.enableDebugLogging = enableDebugLogging;
        }

        /// <summary>
        /// Resets the dial to its initial state and clears all counters.
        /// </summary>
        public void Reset()
        {
            numberOfTimesDialStaysAtZero = 0;
            numberOfTimesDialPointsAtZero = 0;
            currentDialPosition = Configuration.INITIAL_DIAL_POSITION;
        }

        /// <summary>
        /// Processes the default puzzle input file and executes all dial instructions.
        /// </summary>
        public void ProcessInput()
        {
            //var input = PuzzleInputManager.GetInstructions(Configuration.TEST_INPUT_PATH);
            var input = PuzzleInputManager.GetInstructions();
            ProcessInstructions(input);
        }

        /// <summary>
        /// Processes a sequence of dial instructions.
        /// </summary>
        /// <param name="instructions">The instructions to process</param>
        public void ProcessInstructions(IEnumerable<DialInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                ProcessSingleInstruction(instruction);
            }
        }

        /// <summary>
        /// Processes a single dial instruction and updates the dial state.
        /// </summary>
        /// <param name="instruction">The instruction to process</param>
        public void ProcessSingleInstruction(DialInstruction instruction)
        {
            int stepValue = instruction.GetStepValue();
            var result = DialProcessor.ProcessMovement(currentDialPosition, stepValue);

            UpdateCounters(result);
            currentDialPosition = result.NewPosition;

            LogMovement(stepValue, result);
        }

        /// <summary>Gets the number of times the dial landed exactly on zero.</summary>
        public int GetTimesDialStaysAtZero() => numberOfTimesDialStaysAtZero;

        /// <summary>Gets the total number of zero crossings and landings (Part Two answer).</summary>
        public int GetTimesDialPointsAtZero() => numberOfTimesDialPointsAtZero;

        /// <summary>Gets the current position of the dial.</summary>
        public int GetCurrentDialPosition() => currentDialPosition;

        /// <summary>
        /// Outputs the answer for Part One of the puzzle.
        /// </summary>
        public void PartOne()
        {
            // 1141
            System.Console.WriteLine($"Secret Part One: {numberOfTimesDialStaysAtZero}");
        }

        /// <summary>
        /// Outputs the answer for Part Two of the puzzle.
        /// </summary>
        public void PartTwo()
        {
            // 6634
            System.Console.WriteLine($"Secret Part Two: {numberOfTimesDialPointsAtZero}");
        }

        private void UpdateCounters(DialMovementResult result)
        {
            if (result.LandsOnZero)
                numberOfTimesDialStaysAtZero++;
            numberOfTimesDialPointsAtZero += result.ZeroCrossings;
        }

        private void LogMovement(int stepValue, DialMovementResult result)
        {
            if (!enableDebugLogging) return;

            System.Console.WriteLine($"[DEBUG] {result.OldPosition} + {stepValue} = {result.NewPosition}, Wraps: {result.ZeroCrossings}, Position Change: {result.OldPosition + stepValue}, Lands on zero: {result.LandsOnZero}, Total at zero: {numberOfTimesDialPointsAtZero}, Total stays at zero: {numberOfTimesDialStaysAtZero}");
        }

    }

    /// <summary>
    /// Entry point for the Advent of Code Day 1 solution.
    /// </summary>
    public static class Solution
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments. Use --debug to enable debug logging.</param>
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Day 1 - Secret Entrance");
            System.Console.WriteLine("-----------------------");

            // Enable debug logging with --debug flag
            bool enableDebug = args.Length > 0 && args[0] == "--debug";

            SecretEntrance secretEntrance = new SecretEntrance(enableDebug);
            secretEntrance.ProcessInput();
            secretEntrance.PartOne();
            secretEntrance.PartTwo();
        }

    }
}
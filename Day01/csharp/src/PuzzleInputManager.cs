
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Day01
{
    /// <summary>
    /// Represents a single dial movement instruction with direction and step count.
    /// </summary>
    public class DialInstruction
    {
        /// <summary>
        /// Specifies the direction of dial movement.
        /// </summary>
        public enum DialDirection { Left, Right }

        /// <summary>Gets the direction of the dial movement.</summary>
        public DialDirection Direction { get; private set; }

        /// <summary>Gets the number of steps to move in the specified direction.</summary>
        public int Steps { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DialInstruction class with default values.
        /// </summary>
        public DialInstruction() { }

        /// <summary>
        /// Initializes a new instance of the DialInstruction class from a string instruction.
        /// </summary>
        /// <param name="instruction">String instruction in format "L25" or "R10" (direction + number)</param>
        public DialInstruction(string instruction)
        {
            ValidateAndParseInstruction(instruction);
        }
        private void ValidateAndParseInstruction(string instruction)
        {
            System.ArgumentException.ThrowIfNullOrWhiteSpace(instruction, nameof(instruction));
            if (instruction.Length < 2)
                throw new System.ArgumentException("Instruction must be at least two characters.", nameof(instruction));

            Direction = instruction[0] switch
            {
                'L' => DialDirection.Left,
                'R' => DialDirection.Right,
                _ => throw new System.ArgumentException($"Invalid direction: {instruction[0]}", nameof(instruction))
            };

            if (!int.TryParse(instruction[1..], out int parsedSteps) || parsedSteps < 0)
                throw new System.ArgumentException("Steps must be a non-negative integer", nameof(instruction));

            Steps = parsedSteps;
        }

        /// <summary>
        /// Gets the step value with appropriate sign based on direction.
        /// </summary>
        /// <returns>Negative value for left movement, positive for right movement</returns>
        public int GetStepValue() => Direction == DialDirection.Left ? -Steps : Steps;

    }

    /// <summary>
    /// Manages loading and parsing puzzle input files for dial instructions.
    /// </summary>
    public class PuzzleInputManager
    {
        /// <summary>
        /// Loads and parses dial instructions from the specified file.
        /// </summary>
        /// <param name="path">Path to the input file (defaults to puzzle input)</param>
        /// <returns>Collection of parsed dial instructions</returns>
        public static IEnumerable<DialInstruction> GetInstructions(string path = Configuration.DEFAULT_INPUT_PATH)
        {
            var input = GetInput(path);
            return input.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries)
                       .Select(line => new DialInstruction(line));
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
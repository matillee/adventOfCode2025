
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Day02
{
    /// <summary>
    /// Represents a range of product IDs with start and end boundaries.
    /// Parses string input in the format "START-END" where both values are positive long integers.
    /// </summary>
    public class ProductIDRange
    {
        /// <summary>Gets the starting ID of the range (inclusive).</summary>
        public long StartID { get; private set; }

        /// <summary>Gets the ending ID of the range (inclusive).</summary>
        public long EndID { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ProductIDRange class with default values.
        /// </summary>
        public ProductIDRange() { }

        /// <summary>
        /// Initializes a new instance of the ProductIDRange class from a string representation.
        /// </summary>
        /// <param name="id">String in format "START-END" (e.g., "123-456")</param>
        /// <exception cref="ArgumentException">Thrown when the input format is invalid</exception>
        public ProductIDRange(string id)
        {
            ValidateAndParseIDRange(id);
        }

        /// <summary>
        /// Validates and parses the ID range string, setting StartID and EndID properties.
        /// </summary>
        /// <param name="id">String representation of the ID range</param>
        /// <exception cref="ArgumentException">Thrown when validation fails</exception>
        /// <remarks>
        /// Validation rules:
        /// - Must be in format "START-END"
        /// - Both parts must be valid long integers
        /// - Neither part can start with '0' (leading zeros not allowed)
        /// - StartID must be less than or equal to EndID
        /// </remarks>
        private void ValidateAndParseIDRange(string id)
        {
            System.ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));
            var trimmedId = id.Trim();
            var parts = trimmedId.Split('-').Select(part => part.Trim()).ToArray();
            if (parts.Length != 2)
                throw new System.ArgumentException("ID range must be in the format 'START-END'", nameof(id));

            if (!long.TryParse(parts[0], out long startId) || !long.TryParse(parts[1], out long endId))
                throw new System.ArgumentException("Start and End IDs must be integers", nameof(id));

            if (parts[0].StartsWith("0") || parts[1].StartsWith("0"))
                throw new System.ArgumentException("ID should not start with 0", nameof(id));

            if (startId > endId)
                throw new System.ArgumentException("StartID cannot be greater than EndID", nameof(id));

            StartID = startId;
            EndID = endId;
        }

    }

    /// <summary>
    /// Manages loading and parsing puzzle input files for product ID ranges.
    /// Handles comma-separated lists of product ID ranges from text files.
    /// </summary>
    public class PuzzleInputManager
    {
        /// <summary>
        /// Loads and parses product ID ranges from the specified file.
        /// </summary>
        /// <param name="path">Path to the input file (defaults to puzzle input)</param>
        /// <returns>Collection of parsed ProductIDRange objects</returns>
        /// <exception cref="FileNotFoundException">Thrown when the input file is not found</exception>
        /// <exception cref="ArgumentException">Thrown when any ID range format is invalid</exception>
        public static IEnumerable<ProductIDRange> GetIDs(string path = Configuration.DEFAULT_INPUT_PATH)
        {
            var input = GetInput(path);
            return input.Split(new[] { ",", }, System.StringSplitOptions.RemoveEmptyEntries)
                       .Select(idRange => new ProductIDRange(idRange));
        }

        /// <summary>
        /// Reads the entire contents of the specified input file as UTF-8 text.
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
            return System.IO.File.ReadAllText(path, Encoding.UTF8);
        }

    }
}
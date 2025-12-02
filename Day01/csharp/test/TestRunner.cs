using System;

namespace AdventOfCode.Day01.Tests
{
    public static class TestRunner
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Running All Tests...");
            Console.WriteLine("====================");

            try
            {
                // Run all test suites
                DialProcessorTests.RunAllTests();
                Console.WriteLine();

                SecretEntranceTests.RunAllTests();
                Console.WriteLine();

                Console.WriteLine("✅ All tests completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
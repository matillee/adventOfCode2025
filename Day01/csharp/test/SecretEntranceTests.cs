using AdventOfCode.Day01;
using System;
using System.Collections.Generic;

namespace AdventOfCode.Day01.Tests
{
    public static class SecretEntranceTests
    {
        public static void RunAllTests()
        {
            Console.WriteLine("Running SecretEntrance Tests...");

            TestInitialization();
            TestReset();
            TestSingleInstructionProcessing();
            TestMultipleInstructionsProcessing();
            TestCounterUpdates();
            TestStateTracking();

            Console.WriteLine("All SecretEntrance tests completed.");
        }

        private static void TestInitialization()
        {
            Console.WriteLine("Testing initialization...");

            var entrance = new SecretEntrance();
            AssertEqual(Configuration.INITIAL_DIAL_POSITION, entrance.GetCurrentDialPosition(), "Should start at initial position");
            AssertEqual(0, entrance.GetTimesDialStaysAtZero(), "Should start with zero count for stays at zero");
            AssertEqual(0, entrance.GetTimesDialPointsAtZero(), "Should start with zero count for points at zero");

            Console.WriteLine("Initialization tests passed.");
        }

        private static void TestReset()
        {
            Console.WriteLine("Testing reset functionality...");

            var entrance = new SecretEntrance();

            // Process some instructions to change state
            entrance.ProcessSingleInstruction(new DialInstruction("R30"));
            entrance.ProcessSingleInstruction(new DialInstruction("L80"));

            // Verify state has changed
            AssertNotEqual(Configuration.INITIAL_DIAL_POSITION, entrance.GetCurrentDialPosition(), "Position should have changed");

            // Reset and verify
            entrance.Reset();
            AssertEqual(Configuration.INITIAL_DIAL_POSITION, entrance.GetCurrentDialPosition(), "Should reset to initial position");
            AssertEqual(0, entrance.GetTimesDialStaysAtZero(), "Should reset stays at zero count");
            AssertEqual(0, entrance.GetTimesDialPointsAtZero(), "Should reset points at zero count");

            Console.WriteLine("Reset tests passed.");
        }

        private static void TestSingleInstructionProcessing()
        {
            Console.WriteLine("Testing single instruction processing...");

            var entrance = new SecretEntrance();

            // Test simple forward movement
            entrance.ProcessSingleInstruction(new DialInstruction("R10"));
            AssertEqual(60, entrance.GetCurrentDialPosition(), "Should move to position 60");

            // Test simple backward movement
            entrance.Reset();
            entrance.ProcessSingleInstruction(new DialInstruction("L20"));
            AssertEqual(30, entrance.GetCurrentDialPosition(), "Should move to position 30");

            // Test landing on zero
            entrance.Reset();
            entrance.ProcessSingleInstruction(new DialInstruction("L50"));
            AssertEqual(0, entrance.GetCurrentDialPosition(), "Should land on zero");
            AssertEqual(1, entrance.GetTimesDialStaysAtZero(), "Should count landing on zero");
            AssertEqual(1, entrance.GetTimesDialPointsAtZero(), "Should count zero crossing");

            Console.WriteLine("Single instruction processing tests passed.");
        }

        private static void TestMultipleInstructionsProcessing()
        {
            Console.WriteLine("Testing multiple instructions processing...");

            var entrance = new SecretEntrance();
            var instructions = new List<DialInstruction>
            {
                new DialInstruction("R10"),  // 50 -> 60
                new DialInstruction("L30"),   // 60 -> 30  
                new DialInstruction("L30")    // 30 -> 0 (landing on zero)
            };

            entrance.ProcessInstructions(instructions);

            AssertEqual(0, entrance.GetCurrentDialPosition(), "Should end at position 0");
            AssertEqual(1, entrance.GetTimesDialStaysAtZero(), "Should count one landing on zero");
            AssertEqual(1, entrance.GetTimesDialPointsAtZero(), "Should count one zero crossing");

            Console.WriteLine("Multiple instructions processing tests passed.");
        }

        private static void TestCounterUpdates()
        {
            Console.WriteLine("Testing counter updates...");

            var entrance = new SecretEntrance();

            // Test movement that wraps around multiple times
            entrance.ProcessSingleInstruction(new DialInstruction("R250")); // 2.5 rotations, lands on 0

            AssertEqual(0, entrance.GetCurrentDialPosition(), "Should land on zero after 250 steps");
            AssertEqual(1, entrance.GetTimesDialStaysAtZero(), "Should count one landing on zero");
            AssertEqual(3, entrance.GetTimesDialPointsAtZero(), "Should count 3 zero crossings");

            // Test another move that doesn't land on zero but crosses it
            entrance.ProcessSingleInstruction(new DialInstruction("R120")); // 1.2 rotations, lands on 20

            AssertEqual(20, entrance.GetCurrentDialPosition(), "Should land on position 20");
            AssertEqual(1, entrance.GetTimesDialStaysAtZero(), "Should still have one landing on zero");
            AssertEqual(4, entrance.GetTimesDialPointsAtZero(), "Should have 4 zero crossings total (3 + 1)");

            Console.WriteLine("Counter update tests passed.");
        }

        private static void TestStateTracking()
        {
            Console.WriteLine("Testing state tracking...");

            var entrance = new SecretEntrance();

            // Sequence that tests various scenarios
            var instructions = new List<DialInstruction>
            {
                new DialInstruction("R150"), // 1.5 rotations: 50 -> 0, crosses once, lands on zero
                new DialInstruction("L50"),   // 0.5 rotation backward: 0 -> 50, doesn't cross (starts at zero)
                new DialInstruction("L150")   // 1.5 rotations backward: 50 -> 0, crosses once, lands on zero
            };

            entrance.ProcessInstructions(instructions);

            AssertEqual(0, entrance.GetCurrentDialPosition(), "Should end at position 0");
            AssertEqual(2, entrance.GetTimesDialStaysAtZero(), "Should count two landings on zero");
            AssertEqual(4, entrance.GetTimesDialPointsAtZero(), "Should count 4 zero crossings total");

            Console.WriteLine("State tracking tests passed.");
        }

        private static void AssertEqual(int expected, int actual, string message)
        {
            if (expected != actual)
            {
                throw new Exception($"FAIL: {message}. Expected: {expected}, Actual: {actual}");
            }
        }

        private static void AssertNotEqual(int notExpected, int actual, string message)
        {
            if (notExpected == actual)
            {
                throw new Exception($"FAIL: {message}. Expected: NOT {notExpected}, Actual: {actual}");
            }
        }
    }
}
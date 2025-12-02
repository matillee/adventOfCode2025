using AdventOfCode.Day01;
using System;

namespace AdventOfCode.Day01.Tests
{
    public static class DialProcessorTests
    {
        public static void RunAllTests()
        {
            Console.WriteLine("Running DialProcessor Tests...");

            TestBasicMovement();
            TestMovementWrappingForward();
            TestMovementWrappingBackward();
            TestLandingOnZero();
            TestZeroCrossingsCalculation();
            TestEdgeCases();

            Console.WriteLine("All DialProcessor tests completed.");
        }

        private static void TestBasicMovement()
        {
            Console.WriteLine("Testing basic movement...");

            // Test normal forward movement
            var result = DialProcessor.ProcessMovement(50, 10);
            AssertEqual(60, result.NewPosition, "Forward movement should work correctly");
            AssertEqual(0, result.ZeroCrossings, "No zero crossings expected");
            AssertFalse(result.LandsOnZero, "Should not land on zero");

            // Test normal backward movement
            result = DialProcessor.ProcessMovement(50, -10);
            AssertEqual(40, result.NewPosition, "Backward movement should work correctly");
            AssertEqual(0, result.ZeroCrossings, "No zero crossings expected");
            AssertFalse(result.LandsOnZero, "Should not land on zero");

            Console.WriteLine("Basic movement tests passed.");
        }

        private static void TestMovementWrappingForward()
        {
            Console.WriteLine("Testing forward wrapping...");

            // Test wrapping forward once
            var result = DialProcessor.ProcessMovement(90, 20);
            AssertEqual(10, result.NewPosition, "Should wrap to position 10");
            AssertEqual(1, result.ZeroCrossings, "Should have 1 zero crossing");
            AssertFalse(result.LandsOnZero, "Should not land on zero");

            // Test wrapping forward multiple times
            result = DialProcessor.ProcessMovement(50, 250); // 2.5 full rotations
            AssertEqual(0, result.NewPosition, "Should land on position 0");
            AssertEqual(3, result.ZeroCrossings, "Should have 3 zero crossings (crosses at 100, 200, 300)");
            AssertTrue(result.LandsOnZero, "Should land on zero");

            Console.WriteLine("Forward wrapping tests passed.");
        }

        private static void TestMovementWrappingBackward()
        {
            Console.WriteLine("Testing backward wrapping...");

            // Test wrapping backward once
            var result = DialProcessor.ProcessMovement(10, -20);
            AssertEqual(90, result.NewPosition, "Should wrap to position 90");
            AssertEqual(1, result.ZeroCrossings, "Should have 1 zero crossing");
            AssertFalse(result.LandsOnZero, "Should not land on zero");

            // Test wrapping backward multiple times
            result = DialProcessor.ProcessMovement(50, -250); // 2.5 full rotations backward
            AssertEqual(0, result.NewPosition, "Should land on position 0");
            AssertEqual(3, result.ZeroCrossings, "Should have 3 zero crossings including landing");
            AssertTrue(result.LandsOnZero, "Should land on zero");

            Console.WriteLine("Backward wrapping tests passed.");
        }

        private static void TestLandingOnZero()
        {
            Console.WriteLine("Testing landing on zero...");

            // Test direct landing on zero
            var result = DialProcessor.ProcessMovement(50, -50);
            AssertEqual(0, result.NewPosition, "Should land on zero");
            AssertEqual(1, result.ZeroCrossings, "Landing on zero should count as crossing");
            AssertTrue(result.LandsOnZero, "Should detect landing on zero");

            // Test starting from zero
            result = DialProcessor.ProcessMovement(0, 50);
            AssertEqual(50, result.NewPosition, "Should move to position 50");
            AssertEqual(0, result.ZeroCrossings, "Starting from zero shouldn't count as crossing");
            AssertFalse(result.LandsOnZero, "Should not land on zero");

            Console.WriteLine("Landing on zero tests passed.");
        }

        private static void TestZeroCrossingsCalculation()
        {
            Console.WriteLine("Testing zero crossings calculation...");

            // Test large forward movement with multiple crossings
            int crossings = DialProcessor.CalculateZeroCrossingsAndLandings(350, 300, 50, 50);
            AssertEqual(3, crossings, "300 steps from position 50 should have 3 zero crossings");

            // Test large backward movement with multiple crossings  
            crossings = DialProcessor.CalculateZeroCrossingsAndLandings(-200, -250, 50, 0);
            AssertEqual(3, crossings, "250 units backward should have 3 crossings including landing");

            // Test no movement
            crossings = DialProcessor.CalculateZeroCrossingsAndLandings(50, 0, 50, 50);
            AssertEqual(0, crossings, "No movement should have no crossings");

            Console.WriteLine("Zero crossings calculation tests passed.");
        }

        private static void TestEdgeCases()
        {
            Console.WriteLine("Testing edge cases...");

            // Test exact dial size movement
            var result = DialProcessor.ProcessMovement(0, 100);
            AssertEqual(0, result.NewPosition, "Full rotation should return to start");
            AssertEqual(1, result.ZeroCrossings, "Full rotation should count as 1 crossing");
            AssertTrue(result.LandsOnZero, "Should land on zero");

            // Test negative full rotation
            result = DialProcessor.ProcessMovement(0, -100);
            AssertEqual(0, result.NewPosition, "Negative full rotation should return to start");
            AssertEqual(1, result.ZeroCrossings, "Starting from zero, negative rotation shouldn't count crossings only landing on zero");
            AssertTrue(result.LandsOnZero, "Should land on zero");

            Console.WriteLine("Edge case tests passed.");
        }

        private static void AssertEqual(int expected, int actual, string message)
        {
            if (expected != actual)
            {
                throw new Exception($"FAIL: {message}. Expected: {expected}, Actual: {actual}");
            }
        }

        private static void AssertTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception($"FAIL: {message}. Expected: true, Actual: false");
            }
        }

        private static void AssertFalse(bool condition, string message)
        {
            if (condition)
            {
                throw new Exception($"FAIL: {message}. Expected: false, Actual: true");
            }
        }
    }
}
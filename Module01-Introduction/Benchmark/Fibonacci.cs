using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    [DisassemblyDiagnoser(exportCombinedDisassemblyReport: true)]
    [MemoryDiagnoser]
    public class FibonacciCalc
    {
        // HOMEWORK:
        // 1. Write implementations for RecursiveWithMemoization and Iterative solutions
        // 2. Add MemoryDiagnoser to the benchmark
        // 3. Run with release configuration and compare results
        // 4. Open disassembler report and compare machine code
        // 
        // You can use the discussion panel to compare your results with other students

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Data))]
        public ulong Recursive(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            return Recursive(n - 2) + Recursive(n - 1);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong RecursiveWithMemoization(ulong n)
        {
            if (n == 1 || n == 2) return 1;
            var memo = new ulong[n - 2];
            return RecursiveWithMemoizationImpl(n - 1, memo) + RecursiveWithMemoizationImpl(n - 2, memo);
        }

        private ulong RecursiveWithMemoizationImpl(ulong n, ulong[] memo)
        {
            if (n == 1 || n == 2) return 1;
            var cached = memo[n - 3];
            if (cached != 0) return cached;
            var res = RecursiveWithMemoizationImpl(n - 1, memo) + RecursiveWithMemoizationImpl(n - 2, memo);
            memo[n - 3] = res;
            return res;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public ulong Iterative(ulong n)
        {
            if (n == 1 || n == 2) return 1;

            ulong a = 1, b = 1, c = 0;
            for (uint i = 2; i < n; i++)
            {
                c = a + b;
                a = b;
                b = c;
            }
            return c;
        }

        public IEnumerable<ulong> Data()
        {
            yield return 15;
            yield return 35;
        }
    }
}

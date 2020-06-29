using System;
using System.Reflection;
using BenchmarkDotNet.Running;

namespace Dotnetos.AsyncExpert.Homework.Module01.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //var calc = new FibonacciCalc();
            //for (uint i = 1; i < 10; i++)
            //{
            //    Console.WriteLine($"[{i}] = {calc.Recursive(i)} / {calc.RecursiveWithMemoization(i)} / {calc.Iterative(i)}");
            //}
            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(args);
        }
    }
}

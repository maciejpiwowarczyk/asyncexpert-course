using System.Threading;

namespace LowLevelExercises.Core
{
    /// <summary>
    /// A simple class for reporting a specific value and obtaining an average.
    /// </summary>
    public class AverageMetric
    {
        class Metric
        {
            public readonly int Sum;
            public readonly int Count;

            public Metric(int sum, int count)
            {
                Sum = sum;
                Count = count;
            }
        }

        private Metric current;

        public AverageMetric()
        {
            current = new Metric(0, 0);
        }

        public void Report(int value)
        {
            Metric readCurrent;
            do
            {
                readCurrent = Volatile.Read(ref current);
            } while (readCurrent != Interlocked.CompareExchange(ref current, new Metric(readCurrent.Sum + value, readCurrent.Count + 1), readCurrent));
        }

        public double Average => Calculate(Volatile.Read(ref current));

        static double Calculate(Metric metric)
        {
            if (metric.Count == 0)
            {
                return double.NaN;
            }

            return (double)metric.Sum / metric.Count;
        }
    }
}

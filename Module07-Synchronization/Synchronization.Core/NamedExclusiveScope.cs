using System;
using System.Threading;

namespace Synchronization.Core
{
    /*
     * Implement very simple wrapper around Mutex or Semaphore (remember both implement WaitHandle) to
     * provide a exclusive region created by `using` clause.
     *
     * Created region may be system-wide or not, depending on the constructor parameter.
     */
    public class NamedExclusiveScope : IDisposable
    {
        private readonly Semaphore _semaphore;

        public NamedExclusiveScope(string name, bool isSystemWide)
        {
            var createdNew = false;
            if (isSystemWide)
            {
                _semaphore = new Semaphore(1, 1, $"Global\\{name}", out createdNew);
            }
            else
            {
                _semaphore = new Semaphore(1, 1);
            }
            //Console.WriteLine($"Created semaphore {name}, created new: {createdNew}");
            var obtained = _semaphore.WaitOne(TimeSpan.FromMilliseconds(100));
            if (!obtained)
            {
                _semaphore.Dispose();
                var lockPrefix = isSystemWide ? "global" : "local";
                throw new InvalidOperationException($"Unable to get a {lockPrefix} lock {name}.");
            }
        }

        public void Dispose()
        {
            _semaphore.Release(1);
            _semaphore.Dispose();
        }
    }
}

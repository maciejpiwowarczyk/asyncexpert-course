using System;
using System.Threading;

namespace ThreadPoolExercises.Core
{
    public class ThreadingHelpers
    {
        public static void ExecuteOnThread(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            var t = new Thread(() => ExecuteImpl(action, repeats, token, errorAction));
            t.Start();
            t.Join();
        }

        public static void ExecuteOnThreadPool(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            var evt = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem((_) =>
            {
                ExecuteImpl(action, repeats, token, errorAction);
                evt.Set();
            });
            evt.WaitOne();
        }

        private static void ExecuteImpl(Action action, int repeats, CancellationToken token, Action<Exception>? errorAction)
        {
            try
            {
                for (int i = 0; i < repeats; i++)
                {
                    token.ThrowIfCancellationRequested();
                    action();
                }
            }
            catch (Exception e)
            {
                errorAction?.Invoke(e);
            }
        }
    }
}

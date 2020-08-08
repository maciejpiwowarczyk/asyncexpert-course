using System;
using System.Runtime.CompilerServices;

namespace AwaitableExercises.Core
{
    public static class BoolExtensions
    {
        public static BoolAwaiter GetAwaiter(this bool boolVal)
        {
            return new BoolAwaiter(boolVal);
        }
    }

    public class BoolAwaiter : INotifyCompletion
    {
        private readonly bool _boolVal;

        public BoolAwaiter(bool boolVal)
        {
            _boolVal = boolVal;
        }

        public bool IsCompleted => true;

        public bool GetResult() => _boolVal;

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }
    }
}

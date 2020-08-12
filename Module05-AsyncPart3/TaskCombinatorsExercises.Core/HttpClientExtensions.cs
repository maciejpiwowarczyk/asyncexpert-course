using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCombinatorsExercises.Core
{
    public static class HttpClientExtensions
    {
        /*
         Write cancellable async method with timeout handling, that concurrently tries to get data from
         provided urls (first wins and its response is returned, rest is __cancelled__).
         
         Tips:
         * consider using HttpClient.GetAsync (as it is cancellable)
         * consider using Task.WhenAny
         * you may use urls like for testing https://postman-echo.com/delay/3
         * you should have problem with tasks cancellation -
            - how to merge tokens of operations (timeouts) with the provided token? 
            - Tip: you can link tokens with the help of CancellationTokenSource.CreateLinkedTokenSource(token)
         */
        public static async Task<string> ConcurrentDownloadAsync(this HttpClient httpClient,
            string[] urls, int millisecondsTimeout, CancellationToken token)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var downloadTasks = urls.Select(url => httpClient.GetAsync(new Uri(url), cts.Token)).ToArray();
            var timeoutTask = Timeout<HttpResponseMessage>(millisecondsTimeout);
            var firstFinishedTask = await Task.WhenAny(downloadTasks.Concat(new[] {timeoutTask}));
            if (firstFinishedTask == timeoutTask)
            {
                cts.Cancel();
                throw new TaskCanceledException();
            }

            return await (await firstFinishedTask).Content.ReadAsStringAsync();
        }

        private static async Task<T> Timeout<T>(int millisecondsTimeout)
        {
            await Task.Delay(millisecondsTimeout);
            return default;
        }
    }
}

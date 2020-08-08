using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace TaskCompletionSourceExercises.Core
{
    public class AsyncTools
    {
        public static Task<string> RunProgramAsync(string path, string args = "")
        {
            var tcs = new TaskCompletionSource<string>();
            var stdOut = new StringBuilder();
            var errOut = new StringBuilder();

            var process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += (sender, eventArgs) =>
            {
                if (eventArgs.Data?.Length > 0) stdOut.AppendLine(eventArgs.Data);
            };
            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                if (eventArgs.Data?.Length > 0) errOut.AppendLine(eventArgs.Data);
            };

            process.Exited += (sender, eventArgs) =>
            {
                if (errOut.Length > 0) tcs.SetException(new Exception(errOut.ToString()));
                else tcs.SetResult(stdOut.ToString());
            };


            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }
    }
}

using System;
using System.Threading.Tasks;
using TaskCompletionSourceExercises.Core;
using Xunit;

namespace TaskCompletionSourceExercises.Tests
{
    public class AsyncToolsTests
    {
        private string _path = @"..\..\..\..\ExampleApp\bin\Debug\netcoreapp3.1\ExampleApp.exe";

        [Fact]
        public async Task GivenExampleAppRequiringArguments_WhenNoArguments_ThenThrows()
        {
            var exception = await Record.ExceptionAsync(async () =>
                await AsyncTools.RunProgramAsync(_path));

            Assert.NotNull(exception);
            Assert.IsType<Exception>(exception);
            Assert.StartsWith("Unhandled exception. System.Exception: Missing program argument.", exception.Message);
        }

        [Fact]
        public async Task GivenExampleAppRequiringArguments_WhenHaveArguments_ThenSucceeds()
        {
            var result = await AsyncTools.RunProgramAsync(_path, "argument");

            Assert.Equal("Hello argument!\r\nGoodbye.\r\n", result);
        }
    }
}

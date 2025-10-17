using Microsoft.Playwright;
using Reqnroll;

namespace Playwright_ReqRoll
{
    public partial class TestConfig : PageTest
    {
        private IBrowserContext? _context;

        [BeforeScenario]
        public async Task BeforeScenarioAsync(ScenarioContext scenarioContext)
        {
            _context = Page.Context;
            await _context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
            scenarioContext["Page"] = Page;
        }

        [AfterScenario]
        public async Task AfterScenarioAsync(ScenarioContext scenarioContext)
        {
            if (_context == null) return;
            if (scenarioContext.TestError != null)
            {
                var scenarioTitle = FilenameSanitizerRegex().Replace(scenarioContext.ScenarioInfo.Title, "");
                var tracePath = Path.Combine(TestContext.CurrentContext.WorkDirectory.Split("\\bin")[0], "Playwright-Traces", $"{scenarioTitle}-failed.zip");
                Directory.CreateDirectory(Path.GetDirectoryName(tracePath)!);
                await _context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                Console.WriteLine($"Test failed. Playwright trace saved to: {tracePath}");
            }
            else
            {
                await _context.Tracing.StopAsync();
                Console.WriteLine("Test passed. No trace saved.");
            }
        }

        [GeneratedRegex("[^a-zA-Z0-9_]")]
        private static partial Regex FilenameSanitizerRegex();
    }
}

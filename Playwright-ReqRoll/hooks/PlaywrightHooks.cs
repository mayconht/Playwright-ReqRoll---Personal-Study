using Microsoft.Playwright;
using Reqnroll;

namespace Playwright_ReqRoll.hooks;

// Maybe for a non english speaker hooks is not a common term. Consider renaming to PlaywrightLifecycleManager or similar.
// but in essence, hooks is a common term in BDD frameworks for setup/teardown code.
// so we use hooks here to align with that convention.

/// <summary>
/// Provides hooks for setting up and tearing down Playwright browser instances and contexts for Reqnroll scenarios.
/// Manages global browser lifecycle and per-scenario context and page creation.
/// </summary>
[Binding]
public partial class PlaywrightHooks
{
    /// <summary>
    /// The Playwright instance used for browser management.
    /// </summary>
    private static IPlaywright _playwright = null!;

    /// <summary>
    /// The browser instance launched for the test run.
    /// </summary>
    private static IBrowser _browser = null!;

    /// <summary>
    /// The browser context for the current scenario.
    /// </summary>
    private IBrowserContext _context = null!;

    /// <summary>
    /// Gets the current page instance for the scenario.
    /// This is updated for each scenario.
    /// </summary>
    public static IPage Page { get; private set; } = null!;

    /// <summary>
    /// Sets up the Playwright instance and launches the browser before the test run.
    /// Configures browser type, headless mode, slow motion, and download path based on Config settings.
    /// </summary>
    [BeforeTestRun]
    public static async Task GlobalSetup()
    {
        _playwright = await Playwright.CreateAsync();
        var browserType = Config.BrowserType.ToLower();
        var browserTypeInstance = browserType switch
        {
            "chromium" => _playwright.Chromium,
            "firefox" => _playwright.Firefox,
            "webkit" => _playwright.Webkit,
            _ => _playwright.Chromium
        };
        Directory.CreateDirectory(Path.Combine(Config.ReportsPath, Config.DownloadsPath));
        _browser = await browserTypeInstance.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = Config.Headless,
                SlowMo = Config.SlowMo,
                Args = ["--start-maximized"],
                DownloadsPath = Path.Combine(Config.ReportsPath, Config.DownloadsPath)
            });
    }

    /// <summary>
    /// Tears down the browser and disposes the Playwright instance after the test run.
    /// </summary>
    [AfterTestRun]
    public static async Task GlobalTeardown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    /// <summary>
    /// Sets up a new browser context and page for each scenario.
    /// Starts tracing and video recording (if enabled) for the context.
    /// </summary>
    [BeforeScenario]
    public async Task SetupScenario()
    {
        if (Config.RecordVideo) Directory.CreateDirectory(Path.Combine(Config.ReportsPath, Config.VideoDir));
        _context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            RecordVideoDir = Config.RecordVideo ? Path.Combine(Config.ReportsPath, Config.VideoDir) : null
        });

        await _context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        Page = await _context.NewPageAsync();
    }

    /// <summary>
    /// Tears down the browser context for each scenario.
    /// Stops tracing and saves the trace file if the test failed or if configured to save on pass.
    /// </summary>
    /// <param name="scenarioContext">The context of the current scenario, used to check for errors and get scenario title.</param>
    [AfterScenario]
    public async Task TeardownScenario(ScenarioContext scenarioContext)
    {
        if (scenarioContext.TestError != null)
        {
            var scenarioTitle = FilenameSanitizerRegex().Replace(scenarioContext.ScenarioInfo.Title, "");
            var tracePath = Path.Combine(Config.ReportsPath, Config.TracesPath, $"{scenarioTitle}-failed.zip");

            Directory.CreateDirectory(Path.GetDirectoryName(tracePath)!);

            await _context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });

            Console.WriteLine($"Test failed. Playwright trace saved to: {tracePath}");
        }
        else
        {
            if (Config.SaveTracesOnPass)
            {
                var scenarioTitle = FilenameSanitizerRegex().Replace(scenarioContext.ScenarioInfo.Title, "");
                var tracePath = Path.Combine(Config.ReportsPath, Config.TracesPath, $"{scenarioTitle}-passed.zip");
                Directory.CreateDirectory(Path.GetDirectoryName(tracePath)!);
                await _context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                Console.WriteLine($"Test passed. Playwright trace saved to: {tracePath}");
            }
            else
            {
                await _context.Tracing.StopAsync();
                Console.WriteLine("Test passed. No trace saved.");
            }
        }

        await _context.CloseAsync();
    }

    //TODO: Move to a utility class if needed elsewhere
    /// <summary>
    /// Provides a regex for sanitizing filenames by replacing non-alphanumeric characters (except underscores) with underscores.
    /// </summary>
    /// <returns>A compiled regex for filename sanitization.</returns>
    [GeneratedRegex("[^a-zA-Z0-9_]")]
    private static partial Regex FilenameSanitizerRegex();
}
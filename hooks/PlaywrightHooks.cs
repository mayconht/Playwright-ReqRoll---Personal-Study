using Microsoft.Playwright;
using Reqnroll;

namespace Playwright_ReqRoll.hooks;

// Maybe for a non english speaker hooks is not a common term. Consider renaming to PlaywrightLifecycleManager or similar.
// but in essence, hooks is a common term in BDD frameworks for setup/teardown code.
// so we use hooks here to align with that convention.

/// <summary>
///     Provides hooks for setting up and tearing down Playwright browser instances and contexts for Reqnroll scenarios.
///     Manages global browser lifecycle and per-scenario context and page creation.
/// </summary>
[Binding]
public partial class PlaywrightHooks
{
    /// <summary>
    ///     The Playwright instance used for browser management.
    /// </summary>
    private static IPlaywright _playwright = null!;

    /// <summary>
    ///     The browser instance launched for the test run.
    /// </summary>
    private static IBrowser _browser = null!;

    /// <summary>
    ///     The browser context shared across scenarios to keep the browser open for stale element testing.
    /// </summary>
    private static IBrowserContext _context = null!;

    /// <summary>
    ///     Gets the current page instance for the scenario.
    ///     This is updated for each scenario.
    /// </summary>
    public static IPage Page { get; private set; } = null!;

    /// <summary>
    ///     Sets up the Playwright instance, launches the browser, and creates a shared context.
    ///     Configures browser type, headless mode, slow motion, and download path based on Config settings.
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
                DownloadsPath = Path.Combine(Config.ReportsPath, Config.DownloadsPath)
            });

        // Create shared context for all scenarios
        _context = await _browser.NewContextAsync(new BrowserNewContextOptions
        {
            ColorScheme = ColorScheme.Light,
            ViewportSize = new ViewportSize
            {
                Width = 1920,
                Height = 1080
            },
            RecordVideoDir = Config.RecordVideo ? Path.Combine(Config.ReportsPath, Config.VideoDir) : null,
            RecordVideoSize = new RecordVideoSize
            {
                Width = 1920,
                Height = 1080
            }
        });
    }

    /// <summary>
    ///     Tears down the browser and disposes the Playwright instance after the test run.
    /// </summary>
    [AfterTestRun]
    public static async Task GlobalTeardown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    /// <summary>
    ///     Sets up a new page for each scenario from the shared context.
    ///     Starts tracing for the scenario.
    /// </summary>
    [BeforeScenario]
    public async Task SetupScenario()
    {
        if (Config.RecordVideo) Directory.CreateDirectory(Path.Combine(Config.ReportsPath, Config.VideoDir));

        if (Config.ScreenshotOnSuccess || Config.ScreenshotOnFailure) Directory.CreateDirectory(Path.Combine(Config.ReportsPath, Config.ScreenshotsDir));


        await _context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });

        Page = await _context.NewPageAsync();
    }


    /// <summary>
    ///     Tears down the browser context for each scenario.
    ///     Stops tracing and saves the trace file if the test failed or if configured to save on pass.
    ///     Takes screenshots based on configuration settings.
    /// </summary>
    /// <param name="scenarioContext">The context of the current scenario, used to check for errors and get scenario title.</param>
    [AfterScenario]
    public async Task TeardownScenario(ScenarioContext scenarioContext)
    {
        var scenarioTitle = FilenameSanitizerRegex().Replace(scenarioContext.ScenarioInfo.Title, "");
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        if (scenarioContext.TestError != null)
        {
            // Take screenshot on failure if configured
            if (Config.ScreenshotOnFailure)
            {
                var screenshotPath = Path.Combine(Config.ReportsPath, Config.ScreenshotsDir, $"{scenarioTitle}_{timestamp}_failed.png");
                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });
                Console.WriteLine($"Screenshot saved to: {screenshotPath}");
            }

            var tracePath = Path.Combine(Config.ReportsPath, Config.TracesPath, $"{scenarioTitle}_{timestamp}_failed.zip");
            Directory.CreateDirectory(Path.GetDirectoryName(tracePath)!);
            await _context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
            Console.WriteLine($"Test failed. Playwright trace saved to: {tracePath}");
        }
        else
        {
            // Take screenshot on success if configured
            if (Config.ScreenshotOnSuccess)
            {
                var screenshotPath = Path.Combine(Config.ReportsPath, Config.ScreenshotsDir, $"{scenarioTitle}_{timestamp}_passed.png");
                await Page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });
                Console.WriteLine($"Screenshot saved to: {screenshotPath}");
            }

            if (Config.SaveTracesOnPass)
            {
                var tracePath = Path.Combine(Config.ReportsPath, Config.TracesPath, $"{scenarioTitle}_{timestamp}_passed.zip");
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

        if (Config.RecordVideo)
        {
            var videoPath = await Page.Video!.PathAsync();
            var videoDir = Path.GetDirectoryName(videoPath)!;
            var status = scenarioContext.TestError != null ? "failed" : "passed";
            var newVideoPath = Path.Combine(videoDir, $"{scenarioTitle}_{timestamp}_{status}.webm");

            await Page.CloseAsync();

            await Task.Delay(2000); // Increased delay to ensure video file is fully written

            if (File.Exists(videoPath))
            {
                File.Move(videoPath, newVideoPath);
                Console.WriteLine($"Video saved to: {newVideoPath}");
            }
        }
        else
        {
            await Page.CloseAsync();
        }
    }

    //TODO: Move to a utility class if needed elsewhere
    /// <summary>
    ///     Provides a regex for sanitizing filenames by replacing non-alphanumeric characters (except underscores) with
    ///     underscores.
    /// </summary>
    /// <returns>A compiled regex for filename sanitization.</returns>
    [GeneratedRegex("[^a-zA-Z0-9_]")]
    private static partial Regex FilenameSanitizerRegex();
}
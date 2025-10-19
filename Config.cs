using Microsoft.Extensions.Configuration;

namespace Playwright_ReqRoll;

/// <summary>
///     Static class for accessing application configuration settings.
///     Loads configuration from appsettings.json and environment variables.
/// </summary>
public static class Config
{
    private static readonly IConfigurationRoot Configuration;

    static Config()
    {
        var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(configFilePath)!)
            .AddJsonFile(Path.GetFileName(configFilePath), true, true)
            .AddEnvironmentVariables();

        Configuration = builder.Build();
    }

    /// <summary>
    ///     Gets the browser type to use for Playwright (e.g., "chromium", "firefox", "webkit").
    ///     Defaults to "chromium" if not specified.
    /// </summary>
    public static string BrowserType => Configuration["Browser:Type"] ?? "chromium";

    /// <summary>
    ///     Gets whether the browser should run in headless mode.
    ///     Defaults to false if not specified.
    /// </summary>
    public static bool Headless => bool.Parse(Configuration["Browser:Headless"] ?? "false");

    /// <summary>
    ///     Gets the slow motion delay in milliseconds for browser actions.
    ///     Defaults to 0 if not specified.
    /// </summary>
    public static int SlowMo => int.Parse(Configuration["Browser:SlowMo"] ?? "0");

    /// <summary>
    ///     Gets the URL for the login page.
    ///     Defaults to "https://example.com/login" if not specified.
    /// </summary>
    public static string LoginPageUrl => Configuration["BaseUrls:LoginPage"] ?? "https://example.com/login";

    /// <summary>
    ///     Gets whether to save Playwright traces for passed tests.
    ///     Defaults to false if not specified.
    /// </summary>
    public static bool SaveTracesOnPass => bool.Parse(Configuration["Tracing:SaveOnPass"] ?? "false");

    /// <summary>
    ///     Gets the path where Playwright traces are saved.
    ///     Defaults to "Playwright-Traces" if not specified.
    /// </summary>
    public static string TracesPath => Configuration["Tracing:TracesPath"] ?? "Playwright-Traces";

    /// <summary>
    ///     Gets the path where browser downloads are saved.
    ///     Defaults to "Downloads" if not specified.
    /// </summary>
    public static string DownloadsPath => Configuration["Downloads:Path"] ?? "Downloads";

    /// <summary>
    ///     Gets whether to record videos for scenarios.
    ///     Defaults to false if not specified.
    /// </summary>
    public static bool RecordVideo => bool.Parse(Configuration["Video:Record"] ?? "false");

    /// <summary>
    ///     Gets the directory where videos are saved.
    ///     Defaults to "Playwright-Videos" if not specified.
    /// </summary>
    public static string VideoDir => Configuration["Video:Dir"] ?? "Playwright-Videos";

    /// <summary>
    ///     Gets the base path for saving reports (traces, videos, downloads).
    ///     Defaults to "C:\\reports" if not specified.
    /// </summary>
    public static string ReportsPath => Configuration["Reports:Path"] ?? Directory.GetCurrentDirectory();

    /// <summary>
    ///     Gets whether to take screenshots on test success.
    ///     Defaults to false if not specified.
    /// </summary>
    public static bool ScreenshotOnSuccess => bool.Parse(Configuration["Screenshots:OnSuccess"] ?? "false");

    /// <summary>
    ///     Gets whether to take screenshots on test failure.
    ///     Defaults to true if not specified.
    /// </summary>
    public static bool ScreenshotOnFailure => bool.Parse(Configuration["Screenshots:OnFailure"] ?? "true");

    /// <summary>
    ///     Gets the directory where screenshots are saved.
    ///     Defaults to "Screenshots" if not specified.
    /// </summary>
    public static string ScreenshotsDir => Configuration["Screenshots:Dir"] ?? "Screenshots";
}
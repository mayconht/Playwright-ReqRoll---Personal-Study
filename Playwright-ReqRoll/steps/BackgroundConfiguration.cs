using Microsoft.Playwright;
using Reqnroll;

namespace Playwright_ReqRoll.Steps;

/// <summary>
/// Provides step definitions for background configuration steps in Reqnroll scenarios.
/// Handles navigation to pages and basic page load verification.
/// </summary>
[Binding]
public class BackgroundConfiguration
{
    /// <summary>
    /// Navigates to the specified URL or the default login page URL from configuration.
    /// Waits for the page to load completely and verifies that the page has a title and content.
    /// </summary>
    /// <param name="url">The URL to navigate to. If empty or null, uses the default login page URL from Config.</param>
    [Given(@"I navigate to the login page ""(.*)""")]
    [Given(@"I navigate to the page ""(.*)""")]
    [Given(@"I navigate to the webpage ""(.*)""")]
    public async Task GivenINavigateToThePage(string url)
    {
        var page = hooks.PlaywrightHooks.Page;
        // If url is provided, use it; otherwise, use default from config
        var targetUrl = string.IsNullOrEmpty(url) ? Config.LoginPageUrl : url;
        await page.GotoAsync(targetUrl);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Verify basic page load
        var title = await page.TitleAsync();
        var content = await page.ContentAsync();

        Assert.Multiple(() =>
        {
            Assert.That(title, Is.Not.Null.And.Not.Empty, $"Page title is empty for URL: {targetUrl}");
            Assert.That(content, Is.Not.Null.And.Not.Empty, $"Page content is empty for URL: {targetUrl}");
        });
    }
}
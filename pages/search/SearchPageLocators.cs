using Microsoft.Playwright;

namespace Playwright_ReqRoll.pages.search;

/// <summary>
///     Represents the Search page in the application, providing methods to interact with search form elements.
///     Uses Playwright locators to find and manipulate page elements.
/// </summary>
public class SearchPageLocators(IPage page)
{
    /// <summary>
    ///     Gets the locator for the search input textbox.
    /// </summary>
    private ILocator GetSearchInput() => page.GetByTestId("search-input");

    /// <summary>
    ///     Gets the locator for the search button.
    /// </summary>
    private ILocator GetSearchButton() => page.GetByTestId("search-button");

    /// <summary>
    ///     Gets the locator for the search results.
    /// </summary>
    private ILocator SearchResults => page.Locator(".MuiCard-root");

    /// <summary>
    ///     Stores the old input element handle for stale element testing.
    /// </summary>
    private IElementHandle? _oldInputHandle;

    /// <summary>
    ///     Enters the specified query into the search input.
    /// </summary>
    /// <param name="query">The search query to enter.</param>
    public async Task EnterSearchQuery(string query)
    {
        var input = GetSearchInput();
        _oldInputHandle = await input.ElementHandleAsync(); // Store the handle before potential re-render
        await input.FillAsync(query);
    }

    /// <summary>
    ///     Clicks the search button and re-locates the input after submission to handle re-renders.
    /// </summary>
    public async Task ClickSearchButton()
    {
        await GetSearchButton().ClickAsync();
        _ = GetSearchInput(); 
    }

    /// <summary>
    ///     Gets the count of search results.
    /// </summary>
    /// <returns>The number of search results.</returns>
    public async Task<int> GetResultsCount()
    {
        await SearchResults.First.WaitForAsync();
        return await SearchResults.CountAsync();
    }

    /// <summary>
    ///     Checks if each result has title, URL, and snippet.
    /// </summary>
    /// <returns>True if all results have the required elements.</returns>
    public async Task<bool> VerifyResultsStructure()
    {
        var results = SearchResults.Locator("div.MuiPaper-root");
        var count = await results.CountAsync();
        for (int i = 0; i < count; i++)
        {
            var result = results.Nth(i);
            var title = result.Locator("a");
            var url = result.Locator("p.text-green-700");
            var snippet = result.Locator("p.text-slate-700");
            if (await title.CountAsync() == 0 || await url.CountAsync() == 0 || await snippet.CountAsync() == 0)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    ///     Checks if no results are displayed.
    /// </summary>
    /// <returns>True if no results are shown.</returns>
    public async Task<bool> NoResultsDisplayed()
    {
        return await SearchResults.CountAsync() == 0;
    }

    /// <summary>
    ///     Waits for search results to load.
    /// </summary>
    public async Task WaitForResultsToLoad()
    {
        await SearchResults.First.WaitForAsync();
    }
    
    /// <summary>
    ///     Re-locates the search input.
    /// </summary>
    /// <returns>The re-located locator.</returns>
    public ILocator ReLocateSearchInput()
    {
        return GetSearchInput();
    }
}

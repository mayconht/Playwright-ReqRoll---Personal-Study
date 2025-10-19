using Microsoft.Playwright;

namespace Playwright_ReqRoll.Pages;

/*Many methods that exists here are just example on how to wrap user actions or create a user action for easier handling
 in code development, so I am forgiving on how granular this sounds, as this is an experimenting project
 if you want to understand why the wrap might be needed, have a look at the last method of this file*/

/// <summary>
///     Represents the Login page in the application, providing methods to interact with login form elements.
///     Uses Playwright locators to find and manipulate page elements.
/// </summary>
public class LoginPage(IPage page)
{
    /// <summary>
    ///     Gets the locator for the username textbox.
    /// </summary>
    private ILocator UsernameTextbox =>
        page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Username" });

    /// <summary>
    ///     Gets the locator for the password textbox.
    /// </summary>
    private ILocator PasswordTextbox =>
        page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Password" });

    /// <summary>
    ///     Gets the locator for the login button.
    /// </summary>
    private ILocator LoginButton => page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Login" });

    /// <summary>
    ///     Gets the locator for the password visibility toggle button.
    /// </summary>
    private ILocator PasswordVisibilityToggle => page.Locator("button[aria-label='Toggle password visibility']");

    /// <summary>
    ///     Gets the locator for the welcome heading after login.
    /// </summary>
    private ILocator WelcomeHeading => page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Level = 5 });

    /// <summary>
    ///     Gets the locator for the logged-in alert message.
    /// </summary>
    private ILocator LoggedInAlert => page.Locator(".MuiAlert-message");

    /// <summary>
    ///     Enters the specified username into the username textbox.
    /// </summary>
    /// <param name="username">The username to enter.</param>
    public async Task EnterUsername(string username)
    {
        await UsernameTextbox.FillAsync(username);
    }

    /// <summary>
    ///     Enters the specified password into the password textbox.
    /// </summary>
    /// <param name="password">The password to enter.</param>
    public async Task EnterPassword(string password)
    {
        await PasswordTextbox.FillAsync(password);
    }

    /// <summary>
    ///     Clicks the login button to submit the login form.
    /// </summary>
    public async Task ClickLoginButton()
    {
        await LoginButton.ClickAsync();
    }

    /// <summary>
    ///     Clicks the password visibility toggle button to show/hide the password.
    /// </summary>
    public async Task ClickPasswordVisibilityToggle()
    {
        await PasswordVisibilityToggle.ClickAsync();
    }

    /// <summary>
    ///     Waits for the specified error message to appear on the page.
    /// </summary>
    /// <param name="message">The error message text to wait for.</param>
    public async Task WaitForErrorMessage(string message)
    {
        await page.GetByText(message).WaitForAsync();
    }

    /// <summary>
    ///     Asserts that the password field has the 'type' attribute set to 'text', indicating visibility.
    /// </summary>
    public async Task AssertPasswordFieldVisibleAsText()
    {
        await Assertions.Expect(PasswordTextbox).ToHaveAttributeAsync("type", "text");
    }

    /// <summary>
    ///     Asserts that the dashboard is displayed for the specified user.
    ///     Checks for welcome message and logged-in alert containing the username.
    /// </summary>
    /// <param name="userName">The username to verify in the dashboard messages.</param>
    public async Task AssertDashboardForUser(string userName)
    {
        var welcomeMessages = await WelcomeHeading.AllInnerTextsAsync();
        Assert.That(
            welcomeMessages.Any(msg => msg.Contains(userName, StringComparison.OrdinalIgnoreCase)),
            Is.True,
            $"Welcome message does not contain the username '{userName}'. Actual messages: {string.Join(", ", welcomeMessages)}");

        var alertMessages = await LoggedInAlert.AllInnerTextsAsync();
        Assert.That(
            alertMessages.Any(msg =>
                msg.Contains($"You are logged in as {userName.ToUpper()}", StringComparison.OrdinalIgnoreCase)),
            Is.True,
            $"Banner message does not contain the expected text 'You are logged in as {userName.ToUpper()}'. Actual messages: {string.Join(", ", alertMessages)}");
    }

    /// <summary>
    ///     Clicks the logout button to log out the user.
    /// </summary>
    public async Task ClickLogoutButton()
    {
        var logoutButton = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Logout" });
        await logoutButton.ClickAsync();
    }

    /// <summary>
    ///     Gets all cookies from the current browser context.
    /// </summary>
    /// <returns>A list of all cookies in the current context.</returns>
    public async Task<IReadOnlyList<BrowserContextCookiesResult>> GetCookies()
    {
        return await page.Context.CookiesAsync();
    }

    /// <summary>
    ///     Gets cookies for a specific URL from the current browser context.
    /// </summary>
    /// <param name="url">The URL to get cookies for.</param>
    /// <returns>A list of cookies for the specified URL.</returns>
    public async Task<IReadOnlyList<BrowserContextCookiesResult>> GetCookies(string url)
    {
        return await page.Context.CookiesAsync(new[] { url });
    }
}
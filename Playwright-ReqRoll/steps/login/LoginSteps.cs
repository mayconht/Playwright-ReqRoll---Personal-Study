using Microsoft.Playwright;
using Playwright_ReqRoll.Pages;
using Playwright_ReqRoll.Steps;
using Reqnroll;

namespace Playwright_ReqRoll.steps.login;

/// <summary>
/// Provides step definitions for login-related actions in Reqnroll scenarios.
/// Interacts with the LoginPage to perform user actions and assertions.
/// </summary>
[Binding]
public class LoginSteps
{
    /// <summary>
    /// Gets a new instance of LoginPage for the current page.
    /// </summary>
    private LoginPage LoginPage => new(hooks.PlaywrightHooks.Page);

    /// <summary>
    /// Enters the specified username into the login form.
    /// </summary>
    /// <param name="testUser">The username to enter.</param>
    [When("I enter username {string}")]
    public async Task WhenIEnterUsernameString(string testUser)
    {
        await LoginPage.EnterUsername(testUser);
    }

    /// <summary>
    /// Enters the specified password into the login form.
    /// </summary>
    /// <param name="password">The password to enter.</param>
    [When("I enter password {string}")]
    public async Task WhenIEnterPassword(string password)
    {
        await LoginPage.EnterPassword(password);
    }

    /// <summary>
    /// Clicks the login button to submit the login form.
    /// </summary>
    [When("I click the login button")]
    public async Task WhenIClickTheLoginButton()
    {
        await LoginPage.ClickLoginButton();
    }

    /// <summary>
    /// Asserts that the specified error message is displayed on the page.
    /// </summary>
    /// <param name="message">The error message text to check for.</param>
    [Then("I should see the error message {string}")]
    public async Task ThenIShouldSeeTheErrorMessage(string message)
    {
        await LoginPage.WaitForErrorMessage(message);
    }

    /// <summary>
    /// Clicks the password visibility toggle button.
    /// </summary>
    [When("I click the password visibility toggle")]
    public async Task WhenIClickThePasswordVisibilityToggle()
    {
        await LoginPage.ClickPasswordVisibilityToggle();
    }

    /// <summary>
    /// Asserts that the password field is visible as text (not password type).
    /// </summary>
    [Then("the password field should be visible as text")]
    public async Task ThenThePasswordFieldShouldBeVisibleAsText()
    {
        await LoginPage.AssertPasswordFieldVisibleAsText();
    }

    /// <summary>
    /// Asserts that the dashboard is displayed for the specified user.
    /// </summary>
    /// <param name="userName">The username to verify in the dashboard.</param>
    [Then("I should see the dashboard for {string}")]
    public async Task ThenIShouldSeeTheDashboardForString(string userName)
    {
        await LoginPage.AssertDashboardForUser(userName);
    }
}
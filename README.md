# Playwright-ReqRoll

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Playwright](https://img.shields.io/badge/Playwright-1.55.0-green.svg)](https://playwright.dev/)
[![Reqnroll](https://img.shields.io/badge/Reqnroll-3.1.2-orange.svg)](https://reqnroll.net/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

# Playwright-ReqRoll

A simple UI automation testing framework using Playwright and Reqnroll (BDD) for web applications. 

This project is part of my study portfolio, showcasing skills in automated testing, BDD, and handling complex UI scenarios. 
Keep in mind some implementations are simplified, or might not be right due to limitations of both knowledge or time.
So if you come across something that could be improved, please feel free to open an issue or a PR.

## Technologies Used

- **.NET 8.0** - Target framework
- **Playwright** - End-to-end testing framework
- **Reqnroll** - BDD framework for .NET (formerly SpecFlow)
- **NUnit** - Unit testing framework
- **Gherkin** - Business-readable, domain-specific language for behavior descriptions
- **Microsoft.Extensions.Configuration** - Configuration management

## Architecture

The project follows a structured approach for UI automation testing:

- **Features** - Gherkin feature files defining test scenarios
- **Steps** - Step definitions implementing the Gherkin steps
- **Pages** - Page Object Models for UI interactions
- **Hooks** - Test lifecycle management and browser setup

## Features

- BDD Testing: Behavior-Driven Development with Reqnroll
- Cross-Browser Support: Tests run on Chromium, Firefox, and WebKit
- Dynamic DOM Handling: Strategies for dealing with re-rendering UI components
- Stale Element Management: Robust handling of element re-attachment after page updates
- Reporting: Live Interactive Traces, videos, and screenshots for test failures and successes
- Shared Browser Context (under dev): Keeps browser open across scenarios for performance and state management

## Prerequisites

- .NET 8.0 or higher
- Git

## Dev Container (VS Code / Codespaces)

This repo includes a ready-to-use Dev Container to get you coding and running tests fast, with everything pinned for .NET 8 and Playwright.

- Base image: `mcr.microsoft.com/devcontainers/dotnet:1-8.0`
- Auto steps on first open: restore, build, install Playwright browsers, and run tests
- VS Code extensions preinstalled:
   - `ms-dotnettools.csdevkit`
   - `ms-playwright.playwright`
   - `alexkrechik.cucumberautocomplete`
   - `tal7aouy.rainbow-brackets`

How to use:

1. Install VS Code and the “Dev Containers” extension (or open in GitHub Codespaces).
2. Press F1 and type "Dev Container: Start in Container"
3. The post-create hooks will:
    - `dotnet restore`
    - `dotnet build`
    - install Playwright browsers via the generated script
    - `dotnet test`

Notes and troubleshooting:

- The Playwright install script path uses your target framework. This project targets `net8.0`, so the script is generated at `bin/Debug/net8.0/playwright.ps1` (Windows/PowerShell). If you change the target framework, update any references accordingly.
- On first run inside a fresh container, browser downloads can take a few minutes.
- If tests fail during container creation, re-run them after the browsers finish installing.

## Running the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/Playwright-ReqRoll.git
   cd Playwright-ReqRoll
   ```

2. Restore .NET dependencies:
   ```bash
   dotnet restore
   ```

3. Install Playwright browsers:
   ```bash
   dotnet run --project Playwright-ReqRoll.csproj -- install
   ```

## Application Access

The tests target external web applications:

- Login Flow: https://www.cnarios.com/challenges/login-flow
- Search Engine: https://www.cnarios.com/challenges/simple-search-engine

## Running Tests

### Run all tests:
```bash
dotnet test
```

### Run specific test categories:
```bash
# Run only login tests
dotnet test --filter "Category=Login"

# Run only search tests
dotnet test --filter "Category=Search"
```

### Run with specific browser:
Modify `appsettings.json` to set the desired browser:
```json
{
  "BrowserType": "chromium", // or "firefox", "webkit"
  "Headless": false
}
```

## Configuration

The project uses `appsettings.json` for configuration:

- `BrowserType`: "chromium", "firefox", or "webkit"
- `Headless`: true/false for headless mode
- `SlowMo`: Delay between actions in milliseconds
- `ReportsPath`: Path for saving test artifacts
- `RecordVideo`: true/false to record videos
- `ScreenshotOnSuccess/Failure`: Capture screenshots

## Development Workflow

1. Write or update Gherkin feature files in the `features/` directory
2. Implement step definitions in the `steps/` directory
3. Define page objects in the `pages/` directory for UI interactions
4. Run tests to validate changes
5. Review test reports and artifacts for failures

## Project Structure

```
Playwright-ReqRoll/
├── features/           # Gherkin feature files
│   ├── login/         # Login flow scenarios
│   └── search/        # Search engine scenarios
├── steps/             # Step definitions
├── pages/             # Page Object Models
├── hooks/             # Test lifecycle management
├── appsettings.json   # Configuration
└── README.md          # This file
```

## Testing Strategy

The project includes BDD tests covering:

- UI interactions with various scenarios
- Dynamic DOM handling and re-rendering
- Stale element exception management
- Cross-browser compatibility
- Comprehensive reporting and debugging

Test frameworks used:

- Reqnroll for BDD execution
- NUnit for test assertions
- Playwright for browser automation

## Contributing

1. Fork the repository
2. Create a test branch (`git checkout -b tests/amazing-tests`)
3. Commit your changes (`git commit -m 'Add some amazing tests'`)
4. Push to the branch (`git push origin tests/amazing-tests`)
5. Open a Pull Request

## Troubleshooting
Ops, not yet.
### Common Issues

1. Browser not found: Verify if Playwright browsers are installed with `dotnet run --project Playwright-ReqRoll.csproj -- install`

2. Video file locked: Increased delay in teardown to ensure files are fully written

## Viewing Traces

Traces are generated for failed tests and can be viewed to debug issues:

### Online Trace Viewer
1. Go to [Playwright Trace Viewer](https://trace.playwright.dev/)
2. Upload the `.zip` trace file from `C:\reports\Playwright-Traces\`

### Local Trace Viewer
1. Install Playwright CLI if not already:
   ```bash
   npm install -g @playwright/test
   ```

2. View the trace locally:
   ```bash
   npx playwright show-trace "C:\reports\Playwright-Traces\trace-file.zip"
   ```

Traces provide a timeline of actions, screenshots, and network requests for detailed debugging.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For questions or support, please open an issue on GitHub.

---

Built with using Playwright and Reqnroll

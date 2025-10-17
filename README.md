# Playwright-ReqRoll

This project contains automated UI tests using Playwright and Reqnroll (BDD) for login scenarios.

## Prerequisites

- .NET 8.0
- Supported browsers (Chromium, Firefox, WebKit) installed via Playwright

## How to run the tests

1. Install dependencies:
   ```
   dotnet restore
   ```

2. Install Playwright browsers:
   ```
   dotnet run --project Playwright-ReqRoll/Playwright-ReqRoll.csproj -- install
   ```

3. Run the tests:
   ```
   dotnet test
   ```

## Configuration

Configurations are in `appsettings.json`:
- Browser type
- Headless mode
- Paths for reports (traces, videos, downloads)

## Reports

Artifacts are saved in `C:\reports`:
- Traces: `Playwright-Traces/`
- Videos: `Playwright-Videos/`
- Downloads: `Downloads/`

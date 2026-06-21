1. HMCTS Login Automation Tests

  Automated UI tests for the HMCTS SDET technical exercise, using Playwright for .NET, xUnit, and the public 
  “Form Authentication” page on https://the-internet.herokuapp.com/.

This solution covers the key login and logout(positive and negative scenarios) flows, demonstrating clean test design, 
Page Object Models, and the use of fixtures.

Tech stack
.NET (C#) test project

[Playwright for .NET] for browser automation

xUnit as the test framework

Target application: https://the-internet.herokuapp.com/login (Form Authentication)

2. Structure of the project
   
HMCTS_LoginAutomationTests/

  ├── Pages/
  │   ├── LoginPage.cs
  │   └── SecureAreaPage.cs
    ├── Tests/
  │   └── LoginTests.cs
      └── BasePageTests.cs
      └── FixtureTests.cs

3. Playwright + xUnit, not BDD feature files
The task only requires working tests and an explanation of the approach, not Gherkin/Reqnroll integration, so the tests are code‑first:

Using xUnit keeps the stack small and idiomatic for .NET and Playwright.

Readable test names (e.g. Valid_credentials_should_display_secure_area_page) and Given/When/Then‑style comments provide BDD flavour without the overhead of .feature files.

This keeps the solution easy to run, fast, and straightforward to explain in an interview while still showing design thinking.

4. Page Object Model (POM)
The test code is kept thin by moving UI details into Page Objects:

LoginPage

Navigates to /login.

Enters username/password.

Clicks the Login button.

Exposes the flash message text used by negative tests.

SecureAreaPage

Verifies that the secure area is loaded (URL contains /secure and the “Secure Area” heading is visible).

Clicks the Logout button.

Benefits:

Tests express behaviour, not selectors (loginPage.LoginAsync(...) instead of repeating #username, #password, etc.).

If the UI changes, most updates are localised to the Page Objects, keeping tests maintainable.

3. Shared Playwright fixture with isolation per test
The tests use an xUnit class fixture (PlaywrightFixture) to manage expensive resources:

PlaywrightFixture implements IAsyncLifetime to launch the browser once per test class and close it when all tests are done.

Each test calls NewPageAsync() which creates a new browser context and page, so cookies and storage do not leak between tests, matching Playwright’s test isolation guidance.

This strategy balances:

Performance – the browser is reused across tests instead of launching per test.

Isolation – each test still runs in its own fresh context, making failures easier to debug and preventing cascading issues.

5. Headless by default, headed for debugging
In CI and normal dotnet test runs, the browser runs in headless mode to maximise speed and avoid UI dependencies.

For local exploration or demo, switching Headless = false in PlaywrightFixture lets you observe the flow visually.

This mirrors a realistic workflow: headless in pipelines, headed when debugging.

6. Grouping of tests by feature
All login‑related tests are stored in a single LoginTests class:



est scenarios covered
Positive scenario
Valid login

Given I am on the login page

When I log in with tomsmith / SuperSecretPassword!

Then I am redirected to /secure and see the “Secure Area” message

Negative scenarios
Invalid username, valid password → error message shown, stay on /login

Valid username, invalid password → error message shown, stay on /login

Invalid username and password → generic invalid credentials error

Logout behaviour
Given I am logged into the secure area

When I click Logout

Then I am redirected back to the login page and the login form is visible


Notes:
The target application is a public demo site; credentials and messages are test‑only and not sensitive.

Browser warning pop‑ups (e.g. “password found in a data breach”) come from Chrome’s own security features and 
are deliberately ignored, since they are not part of the application under test

  

      

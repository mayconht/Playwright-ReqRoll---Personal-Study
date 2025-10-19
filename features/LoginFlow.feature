#LF stands for Login Flow or Login Feature
#https://www.cnarios.com/challenges/login-flow

@Login
Feature: Login Flow
As a user
I want to log in to the application
So that I can access my account

    Background:
        Given I navigate to the login page "https://www.cnarios.com/challenges/login-flow"

    @Positive
    @High
    Scenario Outline: Successful login with valid credentials
        When I enter username "<username>"
        And I enter password "<password>"
        And I click the login button
        Then I should see the dashboard for "<username>"

        Examples:
          | username | password |
          | admin    | admin123 | # LF_004
          | user     | user123  | # LF_003

    @Negative
    @High
    @LF_001
    @LF_002
    # LF_002, LF_001
    Scenario Outline: Login with invalid credentials
        When I enter username "<username>"
        And I enter password "<password>"
        And I click the login button
        Then I should see the error message "Invalid username or password"

        Examples:
          | username    | password      |
          | invaliduser | password123   | # LF_002 Extension
          | testuser    | wrongpassword | # LF_002 Extension
          | wrongUser   | wrongPass     | # LF_002
          | ""          | ""            | # Empty credentials Covers LF_001

    @Positive
    @Medium
    # LF_005
    Scenario Outline: Login with each user and logout
        When I enter username "<username>"
        And I enter password "<password>"
        And I click the login button
        And I click the login button
        Then I should see the dashboard for "<username>"
        When I click the logout button
        Then I should be redirected to the login page
        Then the cookies should be cleared

        Examples:
          | username | password |
          | admin    | admin123 |
          | user     | user123  |
Feature: Login Flow

As a user
I want to log in to the application
So that I can access my account

    Background:
        Given I navigate to the login page "https://www.cnarios.com/challenges/login-flow"

    Scenario Outline: Successful login with valid credentials
        When I enter username "<username>"
        And I enter password "<password>"
        And I click the login button
        Then I should see the dashboard for "<username>"

        Examples:
          | username | password |
          | admin    | admin123 |
          | user     | user123  |

    Scenario Outline: Login with invalid credentials
        When I enter username "<username>"
        And I enter password "<password>"
        And I click the login button
        Then I should see the error message "Invalid username or password"

        Examples:
          | username    | password      |
          | invaliduser | password123   |
          | testuser    | wrongpassword |
          | invaliduser | wrongpassword |

          #    Scenario Outline: Login with empty fields
          #        When I enter username "<username>"
          #        And I enter password "<password>"
          #        And I click the login button
          #        Then I should see the error message "<error_message>"
          #
          #        Examples:
          #          | username | password    | error_message                      |
          #          | ""       | password123 | Username is required               |
          #          | testuser | ""          | Password is required               |
          ##          | ""       | ""          | Username and Password are required |

          #  Scenario: Toggle password visibility
          #    When I enter password "password123"
          #    And I click the password visibility toggle
          #    Then the password field should be visible as text
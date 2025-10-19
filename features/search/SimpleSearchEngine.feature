#SSE stands for Simple Search Engine
#https://www.cnarios.com/challenges/simple-search-engine

@Search
Feature: Simple Search Engine
As a user
I want to search for information
So that I can find relevant results

    Background:
        Given I navigate to the search page "https://www.cnarios.com/challenges/simple-search-engine"

        
    # You can convert this to Scenario Outline if needed and change the data as required.
    @Positive
    @High
    @SSE_001
    Scenario: Perform a valid search and verify results
        When I enter search query "React Testing"
        And I click the search button
        Then I should see at least 3 search results
        Then each result should have a clickable title, URL, and snippet
        
    # No actual code was needed to implement this step.
    @Positive
    @High
    @SSE_001
    Scenario Outline: Perform a valid search and verify results with multiple entries
        When I enter search query "<inputQuery>"
        And I click the search button
        Then I should see at least 3 search results
        Then each result should have a clickable title, URL, and snippet
        
        Examples:
          | inputQuery         |
          | JavaScript basics  |
          | Selenium WebDriver |
          | Web development    |

    @Negative
    @Medium
    @SSE_002
    Scenario: Attempt to search with empty input
        When I enter search query ""
        And I click the search button
        Then I should see no results displayed

    @Positive
    @Medium
    @SSE_004
    Scenario: Perform a second valid search after recovery
        When I re-locate the search input
        And I enter search query "Node.js tutorials"
        And I click the search button
        Then I should see fresh results for the second query

    @Positive
    @Medium
    @SSE_005
    Scenario Outline: Perform back-to-back searches to observe re-render behavior
        When I enter search query "<firstQuery>"
        And I click the search button
        Then I should see at least 3 search results
        When I enter search query "<secondQuery>"
        And I click the search button
        Then I should see fresh results for the second query

        Examples:
          | firstQuery             | secondQuery       |
          | React Testing          | Node.js tutorials |
          | JavaScript basics      | Web development   |
          | Selenium WebDriver     | API testing       |

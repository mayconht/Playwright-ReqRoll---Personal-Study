//This allows us to use the feature files (fixtures) in parallel
//With this we can have a thread safe execution of our tests

[assembly: Parallelizable(ParallelScope.Fixtures)]
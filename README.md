# Amaris - Interview Preparation Project

.NET 10 solution with xUnit test project.

## Project Structure

```
Amaris.sln
├── Amaris.Core/               # Domain Models & Services
│   ├── Models/
│   │   ├── IEntity.cs
│   │   └── Product.cs
│   └── Services/
│       ├── ICalculatorService.cs / CalculatorService.cs
│       └── IStringService.cs / StringService.cs
├── Amaris.Data/               # Data Access Layer
│   └── Repositories/
│       ├── IRepository.cs
│       └── InMemoryRepository.cs
└── Amaris.Core.Tests/         # xUnit Tests
    ├── Services/
    │   ├── CalculatorServiceTests.cs
    │   └── StringServiceTests.cs
    └── Repositories/
        └── InMemoryRepositoryTests.cs
```

## Quick Commands

```bash
# Build
dotnet build

# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run a specific test class
dotnet test --filter "CalculatorServiceTests"

# Run a specific test method
dotnet test --filter "Add_TwoPositiveNumbers_ReturnsSum"

# Run tests matching a pattern
dotnet test --filter "Divide"

# Add a new class library project
dotnet new classlib -n ProjectName -o src/ProjectName

# Add a new xunit test project
dotnet new xunit -n ProjectName.Tests -o tests/ProjectName.Tests

# Add project reference
dotnet add tests/ProjectName.Tests reference src/ProjectName

# Add project to solution
dotnet sln add src/ProjectName
```

## xUnit Cheat Sheet

```csharp
// --- Attributes ---
[Fact]                                    // Single test case
[Theory]                                  // Parameterized test
[InlineData(1, 2, 3)]                     // Inline test data for Theory

// --- Assertions ---
Assert.Equal(expected, actual);           // Equality check
Assert.NotEqual(expected, actual);
Assert.True(condition);
Assert.False(condition);
Assert.Null(obj);
Assert.NotNull(obj);
Assert.Empty(collection);
Assert.Single(collection);
Assert.Contains(item, collection);
Assert.DoesNotContain(item, collection);
Assert.IsType<ExpectedType>(obj);
Assert.Throws<ExceptionType>(() => ...);  // Sync exception
await Assert.ThrowsAsync<T>(async () =>); // Async exception
Assert.InRange(actual, low, high);

// --- Test Setup (Constructor / IDisposable) ---
public class MyTests : IDisposable
{
    private readonly MyService _sut;

    public MyTests()  // runs before each test
    {
        _sut = new MyService();
    }

    public void Dispose()  // runs after each test
    {
        // cleanup
    }
}

// --- Collection Fixtures (shared context) ---
[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

[Collection("Database")]
public class MyTests { }
```

## Git Cheat Sheet

```bash
# Status and history
git status
git log --oneline
git log --oneline --graph --all
git diff
git diff --staged

# Branching
git branch                       # list branches
git branch feature/my-feature    # create branch
git checkout feature/my-feature  # switch branch
git checkout -b feature/new      # create + switch
git merge feature/my-feature     # merge into current branch
git branch -d feature/done       # delete branch

# Staging and committing
git add .                        # stage all
git add src/MyFile.cs            # stage specific file
git commit -m "message"
git commit -am "message"         # stage tracked + commit

# Remote
git remote -v
git push origin main
git pull origin main
git fetch

# Undo
git checkout -- file.cs          # discard unstaged changes
git reset HEAD file.cs           # unstage a file
git reset --soft HEAD~1          # undo last commit, keep changes staged
git stash                        # stash changes
git stash pop                    # apply stashed changes

# Tags
git tag v1.0.0
git tag -a v1.0.0 -m "Release"
```

## C# Quick Reference

```csharp
// Interface
public interface IMyService { string DoWork(int id); }

// Implementation
public class MyService : IMyService
{
    public string DoWork(int id) => $"Result: {id}";
}

// Records (immutable data)
public record Person(string Name, int Age);

// Pattern matching
var result = obj switch
{
    int n when n > 0 => "positive",
    int n when n < 0 => "negative",
    _ => "zero"
};

// Null safety
ArgumentNullException.ThrowIfNull(param);
string name = input ?? "default";
int length = input?.Length ?? 0;

// LINQ
var filtered = items.Where(x => x.IsActive);
var mapped = items.Select(x => x.Name);
var first = items.FirstOrDefault(x => x.Id == 1);
var grouped = items.GroupBy(x => x.Category);
var ordered = items.OrderBy(x => x.Name).ThenBy(x => x.Age);
bool any = items.Any(x => x.Price > 100);
int count = items.Count(x => x.IsActive);
decimal sum = items.Sum(x => x.Price);
```

# Amaris - Interview Preparation Project

.NET 10 | xUnit | Repository Pattern | Minimal API

## Project Structure

```
Amaris.sln
├── Amaris.Core/          # Domain: Models, Services, Interfaces
├── Amaris.Data/          # DAL: Generic Repository (InMemory)
├── Amaris.Api/           # Presentation: Minimal API + Swagger
└── Amaris.Core.Tests/    # xUnit Tests (36 tests)
```

## Commands

```bash
dotnet build                                    # Build solution
dotnet test                                     # Run all tests
dotnet test --filter "CalculatorServiceTests"   # Run specific test class
dotnet test --filter "Divide"                   # Run tests matching pattern
dotnet run --project Amaris.Api                 # Run API
```

API: `https://localhost:7289/swagger` | `http://localhost:5038/swagger`

## xUnit Cheat Sheet

```csharp
[Fact]                                    // Single test
[Theory]                                  // Parameterized test
[InlineData(1, 2, 3)]                     // Test data

Assert.Equal(expected, actual);
Assert.True(condition);
Assert.Null(obj);
Assert.NotNull(obj);
Assert.Empty(collection);
Assert.Single(collection);
Assert.Contains(item, collection);
Assert.IsType<T>(obj);
Assert.Throws<T>(() => ...);
Assert.InRange(actual, low, high);

// Setup / Teardown
public class MyTests : IDisposable
{
    public MyTests() { /* before each */ }
    public void Dispose() { /* after each */ }
}
```

## Git Cheat Sheet

```bash
git status                          # Working tree status
git log --oneline --graph --all     # Visual history
git diff / git diff --staged        # View changes

git checkout -b feature/new         # Create + switch branch
git merge feature/branch            # Merge branch
git branch -d feature/done          # Delete branch

git add . / git add file.cs         # Stage changes
git commit -m "message"             # Commit
git push origin main                # Push

git reset --soft HEAD~1             # Undo last commit (keep changes)
git stash / git stash pop           # Stash changes
```

## C# Quick Reference

```csharp
// Interface + Implementation
public interface IService { string Do(int id); }
public class Service : IService
{
    public string Do(int id) => $"Result: {id}";
}

// Record, Pattern Matching, Null Safety
public record Person(string Name, int Age);

var r = obj switch { int n when n > 0 => "+", _ => "-" };

ArgumentNullException.ThrowIfNull(param);
string name = input ?? "default";

// LINQ
items.Where(x => x.Active).Select(x => x.Name);
items.FirstOrDefault(x => x.Id == 1);
items.GroupBy(x => x.Category);
items.OrderBy(x => x.Name).ThenBy(x => x.Age);
items.Any(x => x.Price > 100);
items.Sum(x => x.Price);
```

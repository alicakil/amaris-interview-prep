# Amaris - Interview Preparation Project

.NET 10 | xUnit | FluentAssertions | Moq | Repository Pattern | Minimal API

## Project Structure

```
Amaris.sln
├── Amaris.Core/          # Domain: Models, Services, Interfaces
├── Amaris.Data/          # DAL: Generic Repository (InMemory)
├── Amaris.Api/           # Presentation: Minimal API + Swagger
└── Amaris.Core.Tests/    # xUnit + FluentAssertions + Moq (52 tests)
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

## xUnit + FluentAssertions Cheat Sheet

```csharp
[Fact]                                    // Single test
[Theory]                                  // Parameterized test
[InlineData(1, 2, 3)]                     // Inline test data
[MemberData(nameof(TestData))]            // Complex test data
[Theory, AutoData]                        // AutoFixture random data

// --- FluentAssertions ---
result.Should().Be(expected);
result.Should().NotBe(value);
result.Should().BeTrue();
result.Should().BeNull();
result.Should().NotBeNull();
text.Should().BeEmpty();
text.Should().Contain("sub");

// Collections
list.Should().HaveCount(3);
list.Should().BeEmpty();
list.Should().ContainSingle();
list.Should().Contain(item);
list.Should().AllSatisfy(x => x.Active.Should().BeTrue());
list.Should().BeInDescendingOrder(x => x.Price);

// Exceptions
var act = () => sut.Method();
act.Should().Throw<ArgumentException>()
    .WithMessage("*required*")
    .WithParameterName("name");

// --- Moq ---
var mock = new Mock<IRepository<T>>();
mock.Setup(r => r.GetById(1)).Returns(entity);
mock.Setup(r => r.GetAll()).Returns(list);
mock.Setup(r => r.GetById(It.IsAny<int>())).Returns(entity);
mock.Verify(r => r.Add(entity), Times.Once);
mock.Verify(r => r.Add(It.IsAny<T>()), Times.Never);

// --- AutoFixture ---
var fixture = new Fixture();
var product = fixture.Create<Product>();              // Random object
var products = fixture.CreateMany<Product>(5);        // Random list
var custom = fixture.Build<Product>()
    .With(p => p.Price, 99m).Create();                // Partial control

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

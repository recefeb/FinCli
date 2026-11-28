using FinCli.Core;
using Xunit;

namespace FinCli.Tests;

public class TransactionServiceTests : IDisposable
{
    private readonly string _filePath;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
        _service = new TransactionService(_filePath);
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }

    [Fact]
    public void GetTotal_TwoTransactions_ReturnsSum()
    {
        _service.AddTransaction(100m, "Food", null);
        _service.AddTransaction(50m, "Transport", null);

        Assert.Equal(150m, _service.GetTotal());
    }

    [Fact]
    public void GetTotalByCategory_MultipleCategories_ReturnsOnlyMatchingSum()
    {
        _service.AddTransaction(100m, "Food", null);
        _service.AddTransaction(40m, "Food", null);
        _service.AddTransaction(25m, "Transport", null);

        Assert.Equal(140m, _service.GetTotalByCategory("Food"));
    }

    [Fact]
    public void GetTotalByMonth_DifferentMonths_ReturnsOnlyThatMonth()
    {
        _service.AddTransaction(100m, "Food", null, new DateOnly(2026, 11, 5));
        _service.AddTransaction(50m, "Food", null, new DateOnly(2026, 10, 20));

        Assert.Equal(100m, _service.GetTotalByMonth(2026, 11));
    }

    [Fact]
    public void GetTotalsByCategory_MultipleCategories_ReturnsTotalPerCategory()
    {
        _service.AddTransaction(100m, "Food", null);
        _service.AddTransaction(40m, "Food", null);
        _service.AddTransaction(25m, "Transport", null);

        var totals = _service.GetTotalsByCategory();

        Assert.Equal(2, totals.Count);
        Assert.Equal(140m, totals["Food"]);
        Assert.Equal(25m, totals["Transport"]);
    }

    [Fact]
    public void GetTotalsByCategory_NoTransactions_ReturnsEmpty()
    {
        var totals = _service.GetTotalsByCategory();

        Assert.Empty(totals);
    }

    [Fact]
    public void GetTotalsByCategory_DifferentCasing_GroupsTogether()
    {
        _service.AddTransaction(100m, "Food", null);
        _service.AddTransaction(40m, "food", null);
        _service.AddTransaction(10m, "FOOD ", null);

        var totals = _service.GetTotalsByCategory();

        Assert.Single(totals);
        Assert.Equal(150m, totals["Food"]);
    }

    [Fact]
    public void GetTotalByCategory_DifferentCasing_ReturnsSum()
    {
        _service.AddTransaction(100m, "Food", null);
        _service.AddTransaction(40m, "food", null);

        Assert.Equal(140m, _service.GetTotalByCategory("FOOD"));
    }

    [Fact]
    public void DeleteTransaction_ExistingId_RemovesItAndReturnsTrue()
    {
        var first = _service.AddTransaction(10m, "Food", null);
        var second = _service.AddTransaction(20m, "Transport", null);

        var result = _service.DeleteTransaction(first.Id);

        Assert.True(result);
        var remaining = Assert.Single(_service.GetAll());
        Assert.Equal(second.Id, remaining.Id);
    }

    [Fact]
    public void DeleteTransaction_UnknownId_ReturnsFalseAndKeepsData()
    {
        _service.AddTransaction(10m, "Food", null);

        var result = _service.DeleteTransaction(999);

        Assert.False(result);
        Assert.Single(_service.GetAll());
    }

    [Fact]
    public void AddTransaction_DifferentCasing_UsesExistingCategoryName()
    {
        _service.AddTransaction(10m, "Food", null);

        var added = _service.AddTransaction(5m, "food", null);

        Assert.Equal("Food", added.Category);

        Assert.All(_service.GetAll(), t => Assert.Equal("Food", t.Category));
    }

    [Fact]
    public void AddTransaction_NewCategory_KeepsTypedName()
    {
        var added = _service.AddTransaction(5m, "gym", null);

        Assert.Equal("gym", added.Category);
    }
}
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
}
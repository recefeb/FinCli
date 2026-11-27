using FinCli.Core;
using Xunit;

namespace FinCli.Tests;

public class TransactionTests
{
    [Fact]
    public void Constructor_EmptyCategory_UsesOther()
    {
        var transaction = new Transaction(100m, "");

        Assert.Equal("Other", transaction.Category);
    }

    [Fact]
    public void Constructor_NegativeAmount_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Transaction(-5m));
    }

    [Fact]
    public void Constructor_ZeroAmount_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Transaction(0m));
    }

    [Fact]
    public void Constructor_ValidAmount_StoresAmount()
    {
        var transaction = new Transaction(0.01m);

        Assert.Equal(0.01m, transaction.Amount);
    }

    [Fact]
    public void Constructor_CategoryWithSpaces_TrimsCategory()
    {
        var transaction = new Transaction(100m, "  Food  ");

        Assert.Equal("Food", transaction.Category);
    }

    [Fact]
    public void Constructor_WhitespaceCategory_UsesOther()
    {
        var transaction = new Transaction(100m, "   ");

        Assert.Equal("Other", transaction.Category);
    }
}
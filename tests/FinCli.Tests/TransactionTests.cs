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
}
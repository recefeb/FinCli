namespace FinCli.Core;

public class Transaction
{
    private const string DefaultCategory = "Other";
    public int Id { get; set; }
    public decimal Amount { get; private set; }
    public string? Description { get; private set; }
    public string Category { get; private set; }
    public DateOnly TransactionDate { get; set; }

    public Transaction(decimal amount, string category = DefaultCategory, string? description = null, DateOnly transactionDate = default)
    {
        ValidateAmount(amount);
        Amount = amount;


        Category = NormalizeCategory(category);

        Description = description;
        TransactionDate = transactionDate == default ? DateOnly.FromDateTime(DateTime.Today) : transactionDate;
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }
    }

    private static string NormalizeCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            category = DefaultCategory;
        }

        return category;
    }

    public void UpdateAmount(decimal newAmount)
    {
        ValidateAmount(newAmount);
        Amount = newAmount;
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = newDescription;
    }

    public void UpdateCategory(string newCategory)
    {
        Category = NormalizeCategory(newCategory);
    }
}

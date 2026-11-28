namespace FinCli.Core;

public class TransactionService
{
    private string _path;
    private TransactionStorage _storage;

    public TransactionService(string path)
    {
        _path = path;
        _storage = new TransactionStorage();
    }

    public int GetNextId(List<Transaction> transactions)
    {
        if (transactions.Count == 0)
        {
            return 1;
        }

        return transactions[transactions.Count - 1].Id + 1;
    }

    public List<Transaction> GetAll()
    {
        return _storage.Load(_path);
    }

    public Transaction AddTransaction(decimal amount, string category, string? description, DateOnly transactionDate = default)
    {
        var transactions = GetAll();
        var newExpense = new Transaction(amount, category, description, transactionDate);
        newExpense.Id = GetNextId(transactions);
        transactions.Add(newExpense);
        _storage.Save(transactions, _path);

        return newExpense;
    }

    public decimal GetTotal()
    {
        var transactions = GetAll();
        return transactions.Sum(t => t.Amount);
    }

    public decimal GetTotalByCategory(string category)
    {
        var transactions = GetAll();
        var categoryTransactions = transactions.Where(t => string.Equals(t.Category, category.Trim(), StringComparison.OrdinalIgnoreCase));
        return categoryTransactions.Sum(t => t.Amount);
    }

    public decimal GetTotalByMonth(int year, int month)
    {
        var transactions = GetAll();
        var matchingDateTransactions = transactions.Where(t => t.TransactionDate.Year == year && t.TransactionDate.Month == month);
        return matchingDateTransactions.Sum(t => t.Amount);
    }

    public Dictionary<string, decimal> GetTotalsByCategory()
    {
        var transactions = GetAll();

        return transactions
            .GroupBy(t => t.Category, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(t => t.Amount),
                StringComparer.OrdinalIgnoreCase);
    }

    public bool DeleteTransaction(int id)
    {
        var transactions = GetAll();
        var target = transactions.FirstOrDefault(t => t.Id == id);

        if (target == null)
        {
            return false;
        }

        transactions.Remove(target);
        _storage.Save(transactions, _path);

        return true;
    }
}


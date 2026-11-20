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
}


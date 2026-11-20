namespace FinCli.Core;

public class TransactionService
{
    public int GetNextId(List<Transaction> transactions)
    {
        if (transactions.Count == 0)
        {
            return 1;
        }

        return transactions[transactions.Count - 1].Id + 1;
    }
}


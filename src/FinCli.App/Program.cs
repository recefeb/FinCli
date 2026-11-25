using FinCli.Core;

var service = new TransactionService("deneme.json");

while (true)
{
    Console.WriteLine("1. Show Total");
    Console.WriteLine("2. Add Expense");
    Console.WriteLine("3. List Expenses");
    Console.WriteLine("0. Exit");
    Console.Write("Choice:  ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            decimal result = service.GetTotal();
            Console.WriteLine($"Total: {result}");
            break;


        case "2":
            Console.Write("Please enter the amount spent: ");
            string? inputAmount = Console.ReadLine();
            if (decimal.TryParse(inputAmount, out decimal amount))
            {
                Console.Write("Enter the Category: ");
                string category = Console.ReadLine() ?? "Other";

                Console.Write("Enter the Description: ");
                string? description = Console.ReadLine();

                var newExpense = service.AddTransaction(amount, category, description);
                Console.WriteLine($"Transaction Id: {newExpense.Id}");
            }
            else
            {
                Console.WriteLine("Invalid amount! Please try again.");
            }
            break;

        case "3":
            var expenses = service.GetAll();
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expense yet.");
                break;
            }

            foreach (var expense in expenses)
            {
                Console.WriteLine($"Id:{expense.Id}, Amount:{expense.Amount}, Category:{expense.Category}, Description:{expense.Description}, Date:{expense.TransactionDate}");
            }
            break;

        case "0":
            Console.WriteLine("Exiting the FinCli GoodBye!..");
            return;

        default:
            Console.WriteLine("Invalid Choice! Please try again.");
            break;
    }
}
using FinCli.Core;
using Spectre.Console;

string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
string appFolderPath = Path.Combine(appDataPath, "FinCli");
Directory.CreateDirectory(appFolderPath);
string dataFilePath = Path.Combine(appFolderPath, "transactions.json");
var service = new TransactionService(dataFilePath);

AnsiConsole.MarkupLine("[bold green]FinCli[/] - Personal Finance Tracker");

while (true)
{
    Console.WriteLine("1. Show Total");
    Console.WriteLine("2. Add Expense");
    Console.WriteLine("3. List Expenses");
    Console.WriteLine("4. Monthly Summary");
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
                AnsiConsole.MarkupLine("[bold red]Invalid amount! Please try again.[/]");
            }
            break;

        case "3":
            var expenses = service.GetAll();
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses yet.");
                break;
            }

            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("AMOUNT");
            table.AddColumn("CATEGORY");
            table.AddColumn("DESCRIPTION");
            table.AddColumn("DATE");

            foreach (var expense in expenses)
            {
                table.AddRow(expense.Id.ToString(), expense.Amount.ToString(), Markup.Escape(expense.Category), Markup.Escape(expense.Description ?? ""), expense.TransactionDate.ToString());
            }

            AnsiConsole.Write(table);
            break;

        case "4":
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;

            decimal monthlyTotal = service.GetTotalByMonth(year, month);

            Console.WriteLine($"Total for {year}/{month}: {monthlyTotal}");
            break;

        case "0":
            Console.WriteLine("Exiting the FinCli GoodBye!..");
            return;

        default:
            AnsiConsole.MarkupLine("[bold red]Invalid Choice! Please try again.[/]");
            break;
    }
}
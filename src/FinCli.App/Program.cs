using FinCli.Core;
using Spectre.Console;

Console.OutputEncoding = System.Text.Encoding.UTF8;

string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
string appFolderPath = Path.Combine(appDataPath, "FinCli");
Directory.CreateDirectory(appFolderPath);
string dataFilePath = Path.Combine(appFolderPath, "transactions.json");
var service = new TransactionService(dataFilePath);

string fontPath = Path.Combine(AppContext.BaseDirectory, "Fonts", "AnsiShadow.flf");
var font = FigletFont.Load(fontPath);

AnsiConsole.Write(
    new FigletText(font, "FinCli")
    .Color(Color.Green));

AnsiConsole.MarkupLine("[grey]Personal Finance Tracker[/]");

Console.WriteLine();

while (true)
{
    string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .AddChoices("Show Total", "Add Expense", "List Expenses", "Monthly Summary", "Exit"));

    switch (choice)
    {
        case "Show Total":
            decimal result = service.GetTotal();
            var totalPanel = new Panel($"Total: {result:C2}")
                .Header("Total")
                .BorderColor(Color.Green)
                .Border(BoxBorder.Rounded);

            AnsiConsole.Write(totalPanel);
            Console.WriteLine();
            break;


        case "Add Expense":
            decimal amount = AnsiConsole.Prompt(
                new TextPrompt<decimal>("Amount:")
                .Validate(value => value > 0, "[bold red]Amount must be greater than zero.[/]"));

            string category = AnsiConsole.Prompt(
                new TextPrompt<string>("Category:")
                .DefaultValue("Other"));

            string description = AnsiConsole.Prompt(
                new TextPrompt<string>("Description:")
                .AllowEmpty());

            var newExpense = service.AddTransaction(amount, category, description);
            AnsiConsole.MarkupLine($"[green]Expense added (Id: {newExpense.Id})[/]");
            Console.WriteLine();
            break;

        case "List Expenses":
            var expenses = service.GetAll();
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses yet.");
                Console.WriteLine();
                break;
            }

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("ID");
            table.AddColumn(new TableColumn("AMOUNT").RightAligned());
            table.AddColumn("CATEGORY");
            table.AddColumn("DESCRIPTION");
            table.AddColumn("DATE");

            foreach (var expense in expenses)
            {
                table.AddRow(expense.Id.ToString(), expense.Amount.ToString("C2"), Markup.Escape(expense.Category), Markup.Escape(expense.Description ?? ""), expense.TransactionDate.ToString());
            }

            AnsiConsole.Write(table);
            Console.WriteLine();
            break;

        case "Monthly Summary":
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;

            decimal monthlyTotal = service.GetTotalByMonth(year, month);
            var monthlySummaryPanel = new Panel($"Total for {year}/{month}: {monthlyTotal:C2}")
                .Header("Monthly Summary")
                .BorderColor(Color.Blue)
                .Border(BoxBorder.Rounded);

            AnsiConsole.Write(monthlySummaryPanel);
            Console.WriteLine();
            break;

        case "Exit":
            Console.WriteLine("Exiting the FinCli GoodBye!..");
            return;
    }
}
using FinCli.Core;
using Spectre.Console;
using System.Globalization;

Console.OutputEncoding = System.Text.Encoding.UTF8;

const string accentHex = "#e0a646";
const string sageHex = "#93b47c";
const string mutedHex = "#8f897d";
const string dangerHex = "#c8604f";

var accent = new Color(224, 166, 70);
var sage = new Color(147, 180, 124);
var dim = new Color(85, 82, 75);

string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
string appFolderPath = Path.Combine(appDataPath, "FinCli");
Directory.CreateDirectory(appFolderPath);
string dataFilePath = Path.Combine(appFolderPath, "transactions.json");
var service = new TransactionService(dataFilePath);

string fontPath = Path.Combine(AppContext.BaseDirectory, "Fonts", "AnsiShadow.flf");
var font = FigletFont.Load(fontPath);

AnsiConsole.Write(
    new FigletText(font, "FinCli")
    .Color(accent));

AnsiConsole.MarkupLine($"[{mutedHex}]Personal Finance Tracker[/]");

Console.WriteLine();

while (true)
{
    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title($"[{mutedHex}]What would you like to do?[/]")
        .HighlightStyle(new Style(foreground: accent))
        .AddChoices("Show Total", "Add Expense", "List Expenses", "Monthly Summary", "Category Chart", "Exit"));

    switch (choice)
    {
        case "Show Total":
            decimal result = service.GetTotal();
            var totalPanel = new Panel($"[{mutedHex}]Total:[/] [bold {accentHex}]{result:C2}[/]")
                .Header($"[{sageHex}]Total[/]")
                .BorderColor(sage)
                .Border(BoxBorder.Rounded);

            AnsiConsole.Write(totalPanel);
            Console.WriteLine();
            break;


        case "Add Expense":
            decimal amount = AnsiConsole.Prompt(
                new TextPrompt<decimal>($"[{mutedHex}]Amount:[/]")
                .Validate(value => value > 0, $"[bold {dangerHex}]Amount must be greater than zero.[/]")
                .Culture(CultureInfo.CurrentCulture));

            string category = AnsiConsole.Prompt(
                new TextPrompt<string>($"[{mutedHex}]Category:[/]")
                .DefaultValue("Other"));

            string description = AnsiConsole.Prompt(
                new TextPrompt<string>($"[{mutedHex}]Description:[/]")
                .AllowEmpty());

            var newExpense = service.AddTransaction(amount, category, description);
            AnsiConsole.MarkupLine($"[{sageHex}]Expense added (Id: {newExpense.Id})[/]");
            Console.WriteLine();
            break;

        case "List Expenses":
            var expenses = service.GetAll();
            if (expenses.Count == 0)
            {
                AnsiConsole.MarkupLine($"[{mutedHex}]No expenses yet.[/]");
                Console.WriteLine();
                break;
            }

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.BorderColor(dim);
            table.AddColumn($"[bold {accentHex}]ID[/]");
            table.AddColumn(new TableColumn($"[bold {accentHex}]AMOUNT[/]").RightAligned());
            table.AddColumn($"[bold {accentHex}]CATEGORY[/]");
            table.AddColumn($"[bold {accentHex}]DESCRIPTION[/]");
            table.AddColumn($"[bold {accentHex}]DATE[/]");

            foreach (var expense in expenses)
            {
                table.AddRow(
                    expense.Id.ToString(),
                    $"[{accentHex}]{expense.Amount:C2}[/]",
                    Markup.Escape(expense.Category),
                    Markup.Escape(expense.Description ?? ""),
                    expense.TransactionDate.ToString());
            }

            AnsiConsole.Write(table);
            Console.WriteLine();
            break;

        case "Monthly Summary":
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;

            decimal monthlyTotal = service.GetTotalByMonth(year, month);
            var monthlySummaryPanel = new Panel($"[{mutedHex}]Total for {year}/{month}:[/] [bold {accentHex}]{monthlyTotal:C2}[/]")
                .Header($"[{accentHex}]Monthly Summary[/]")
                .BorderColor(accent)
                .Border(BoxBorder.Rounded);

            AnsiConsole.Write(monthlySummaryPanel);
            Console.WriteLine();
            break;

        case "Category Chart":
            var categoryTotals = service.GetTotalsByCategory();

            if (categoryTotals.Count == 0)
            {
                AnsiConsole.MarkupLine($"[{mutedHex}]No expenses yet.[/]");
                Console.WriteLine();
                break;
            }

            var categoryExpenseChart = new BarChart()
                .Width(100)
                .UseValueFormatter(value => value.ToString("C2"));

            Color[] barColors = { accent, sage };
            int colorIndex = 0;

            foreach (var entry in categoryTotals.OrderByDescending(entry => entry.Value))
            {
                categoryExpenseChart.AddItem(
                    Markup.Escape(entry.Key),
                    (double)entry.Value,
                    barColors[colorIndex % barColors.Length]);
                colorIndex++;
            }

            var categoryPanel = new Panel(categoryExpenseChart)
                .Header($"[bold {accentHex}]Total by category[/]")
                .BorderColor(accent)
                .Border(BoxBorder.Rounded)
                .Padding(2, 1);

            AnsiConsole.Write(categoryPanel);
            Console.WriteLine();
            break;

        case "Exit":
            AnsiConsole.MarkupLine($"[{mutedHex}]Exiting the FinCli GoodBye!..[/]");
            return;
    }
}
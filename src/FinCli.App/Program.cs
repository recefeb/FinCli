using FinCli.Core;

var service = new TransactionService("deneme.json");

while (true)
{
    Console.WriteLine("1. Show Total");
    Console.WriteLine("0. Exit");
    Console.Write("Choice:  ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            decimal result = service.GetTotal();
            Console.WriteLine($"Total: {result}");
            break;

        case "0":
            Console.WriteLine("Exiting the FinCli GoodBye!..");
            return;

        default:
            Console.WriteLine("Invalid Choice! Please try again.");
            break;
    }
}
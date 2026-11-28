using FinCli.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinCli.Web.Pages;

public class ExpensesModel : PageModel
{
    private readonly TransactionService _service;

    public List<Transaction> Items { get; set; } = new();

    public ExpensesModel(TransactionService service)
    {
        _service = service;
    }
    public void OnGet()
    {
        Items = _service.GetAll();
    }
}

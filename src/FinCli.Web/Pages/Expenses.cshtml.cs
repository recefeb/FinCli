using FinCli.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinCli.Web.Pages;

public class ExpensesModel : PageModel
{
    private readonly TransactionService _service;

    public List<Transaction> Items { get; set; } = new();

    [BindProperty]
    public decimal Amount { get; set; }

    [BindProperty]
    public string Category { get; set; } = "";

    [BindProperty]
    public string? Description { get; set; }

    public ExpensesModel(TransactionService service)
    {
        _service = service;
    }
    public void OnGet()
    {
        Items = _service.GetAll();
    }

    public IActionResult OnPost()
    {
        if (Amount <= 0)
        {
            ModelState.AddModelError(nameof(Amount), "Amount must be greater than zero.");
        }

        if (!ModelState.IsValid)
        {
            Items = _service.GetAll();
            return Page();
        }

        _service.AddTransaction(Amount, Category, Description);

        return RedirectToPage();
    }
}

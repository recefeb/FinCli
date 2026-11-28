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
    public string? Category { get; set; }

    [BindProperty]
    public string? Description { get; set; }

    public ExpensesModel(TransactionService service)
    {
        _service = service;
    }
    public void OnGet()
    {
        Items = LoadItems();
    }

    public IActionResult OnPost()
    {
        if (Amount <= 0)
        {
            ModelState.AddModelError(nameof(Amount), "Amount must be greater than zero.");
        }

        if (!ModelState.IsValid)
        {
            Items = LoadItems();
            return Page();
        }

        _service.AddTransaction(Amount, Category ?? "", Description);

        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        _service.DeleteTransaction(id);

        return RedirectToPage();
    }

    private List<Transaction> LoadItems()
    {
        return _service.GetAll()
            .OrderByDescending(t => t.TransactionDate)
            .ThenByDescending(t => t.Id)
            .ToList();
    }

}

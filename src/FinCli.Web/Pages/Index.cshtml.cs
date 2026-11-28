using FinCli.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinCli.Web.Pages;

public class IndexModel : PageModel
{
    private readonly TransactionService _service;

    public decimal Total { get; set; }

    public Dictionary<string, decimal> CategoryTotals { get; set; } = new();

    public IndexModel(TransactionService service)
    {
        _service = service;
    }

    public void OnGet()
    {
        Total = _service.GetTotal();
        CategoryTotals = _service.GetTotalsByCategory();
    }
}

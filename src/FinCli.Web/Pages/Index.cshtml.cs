using FinCli.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinCli.Web.Pages;

public class IndexModel : PageModel
{
    private readonly TransactionService _service;

    public decimal Total { get; set; }

    public IndexModel(TransactionService service)
    {
        _service = service;
    }

    public void OnGet()
    {
        Total = _service.GetTotal();
    }
}

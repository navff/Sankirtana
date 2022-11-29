using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Business.Sales;

namespace Sankirtana.Web.Pages.Sales;

public class EditSale : PageModel
{
    public Sale Sale { get; set; }
    public List<Book> BookList;
    
    private readonly BookService _bookService;
    private readonly SalesService _salesService;

    public EditSale(SalesService salesService, BookService bookService)
    {
        _salesService = salesService;
        _bookService = bookService;
    }

    public async Task OnGet(string id)
    {
        this.BookList = await _bookService.GetBooks();
        this.Sale = await _salesService.GetById(id);
    }
    
    public async Task<IActionResult> OnPost(SaleUpdateViewModel saleUpdateViewModel)
    {
        var sale = await _salesService.GetById(saleUpdateViewModel.Id);
        var book = await _bookService.GetById(saleUpdateViewModel.BookId);
        sale.Book = book;
        sale.Count = saleUpdateViewModel.Count;
        await _salesService.EditSale(sale);
        return RedirectToPage("/Sales/AddSale");
    }
}
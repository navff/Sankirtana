using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Business.PortalUsers;
using Sankirtana.Web.Business.Sales;

namespace Sankirtana.Web.Pages.Sales;

public class AddSale : PageModel
{
    public List<Book> BookList;
    public List<Sale> Sales;
    
    private BookService _bookService;
    private SalesService _salesService;
    private PortalUserService _portalUserService;
    
    public AddSale(BookService bookService, 
                   SalesService salesService, 
                   PortalUserService portalUserService)
    {
        _bookService = bookService;
        _salesService = salesService;
        _portalUserService = portalUserService;
    }

    public async Task OnGet()
    {
        this.BookList = await _bookService.GetBooks();
        this.Sales = await _salesService.GetSales();
    }

    public async Task<IActionResult> OnPost(SaleUpdateViewModel viewModel)
    {
        var book = await _bookService.GetById(viewModel.BookId);
        var user = await _portalUserService.GetById("63005e8fbe776597bb4b48b8"); // TODO: получить из сессии

        var sale = new Sale()
        {
            Book = book,
            User = new PortaUserShort() {City = user.City, Id = user.Id, Name = user.Name},
            Date = DateTime.Now,
            ContactName = viewModel.ContactName,
            ContactPhone = viewModel.ContactPhone
        };
        
        await _salesService.AddSale(sale);
        return RedirectToPage("/Sales/AddSale");
    }
}
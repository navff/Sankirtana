using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Business.PortalUsers;
using Sankirtana.Web.Business.Sales;
using Sankirtana.Web.Common.Helpers;

namespace Sankirtana.Web.Pages.Sales;

[Authorize]
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
        this.Sales = await _salesService.GetSalesByUser(
            User.GetId(), 
            DateTime.Today);
    }

    public async Task<IActionResult> OnPost(SaleUpdateViewModel viewModel)
    {
        var book = await _bookService.GetById(viewModel.BookId);
        var user = await _portalUserService.GetById(User.GetId());

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
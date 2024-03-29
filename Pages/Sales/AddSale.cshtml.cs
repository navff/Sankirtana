﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Business.PortalUsers;
using Sankirtana.Web.Business.Sales;
using Sankirtana.Web.Common;

namespace Sankirtana.Web.Pages.Sales;

[RESTAuthorize]
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
            (User.Identity as Identity).Id, 
            DateTime.Today);
    }

    public async Task<IActionResult> OnPost(SaleUpdateViewModel viewModel)
    {
        var book = await _bookService.GetById(viewModel.BookId);
        var user = await _portalUserService.GetById((User.Identity as Identity).Id);
        DateTime date = viewModel.Date.LocalDateTime; 
        DateTime.SpecifyKind(date, DateTimeKind.Utc);
        date = date.Add(viewModel.Date.Offset);
        
        var sale = new Sale()
        {
            Book = book,
            User = new PortalUserShort() {City = user.City, Id = user.Id, Name = user.Name},
            Date = date, // viewModel.Date.ToLocalTime().DateTime,
            ContactName = viewModel.ContactName,
            ContactPhone = viewModel.ContactPhone,
            Count = 1
        };
        
        await _salesService.AddSale(sale);
        return RedirectToPage("/Sales/AddSale");
    }
}
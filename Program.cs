using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Sankirtana.Web.Business.Books;
using Sankirtana.Web.Business.PortalUsers;
using Sankirtana.Web.Business.Sales;
using Sankirtana.Web.Common;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
port = port ?? "8080";
builder.WebHost.UseUrls($"http://localhost:{port}");

var directory = Directory.GetCurrentDirectory();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(cookieOptions => {
    cookieOptions.LoginPath = "/auth/NotAllowed";
    cookieOptions.AccessDeniedPath = "/auth/NotAllowed";
    });

builder.Services
    .AddRazorPages(options =>
    {
        // отключаем глобально Antiforgery-токен
        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    })
    .AddRazorRuntimeCompilation(options => options.FileProviders.Add(new PhysicalFileProvider(directory)));

builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic }));
builder.Services.AddSingleton<EnvironmentConfig>();
builder.Services.AddSingleton<DbStore>();
builder.Services.AddTransient<BookService>();
builder.Services.AddTransient<PortalUserService>();
builder.Services.AddTransient<SalesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapRazorPages();

app.Run();
using Microsoft.EntityFrameworkCore;
using Playon24.BusinessLayer.Modules.Customers;
using Playon24.BusinessLayer.Modules.Customers.Interface;
using Playon24.BusinessLayer.Modules.Dashboard;
using Playon24.BusinessLayer.Modules.Dashboard.Interface;
using Playon24.BusinessLayer.Modules.Files;
using Playon24.BusinessLayer.Modules.Files.Interface;
using Playon24.BusinessLayer.Modules.Invoices;
using Playon24.BusinessLayer.Modules.Invoices.Interface;
using Playon24.BusinessLayer.Modules.Products;
using Playon24.BusinessLayer.Modules.Products.Interface;
using Playon24.DataAccessLayer.Data;
using Playon24.DataAccessLayer.Modules.Customers;
using Playon24.DataAccessLayer.Modules.Customers.Interfaces;
using Playon24.DataAccessLayer.Modules.Dashboard;
using Playon24.DataAccessLayer.Modules.Dashboard.Interfaces;
using Playon24.DataAccessLayer.Modules.Invoices;
using Playon24.DataAccessLayer.Modules.Invoices.Interfaces;
using Playon24.DataAccessLayer.Modules.Products;
using Playon24.DataAccessLayer.Modules.Products.Interfaces;
using Playon24.PresentationLayer.Modules.Customers;
using Playon24.PresentationLayer.Modules.Customers.Interface;
using Playon24.PresentationLayer.Modules.Dashboard;
using Playon24.PresentationLayer.Modules.Dashboard.Interface;
using Playon24.PresentationLayer.Modules.Invoices;
using Playon24.PresentationLayer.Modules.Invoices.Interface;
using Playon24.PresentationLayer.Modules.Products;
using Playon24.PresentationLayer.Modules.Products.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Payon24DbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IFileService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return new FileService(env.WebRootPath);
});

builder.Services.AddScoped<IProductViewModelProvider, ProductViewModelProvider>();
builder.Services.AddScoped<ICustomerViewModelProvider, CustomerViewModelProvider>();
builder.Services.AddScoped<IInvoiceViewModelProvider, InvoiceViewModelProvider>();
builder.Services.AddScoped<IDashboardViewModelProvider, DashboardViewModelProvider>();

IServiceCollection serviceCollection = builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

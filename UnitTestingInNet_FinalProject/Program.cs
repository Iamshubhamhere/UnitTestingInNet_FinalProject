using Microsoft.EntityFrameworkCore;
using UnitTestingInNet_FinalProject.Data;
using UnitTestingInNet_FinalProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("ECommerceContextConnection") ?? throw new InvalidOperationException("Connection string  not found.");

builder.Services.AddDbContext<EcommerceContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddScoped(typeof(IRepository<Product>), typeof(ProductRepository));
builder.Services.AddScoped(typeof(ICartRepository<Cart>), typeof(CartRepository));
builder.Services.AddScoped(typeof(IRepository<ProductCart>), typeof(ProductCartRepository));
builder.Services.AddScoped(typeof(IRepository<Country>), typeof(CountryRepository));
builder.Services.AddScoped(typeof(IRepository<Order>), typeof(OrderRepository));

var app = builder.Build();
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider serviceProvider = scope.ServiceProvider;

    await SeedData.Initialize(serviceProvider);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Store}/{action=Index}");

app.Run();

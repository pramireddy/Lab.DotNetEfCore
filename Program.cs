using Lab.DotNetEfCore._Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            //.AddDbContext<AppDbContext>(options =>
            //    options.UseInMemoryDatabase("InMemoryDb")) // Note. InMemory does not support to run db commands in transaction
            .AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("YourConnectionString"))
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<IProductDataService, ProductDataService>() // Register Interface
            .BuildServiceProvider();

        var productService = serviceProvider.GetRequiredService<IProductDataService>();

        // Add Product Using Service Layer
        var newProduct = new Product { Id = 1, Name = "Laptop", Price = 1200 };
        await productService.AddProductAsync(newProduct);

        // Fetch Products Using Service Layer
        var products = await productService.GetAllProductsAsync();
        foreach (var product in products)
        {
            Console.WriteLine($"Product: {product.Name}, Price: {product.Price}");
        }
    }
}
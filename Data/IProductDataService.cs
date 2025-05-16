namespace Lab.DotNetEfCore.Data
{
    public interface IProductDataService
    {
        Task AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}


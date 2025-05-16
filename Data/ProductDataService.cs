namespace Lab.DotNetEfCore.Data
{
    public class ProductDataService : IProductDataService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductDataService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
    }
}


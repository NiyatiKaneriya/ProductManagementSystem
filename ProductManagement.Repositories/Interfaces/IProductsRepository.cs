using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
namespace ProductManagement.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        public List<City> GetCites();
        public bool AddEditProduct(ProductDetails productAddEdit);
        public List<ProductDetails> GetAllProducts(ProductListParams listParams);
        public ProductDetails GetProductDetail(int productId);
        public bool DeleteProduct(int productId);
        public int GetAllProductsCount(ProductListParams listParams);
    }
}

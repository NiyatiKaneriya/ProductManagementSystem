using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        public List<City> GetCites();
        public bool AddEditProduct(ProductAddEdit productAddEdit);
        public List<ProductAddEdit> GetAllProducts(int categoryfilter, string generalSearch, string multiselect, string columnName, string sorttype, int page = 1, int pageSize = 5);
        public ProductAddEdit GetProductDetail(int productId);
        public bool DeleteProduct(int productId);
        public int GetAllProductsCount(int categoryfilter, string generalSearch, string multiselect);
    }
}

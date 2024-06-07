using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public bool AddEditCategory(CategoryDetails categoryAddEdit);
        public List<CategoryDetails> GetAllCategories(int page, int pageSize);
        public bool DeleteCategory(int categoryId);
        public CategoryDetails GetCategoryDetails(int categoryId);
        public int GetAllCategoriesCount();
        public List<CategoryDetails> GetCategories();

    }
}

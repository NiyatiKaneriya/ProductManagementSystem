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
        public bool AddEditCategory(CategoryAddEdit categoryAddEdit);
        public List<CategoryAddEdit> GetAllCategories();
        public bool DeleteCategory(int categoryId);
        public CategoryAddEdit GetCategoryDetails(int categoryId);

    }
}

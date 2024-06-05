using ProductManagement.Entities.DataContext;
using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
using ProductManagement.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddEditCategory(CategoryAddEdit categoryAddEdit)
        {
            try
            {

                Category category = categoryAddEdit.CategoryId != default ? _context.Categories.First(e => e.CategoryId == categoryAddEdit.CategoryId) : new Category();
                category.CategoryName = categoryAddEdit.CategoryName;
                category.Sequence = categoryAddEdit.Sequence;

                InsertOrUpdateCategory(category);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void InsertOrUpdateCategory(Category category)
        {
            try
            {
                if (category.CategoryId != default)
                {
                    category.ModifiedDate = DateTime.Now;
                    _context.Categories.Update(category);
                }
                else
                {
                    category.CreatedDate = DateTime.Now;
                    category.IsDeleted = false;
                    _context.Categories.Add(category);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool DeleteCategory(int categoryId)
        {
            try
            {
                Category category = _context.Categories.First(e => e.CategoryId == categoryId);
                List<Product> product = _context.Products.Where(e => e.CategoryId == categoryId).ToList();
                if (product.Count != 0)
                {
                    return false;
                }
                if (category != null && product.Count == 0)
                {
                    category.IsDeleted = true;
                    InsertOrUpdateCategory(category);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CategoryAddEdit> GetAllCategories()
        {
            try
            {
                List<CategoryAddEdit> data = (from c in _context.Categories
                                              where !c.IsDeleted
                                              orderby c.Sequence, c.CategoryId ascending
                                              select new CategoryAddEdit
                                              {
                                                  CategoryId = c.CategoryId,
                                                  CategoryName = c.CategoryName,
                                                  TotalProducts = _context.Products.Where(x => x.CategoryId == c.CategoryId).Count(),
                                                  Sequence = c.Sequence,
                                              }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CategoryAddEdit GetCategoryDetails(int CategoryId)
        {
            try
            {
                Category categoryDetail = _context.Categories.Where(e => e.CategoryId == CategoryId).First();
                CategoryAddEdit data = new CategoryAddEdit
                {
                    CategoryId = categoryDetail.CategoryId,
                    CategoryName = categoryDetail.CategoryName,
                    Sequence = categoryDetail.Sequence
                };

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}

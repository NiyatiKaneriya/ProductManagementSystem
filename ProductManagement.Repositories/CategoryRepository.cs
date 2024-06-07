﻿using ProductManagement.Entities.DataContext;
using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
using ProductManagement.Repositories.Interfaces;

namespace ProductManagement.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Add And Edit category
        /// <summary>
        /// Add and Edit Category
        /// </summary>
        /// <param name="categoryAddEdit"></param>
        /// <returns></returns>
        public bool AddEditCategory(CategoryDetails categoryAddEdit)
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
        #endregion

        #region Generic Function for Insert or Update Category
        /// <summary>
        /// Generic Function for Insert or Update Category
        /// </summary>
        /// <param name="category"></param>
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
                    _context.Categories.Add(category);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion


        #region Delete Category
        /// <summary>
        /// Delete category only if no products are avilable in this category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
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
                    category.DeletedAt = DateTime.Now;
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
        #endregion
        #region Get All Category for displaing category List
        /// <summary>
        /// Get All Category for displaing category List
        /// </summary>
        /// <returns></returns>
        public List<CategoryDetails> GetAllCategories(int page, int pageSize)
        {
            try
            {
                List<CategoryDetails> data = (from c in _context.Categories
                                              where !c.DeletedAt.HasValue
                                              orderby c.Sequence, c.CategoryId ascending
                                              select new CategoryDetails
                                              {
                                                  CategoryId = c.CategoryId,
                                                  CategoryName = c.CategoryName,
                                                  TotalProducts = _context.Products.Where(x => x.CategoryId == c.CategoryId).Count(),
                                                  Sequence = c.Sequence,
                                              }).ToList();
                List<CategoryDetails> paginatedData = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return paginatedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int GetAllCategoriesCount()
        {
            try
            {
                List<CategoryDetails> data = (from c in _context.Categories
                                              where !c.DeletedAt.HasValue
                                              orderby c.Sequence, c.CategoryId ascending
                                              select new CategoryDetails
                                              {
                                                  CategoryId = c.CategoryId,
                                                  CategoryName = c.CategoryName,
                                                  TotalProducts = _context.Products.Where(x => x.CategoryId == c.CategoryId).Count(),
                                                  Sequence = c.Sequence,
                                              }).ToList();
                
                return data.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<CategoryDetails> GetCategories()
        {
            try
            {
                List<CategoryDetails> data = (from c in _context.Categories
                                              where !c.DeletedAt.HasValue
                                              orderby c.Sequence, c.CategoryId ascending
                                              select new CategoryDetails
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
        #endregion
        #region GetCategoryDetails for edit category
        /// <summary>
        /// GetCategoryDetails for edit category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public CategoryDetails GetCategoryDetails(int CategoryId)
        {
            try
            {
                Category categoryDetail = _context.Categories.Where(e => e.CategoryId == CategoryId).First();
                CategoryDetails data = new CategoryDetails
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
        #endregion
    }
}

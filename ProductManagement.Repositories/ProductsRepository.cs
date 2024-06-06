using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductManagement.Entities.DataContext;
using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
using ProductManagement.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductManagement.Repositories
{
    
    public class ProductsRepository : IProductsRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #region UploadImage
        /// <summary>
        /// function for image upload
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public bool UploadProductImage(ProductDetails model, int ProductId)
        {

            if (model.ProductImage != null)
            {
                string FilePath = "wwwroot\\UploadedProductImage\\" + ProductId;
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path,true);
                    Directory.CreateDirectory(path);
                }
                else
                {
                    Directory.CreateDirectory(path);
                }
                   
                
                if (model.ProductImage != null)
                {
                    string newfilename = $"{"ProductIamge"}.{Path.GetExtension(model.ProductImage.FileName).Trim('.')}";
                    string fileNameWithPath = Path.Combine(path, newfilename);
                    model.FilePath = FilePath.Replace("wwwroot\\UploadedProductImage\\", "/UploadedProductImage/") + "/" + newfilename;
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.ProductImage.CopyTo(stream);
                    }
                    Product product = _context.Products.FirstOrDefault(e => e.ProductId == ProductId);
                    if (product != null)
                    {
                        product.FilePath = model.FilePath;
                        _context.Products.Update(product);
                        _context.SaveChanges();
                    }
                }
                return true;
            }

            return false;
        }
        #endregion

        #region Add Edit Product

        /// <summary>
        /// Adds the edit product.
        /// </summary>
        /// <param name="productAddEdit">The product add edit.</param>
        /// <returns></returns>
        public bool AddEditProduct(ProductDetails productAddEdit)
        {
            try
            {
                Product product = productAddEdit.ProductId != default ? _context.Products.First(e => e.ProductId == productAddEdit.ProductId) : new Product();
                product.ProductName = productAddEdit.ProductName;
                product.CategoryId = productAddEdit.CategoryId;
                product.Price = productAddEdit.Price;
                product.SupplierName = productAddEdit.SupplierName;
                product.SupplierEmail = productAddEdit.SupplierEmail;
                product.SupplierPhoneNo = productAddEdit.SupplierPhoneNo;
                product.ProductDescription = productAddEdit.ProductDescription;
                product.AvailableFrom = productAddEdit.AvailableFrom;
                product.ProductWebsite = productAddEdit.ProductWebsite;
                product.IsActive = productAddEdit.IsActive;
                product.AvailableAt = productAddEdit.AvailableAt;

                InsertOrUpdate(product);

                UploadProductImage(productAddEdit, product.ProductId);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        #endregion

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="product">The product.</param>
        public void InsertOrUpdate(Product product)
        {
            try
            {
                if (product.ProductId != default)
                {
                    product.ModifiedDate = DateTime.Now;
                    _context.Products.Update(product);
                }
                else
                {
                    product.CreatedDate = DateTime.Now;
                    _context.Products.Add(product);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region Product detail for edit product

        /// <summary>
        /// Gets the product detail.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        public ProductDetails GetProductDetail(int ProductId)
        {
            try
            {
                Product product = _context.Products.First(e => e.ProductId == ProductId);
                ProductDetails productAddEdit = new ProductDetails
                {
                    ProductId = ProductId,
                    ProductName = product.ProductName,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    SupplierName = product.SupplierName,
                    SupplierEmail = product.SupplierEmail,
                    SupplierPhoneNo = product.SupplierPhoneNo,
                    ProductDescription = product.ProductDescription,
                    AvailableFrom = (DateTime)product.AvailableFrom,
                    ProductWebsite = product.ProductWebsite,
                    FilePath = product.FilePath,
                    IsActive = product.IsActive,
                    AvailableAtcity = product.AvailableAt.Split(',').Select(int.Parse).ToList()
                };
                return productAddEdit;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Productlist for display table

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <param name="categoryfilter">The categoryfilter.</param>
        /// <param name="generalSearch">The general search.</param>
        /// <param name="multiselect">The multiselect.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="sorttype">The sorttype.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public List<ProductDetails> GetAllProducts(ProductListParams listParams)
        {
            try
            {
                List<ProductDetails> products = (from product in _context.Products
                                                 where (!product.DeletedAt.HasValue
                                                 && (product.CategoryId == listParams.Categoryfilter || listParams.Categoryfilter == 0)
                                                 && (listParams.GeneralSearch == null || (product.ProductName.ToLower().Contains(listParams.GeneralSearch.ToLower())
                                                 || product.SupplierName.ToLower().Contains(listParams.GeneralSearch.ToLower())
                                                 || product.ProductDescription.ToLower().Contains(listParams.GeneralSearch.ToLower())
                                                 || product.SupplierEmail.ToLower().Contains(listParams.GeneralSearch.ToLower())
                                                 || product.ProductWebsite.ToLower().Contains(listParams.GeneralSearch.ToLower()))
                                                 || product.Price.ToString().Contains(listParams.GeneralSearch)
                                                 || product.AvailableFrom.ToString().Contains(listParams.GeneralSearch)))
                                                 select new ProductDetails
                                                 {
                                                     ProductId = product.ProductId,
                                                     ProductName = product.ProductName,
                                                     ProductDescription = product.ProductDescription,
                                                     CategoryId = product.CategoryId,
                                                     Category = _context.Categories.First(e => e.CategoryId == product.CategoryId).CategoryName,
                                                     ProductWebsite = product.ProductWebsite,
                                                     IsActive = product.IsActive,
                                                     Price = product.Price,
                                                     SupplierName = product.SupplierName,
                                                     SupplierEmail = product.SupplierEmail,
                                                     AvailableFrom = (DateTime)product.AvailableFrom,
                                                     AvailableAt = product.AvailableAt,
                                                     FilePath = product.FilePath,
                                                     AvailableAtIds = product.AvailableAt,
                                                 }).ToList();
                if (!string.IsNullOrWhiteSpace(listParams.Multiselectlist))
                {
                    List<int> availableCities = listParams.Multiselectlist.Split(',').Select(int.Parse).ToList();
                    products = (from pro in products.Where(a => !string.IsNullOrWhiteSpace(a.AvailableAt))
                                let sadf = pro.AvailableAt.Split(",").Select(int.Parse).ToList()
                                where sadf.Any(availableCities.Contains)
                                select pro
                              ).ToList();
                }

                List<City> cities = _context.Cities.ToList();
                foreach (ProductDetails item in products)
                {
                    List<int> cityIds = item.AvailableAtIds.Split(',').Select(int.Parse).ToList();
                    List<string> availablecities = cities.Where(e => cityIds.Contains(e.CityId)).Select(e => e.CityName).ToList();
                    item.AvailableAt = string.Join(",", availablecities);
                }

                List<ProductDetails> orderdlist = new List<ProductDetails>();

                if (listParams.Sorttype == "Asce")
                {
                    orderdlist = listParams.ColumnName switch
                    {
                        "ProductId" => products.OrderBy(c => c.ProductId).ToList(),
                        "ProductName" => products.OrderBy(c => c.ProductName).ToList(),
                        "Category" => products.OrderBy(c => c.Category).ToList(),
                        "Price" => products.OrderBy(c => c.Price).ToList(),
                        "SupplierName" => products.OrderBy(c => c.SupplierName).ToList(),
                        "SupplierEmail" => products.OrderBy(c => c.SupplierEmail).ToList(),
                        "AvailableFrom" => products.OrderBy(c => c.AvailableFrom).ToList(),
                        _ => products.OrderBy(c => c.ProductId).ToList(),
                    };
                }
                else
                {
                    orderdlist = listParams.ColumnName switch
                    {
                        "ProductId" => products.OrderByDescending(c => c.ProductId).ToList(),
                        "ProductName" => products.OrderByDescending(c => c.ProductName).ToList(),
                        "Category" => products.OrderByDescending(c => c.Category).ToList(),
                        "Price" => products.OrderByDescending(c => c.Price).ToList(),
                        "SupplierName" => products.OrderByDescending(c => c.SupplierName).ToList(),
                        "SupplierEmail" => products.OrderByDescending(c => c.SupplierEmail).ToList(),
                        "AvailableFrom" => products.OrderByDescending(c => c.AvailableFrom).ToList(),
                        _ => products.OrderByDescending(c => c.ProductId).ToList(),
                    };
                }

                List<ProductDetails> distinctItems = orderdlist.GroupBy(x => x.ProductId).Select(y => y.First()).ToList();
                List<ProductDetails> paginatedData = distinctItems.Skip((listParams.Page - 1) * listParams.PageSize).Take(listParams.PageSize).ToList();
                return paginatedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets all products count.
        /// </summary>
        /// <param name="categoryfilter">The categoryfilter.</param>
        /// <param name="generalSearch">The general search.</param>
        /// <param name="multiselect">The multiselect.</param>
        /// <returns></returns>
        public int GetAllProductsCount(ProductListParams listParams)
        {
            try
            {

                List<ProductDetails> products = (from product in _context.Products
                                                 where (!product.DeletedAt.HasValue
                                                 && (product.CategoryId == listParams.Categoryfilter || listParams.Categoryfilter == 0)
                                                 && (listParams.GeneralSearch == null ||
                                                 (product.ProductName.ToLower().Contains(listParams.GeneralSearch.ToLower())
                                                    || product.SupplierName.ToLower().Contains(listParams.GeneralSearch.ToLower())
                                                    || product.ProductDescription.ToLower().Contains(listParams.GeneralSearch.ToLower())
                                                    || product.SupplierEmail.ToLower().Contains(listParams.GeneralSearch.ToLower())
                                                    || product.ProductWebsite.ToLower().Contains(listParams.GeneralSearch.ToLower()))
                                                    || product.Price.ToString().Contains(listParams.GeneralSearch)
                                                    || product.AvailableFrom.ToString().Contains(listParams.GeneralSearch)))
                                                 select new ProductDetails
                                                 {
                                                     ProductId = product.ProductId,
                                                     AvailableAt = product.AvailableAt,
                                                 }).ToList();
                if (!string.IsNullOrWhiteSpace(listParams.Multiselectlist))
                {
                    List<int> availableCities = listParams.Multiselectlist.Split(',').Select(int.Parse).ToList();
                    products = (from pro in products.Where(a => !string.IsNullOrWhiteSpace(a.AvailableAt))
                                let sadf = pro.AvailableAt.Split(",").Select(int.Parse).ToList()
                                where sadf.Any(availableCities.Contains)
                                select pro
                              ).ToList();
                }

                var distinctItems = products.GroupBy(x => x.ProductId).Select(y => y.First());
                return distinctItems.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        #endregion

        #region Delete Product

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="ProductId">The product identifier.</param>
        /// <returns></returns>
        public bool DeleteProduct(int ProductId)
        {
            try
            {
                Product product = _context.Products.First(p => p.ProductId == ProductId);
                product.DeletedAt = DateTime.Now;
                InsertOrUpdate(product);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetCities

        /// <summary>
        /// Gets the cites.
        /// </summary>
        /// <returns></returns>
        public List<City> GetCites()
        {
            try
            {
                return _context.Cities.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}

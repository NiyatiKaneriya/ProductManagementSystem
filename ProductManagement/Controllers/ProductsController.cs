using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
using ProductManagement.Models;
using ProductManagement.Repositories.Interfaces;
using System.Diagnostics;
using System.Drawing;

namespace ProductManagement.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly INotyfService _notyf;
        public ProductsController(ICategoryRepository categoryRepository, IProductsRepository productsRepository, INotyfService notyf)
        {
            _categoryRepository = categoryRepository;
            _productsRepository = productsRepository;
            _notyf = notyf;
        }
        public IActionResult Index(int? CategoryId)
        {
            ViewBag.Category = _categoryRepository.GetAllCategories();
            ViewBag.Cities = _productsRepository.GetCites();
            ViewBag.CategoryId = CategoryId;
            return View();
        }
        public IActionResult GetProductsByFilter(int categoryfilter, string generalSearch, string multiselectlist, string columnName, string sorttype, int page = 1, int pageSize = 5)
        {
            
                ViewBag.CategoryId = null;
                int totalCount = _productsRepository.GetAllProductsCount(categoryfilter, generalSearch, multiselectlist);
                int totalpages = (int)Math.Ceiling(totalCount / (double)pageSize);

                ProductAddEdit data = new ProductAddEdit
                {
                    Products = _productsRepository.GetAllProducts(categoryfilter, generalSearch, multiselectlist, columnName, sorttype, page, pageSize)
                };
                ViewBag.TotalPages = totalpages;
                ViewBag.CurrentPage = page;
                return PartialView("_ProductsList", data);
            
        }
        public IActionResult ProductAddEdit(int? ProductId)
        {
            ViewBag.Category = _categoryRepository.GetAllCategories();
            ViewBag.Cities = _productsRepository.GetCites();
            if (ProductId.HasValue)
            {
                ProductAddEdit data = _productsRepository.GetProductDetail(ProductId.Value);
                return View(data);
            }
            return View();
        }
        public IActionResult AddEditProduct(ProductAddEdit productAddEdit)
        {
            if (_productsRepository.AddEditProduct(productAddEdit))
            {
                _notyf.Success("procduct added ...");
            }
            else
            {
                _notyf.Error("procduct not added ...");
            }
            
            
            return RedirectToAction("Index");
        }
        public IActionResult DeleteProduct(int ProductId)
        {
            if (_productsRepository.DeleteProduct(ProductId))
            {
                _notyf.Success("procduct deleted ...");
            }
            else
            {
                _notyf.Error("product not deleted ...");
            }
            return RedirectToAction("Index");
        }
    }
}
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
        public IActionResult GetProductsByFilter(ProductListParams listParams)
        {

            ViewBag.CategoryId = null;
            int totalCount = _productsRepository.GetAllProductsCount(listParams);
            int totalpages = (int)Math.Ceiling(totalCount / (double)listParams.PageSize);

            ProductDetails data = new ProductDetails
            {
                Products = _productsRepository.GetAllProducts(listParams)
            };
            ViewBag.TotalPages = totalpages;
            ViewBag.CurrentPage = listParams.Page;
            return PartialView("_ProductsList", data);

        }



        public IActionResult ProductAddEdit(int? ProductId)
        {
            ViewBag.Category = _categoryRepository.GetAllCategories();
            ViewBag.Cities = _productsRepository.GetCites();
            if (ProductId.HasValue)
            {
                ProductDetails data = _productsRepository.GetProductDetail(ProductId.Value);
                return View(data);
            }
            return View();
        }
        public IActionResult AddEditProduct(ProductDetails productAddEdit)
        {
            if (_productsRepository.AddEditProduct(productAddEdit))
            {
                if (productAddEdit.ProductId != default)
                {
                    _notyf.Success(Constant.ProductEditSuccess);
                }
                else
                {
                    _notyf.Success(Constant.ProductAddSuccess);
                }
            }
            else
            {
                if (productAddEdit.ProductId != default)
                {
                    _notyf.Error(Constant.ProductEditError);
                }
                else
                {
                    _notyf.Error(Constant.ProductAddError);
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult DeleteProduct(int ProductId)
        {
            if (_productsRepository.DeleteProduct(ProductId))
            {
                _notyf.Success(Constant.ProductDeleteSuccess);
            }
            else
            {
                _notyf.Error(Constant.ProductDeleteError);
            }
            return RedirectToAction("Index");
        }
    }
}
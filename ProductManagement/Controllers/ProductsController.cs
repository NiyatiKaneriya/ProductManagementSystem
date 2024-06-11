using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities.ViewModels;
using ProductManagement.Repositories.Interfaces;

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
            ViewBag.Category = _categoryRepository.GetCategories();
            ViewBag.Cities = _productsRepository.GetCites();
            ViewBag.CategoryId = CategoryId;
            return View();
        }

        public IActionResult GetProductsByFilter(ProductListParams listParams)
        {
            ViewBag.CategoryId = null;
            int totalCount = _productsRepository.GetAllProductsCount(listParams);
            int totalpages = (int)Math.Ceiling(totalCount / (double)listParams.PageSize);

            ProductDetails productDetails = new ProductDetails
            {
                Products = _productsRepository.GetAllProducts(listParams)
            };
            ViewBag.TotalPages = totalpages;
            ViewBag.CurrentPage = listParams.Page;
            return PartialView("_ProductsList", productDetails);
        }

        //for display view
        public IActionResult ProductAddEdit(int? ProductId)
        {
            ViewBag.Category = _categoryRepository.GetCategories();
            ViewBag.Cities = _productsRepository.GetCites();
            if (ProductId.HasValue)
            {
                ProductDetails data = _productsRepository.GetProductDetail(ProductId.Value);
                return View(data);
            }
            return View();
        }

        // for the form submission
        public IActionResult AddEditProduct(ProductDetails productAddEdit)
        {
            if (_productsRepository.AddEditProduct(productAddEdit))
            {
                _notyf.Success(productAddEdit.ProductId != default ? Constant.ProductEditSuccess : Constant.ProductAddSuccess);
            }
            else
            {
                _notyf.Error(productAddEdit.ProductId != default ? Constant.ProductEditError : Constant.ProductAddError);
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
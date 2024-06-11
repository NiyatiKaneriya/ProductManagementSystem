using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities.ViewModels;
using ProductManagement.Repositories.Interfaces;

namespace ProductManagement.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotyfService _notyf;

        public CategoryController(ICategoryRepository categoryRepository, INotyfService notyf)
        {
            _categoryRepository = categoryRepository;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCategoryByFilter(int page, int pageSize)
        {
            int totalCount = _categoryRepository.GetAllCategoriesCount();
            int totalpages = (int)Math.Ceiling(totalCount / (double)pageSize);

            CategoryDetails categoryDetails = new CategoryDetails
            {
                Categories = _categoryRepository.GetAllCategories(page, pageSize)
            };
            ViewBag.TotalPages = totalpages;
            ViewBag.CurrentPage = page;
            return PartialView("_CategoryList", categoryDetails);

        }

        public IActionResult CategoryAddEdit(int? CategoryId)
        {
            if (!CategoryId.HasValue)
            {
                return View();
            }

            CategoryDetails categoryDetails = _categoryRepository.GetCategoryDetails(CategoryId.Value);
            return View(categoryDetails);
        }

        public IActionResult AddEditCategory(CategoryDetails category)
        {
            if (_categoryRepository.AddEditCategory(category))
            {
                _notyf.Success(category.CategoryId != default ? Constant.CategoryEditSuccess : Constant.CategoryAddSuccess);
            }
            else
            {
                _notyf.Error(category.CategoryId != default ? Constant.CategoryEditError : Constant.CategoryAddError);
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteCategory(int CategoryId)
        {
            if (_categoryRepository.DeleteCategory(CategoryId))
            {
                _notyf.Success(Constant.DeleteCategorySuccess);
            }
            else
            {
                _notyf.Error(Constant.DeleteCategoryError);
            }

            return RedirectToAction("Index");
        }
    }
}
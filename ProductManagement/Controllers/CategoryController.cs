using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
using ProductManagement.Models;
using ProductManagement.Repositories;
using ProductManagement.Repositories.Interfaces;
using System.Diagnostics;

namespace ProductManagement.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotyfService _notyf;

        public CategoryController(ICategoryRepository categoryRepository,INotyfService notyf)
        {
            _categoryRepository = categoryRepository;
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            //CategoryDetails data = new CategoryDetails
            //{
            //    Categories = _categoryRepository.GetCategories()
            //};
            return View();
        }
        public IActionResult GetCategoryByFilter(int page , int pageSize)
        {

            
            int totalCount = _categoryRepository.GetAllCategoriesCount();
            int totalpages = (int)Math.Ceiling(totalCount / (double)pageSize);

            CategoryDetails data = new CategoryDetails
            {
                Categories = _categoryRepository.GetAllCategories(page,pageSize)
            };
            ViewBag.TotalPages = totalpages;
            ViewBag.CurrentPage = page;
            return PartialView("_CategoryList", data);

        }
        public IActionResult CategoryAddEdit(int? CategoryId)
        {
            if(!CategoryId.HasValue)
            {
                return View();
            }
            else
            {
                CategoryDetails data = _categoryRepository.GetCategoryDetails(CategoryId.Value);
                return View(data);
            }
           
        }
        public IActionResult AddEditCategory(CategoryDetails category)
        {
            if (_categoryRepository.AddEditCategory(category))
            {
                if(category.CategoryId != default)
                {
                    _notyf.Success(Constant.CategoryEditSuccess);
                }
                else
                {
                    _notyf.Success(Constant.CategoryAddSuccess);
                }
                
            }
            else
            {
                if (category.CategoryId != default)
                {
                    _notyf.Error(Constant.CategoryEditError);
                }
                else
                {
                    _notyf.Error(Constant.CategoryAddError);
                }
               
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
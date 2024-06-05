using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Entities.Models;
using ProductManagement.Entities.ViewModels;
using ProductManagement.Models;
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
            CategoryAddEdit data = new CategoryAddEdit
            {
                Categories = _categoryRepository.GetAllCategories()
            };
            return View(data);
        }
        public IActionResult CategoryAddEdit(int? CategoryId)
        {
            if(!CategoryId.HasValue)
            {
                return View();
            }
            else
            {
                CategoryAddEdit data = _categoryRepository.GetCategoryDetails(CategoryId.Value);
                return View(data);
            }
           
        }
        public IActionResult AddEditCategory(CategoryAddEdit category)
        {
            if (_categoryRepository.AddEditCategory(category))
            {
                _notyf.Success("Category Changes applied..");
            }
            else
            {
                _notyf.Error("Changes not applied...");
            }

            return RedirectToAction("Index");
        }
        public IActionResult DeleteCategory(int CategoryId)
        {
            if (_categoryRepository.DeleteCategory(CategoryId))
            {
                _notyf.Success("Category deteled ..");
            }
            else
            {
                _notyf.Error("Category not deteled... there are some products in this category ");
            }
           
            return RedirectToAction("Index");
        }
    }
}
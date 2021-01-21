using ECommerce.Admin.Helper;
using ECommerce.Admin.Models;
using ECommerce.Business.Abstract;
using ECommerce.Entities.Concrete;
using ECommerce.Entities.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Admin.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IBrandService _brandService;
        ICategoryService _categoryService;
        IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductService productService, IWebHostEnvironment webHostEnvironment,IBrandService brandService,ICategoryService categoryService)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var model=_productService.GetAll();
            return View(model);
        }
        
        public IActionResult Insert(int ProductId)
        {

            var model = new ProductViewModel {
                Categories = _categoryService.GetCategoriesByLevel(CategoryLevel.Category) 
            };
            if (ProductId != 0)
            {

                var product = _productService.Get(ProductId);
                model.Id = product.Id;
                model.Name = product.Name;
                model.Description = product.Description;
                model.Price = product.Price;
                model.Discount = product.Discount;
                model.Stock = product.Stock;
                //model.BrandName = product.Brand.Name;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Insert(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = new FileUpload().UploadedFile(model.ProductImages.FirstOrDefault(), _webHostEnvironment);
                Product product = new Product
                {
                    Name = model.Name,
                    CategoryId = model.SubSubCategoryId,
                    Description = model.Description,
                    Discount = model.Discount,
                    Stock = model.Stock,
                    Price = model.Price,
                    BrandId = _brandService.Insert(new Brand {Name=model.BrandName }).Id
                };
                if (model.Id == 0)
                {
                    _productService.Insert(product);
                }
                else
                {
                    product.Id = model.Id;
                    _productService.Update(product);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }/*
        public IActionResult Update(int id)
        {
            var category = _productService.Get(id);
            CategoryViewModel model = new CategoryViewModel
            {
                Name = category.Name,
                ImagePath=category.ImagePath,
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = new FileUpload().UploadedFile(model.CategoryImage, _webHostEnvironment);

                Category category = _categoryService.Get(model.Id);
                category.ImagePath = uniqueFileName;
                category.Name = model.Name;
                _productService.Update(category);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
*/
        public IActionResult Delete(int id)
        {
            _productService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        public JsonResult LoadSubCategory(int CategoryId)
        {
            var subcategories = _categoryService.GetSubCategoriesById(CategoryId, CategoryLevel.SubCategory);
            return Json(new SelectList(subcategories, "Id", "Name"));
        }

        public JsonResult LoadSubSubCategory(int CategoryId)
        {
            var subcategories = _categoryService.GetSubCategoriesById(CategoryId, CategoryLevel.SubSubCategory);
            return Json(new SelectList(subcategories, "Id", "Name"));
        }
    }
}

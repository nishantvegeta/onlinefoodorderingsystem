using Microsoft.AspNetCore.Mvc;
using onlinefood.Entity;
using onlinefood.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.CategoryVms;

namespace onlinefood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly FirstRunDbContext dbContext;
        private readonly ICategoryService categoryService;
        public CategoriesController(FirstRunDbContext dbContext, ICategoryService categoryService)
        {
            this.categoryService = categoryService;
            this.dbContext = dbContext;
        }

        // search categories
        public async Task<IActionResult> Search(string searchTerm)
        {
            var categories = await categoryService.SearchCategories(searchTerm); // Await the Task to get the result
            if (categories == null || !categories.Any()) // Now you can use .Any() on the collection
            {
                return NotFound("No categories found");
            }

            return View(categories); // Return the categories to the view
        }

        public async Task<IActionResult> Index()
        {
            var categories = await categoryService.GetAllCategories();
            return View(categories);
        }

        public IActionResult Create()
        {
            var vm = new CreateCategoryVm();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var dto = new Dto.CategoryDtos.CreateCategoryDto();
                dto.Name = vm.Name;
                dto.IsActive = vm.IsActive;


                await categoryService.Create(dto);
                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var category = await dbContext.Categories.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
                if (category == null)
                {
                    return NotFound();
                }

                var vm = new UpdateCategoryVm();
                vm.Name = category.Name;
                vm.IsActive = category.IsActive;

                return View(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateCategoryVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var category = await dbContext.Categories.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
                if (category == null)
                {
                    return NotFound();
                }

                var dto = new Dto.CategoryDtos.UpdateCategoryDto();
                dto.Name = vm.Name;
                dto.IsActive = vm.IsActive;

                await categoryService.Update(id, dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await categoryService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

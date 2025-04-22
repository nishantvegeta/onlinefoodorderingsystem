using Microsoft.AspNetCore.Mvc;
using onlinefood.Entity;
using onlinefood.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using onlinefood.Services.Interfaces;
using onlinefood.ViewModels.FoodItemVms;

namespace onlinefood.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FoodItemController : Controller
    {
        private readonly FirstRunDbContext dbContext;
        private readonly IFoodItemService foodItemService;
        public FoodItemController(FirstRunDbContext dbContext, IFoodItemService foodItemService)
        {
            this.foodItemService = foodItemService;
            this.dbContext = dbContext;
        }

        //search food item
        public async Task<IActionResult> Search(string searchTerm)
        {
            var foodItems = await foodItemService.SearchFoodItems(searchTerm); // Await the Task to get the result
            if (foodItems == null || !foodItems.Any()) // Now you can use .Any() on the collection
            {
                return NotFound("No food items found");
            }

            return View(foodItems); // Return the food items to the view
        }

        public IActionResult Index()
        {
            var foodItems = foodItemService.GetAllFoodItems();
            return View(foodItems);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await dbContext.Categories.ToListAsync();
            var vm = new CreateFoodItemVm();
            vm.Categorie = categories;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFoodItemVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var foodCategory = await dbContext.Categories
                    .Where(x => x.CategoryId == vm.CategoryId).FirstOrDefaultAsync();

                if (foodCategory == null)
                {
                    throw new Exception("Category not found");
                }

                var dto = new Dto.FoodItemDtos.CreateFoodItemDto();
                dto.Name = vm.Name;
                dto.Description = vm.Description;
                dto.Price = vm.Price;
                dto.ImageUrl = vm.ImageUrl;
                dto.CategoryId = vm.CategoryId;
                dto.IsActive = vm.IsActive;

                await foodItemService.CreateFoodItem(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var foodItem = await dbContext.FoodItems.Where(x => x.FoodItemId == id)
                    .Include(x => x.Category).FirstOrDefaultAsync();
                if (foodItem == null)
                {
                    return NotFound();
                }

                var categories = await dbContext.Categories.OrderBy(x => x.Name)
                    .ToListAsync();

                var vm = new UpdateFoodItemVm();
                vm.Name = foodItem.Name;
                vm.Description = foodItem.Description;
                vm.Price = foodItem.Price;
                vm.ImageUrl = foodItem.ImageUrl;
                vm.IsActive = foodItem.IsActive;
                vm.CategoryId = foodItem.CategoryId;

                return View(vm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateFoodItemVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vm);
                }

                var foodItem = await dbContext.FoodItems.Where(x => x.FoodItemId == id)
                    .Include(x => x.Category).FirstOrDefaultAsync();
                if (foodItem == null)
                {
                    return NotFound();
                }

                var dto = new Dto.FoodItemDtos.UpdateFoodItemDto();
                dto.Name = vm.Name;
                dto.Description = vm.Description;
                dto.Price = vm.Price;
                dto.ImageUrl = vm.ImageUrl;
                dto.CategoryId = vm.CategoryId;
                dto.IsActive = vm.IsActive;

                await foodItemService.UpdateFoodItem(id, dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await foodItemService.DeleteFoodItem(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

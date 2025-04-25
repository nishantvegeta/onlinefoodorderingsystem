using Microsoft.AspNetCore.Mvc;
using onlinefood.Services.Interfaces;

namespace onlinefood.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class FoodItemController : Controller
    {

        private readonly IFoodItemService foodItemService;

        public FoodItemController(IFoodItemService foodItemService)
        {
            this.foodItemService = foodItemService;
        }

        // GET: FoodItemController
        public async Task<IActionResult> Index()
        {
            var foodItems = await foodItemService.GetAllFoodItems();
            return View(foodItems);
        }

        public async Task<IActionResult> Featured()
        {
            var foodItems = await foodItemService.GetFeaturedFoodItems();
            return View(foodItems);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            var foodItems = await foodItemService.SearchFoodItems(searchTerm);
            return View(foodItems);
        }

        public async Task<IActionResult> Details(int id)
        {
            var foodItem = await foodItemService.GetFoodItemById(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            return View(foodItem);
        }

        public async Task<IActionResult> Menu(int id)
        {
            var foodItems = await foodItemService.GetFoodItemsByCategory(id);
            return View(foodItems);
        }

        public async Task<IActionResult> AllActive()
        {
            var foodItems = await foodItemService.GetAllActiveFoodItems();
            return View(foodItems);
        }

    }
}

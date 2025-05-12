using Microsoft.AspNetCore.Mvc;
using onlinefood.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace onlinefood.ViewComponents;

public class CartIconViewComponent : ViewComponent
{
    private readonly ICartService cartService;
    private readonly IUserService userService;

    public CartIconViewComponent(ICartService cartService, IUserService userService)
    {
        this.cartService = cartService;
        this.userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // If user is not logged in, either show 0 or return empty
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            // Return 0 items in the cart icon
            return View(0);
            // Or skip rendering altogether:
            // return Content(string.Empty);
        }

        // User is authenticated; retrieve their ID and cart count
        var userId = userService.GetCurrentUserId();
        if (userId == 0)
        {
            return View(0); // No valid user, show empty cart
        }
        var cartItemCount = await cartService.GetCartItemCount(userId);
        return View(cartItemCount);
    }
}

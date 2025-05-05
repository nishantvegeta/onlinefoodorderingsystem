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
        var userId = userService.GetCurrentUserId();
        var cartItemCount = await cartService.GetCartItemCount(userId);
        return View(cartItemCount);
    }
}

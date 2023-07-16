using Blazored.LocalStorage;
using System.Data;
using Tangy_Common;
using TangyWeb_Client.Service.IService;
using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Service;

public class CartService : ICartService
{
    private readonly ILocalStorageService _localStorage;

    public event Action OnChange;

    public CartService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task IncrementCart(ShoppingCart cartToAdd)
    {
        var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
        bool itemInCart = false;

        if (cart is null)
        {
            cart = new List<ShoppingCart>();
        }
        foreach (var item in cart)
        {
            if (item.ProductId == cartToAdd.ProductId && item.ProductPriceId == cartToAdd.ProductPriceId)
            {
                itemInCart = true;
                item.Count += cartToAdd.Count;
            }
        }
        if (!itemInCart)
        {
            cart.Add(new ShoppingCart()
            {
                ProductId = cartToAdd.ProductId,
                ProductPriceId = cartToAdd.ProductPriceId,
                Count = cartToAdd.Count
            });
        }
        await _localStorage.SetItemAsync<List<ShoppingCart>>(SD.ShoppingCart, cart);
        OnChange.Invoke();
    }

    public async Task DecrementCart(ShoppingCart cartToDecrement)
    {
        var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);

        for(int i = 0; i < cart.Count; i++)
        {
            if (cart[i].ProductId == cartToDecrement.ProductId && cart[i].ProductPriceId == cartToDecrement.ProductPriceId)
            {
                // if count is 0 or 1... 
                if(cart[i].Count == 1 || cartToDecrement.Count == 0)
                {
                    // remove the item
                    cart.Remove(cart[i]);
                }
                else
                {
                    // just decrement
                    cart[i].Count -= 1;
                }
            }
        }
        await _localStorage.SetItemAsync<List<ShoppingCart>>(SD.ShoppingCart, cart);
        OnChange.Invoke();
    }
}
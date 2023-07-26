using Microsoft.AspNetCore.Components;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication;

public partial class Login
{
    private SignInRequestDTO SignInRequest = new();

    public bool IsProcessing = false;
    
    public bool ShowSignInErrors { get; set; }
    
    public string Errors { get; set; }
    

    [Inject]
    public IAuthenticationService _authService { get; set; }

    [Inject]
    public NavigationManager _navigationManager { get; set; }


    private async Task LoginUser()
    {
        ShowSignInErrors = false;
        IsProcessing = true;
        var result = await _authService.Login(SignInRequest);
        if (result.IsAuthSuccessful)
        {
            //_navigationManager.NavigateTo("/", forceLoad: true);
            _navigationManager.NavigateTo("/");
        }
        else
        {
            Errors = result.ErrorMessage;
            ShowSignInErrors = true;
        }
        IsProcessing = false;
    }

}

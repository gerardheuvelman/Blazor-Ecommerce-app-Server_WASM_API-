using Microsoft.AspNetCore.Components;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication;

public partial class Logout
{
    [Inject]
    public IAuthenticationService _authService { get; set; }

    [Inject]
    public NavigationManager _navigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await _authService.Logout();
        _navigationManager.NavigateTo("/", forceLoad: true);
        //_navigationManager.NavigateTo("/");

        // Unfortunately, I still need to use forceload, because the alternative way (AuthStateProvider.NotifyUserLogout) does not seem to work for me. Too bad because now the whole page will need to be rerendered. See also login.razor.cs
    }

}

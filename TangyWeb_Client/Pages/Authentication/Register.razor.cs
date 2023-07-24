using Microsoft.AspNetCore.Components;
using System;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Pages.Authentication;

public partial class Register
{
    [Inject]
    public IAuthenticationService _authService{get; set;}

    [Inject]
    public NavigationManager _navigationManager {get; set; }

    public bool IsProcessing = false;
    public IEnumerable<string> Errors { get; set; }
    public bool ShowRegistrationErrors { get; set; }

    private SignUpRequestDTO SignUpRequest = new();

    private async Task RegisterUser()
    {
        ShowRegistrationErrors = false;
        IsProcessing = true;
        var result = await _authService.RegisterUser(SignUpRequest);
        if (result.IsRegistrationSuccessful)
        {
            _navigationManager.NavigateTo("/");
        }
        else
        {
            Errors = result.Errors;
            ShowRegistrationErrors = true;
        }
        IsProcessing = false;
    }
}

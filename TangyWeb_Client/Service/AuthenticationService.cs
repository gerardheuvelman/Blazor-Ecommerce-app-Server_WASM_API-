using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Tangy_Common;
using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthenticationService(HttpClient client, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _client = client;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<SignInResponseDTO> Login(SignInRequestDTO signInRequest)
    {
        var content = JsonConvert.SerializeObject(signInRequest);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("api/account/signin", bodyContent);
        var contentTemp = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<SignInResponseDTO>(contentTemp);

        if (response.IsSuccessStatusCode)
        {
            await _localStorage.SetItemAsync(SD.Local_Token, result.Token);
            await _localStorage.SetItemAsync(SD.Local_UserDetails, result.UserDTO);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            return new SignInResponseDTO() { IsAuthSuccessful = true };
        }
        else return result;
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }

    public Task<SignUpResponseDTO> RegisterUser(SignUpRequestDTO signUpRequest)
    {
        throw new NotImplementedException();
    }
}

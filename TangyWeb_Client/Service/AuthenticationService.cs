using Tangy_Models;
using TangyWeb_Client.Service.IService;

namespace TangyWeb_Client.Service;

public class AuthenticationService : IAuthenticationService
{
    public Task<SignInResponseDTO> Login(SignInRequestDTO signInRequestDTO)
    {
        throw new NotImplementedException();
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }

    public Task<SignUpResponseDTO> RegisterUser(SignUpRequestDTO signUpRequestDTO)
    {
        throw new NotImplementedException();
    }
}

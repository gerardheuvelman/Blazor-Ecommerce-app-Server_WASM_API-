﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_Models;
using TangyWeb_API.Helper;

namespace TangyWeb_API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly APISettings _aPISettings;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<APISettings> options)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _aPISettings = options.Value;
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody]SignUpRequestDTO signUpRequestDTO)
    {
        if (signUpRequestDTO is null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = new ApplicationUser
        {
            UserName = signUpRequestDTO.Email,
            Email = signUpRequestDTO.Email,
            Name = signUpRequestDTO.Name,
            PhoneNumber = signUpRequestDTO.PhoneNumber,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, signUpRequestDTO.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new SignUpResponseDTO()
            {
                IsRegisterationSuccessful = false,
                Errors = result.Errors.Select(e=>e.Description)
            });
        }

        var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Customer);
        if (!roleResult.Succeeded)
        {
            return BadRequest(new SignUpResponseDTO()
            {
                IsRegisterationSuccessful = false,
                Errors = result.Errors.Select(e => e.Description)
            });
        }
        return StatusCode(201);
    }

    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequestDTO)
    {
        if (signInRequestDTO is null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var result = await _signInManager.PasswordSignInAsync(signInRequestDTO.UserName, signInRequestDTO.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(signInRequestDTO.UserName);
            if (user is null) {
                return Unauthorized(new SignInResponseDTO()
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Authentication"
                });

            }

            // Everything is valid and we need to log in

        }
        else
        {
            return Unauthorized(new SignInResponseDTO()
            {
                IsAuthSuccessful = false,
                ErrorMessage = "Invalid Authentication"
            });
        }
        return StatusCode(201);
    }

    private SigningCredentials GetSigningCredentials(string username, string password)
    {
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_aPISettings.SecretKey));

        return new SigningCredentials(secret, SecurityAlgorithms.Sha256);
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Id", user.Id)
        };

        var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role);
        }
        return claims;
    }
}

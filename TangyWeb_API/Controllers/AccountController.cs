﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_Models;

namespace TangyWeb_API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
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


}

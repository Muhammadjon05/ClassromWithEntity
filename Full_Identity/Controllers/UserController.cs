using Full_Identity.DTOs;
using Full_Identity.Helper;
using IdentityData.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Full_Identity.Controllers;

public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInDTO signInDto)
    {
        var result = await _signInManager.PasswordSignInAsync(signInDto.UserName, signInDto.Password, true,false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("UserName","Username or Password is Incorrect");
            return View();
        }
        return RedirectToAction("Profile");
    }
    
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SignUp([FromForm]SignUpDTO signUpDto)
    {
        if (!ModelState.IsValid)
        {
            return View(signUpDto);
        }
        var user = new User()
        {
            UserName = signUpDto.UserName,
            FirstName = signUpDto.FirstName,
            LastName = signUpDto.LastName,
        };
        user.PhotoUrl = await FileHelper.SaveUserFile(signUpDto.Photo);
        var result = await _userManager.CreateAsync(user, signUpDto.Password);
        if (!result.Succeeded)
        { 
            ModelState.AddModelError("UserName",result.Errors.First().Description);
            return View();
        }
        await _signInManager.SignInAsync(user, isPersistent:true);
        
        return RedirectToAction("Profile");
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("SignUp");
    }
}
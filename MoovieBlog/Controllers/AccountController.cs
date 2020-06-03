using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoovieBlog.Data;
using MoovieBlog.Models;
using MoovieBlog.ViewModels;
namespace MoovieBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataContext context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.context = context;
        }

        //register Get
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //register post
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email, Name = model.FirstName, LastName = model.LastName, RegisterDate = DateTime.Now.ToString() };
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("home", "Moovies");
                }
                else
                {
                    ModelState.AddModelError("Error1", "User already exists in site.");
                }
            }
            return NoContent();
        }
        
        //sign Get 
        [HttpGet]
        public IActionResult login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        //sign Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    else
                    {
                        return RedirectToAction("home", "Moovies");
                    }
                }
                else
                {
                    return View(new LoginViewModel());
                }
            }
            return NoContent();
        }

        //exit account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("home", "Moovies");
        }
    }
}

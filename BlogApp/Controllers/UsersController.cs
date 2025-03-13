using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsersDTO model)
        {
            if (ModelState.IsValid)
            {
                var isUser = _userRepository.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (isUser != null)
                {
                    List<Claim> userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name ?? ""));
                    userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));


                    if (isUser.Email == "erdem@erdem.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                        userClaims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    return RedirectToAction("Index", "Posts");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Posts");
        }

    }
}
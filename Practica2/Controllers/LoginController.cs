using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica2.Contexts;
using Practica2.Helper;
using Practica2.Mapper.DTOs;
using Practica2.Models;
using Practica2.Services;
using System.Security.Claims;
using System.Web;
namespace Practica2.Controllers
{

    public class LoginController : Controller
    {
        private readonly Context _context;
        private readonly IConfiguration _configuration;

        public LoginController(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public ActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                userDto.Password = LoginServices.GetSHA256(userDto.Password);
                var check = _context.Users.Where(x => x.Email == userDto.Email && x.Password == userDto.Password && x.IsDeleted == false).FirstOrDefault();
                if (check != null)
                {
                    var claim = new List<Claim>{
                        new Claim(ClaimTypes.Email,userDto.Email)
                    };
                    var claims = new ClaimsIdentity(claim, "Login");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claims), new AuthenticationProperties
                    {
                       
                        IsPersistent = true,
                      
                    });
                    return RedirectToAction("Index", "Home");

                }


            }
            else
            {
                return View();
            }
            ViewBag.Message = "The email address or password is incorrect.";
            return View();
        }
        public ActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(UserDto userDto)
        {
            if(ModelState.IsValid)
            {
                if (!(_context.Users.Any(x => x.Email == userDto.Email)))
                {
                    userDto.Password = LoginServices.GetSHA256(userDto.Password);
                    _context.Users.Add(new User
                    {
                        Email = userDto.Email,
                        Password = userDto.Password,
                        IsDeleted = false,
                        LastUpdated = DateTime.Now
                    });
                    await _context.SaveChangesAsync();
                    var claim = new List<Claim>{
                        new Claim(ClaimTypes.Email,userDto.Email)
                    };
                    var claims = new ClaimsIdentity(claim, "Login");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claims), new AuthenticationProperties
                    {

                        IsPersistent = true,

                    });
                    EmailHelper emailHelper = new EmailHelper(_configuration);
                    emailHelper.SendEmail(userDto.Email);
                    return RedirectToAction("Index", "Home");

                }
                else{
                    ViewBag.Message = "The email is already associated with another account.";
                }

            }
           
            return View();
        }
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return View("Login");
        }
    }
}

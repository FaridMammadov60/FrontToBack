using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly SignInManager<AppUser> _signInManager;


        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> rolemanager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _rolemanager = rolemanager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registr()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registr(RegistrVM registrVM)
        {

            if (!ModelState.IsValid) return View();
            AppUser appUser = new AppUser
            {
                FullName = registrVM.FullName,
                UserName = registrVM.UserName,
                Email = registrVM.Email,
            };
            IdentityResult result = await _userManager.CreateAsync(appUser, registrVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);                    
                }
                return View();
            }

            return RedirectToAction("login", "account");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("","Istifadeci adi ve ya sifre yanlishdir");
                return View(loginVM);
            }
            
            SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
                        
            if (result.IsLockedOut)
            {
                TimeSpan timeSpan = appUser.LockoutEnd.Value.UtcDateTime.ToUniversalTime() - DateTime.Now.ToUniversalTime();
                var timeSpanFromMinutes = TimeSpan.FromMinutes(timeSpan.TotalMinutes);
                int mm = timeSpanFromMinutes.Minutes;
                int ss = timeSpanFromMinutes.Seconds;
                TempData["Error"] = $"{mm} deq {ss} saniye sonra daxil ola bilersiniz";
                //ModelState.AddModelError("", "account bloklandir");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Istifadeci adi ve ya sifre yanlishdir");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

using FrontToBack.Helper;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> rolemanager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _rolemanager = rolemanager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(string search)
        {
            var users = search == null ? _userManager.Users.ToList() :
                _userManager.Users.Where(user => user.FullName.ToLower()
                .Contains(search.ToLower())).ToList();

            //var users = _userManager.Users.ToList();
            //var user = _userManager.Users.FirstOrDefault(u=> u.Id == Id); 
            //var userRoles = await _userManager.GetRolesAsync(user);

            //UserVM userVM = new UserVM()
            //{
            //    Users = users,
            //    userRoles = userRoles,
            //};
            //return View(userVM);
            return View(users);
        }

        public async Task<IActionResult> Update(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _rolemanager.Roles.ToList();
            RoleVM rolevm = new RoleVM
            {
                FullName = user.FullName,
                roles = roles,
                userRoles = userRoles,
                UserId = user.Id
            };

            return View(rolevm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(List<string> roles, string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);

            //var addRoles=roles.Except(userRoles);
            //var removedRoles=userRoles.Except(roles);
            //await _userManager.AddToRolesAsync(user,addRoles);
            //await _userManager.RemoveFromRolesAsync(user, removedRoles);


            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, roles);

            return RedirectToAction("index");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistrVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new AppUser
            {
                FullName = registerVM.FullName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());
            return RedirectToAction("index", "user");
        }
    }
}

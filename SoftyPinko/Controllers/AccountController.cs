using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoftyPinko.DAL;
using SoftyPinko.Models;
using SoftyPinko.ViewModels;
using System.Threading.Tasks;

namespace SoftyPinko.Controllers
{

    public class AccountController : Controller
    {
        UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm) 
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = new AppUser()
            {
                Name = registerVm.Name,
                Email = registerVm.Email,
                Surname = registerVm.Surname,
                UserName = registerVm.Username,
            };
            var result = await userManager.CreateAsync(appUser,registerVm.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            await signInManager.SignInAsync(appUser, true);

            return RedirectToAction("Index","Home");

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await userManager.FindByEmailAsync(loginVm.Email);
            if(user == null)
            {
                return View("Login");
            }
            var result = await signInManager.CheckPasswordSignInAsync(user, loginVm.Password,false);
            if (!result.Succeeded) 
            {
                ModelState.AddModelError("", "Failed");
                return View();
            }
            await signInManager.SignInAsync(user,false);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}

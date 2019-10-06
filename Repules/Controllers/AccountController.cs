using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repules.Model;
using Repules.Models;

namespace Okosgardrob.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager; //dependency injection
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl) //fontos az elnevezési konvenciók betartása, hogy működjön az alkalmazás
        {
            ViewBag.returnUrl = returnUrl;
            return View(); //LoginView-t adja vissza
        }

        [HttpPost]
        [AllowAnonymous] //ehhez hozzáférhet olyan ember, aki nincs belépve
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel details, string returnUrl) //mikor a felhasználó lenyomja a belépés gombot, akkor fut ez a postos kérés
        {
            if (ModelState.IsValid) //validáció
            {
                ApplicationUser user = await userManager.FindByEmailAsync(details.Email); //email alapjan keressük meg az usert
                if (user != null)
                {
                    //signInManager -> ki-és bejelentkezés
                    await signInManager.SignOutAsync(); //a biztonság kedvéért kijelentkeztetem, mielott bejelentkeztetném (hogy nehogy 2x legyen belépve)
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false); //belépés
                    if (result.Succeeded)
                        return Redirect(returnUrl ?? "/"); //ha van return URL, akkor oda iranyitom vissza
                }
                ModelState.AddModelError(nameof(LoginViewModel.Email), "Invalid user or password");
            }
            return View(details); //ha nem találtuk meg a usert, vagy ha nem voltak validok a megadott adatok
        }

        [Authorize] //csak belépett felhasználók férhetnek hozzá
        //ez az attributum automatikusan átirányít bárhonnan a login felületre, majd miután bejelentkezett a felhasználó, oda iranyit vissza
        public async Task<IActionResult> Logout() //kilépés
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }      
    }
}
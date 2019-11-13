using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repules.Model;
using Repules.Models;

namespace Okosgardrob.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller //felhasznalók felvétele
    {
        private readonly UserManager<ApplicationUser> userManager; //itt nem kellett service-t irni, mivel ez már meg van irva (Identity Core)

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public ViewResult Index()
        {
            List<ApplicationUser> users = userManager.Users.ToList();
            List<ListApplicationUserViewModel> viewModels = users.Select(u => new ListApplicationUserViewModel { UserId = u.Id, Email = u.Email, UserName = u.UserName }).ToList();
            return View(viewModels);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApplicationUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = viewModel.Name, Email = viewModel.Email };
                IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    if (viewModel.IsAdmin)
                    {
                        await userManager.AddToRoleAsync(user, "admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "user");
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            string uid = id.ToString();
            var user = await userManager.FindByIdAsync(uid);
            ListApplicationUserViewModel appUserViewModel = new ListApplicationUserViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
            return View(appUserViewModel);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            if (ModelState.IsValid)
            {
                string uid = id.ToString();
                var user = await userManager.FindByIdAsync(uid);
                await userManager.DeleteAsync(user);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            string uid = id.ToString();
            var user = await userManager.FindByIdAsync(uid);
            ListApplicationUserViewModel appUserViewModel = new ListApplicationUserViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email
                //isAdmin = if (role==admin)-> true admin jogot lehessen adni valakinek?
            };
            return View(appUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ListApplicationUserViewModel userViewModel)
        {

            string id = userViewModel.UserId.ToString();
            var appUser = await userManager.FindByIdAsync(id);
            appUser.UserName = userViewModel.UserName;
            appUser.Email = userViewModel.Email;
            await userManager.UpdateAsync(appUser);
            return RedirectToAction("Index");
        }
    }
}

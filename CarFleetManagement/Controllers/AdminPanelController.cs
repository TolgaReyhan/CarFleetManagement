using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarFleetManagement.Data.Models;

namespace CarFleetManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AdminPanelController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = userManager.Users.Where(u => !u.IsDeleted).ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var currentUserId = userManager.GetUserId(User);
            if (id == currentUserId)
            {
                TempData["ErrorMessage"] = "Администратор не може да изтрие своя акаунт!";
                return RedirectToAction("Index");
            }
            var user = await userManager.FindByIdAsync(id) as ApplicationUser;
            if (user != null)
            {
                user.IsDeleted = true;
                await userManager.UpdateAsync(user);
            }

            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace SmartStudentReminder.Controllers
{
    public class AccountController : Controller
    {
        // Displays the login page.
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Handles the login form submission.
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Temporary login logic for the interface prototype.
            if (email == "student@example.com" && password == "Password123")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid email address or password.";
            return View();
        }
    }
}
using LoginApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Http;
namespace LoginApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public HttpContext GetHttpContext()
        {
            return HttpContext;
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("SendEmail", "OTP");
            }
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return RedirectToAction("Index", "Home");
            }

            else if (IsValidUser(username, password))
            {
                HttpContext.Session.SetString("Username", username);
                string sessionUsername = HttpContext.Session.GetString("Username") ?? string.Empty;
                if (string.IsNullOrEmpty(sessionUsername))
                {
                    // Session is null or empty, deny access and redirect to the index of home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Session is not null or empty, grant access and redirect to the index of customers
                    return RedirectToAction("SendEmail", "OTP");
                }
            }
            ViewBag.Error = "Invalid username or password";
            return View();
        }
        private static bool IsValidUser(string username, string password)
        {
            Customers validCustomer = GetValidCustomer();
            if (validCustomer != null && username == validCustomer.Username && password == validCustomer.Password)
            {
                return true;
            }
            return false;
        }

        private static Customers GetValidCustomer()
        {
            Customers customer = new()
            {
                Username = "admin",
                Password = "admin"
            };
            return customer;
        }
    }

}


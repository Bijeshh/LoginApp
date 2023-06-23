using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoginApp.Data;
using LoginApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace LoginApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerRepository customerRepository;
        private readonly IHttpContextAccessor? contxt;
        public CustomersController(IHttpContextAccessor httpContextAccessor)
        {
            string connectionString = "Server=.\\SQLEXPRESS; Database=LoginDB; User ID=; Password=; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=true;";
            customerRepository = new CustomerRepository(connectionString);
            contxt = httpContextAccessor;
        }

        // GET: Customers
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Customer> customers = customerRepository.GetAllCustomers();
            return View(customers);
        }

        // GET: Customers/Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = customerRepository.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Address,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                bool created = customerRepository.CreateCustomer(customer);

                if (created)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(customer);
        }

        // GET: Customers/Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = customerRepository.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Address,Email")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool updated = customerRepository.UpdateCustomer(customer);

                if (updated)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(customer);
        }

        // GET: Customers/Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = customerRepository.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleted = customerRepository.DeleteCustomer(id);

            if (deleted)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }
}



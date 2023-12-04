using BarberShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BarberShop.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _dbContext; // Injected database context

        public ServiceController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Menu()
        {
            var services = _dbContext.Services.ToList();
            return View(services);
        }

        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddService(Service service)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Services.Add(service);
                _dbContext.SaveChanges();
                return RedirectToAction("Menu");
            }

            return View(service);
        }

        [HttpGet]
        public IActionResult EditService(int id)
        {
            var service = _dbContext.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        public IActionResult EditService(Service service)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Services.Update(service);
                _dbContext.SaveChanges();
                return RedirectToAction("Menu");
            }

            return View(service);
        }

        [HttpPost]
        public IActionResult DeleteService(int id)
        {
            var service = _dbContext.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            _dbContext.Services.Remove(service);
            _dbContext.SaveChanges();

            return RedirectToAction("Menu");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

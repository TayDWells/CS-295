using BarberShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BarberShop.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext _dbContext; // Injected database context

        public ClientController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult List(string search)
        {
            // Retrieve clients based on the search parameter
            var clients = string.IsNullOrEmpty(search) ?
                            _dbContext.Clients.ToList() :
                            _dbContext.Clients.Where(c => c.Name.Contains(search) || c.Pronouns.Contains(search) || c.Number.Contains(search)).ToList();

            return View(clients);
        }

        public IActionResult AddClient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddClient(Client client)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Clients.Add(client);
                _dbContext.SaveChanges();
                return RedirectToAction("List");
            }

            return View(client);
        }

        [HttpGet]
        public IActionResult EditClient(int id)
        {
            var client = _dbContext.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        public IActionResult EditClient(Client client)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Clients.Update(client);
                _dbContext.SaveChanges();
                return RedirectToAction("List");
            }

            return View(client);
        }

        [HttpPost]
        public IActionResult DeleteClient(int id)
        {
            var client = _dbContext.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            _dbContext.Clients.Remove(client);
            _dbContext.SaveChanges();

            return RedirectToAction("List");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

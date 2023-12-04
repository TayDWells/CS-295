using BarberShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BarberShop.Controllers
{
    public class AppointmentController : Controller
    {
        AppDbContext _dbContext;

        public AppointmentController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Calendar()
        {
            var appointments = _dbContext.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .ToList();

            return View(appointments);
        }


        public IActionResult AddAppt()
        {
            ViewBag.Clients = _dbContext.Clients.ToList();
            ViewBag.Services = _dbContext.Services.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddAppt(Appointment appointment)
        {
            _dbContext.Appointments.Add(appointment);
            _dbContext.SaveChanges();


            return RedirectToAction("Calendar");
        }

        public IActionResult EditAppointment(int id)
        {
            var appointment = _dbContext.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .FirstOrDefault(a => a.ApptId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.Clients = _dbContext.Clients.ToList();
            ViewBag.Services = _dbContext.Services.ToList();

            return View("EditAppt", appointment);
        }


        [HttpPost]
        public IActionResult UpdateAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clients = _dbContext.Clients.ToList();
                ViewBag.Services = _dbContext.Services.ToList();
            }

            _dbContext.Appointments.Update(appointment);
            _dbContext.SaveChanges();

            return RedirectToAction("Calendar");
        }


        [HttpPost]
        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _dbContext.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _dbContext.Appointments.Remove(appointment);
            _dbContext.SaveChanges();

            return RedirectToAction("Calendar");
        }




        [HttpPost]
        public IActionResult MarkAsCompleted(int id)
        {
            var appointment = _dbContext.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Completed = true;
            _dbContext.SaveChanges();

            return RedirectToAction("Calendar");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

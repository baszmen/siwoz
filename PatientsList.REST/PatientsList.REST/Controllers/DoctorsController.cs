﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PatientsList.Model;
using PatientsList.Model.Entities;
using PatientsList.Model.Repository;

namespace PatientsList.REST.Controllers
{
    public class DoctorsController : Controller
    {
        //
        // GET: /Doctors/

        public ActionResult Index()
        {
            List<Doctor> doctors;
            using (var uow = new UnitOfWork())
            {
                var repo = new Repository<Doctor>(uow);
                doctors = new List<Doctor>(repo.Query());
            }
            return View(doctors);
        }

        //
        // GET: /Doctors/Details/5

        public ActionResult Details(int id)
        {
            List<Patient> patients;
            Doctor doctor;
            using (var uow = new UnitOfWork())
            {
                var doctorsRepo = new Repository<Doctor>(uow);
                doctor = doctorsRepo.Get(id);
                patients = new List<Patient>(doctor.PatientsList);
            }
            ViewBag.Doctor = doctor;
            return View(patients);
        }

        //
        // GET: /Doctors/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Doctors/Create

        [HttpPost]
        public ActionResult Create([Bind(Include = "FirstName, LastName, Titles, Photo")]Doctor doctor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var uow = new UnitOfWork())
                    {
                        var repository = new Repository<Doctor>(uow);
                        repository.Add(doctor);
                        uow.Commit();
                    }
                    return RedirectToAction("Details", new
                    {
                        id = doctor.Id
                    });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Niepoprawne dane.");
            }
            return View(doctor);
        }

        //
        // GET: /Doctors/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Doctors/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Doctors/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Doctors/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

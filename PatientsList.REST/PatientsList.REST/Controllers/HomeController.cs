﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatientsList.Model;
using PatientsList.Model.Entities;
using PatientsList.Model.Repository;

namespace PatientsList.REST.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Doctors()
        {
            List<Doctor> doctors;
            using (var uow = new UnitOfWork())
            {
                var repo = new Repository<Doctor>(uow);
                doctors = new List<Doctor>(repo.Query());
            }
            return View(doctors);
        }

        public ActionResult Patients(int id)
        {
            List<Patient> patients;
            using (var uow = new UnitOfWork())
            {
                var doctorsRepo = new Repository<Doctor>(uow);
                var doctor = doctorsRepo.Get(id);
                patients = new List<Patient>(doctor.PatientsList);
            }
            return View(patients);
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
        }

        public enum ManageMessageId
        {
            UserRemoved,
            UserNotRemoved
        }

        public ActionResult RemoveDoctor(int userId, string returnUrl)
        {
            TempData["msg"] = removeUser(userId) ? ManageMessageId.UserRemoved : ManageMessageId.UserNotRemoved;
            return RedirectToLocal(returnUrl);
        }

        public ActionResult AddDoctor(string returnUrl)
        {
            using (var uow = new UnitOfWork())
            {
                var repo = new Repository<Doctor>(uow);
                repo.Add(new Doctor()
                {
                    Name = "Mateusz",
                    Surname = "Dembski",
                    Titles = "dr hab. inż. cz. rzecz. PAN"
                });
                uow.Commit();
            }
            return RedirectToLocal(returnUrl);
        }

        public ActionResult RemovePatient(int patientId, string returnUrl)
        {
            TempData["msg"] = removePatient(patientId) ? ManageMessageId.UserRemoved : ManageMessageId.UserNotRemoved;
            return RedirectToLocal(returnUrl);
        }

        private bool removeUser(int doctorId)
        {
            using (var uow = new UnitOfWork())
            {
                var repo = new Repository<Doctor>(uow);
                var repoPatients = new Repository<Patient>(uow);
                repoPatients.Delete(repo.Get(doctorId).PatientsList);
                repo.Delete(repo.Get(doctorId));
                uow.Commit();
            }
            return true;
        }

        private bool removePatient(int patientId)
        {
            using (var uow = new UnitOfWork())
            {
                var repo = new Repository<Patient>(uow);
                repo.Delete(repo.Get(patientId));
                uow.Commit();
            }
            return true;
        }
    }
}

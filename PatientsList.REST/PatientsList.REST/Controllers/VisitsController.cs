using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatientsList.Model;
using PatientsList.Model.Entities;
using PatientsList.Model.Repository;
using PatientsList.REST.Models;

namespace PatientsList.REST.Controllers
{
    public class VisitsController : Controller
    {

        private ActionResult CreateEditView(int doctorId, Patient visit = null)
        {
            ViewBag.DoctorId = doctorId;
            ViewBag.DurationTimes = new List<SelectListItem>
            {
                new SelectListItem {Text = "00:15", Value = "00:15:00"},
                new SelectListItem {Text = "00:30", Value = "00:30:00"},
                new SelectListItem {Text = "00:45", Value = "00:45:00"},
            };
            return View("Create", visit);
        }


        //
        // GET: /Visits/Create

        public ActionResult Create(int doctorId)
        {
            return CreateEditView(doctorId);
        }

        //
        // POST: /Visits/Create/DoctorId

        [HttpPost]
        public ActionResult Create(int doctorId, Patient visit)
        {
            return CreateEdit(doctorId, visit);
        }

        protected ActionResult CreateEdit(int doctorId, Patient visit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (visit.IsEnded)
                        throw new HttpException(400, "Nie można edytować lub tworzyć zakończonej wizyty");
                    var uow = UnitOfWorkPerRequest.Get();
                    var repository = new Repository<Patient>(uow);
                    var docRepo = new Repository<Doctor>(uow);
                    var doctor = docRepo.Get(doctorId);
                    if (doctor == null)
                        throw new HttpException(404, "Podany doktor nie istnieje");

                    if (visit.Id <= 0)
                    {
                        repository.Add(visit);
                        doctor.PatientsList.Add(visit);
                    }
                    uow.Commit();
                    return RedirectToAction("Details", "Doctors", new { id = doctorId, visitsDate = visit.VisitTime.Date });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Niepoprawne dane.");
            }
            return RedirectToAction("Details", "Doctors", new { id = doctorId });
        }

        //
        // GET: /Visits/Edit

        public ActionResult Edit(int doctorId, int visitId)
        {
            Patient visit = null;
            Doctor doctor = null;
            var uow = UnitOfWorkPerRequest.Get();
            var repository = new Repository<Doctor>(uow);
            doctor = repository.Get(doctorId);
            if (doctor != null)
            {
                visit = doctor.PatientsList.FirstOrDefault(x => x.Id == visitId);
            }
            if (visit != null)
            {
                return CreateEditView(doctorId, visit);
            }
            throw new HttpException(404, "Brak doktora z podanym id.");
        }

        //
        // POST: /Visits/Edit/5/1

        [HttpPost]
        public ActionResult Edit(int doctorId, int visitId, Patient visit)
        {
            return CreateEdit(doctorId, visit);
        }

        //
        // POST: /Visits/Delete/5

        [HttpPost]
        public ActionResult Delete(int doctorId, int visitId)
        {
            try
            {
                var uow = UnitOfWorkPerRequest.Get();
                var visitRepo = new Repository<Patient>(uow);
                try
                {
                    visitRepo.Delete(visitRepo.Get(visitId));
                }
                catch (NullReferenceException)
                {
                    // don't give a shit
                }
                uow.Commit();
                return RedirectToAction("Details", "Doctors", new { id = doctorId });
            }
            catch
            {
                return RedirectToAction("Details", "Doctors", new { id = doctorId });
            }
        }

        public ActionResult Finish(int doctorId, int visitId)
        {
            try
            {
                var uow = UnitOfWorkPerRequest.Get();
                var visitRepo = new Repository<Patient>(uow);
                var visit = visitRepo.Get(visitId);
                if (visit != null)
                {
                    visit.IsEnded = true;
                    uow.Commit();
                    return RedirectToAction("Details", "Doctors", new { id = doctorId, visitDate = visit.VisitTime.Date});
                }
                return RedirectToAction("Details", "Doctors", new { id = doctorId });
            }
            catch
            {
                return RedirectToAction("Details", "Doctors", new { id = doctorId });
            }
        }
    }
}

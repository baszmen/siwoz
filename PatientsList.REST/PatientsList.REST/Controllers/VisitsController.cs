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

namespace PatientsList.REST.Controllers
{
    public class VisitsController : Controller
    {
        
        
        //
        // GET: /Visits/Create

        public ActionResult Create(int doctorId)
        {
            return View();
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
                    using (var uow = new UnitOfWork())
                    {
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
                    }
                    return RedirectToAction("Details", "Doctors", new {id = doctorId, visitsDate = visit.VisitTime.Date});
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Niepoprawne dane.");
            }
            return RedirectToAction("Details", "Doctors", new {id = doctorId});
        }

        //
        // GET: /Visits/Edit/5/1

        public ActionResult Edit(int doctorId, int visitId)
        {
            Doctor doctor;
            using (var uow = new UnitOfWork())
            {
                var repository = new Repository<Doctor>(uow);
                doctor = repository.Get(doctorId);
            }
            if (doctor != null)
            {
                return View("Create", doctor);

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
                using (var uow = new UnitOfWork())
                {
                    var visitRepo = new Repository<Patient>(uow);
                    try
                    {
                        visitRepo.Delete(visitRepo.Get(visitId));
                    }
                    catch (NullReferenceException)
                    {
                        // don't give a shit
                    }
                    return RedirectToAction("Details", "Doctors", new { id = doctorId });
                }
            }
            catch
            {
                return RedirectToAction("Details", "Doctors", new {id = doctorId});
            }
        }
    }
}

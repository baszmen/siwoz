using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Razor.Text;
using System.Web.Routing;
using PatientsList.Model;
using PatientsList.Model.Entities;
using PatientsList.Model.Repository;

namespace PatientsList.REST.Controllers
{
    public class DoctorsController : Controller
    {
        private int LARGE_IMG_SIZE = 640;
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
        // GET: /UserImageSmall/1
        public FileResult GetImage(int id)
        {
            using (var uow = new UnitOfWork())
            {
                Doctor doctor = new Repository<Doctor>(uow).Get(id);
                if (doctor != null)
                {
                    byte[] imageBytes = doctor.Photo;
                    if (imageBytes != null)
                    {
                        WebImage image = new WebImage(imageBytes);
                        if (image.Height > LARGE_IMG_SIZE || image.Width > LARGE_IMG_SIZE)
                        {
                            image = image.Resize(LARGE_IMG_SIZE, LARGE_IMG_SIZE);
                        }
                        image = image.Resize(200, 200, true);
                        return File(image.GetBytes(), "image/" + image.ImageFormat);
                    }
                }
            }
            return null;
        }

        //
        // GET: /Doctors/Create

        public ActionResult Create()
        {
            return View();
        }


        private Byte[] getPassedImage()
        {
            var image = WebImage.GetImageFromRequest();
            Byte[] imageBytes = null;

            if (image != null)
            {
                if (image.Height > LARGE_IMG_SIZE || image.Width > LARGE_IMG_SIZE)
                {
                    image = image.Resize(LARGE_IMG_SIZE, LARGE_IMG_SIZE);
                }

                imageBytes = image.GetBytes();
            }
            return imageBytes;
        }

        //
        // POST: /Doctors/Create

        [HttpPost]
        public ActionResult Create(FormCollection doctorFormCollection)
        {
            var doctor = new Doctor
            {
                Name = doctorFormCollection.Get("Name"),
                Surname = doctorFormCollection.Get("Surname"),
                Titles = doctorFormCollection.Get("Titles"),
                Photo = getPassedImage()
            };
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

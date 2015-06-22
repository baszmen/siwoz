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

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
        }

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
        
        public ActionResult Details(int id = 0, DateTime? visitsDate = null)
        {
            if (id == 0) return RedirectToAction("Index");
            var date = visitsDate ?? DateTime.Now;
            List<Patient> patients;
            Doctor doctor;
            using (var uow = new UnitOfWork())
            {
                var doctorsRepo = new Repository<Doctor>(uow);
                doctor = doctorsRepo.Get(id);
                if (doctor == null)
                    throw new HttpException(404, "Brak doktora z podanym id."); 
                patients = new List<Patient>(doctor.PatientsList.Where(x => x.VisitTime.Date == date.Date));
            }
            ViewBag.Doctor = doctor;
            ViewBag.Date = date;
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
            Doctor doctor = null;
            try
            {
                if (ModelState.IsValid)
                {
                    
                    using (var uow = new UnitOfWork())
                    {
                        var repository = new Repository<Doctor>(uow);

                        int id = 0;
                        try
                        {
                            id = Convert.ToInt32(doctorFormCollection.Get("Id"));
                        }
                        catch (FormatException) {}
                        if (id > 0)
                        {
                            doctor = repository.Get(id);
                        }
                        
                        if (doctor == null)
                            doctor = new Doctor();
                        doctor.Name = doctorFormCollection.Get("Name");
                        doctor.Surname = doctorFormCollection.Get("Surname");
                        doctor.Titles = doctorFormCollection.Get("Titles");
                        var image = getPassedImage();
                        if (image != null)
                            doctor.Photo = image;
                        
                        if (id <= 0)
                            repository.Add(doctor);
                        uow.Commit();
                    }
                    return RedirectToAction("Index");
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
            Doctor doctor;
            using (var uow = new UnitOfWork())
            {
                var repository = new Repository<Doctor>(uow);
                doctor = repository.Get(id);
            }
            if (doctor != null)
                return View("Create", doctor);
            throw new HttpException(404, "Brak doktora z podanym id."); 
        }

        //
        // POST: /Doctors/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection doctorFormCollection)
        {
            return Create(doctorFormCollection);
        }

        public enum ManageMessageId
        {
            UserRemoved,
            UserNotRemoved
        }

        //
        // POST: /Doctors/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, string returnUrl)
        {
            TempData["msg"] = removeUser(id) ? ManageMessageId.UserRemoved : ManageMessageId.UserNotRemoved;
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
    }
}

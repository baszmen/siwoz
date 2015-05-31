using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using PatientsList.Model;
using PatientsList.Model.Entities;
using PatientsList.Model.Repository;
using Doctor = PatientsList.Model.Entities.Doctor;

namespace PatientsList.REST.Controllers
{
    public class DoctorsController : ApiController
    {
        // GET api/doctors
        public List<Doctor> Get()
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    Name = "Anna Zawodna",
                    CheckTime = DateTime.Now + TimeSpan.FromMinutes(1)
                },

                new Patient
                {
                    Name = "Anna Zdrowa",
                    CheckTime = DateTime.Now + TimeSpan.FromMinutes(20)
                },

                new Patient
                {
                    Name = "Henryka Prostonos",
                    CheckTime = DateTime.Now + TimeSpan.FromMinutes(30)
                }
            };
            var docs = new List<Doctor>
            {
                new Doctor
                {
                    Name = "Andrzej",
                    PatientsList = patients,
                    Surname = "Góralczyk",
                    Titles = "dr hab."
                },
                new Doctor
                {
                    Name = "Paweł",
                    PatientsList = patients,
                    Surname = "Niskowłos",
                    Titles = "prof. dr hab."
                },
                new Doctor
                {
                    Name = "Hieronim",
                    PatientsList = patients,
                    Surname = "Anonim",
                    Titles = "prof. zw. dr hab"
                }
            };

            return docs;

            using (var uow = new UnitOfWork())
                return new Repository<Doctor>(uow).Query().ToList();
        }

        // GET api/doctors/5
        public Doctor Get(int id)
        {
            using (var uow = new UnitOfWork())
                return new Repository<Doctor>(uow).Get(id);
        }

        // POST api/doctors
        public void Post([FromBody]Doctor value)
        {
            using (var uow = new UnitOfWork())
                new Repository<Doctor>(uow).Add(value);
        }

        // PUT api/doctors/5
        public void Put(int id, [FromBody]Doctor value)
        {
            using (var uow = new UnitOfWork())
                new Repository<Doctor>(uow).AddOrUpdate(value);
        }

        // DELETE api/doctors/5
        public void Delete(int id)
        {
            using (var uow = new UnitOfWork())
            {
                var repository = new Repository<Doctor>(uow);
                var entity = repository.Get(id);
                repository.Delete(entity);
            }
        }
    }
}

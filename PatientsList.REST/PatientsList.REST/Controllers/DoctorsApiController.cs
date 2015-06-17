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
    public class DoctorsApiController : ApiController
    {
        // GET api/doctors
        public List<Doctor> Get()
        {
            var patients_1 = new List<Patient>
            {
                new Patient
                {
                    Id = 1,
                    Name = "Anna Zawodna 1",
                    VisitTime = DateTime.Now + TimeSpan.FromSeconds(10)
                },

                new Patient
                {
                    Id = 2,
                    Name = "Anna Zdrowa 1",
                    VisitTime = DateTime.Now + TimeSpan.FromMinutes(20)
                },

                new Patient
                {
                    Id = 3,
                    Name = "Henryka Prostonos 1",
                    VisitTime = DateTime.Now + TimeSpan.FromMinutes(30)
                }
            };
            var patients_2 = new List<Patient>
            {
                new Patient
                {
                    Id = 4,
                    Name = "Anna Zawodna 2",
                    VisitTime = DateTime.Now + TimeSpan.FromSeconds(10)
                },

                new Patient
                {
                    Id = 5,
                    Name = "Anna Zdrowa 2",
                    VisitTime = DateTime.Now + TimeSpan.FromMinutes(20)
                },

                new Patient
                {
                    Id = 6,
                    Name = "Henryka Prostonos 2",
                    VisitTime = DateTime.Now + TimeSpan.FromMinutes(30)
                }
            };
            var patients_3 = new List<Patient>
            {
                new Patient
                {
                    Id = 7,
                    Name = "Anna Zawodna 3",
                    VisitTime = DateTime.Now + TimeSpan.FromSeconds(10)
                },

                new Patient
                {
                    Id = 8,
                    Name = "Anna Zdrowa 3",
                    VisitTime = DateTime.Now + TimeSpan.FromMinutes(20)
                },

                new Patient
                {
                    Id = 9,
                    Name = "Henryka Prostonos 3",
                    VisitTime = DateTime.Now + TimeSpan.FromMinutes(30)
                }
            };
            var docs = new List<Doctor>
            {
                new Doctor
                {
                    Id = 1,
                    Name = "Andrzej",
                    PatientsList = patients_1,
                    Surname = "Góralczyk",
                    Titles = "dr hab."
                },
                new Doctor
                {
                    Id = 2,
                    Name = "Paweł",
                    PatientsList = patients_2,
                    Surname = "Niskowłos",
                    Titles = "prof. dr hab."
                },
                new Doctor
                {
                    Id = 3,
                    Name = "Hieronim",
                    PatientsList = patients_3,
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

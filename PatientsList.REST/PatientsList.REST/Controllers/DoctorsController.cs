using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using PatientsList.Model;
using PatientsList.Model.Repository;
using PatientsList.REST.Model;
using Doctor = PatientsList.Model.Entities.Doctor;

namespace PatientsList.REST.Controllers
{
    public class DoctorsController : Controller
    {
        // GET api/values
        public List<Doctor> Get()
        {
            using (var uow = new UnitOfWork())
                return new Repository<Doctor>(uow).Query().ToList();
        }

        // GET api/values/5
        public Doctor Get(int id)
        {
            using (var uow = new UnitOfWork())
                return new Repository<Doctor>(uow).Get(id);
        }

        // POST api/values
        public void Post([FromBody]Doctor value)
        {
            using (var uow = new UnitOfWork())
                new Repository<Doctor>(uow).Add(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]Doctor value)
        {
            using (var uow = new UnitOfWork())
                new Repository<Doctor>(uow).AddOrUpdate(value);
        }

        // DELETE api/values/5
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

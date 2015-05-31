using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientsList.Model;
using PatientsList.Model.Entities;
using PatientsList.Model.Repository;

namespace Model.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var uow = new UnitOfWork())
            {
                var doctorRepository = new Repository<Doctor>(uow);
                doctorRepository.Add(new Doctor
                {
                    Name = "Test",  
                    Surname = "Test2",
                    PatientsList = new List<Patient>(),
                    Id = 1
                });
                uow.Commit();

                var result = doctorRepository.Query().Count();
                Assert.IsTrue(result > 0);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientsList.Model.Entities;
using PatientsList.Model.Repository;

namespace PatientsList.Model
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var uow = new UnitOfWork())
            {
                var doctorRepository = new Repository<Doctor>(uow);
                doctorRepository.Add(new Doctor
                {
                    Name = "Test",
                    Surname = "Test2"
                });
                uow.Commit();

                var result = doctorRepository.Query().Count();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsList.DataModel
{
    public static class DoctorsDataSource
    {
        private static ObservableCollection<Doctor> _doctors;

        static DoctorsDataSource()
        {
            var patients = new ObservableCollection<Patient>
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
            _doctors = new ObservableCollection<Doctor>
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
        }

        public static ObservableCollection<Doctor> GetDoctors()
        {
            return _doctors;          
        }

        public static Doctor GetDoctor(int id)
        {
            return _doctors.FirstOrDefault(x => x.Id == id);
        }
    }
}

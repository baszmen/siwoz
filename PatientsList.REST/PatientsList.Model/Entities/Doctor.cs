using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PatientsList.Model.Entities
{
    public class Doctor : BaseEntity
    {
        [DisplayName("Imię")]
        public virtual string Name { get; set; }
        [DisplayName("Nazwisko")]
        public virtual string Surname { get; set; }
        [DisplayName("Pacjenci")]
        public virtual IList<Patient> PatientsList { get; set; }
        [DisplayName("Tytuł")]
        public virtual string Titles { get; set; }
        [DisplayName("Zdjęcie")]
        public virtual byte[] Photo { get; set; }
        public Doctor()
        {
            PatientsList = new List<Patient>();
        }
    }

    public class DoctorClassMap : BaseClassMap<Doctor>
    {
        public DoctorClassMap()
        {
            Map(x => x.Name);
            Map(x => x.Surname);
            Map(x => x.Titles);
            Map(x => x.Photo);
            HasMany(x => x.PatientsList);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsList.Model.Entities
{
    public class Doctor : BaseEntity
    {
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual IList<Patient> PatientsList { get; set; }
        public virtual string Titles { get; set; }

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsList.Model.Entities
{
    public class Patient : BaseEntity
    {
        [DisplayName("Imię")]
        public virtual string Name { get; set; }

        [DisplayName("Nazwisko")]
        public virtual string Surname { get; set; }

        [DisplayName("Czas wizyty")]
        public virtual DateTime VisitTime { get; set; }

        [DisplayName("Czas trwania")]
        public virtual TimeSpan Duration { get; set; }

        [DisplayName("Czy zakończona")]
        public virtual bool IsEnded { get; set; }

    }

    public class PatientClassMap : BaseClassMap<Patient>
    {
        public PatientClassMap()
        {
            Map(x => x.Name);
            Map(x => x.Surname);
            Map(x => x.VisitTime);
            Map(x => x.Duration);
            Map(x => x.IsEnded);
        }
    }
}

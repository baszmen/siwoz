using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsList.Model.Entities
{
    public class Patient : BaseEntity
    {
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual DateTime VisitTime { get; set; }
        public virtual TimeSpan Duration { get; set; }
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

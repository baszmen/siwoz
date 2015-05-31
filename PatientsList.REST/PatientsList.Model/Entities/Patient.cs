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
        public virtual DateTime CheckTime { get; set; }

    }

    public class PatientClassMap : BaseClassMap<Patient>
    {
        public PatientClassMap()
        {
            Map(x => x.CheckTime);
            Map(x => x.Name);
        }
    }
}

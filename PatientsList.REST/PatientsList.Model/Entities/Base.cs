using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;

namespace PatientsList.Model.Entities
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
    }

    public class BaseClassMap<T> : ClassMap<T> where T : BaseEntity
    {
        public BaseClassMap()
        {
            Id(x => x.Id).Not.Nullable();
        }
    }

}

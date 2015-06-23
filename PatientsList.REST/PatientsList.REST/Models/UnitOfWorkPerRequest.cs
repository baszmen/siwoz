using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatientsList.Model;

namespace PatientsList.REST.Models
{
    public class UnitOfWorkPerRequest
    {
        private const string key = "__UnitOfWork";

        public static IUnitOfWork Get()
        {
            var unitOfWork = HttpContext.Current.Items[key];
            if (unitOfWork == null)
            {
                unitOfWork = new UnitOfWork();
                HttpContext.Current.Items[key] = unitOfWork;
            }
            return (IUnitOfWork) unitOfWork;
        }

        public static void Dispose()
        {
            var uow = HttpContext.Current.Items[key];
            if (uow != null)
            {
                ((IUnitOfWork)uow).Dispose();
            }
        }
    }
}
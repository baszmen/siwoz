using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientsList.REST.Model
{
    public class Patient
    {
        public string Name { get; set; }
        public  DateTime CheckTime { get; set; }
    }
}
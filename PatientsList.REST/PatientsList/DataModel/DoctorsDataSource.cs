using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Newtonsoft.Json;

namespace PatientsList.DataModel
{
    public static class DoctorsDataSource
    {
        public static ObservableCollection<Doctor> _doctors;

        public static Doctor GetDoctor(int id)
        {
            return _doctors.FirstOrDefault(x => x.Id == id);
        }

        public async static Task<ObservableCollection<Doctor>> ActualizeDoctors()
        {
            try
            {
                var _docs = new List<Doctor>();
                var client = new HttpClient { BaseAddress = new Uri("http://localhost:59901/") };
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var responseMsg = client.GetAsync("api/Doctors");
                var resp = responseMsg.Result;
                if (!resp.IsSuccessStatusCode) _docs = new List<Doctor>();
                var jsonText = await resp.Content.ReadAsStringAsync();
                _docs = JsonConvert.DeserializeObject<List<Doctor>>(jsonText);

                foreach (var d in _docs)
                {
                    var doctor = _doctors.FirstOrDefault(x => x.Id == d.Id);
                    if (doctor == null)
                    {
                        _doctors.Add(d);
                        foreach (var p in d.PatientsList)
                            p.TimeStuck = d.TimeStuck;
                        continue;
                    }

                    foreach (var p in d.PatientsList)
                    {
                        var patient = doctor.PatientsList.FirstOrDefault(x => x.Id == p.Id);

                        //dodawanie
                        if (patient == null)
                        {
                            p.TimeStuck = doctor.TimeStuck;
                            doctor.PatientsList.Add(p);
                            continue;
                        }
                        // edycja
                        if (patient.VisitTime != p.VisitTime || patient.Duration != p.Duration)
                        {
                            patient.Duration = p.Duration;
                            patient.VisitTime = p.VisitTime;
                            patient.TimeStuck = doctor.TimeStuck;
                            patient.Inside = false;
                        }
                    }

                    // usuwanie
                    foreach (var p in doctor.PatientsList)
                    {
                        var patient = d.PatientsList.FirstOrDefault(x => x.Id == p.Id);
                        if (patient == null)
                        {
                            p.TimeStuck = null;
                            doctor.PatientsList.Remove(p);
                        }

                    }
                }
            }
            catch (Exception)
            {
                return _doctors;
            }
            return _doctors;
        }

        public async static Task<ObservableCollection<Doctor>> GetDoctors()
        {
            await DoctorsFromREST();
            if (_doctors != null)
                foreach (var t in _doctors)
                    foreach (var pat in t.PatientsList)
                        pat.TimeStuck = t.TimeStuck;
            return _doctors;
        }

        private static async Task DoctorsFromREST()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:59901/") };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var responseMsg = client.GetAsync("api/Doctors");
            var resp = responseMsg.Result;
            if (!resp.IsSuccessStatusCode) _doctors = new ObservableCollection<Doctor>();
            var jsonText = await resp.Content.ReadAsStringAsync();
            if (_doctors != null)
                _doctors.Clear();
            _doctors = JsonConvert.DeserializeObject<ObservableCollection<Doctor>>(jsonText);
        }
    }
}

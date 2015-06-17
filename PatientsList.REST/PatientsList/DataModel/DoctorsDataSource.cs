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
        private static string _token = "1";
        private static ObservableCollection<Doctor> _doctors;

        public static Doctor GetDoctor(int id)
        {
            return _doctors.FirstOrDefault(x => x.Id == id);
        }

        public async static Task<bool> ActualizeDoctors()
        {
            try
            {
                var _docs = new List<Doctor>();
                var client = new HttpClient {BaseAddress = new Uri("http://localhost:59901/")};
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var responseMsg = client.GetAsync("api/Doctors");
                var resp = responseMsg.Result;
                if (!resp.IsSuccessStatusCode) _docs = new List<Doctor>();
                var jsonText = await resp.Content.ReadAsStringAsync();
                _docs = JsonConvert.DeserializeObject<List<Doctor>>(jsonText);
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        foreach (var d in _docs)
                        {
                            var doc = _doctors.FirstOrDefault(x => x.Id == d.Id);
                            if (doc == null)
                            {
                                _doctors.Add(d);
                                continue;
                            }
                            doc.Name = d.Name;
                            doc.Surname = d.Surname;
                            doc.Titles = d.Titles;
                            doc.PatientsList = new ObservableCollection<Patient>();
                            foreach (var p in d.PatientsList)
                            {
                                doc.PatientsList.Add(p);
                                //var pat = doc.PatientsList.FirstOrDefault(x => x.Id == p.Id);
                                //if (pat == null)
                                //    doc.PatientsList.Add(p);
                                //else
                                //{
                                //    pat.VisitTime = p.VisitTime;
                                //    pat.Name = p.Name;
                                //}
                            }

                            //var toRemovePatients = doc.PatientsList.Where(x => _docs.All(y => y.Id != x.Id)).ToList();
                            //foreach (var tr in toRemovePatients)
                            //    doc.PatientsList.Remove(tr);
                        }
                        //var toRemove = _doctors.Where(d => _docs.All(x => x.Id != d.Id)).ToList();
                        //foreach (var tr in toRemove)
                        //    _doctors.Remove(tr);
                    });
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public async static Task<ObservableCollection<Doctor>> GetDoctors()
        {
            if (_doctors != null) return _doctors;
            await DoctorsFromREST();
            return _doctors;
        }

        private static async Task DoctorsFromREST()
        {
            var client = new HttpClient {BaseAddress = new Uri("http://localhost:59901/")};
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var responseMsg = client.GetAsync("api/Doctors");
            var resp = responseMsg.Result;
            if (!resp.IsSuccessStatusCode) _doctors = new ObservableCollection<Doctor>();
            var jsonText = await resp.Content.ReadAsStringAsync();
            _doctors = JsonConvert.DeserializeObject<ObservableCollection<Doctor>>(jsonText);
        }
    }
}

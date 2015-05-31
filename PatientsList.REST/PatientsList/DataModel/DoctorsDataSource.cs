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
using Newtonsoft.Json;

namespace PatientsList.DataModel
{
    public static class DoctorsDataSource
    {
        private static ObservableCollection<Doctor> _doctors;

        public static Doctor GetDoctor(int id)
        {
            return _doctors.FirstOrDefault(x => x.Id == id);
        }

        public async static Task<ObservableCollection<Doctor>> GetDoctors()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:59901/") };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var responseMsg = client.GetAsync("api/Doctors");
            var resp = responseMsg.Result;
            if (!resp.IsSuccessStatusCode) _doctors = new ObservableCollection<Doctor>();
            var jsonText = await resp.Content.ReadAsStringAsync();
            _doctors = JsonConvert.DeserializeObject<ObservableCollection<Doctor>>(jsonText);

            return _doctors;
        }
    }
}

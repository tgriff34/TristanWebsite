﻿using Newtonsoft.Json;
using System.Net.Http.Headers;
using TristanWebsite.Models;

namespace TristanWebsite
{
    public sealed class ActivitiesAPI
    {
        private static ActivitiesAPI? instance = null;
        private static readonly HttpClient _httpClient = new HttpClient();
        private string access_token = "465cfbcb86982dbc6759c27aec3437ed2d4600b2";
        private string refresh_token = "7ee68e8dcfb7ede11bb720ddcd7b3cda41bb366b";

        private ActivitiesAPI() {
            
        }

        public static ActivitiesAPI Instance()
        {
            if (instance == null)
            {
                instance = new ActivitiesAPI();
            }
            return instance;
        }

        public async Task<List<Activities>> GetActivities()
        {
            HttpResponseMessage output;

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.strava.com/api/v3/athlete/activities"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                output = await _httpClient.SendAsync(requestMessage);
            }

            string json = await output.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<Activities>>(json);

            foreach (var entity in result!)
            {
                System.Diagnostics.Debug.WriteLine(entity.Id);
                System.Diagnostics.Debug.WriteLine(entity.Name);
                System.Diagnostics.Debug.WriteLine(entity.Distance);
                System.Diagnostics.Debug.WriteLine(entity.Moving_Time);
                System.Diagnostics.Debug.WriteLine(entity.Type);
                System.Diagnostics.Debug.WriteLine(entity.Average_Speed * 2.237);
                System.Diagnostics.Debug.WriteLine(entity.Max_Speed * 2.237);
                System.Diagnostics.Debug.WriteLine(entity.Average_Cadence);
                System.Diagnostics.Debug.WriteLine(entity.Average_Watts);
                System.Diagnostics.Debug.WriteLine(entity.Max_Watts);
                System.Diagnostics.Debug.WriteLine(entity.Average_Heartrate);
                System.Diagnostics.Debug.WriteLine(entity.Max_Heartrate);
            }

            return result;
        }

        public async Task<Athlete> GetAthlete()
        {
            HttpResponseMessage output;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.strava.com/api/v3/athlete"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                output = await _httpClient.SendAsync(requestMessage);
            }
            string json = await output.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Athlete>(json)!;
        }
    }
}
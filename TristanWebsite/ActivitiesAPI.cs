using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using TristanWebsite.Models;

namespace TristanWebsite
{
    public sealed class ActivitiesAPI
    {
        private static ActivitiesAPI? instance = null;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string? access_token;

        private ActivitiesAPI() {
            
        }

        public static ActivitiesAPI Instance()
        {
            if (instance == null)
            {
                IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<ActivitiesAPI>().Build();
                access_token = config["StravaAPIKey"];
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

            var result = JsonConvert.DeserializeObject<List<Activities>>(json)!;

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

        public async Task<ActivityStats> GetAthleteStats(Athlete athlete)
        {
            HttpResponseMessage output;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://www.strava.com/api/v3/athletes/{athlete.Id}/stats"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                output = await _httpClient.SendAsync(requestMessage);
            }
            string json = await output.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ActivityStats>(json)!;
        }
    }
}

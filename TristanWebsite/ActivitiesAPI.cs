using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using TristanWebsite.Models;

namespace TristanWebsite
{
    public sealed class ActivitiesAPI
    {
        private static ActivitiesAPI? instance = null;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string? access_token, maps_key;

        private ActivitiesAPI() {
            
        }

        public static ActivitiesAPI Instance()
        {
            if (instance == null)
            {
                IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<ActivitiesAPI>().Build();
                access_token = config["StravaAPIKey"];
                maps_key = config["MapsAPIKey"];
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

            foreach (var activity in result)
            {
                activity.Map = GetMapImgSrc(activity.Map);
                //activity.Location = await GetLocation(activity.Start_latlng);
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

        public async Task<string> GetLocation(float[] latlng)
        {
            float lat = latlng[0];
            float lng = latlng[1];

            HttpResponseMessage output;
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.mapbox.com/search/geocode/v6/reverse?longitude={lng}&latitude={lat}&access_token={maps_key}"))
            {
                requestMessage.Headers.Referrer = new Uri("https://localhost");
                output = await _httpClient.SendAsync(requestMessage);
            }
            string json = await output.Content.ReadAsStringAsync();

            Location location = JsonConvert.DeserializeObject<Location>(json)!;

            return $"{location.Features[0].Properties.Context.Place.Name}, {location.Features[0].Properties.Context.Region.Name}";
        }

        public Map GetMapImgSrc(Map map)
        {
            string polyline = map.Summary_Polyline;
            string encodedPolyline = "";

            encodedPolyline = polyline.Replace(@"\\", @"\");
            encodedPolyline = HttpUtility.UrlEncode(encodedPolyline);

            map.MapImageURL = $"https://api.mapbox.com/styles/v1/mapbox/streets-v12/static/path({encodedPolyline})/auto/600x400?access_token={maps_key}";

            return map;
        }
    }
}

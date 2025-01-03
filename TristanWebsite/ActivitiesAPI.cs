﻿using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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
        private class APIKey
        {
            public string Access_token { get; set; }
            public long Expires_at {  get; set; }
        }

        private static ActivitiesAPI? instance = null;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string? access_token, maps_key, refresh_token, client_id, client_secret;
        private static string? refresh_api_base, client_param, client_secret_param, refresh_token_param, refresh_api_end;
        private static long? access_token_expires;

        private ActivitiesAPI() {
            
        }

        public static ActivitiesAPI Instance(API? api)
        {
            if (instance == null)
            {
                client_id = api?.client_id;
                client_secret = api?.client_secret;
                refresh_token = api?.refresh_token;
                maps_key = api?.maps_key;
                refresh_api_base = api?.refresh_api_base;
                client_param = api?.client_param;
                client_secret_param = api?.client_secret_param;
                refresh_token_param = api?.refresh_token_param;
                refresh_api_end = api?.refresh_api_end;

                GetKey();

                instance = new ActivitiesAPI();
            }
            return instance;
        }

        private static void GetKey()
        {
            HttpResponseMessage? output = null;

            long now = DateTimeOffset.Now.ToUnixTimeSeconds();

            if (now < access_token_expires && access_token != null) {
                return;
            }
            
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{refresh_api_base}{client_param}{client_id}{client_secret_param}{client_secret}{refresh_token_param}{refresh_token}{refresh_api_end}"))
            {
                output = _httpClient.Send(requestMessage);
            }

            try
            {
                output.EnsureSuccessStatusCode();

                string json = output.Content.ReadAsStringAsync().Result;

                APIKey apiKey = JsonConvert.DeserializeObject<APIKey>(json)!;

                access_token_expires = apiKey.Expires_at;
                access_token = apiKey.Access_token;
            }
            catch (Exception) {
                Debug.WriteLine("Failed to get access token");
            }

        }

        public async Task<List<Activities>>? GetActivities()
        {
            HttpResponseMessage output;
            List<Activities>? activities = null;

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.strava.com/api/v3/athlete/activities"))
            {
                GetKey();
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                output = await _httpClient.SendAsync(requestMessage);
            }

            try
            {
                output.EnsureSuccessStatusCode();
                string json = await output.Content.ReadAsStringAsync();

                activities = JsonConvert.DeserializeObject<List<Activities>>(json)!;
                foreach (var activity in activities)
                {
                    activity.Map = GetMapImgSrc(activity.Map);
                    //activity.Location = await GetLocation(activity.Start_latlng);
                }
            }
            catch (Exception) {
                Debug.WriteLine("Failed to get activities");
            }

            return activities!;
        }

        public async Task<Athlete>? GetAthlete()
        {
            HttpResponseMessage output;
            Athlete? athlete = null;


            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.strava.com/api/v3/athlete"))
            {
                GetKey();
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                output = await _httpClient.SendAsync(requestMessage);
            }

            try
            {
                output.EnsureSuccessStatusCode();
                string json = await output.Content.ReadAsStringAsync();
                athlete = JsonConvert.DeserializeObject<Athlete>(json);
            }
            catch (Exception) { 
                Debug.WriteLine("Failed to get athlete");
            }
                
            return athlete!;
        }

        public async Task<ActivityStats>? GetAthleteStats(Athlete athlete)
        {
            HttpResponseMessage output;
            ActivityStats? activityStats = null;

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://www.strava.com/api/v3/athletes/{athlete.Id}/stats"))
            {
                GetKey();
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                output = await _httpClient.SendAsync(requestMessage);
            }

            try
            {
                output.EnsureSuccessStatusCode();
                string json = await output.Content.ReadAsStringAsync();
                activityStats = JsonConvert.DeserializeObject<ActivityStats>(json);
            }
            catch (Exception) {
                Debug.WriteLine("Failed to get activity stats");
            }

            return activityStats!;
        }

        public async Task<string>? GetLocation(float[] latlng)
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

            Location? location = JsonConvert.DeserializeObject<Location>(json);

            return $"{location?.Features[0].Properties.Context.Place.Name}, {location?.Features[0].Properties.Context.Region.Name}";
        }

        public Map GetMapImgSrc(Map map)
        {
            string polyline = map.Summary_Polyline;
            string encodedPolyline = "";

            encodedPolyline = HttpUtility.UrlEncode(polyline);

            map.MapImageURL = $"https://api.mapbox.com/styles/v1/mapbox/streets-v12/static/path({encodedPolyline})/auto/600x400?access_token={maps_key}";

            return map;
        }
    }
}

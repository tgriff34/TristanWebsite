﻿using TristanWebsite.Models;

namespace TristanWebsite
{
    public static class Utilities
    {
        public static void parseData(Athlete? athlete, List<Activities>? activities)
        {
            if (athlete != null)
            {
                athlete.ActivityStats.Biggest_Ride_Distance = athlete.ActivityStats.Biggest_Ride_Distance / 1609;
                athlete.ActivityStats.Biggest_Climb_Elevation_Gain = athlete.ActivityStats.Biggest_Climb_Elevation_Gain * (float)3.281;

                // All-Time-Rides
                athlete.ActivityStats.All_Ride_totals.Distance = athlete.ActivityStats.All_Ride_totals.Distance / 1609;
                athlete.ActivityStats.All_Ride_totals.Elevation_Gain = athlete.ActivityStats.All_Ride_totals.Elevation_Gain * (float)3.281;
                athlete.ActivityStats.All_Ride_totals.Elaspsed_Time_Str = GetMovingTime(athlete.ActivityStats.All_Ride_totals.Elapsed_Time);
                athlete.ActivityStats.All_Ride_totals.Moving_Time_Str = GetMovingTime(athlete.ActivityStats.All_Ride_totals.Moving_Time);

                // Recent Rides
                athlete.ActivityStats.Recent_Ride_Totals.Distance = athlete.ActivityStats.Recent_Ride_Totals.Distance / 1609;
                athlete.ActivityStats.Recent_Ride_Totals.Elevation_Gain = athlete.ActivityStats.Recent_Ride_Totals.Elevation_Gain * (float)3.281;
                athlete.ActivityStats.Recent_Ride_Totals.Elaspsed_Time_Str = GetMovingTime(athlete.ActivityStats.Recent_Ride_Totals.Elapsed_Time);
                athlete.ActivityStats.Recent_Ride_Totals.Moving_Time_Str = GetMovingTime(athlete.ActivityStats.Recent_Ride_Totals.Moving_Time);
            }

            if (activities != null)
            {
                foreach (var entity in activities)
                {
                    DateTime dateTime = DateTime.Parse(entity.Start_Date);
                    dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, "Eastern Standard Time");
                    entity.Start_Date_Formatted = $"{GetMonthName(dateTime.Month)} {dateTime.Day}, {dateTime.Year}";
                    entity.Start_Time_Formatted = $"{GetHourAMPM(dateTime)}";
                    entity.Moving_Time_Str = GetMovingTime(entity.Moving_Time);
                    entity.Distance = entity.Distance / 1609;
                    entity.Total_Elevation_Gain = entity.Total_Elevation_Gain * (float)3.281;
                    entity.Average_Speed = entity.Average_Speed * (float)2.237;
                    entity.Max_Speed = entity.Max_Speed * (float)2.237;
                }
            }

            return;
        }
        private static string GetMovingTime(long movingTime)
        {
            TimeSpan time = TimeSpan.FromSeconds(movingTime);
            string result = string.Format("{0}h {1}m {2}s", (long)time.TotalHours, time.Minutes, time.Seconds);
            return result;
        }
        private static string GetHourAMPM(DateTime dateTime)
        {
            string result = "";
            string suffix = "";

            if (dateTime.Hour / 12 == 0)
            {
                result = $"{dateTime.Hour}";
                suffix = "AM";
            } 
            else
            {
                result = $"{dateTime.Hour / 12}";
                suffix = "PM";
            }
            result += $":{(dateTime.Minute < 10 ? "0" : "")}{dateTime.Minute} {suffix}";

            return result;
        }
        private static string GetMonthName(int month)
        {
            string result = "";
            switch (month)
            {
                case 1:
                    result = "Jan";
                    break;
                case 2:
                    result = "Feb";
                    break;
                case 3:
                    result = "Mar";
                    break;
                case 4:
                    result = "Apr";
                    break;
                case 5:
                    result = "May";
                    break;
                case 6:
                    result = "June";
                    break;
                case 7:
                    result = "July";
                    break;
                case 8:
                    result = "Aug";
                    break;
                case 9:
                    result = "Sep";
                    break;
                case 10:
                    result = "Oct";
                    break;
                case 11:
                    result = "Nov";
                    break;
                case 12:
                    result = "Dec";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}

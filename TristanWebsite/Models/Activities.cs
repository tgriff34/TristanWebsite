﻿namespace TristanWebsite.Models
{
    public class Activities
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Distance { get; set; }
        public int Moving_Time { get; set; }
        public string Moving_Time_Str { get; set; }
        public float Total_Elevation_Gain { get; set; }
        public string Type { get; set; }
        public string Start_Date { get; set; }
        public string Start_Date_Formatted { get; set; }
        public string Start_Time_Formatted { get; set; }
        public float Average_Speed { get; set; }
        public float Max_Speed { get; set; }
        public float Average_Cadence { get; set; }
        public float Average_Watts { get; set; }
        public float Max_Watts { get; set; }
        public float Average_Heartrate { get; set; }
        public float Max_Heartrate { get; set; }
    }
}
namespace TristanWebsite.Models
{
    public class ActivityStats
    {
        public double Biggest_Ride_Distance { get; set; }
        public double Biggest_Climb_Elevation_Gain { get; set; }
        public ActivityTotal Recent_Ride_Totals { get; set; }
        public ActivityTotal YTD_Ride_Totals { get; set; }
        public ActivityTotal All_Ride_totals { get; set; }
    }
}

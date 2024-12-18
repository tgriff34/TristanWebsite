using System.ComponentModel.DataAnnotations;

namespace TristanWebsite.Models
{
    public class ActivityTotal
    {
        public int Count { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float Distance { get; set; }
        public long Moving_Time { get; set; }
        public string Moving_Time_Str { get; set; }
        public long Elapsed_Time { get; set; }
        public string Elaspsed_Time_Str { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public float Elevation_Gain { get; set; }
        public int Achievement_Count { get; set; }
    }
}

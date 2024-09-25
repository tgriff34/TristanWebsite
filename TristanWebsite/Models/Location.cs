namespace TristanWebsite.Models
{
    public class Location
    {
        public List<Features> Features { get; set; }
    }

    public class Features
    {
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        public Context Context { get; set; }
    }

    public class Context
    {
        public Place Place { get; set; }
        public Region Region { get; set; }
    }

    public class Place
    {
        public string Name { get; set; }
    }
    public class Region
    { 
        public string Name { get; set; } 
    }
}

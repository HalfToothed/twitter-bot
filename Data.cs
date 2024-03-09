
namespace Data
{
    public class Root
    {
        public Result[] result { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public Provider provider { get; set; }
        public Vehicle vehicle { get; set; }
        public Pad pad { get; set; }
        public string mission_description { get; set; }
        public string launch_description { get; set; }
        public string t0 { get; set; }
    }

    public class Pad
    {
        public Location location { get; set; }
    }

    public class Location
    {
        public string name { get; set; }
        public string country { get; set; }
    }

    public class Provider
    {
        public string name { get; set; }
    }

    public class Vehicle
    {
        public string name { get; set; }
    }
}
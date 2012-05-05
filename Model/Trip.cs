using System;

namespace Model
{
    public class Trip
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public string PointOfDeparture { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public int TripLengthInDays { get; set; }
        public string Description { get; set; }
    }
}
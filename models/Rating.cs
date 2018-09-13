using System;

namespace Oteam15
{
    public class Rating
    {
        public Guid id { get; set; }
        public string userId { get; set; }
        public string productId { get; set; }
        public DateTime timestamp { get; set; }
        public string locationName { get; set; }
        public int rating { get; set; }
        public string userNotes { get; set; }
        public double sentimentScore{get; set;}
    }
}
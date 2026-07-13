using System;

namespace SeenCL.Domain.Entities
{
    public class Subscription
    {
        public int SubscriptionID { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int MaxPlayers { get; set; }
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

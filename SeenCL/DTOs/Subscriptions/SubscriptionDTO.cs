namespace SeenCL.DTOs
{
    public class SubscriptionDTO
    {
        public int SubscriptionID { get; set; }
        public string PlanName { get; set; }
        public string? Description { get; set; }
        public int MaxPlayers { get; set; }
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public SubscriptionDTO(int SubscriptionID, string PlanName, string? Description, int MaxPlayers,
            int DurationDays, decimal Price, DateTime CreatedAt)
        {
            this.SubscriptionID = SubscriptionID;
            this.PlanName = PlanName;
            this.Description = Description;
            this.MaxPlayers = MaxPlayers;
            this.DurationDays = DurationDays;
            this.Price = Price;
            this.CreatedAt = CreatedAt;
        }
    }
}

namespace calorietraker.Model
{
    public class Achievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    }
}

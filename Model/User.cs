namespace calorietraker.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int DailyCalorieGoal { get; set; } = 2000;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

namespace calorietraker.Model
{
    public class Goal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TargetType { get; set; }
        public decimal? TargetValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}

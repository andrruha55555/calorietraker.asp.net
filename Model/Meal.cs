namespace calorietraker.Model
{
    public class Meal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public decimal WeightGrams { get; set; }
        public DateTime ConsumedAt { get; set; } = DateTime.UtcNow;
    }
}

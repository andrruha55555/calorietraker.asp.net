﻿namespace calorietraker.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Calories { get; set; }
        public decimal? Proteins { get; set; }
        public decimal? Fats { get; set; }
        public decimal? Carbs { get; set; }
    }
}

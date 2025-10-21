using calorietraker.Model;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace calorietraker.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Meal> Meals => Set<Meal>();
        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<Achievement> Achievements => Set<Achievement>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseMySql("server=127.0.0.1;uid=root;pwd=;database=CalorieTracker",
                new MySqlServerVersion(new Version(8, 0, 11)));

        protected override void OnModelCreating(ModelBuilder m)
        {
            m.Entity<User>().ToTable("users");
            m.Entity<User>().Property(x => x.Id).HasColumnName("id");
            m.Entity<User>().Property(x => x.Username).HasColumnName("username");
            m.Entity<User>().Property(x => x.Email).HasColumnName("email");
            m.Entity<User>().Property(x => x.PasswordHash).HasColumnName("password_hash");
            m.Entity<User>().Property(x => x.DailyCalorieGoal).HasColumnName("daily_calorie_goal");
            m.Entity<User>().Property(x => x.CreatedAt).HasColumnName("created_at");

            m.Entity<Product>().ToTable("products");
            m.Entity<Product>().Property(x => x.Id).HasColumnName("id");
            m.Entity<Product>().Property(x => x.Name).HasColumnName("name");
            m.Entity<Product>().Property(x => x.Calories).HasColumnName("calories");
            m.Entity<Product>().Property(x => x.Proteins).HasColumnName("proteins");
            m.Entity<Product>().Property(x => x.Fats).HasColumnName("fats");
            m.Entity<Product>().Property(x => x.Carbs).HasColumnName("carbs");

            m.Entity<Meal>().ToTable("meals");
            m.Entity<Meal>().Property(x => x.Id).HasColumnName("id");
            m.Entity<Meal>().Property(x => x.UserId).HasColumnName("user_id");
            m.Entity<Meal>().Property(x => x.ProductId).HasColumnName("product_id");
            m.Entity<Meal>().Property(x => x.WeightGrams).HasColumnName("weight_grams");
            m.Entity<Meal>().Property(x => x.ConsumedAt).HasColumnName("consumed_at");

            m.Entity<Goal>().ToTable("goals");
            m.Entity<Goal>().Property(x => x.Id).HasColumnName("id");
            m.Entity<Goal>().Property(x => x.UserId).HasColumnName("user_id");
            m.Entity<Goal>().Property(x => x.TargetType).HasColumnName("target_type");
            m.Entity<Goal>().Property(x => x.TargetValue).HasColumnName("target_value");
            m.Entity<Goal>().Property(x => x.StartDate).HasColumnName("start_date");
            m.Entity<Goal>().Property(x => x.EndDate).HasColumnName("end_date");
            m.Entity<Goal>().Property(x => x.IsCompleted).HasColumnName("is_completed");

            m.Entity<Achievement>().ToTable("achievements");
            m.Entity<Achievement>().Property(x => x.Id).HasColumnName("id");
            m.Entity<Achievement>().Property(x => x.UserId).HasColumnName("user_id");
            m.Entity<Achievement>().Property(x => x.Name).HasColumnName("name");
            m.Entity<Achievement>().Property(x => x.Description).HasColumnName("description");
            m.Entity<Achievement>().Property(x => x.EarnedAt).HasColumnName("earned_at");
        }
    }
}

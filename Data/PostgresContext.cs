using System.ComponentModel.DataAnnotations.Schema;
using KitchenKitApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KitchenKitApi.Data;

public class PostgresContext : DbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
    {
    }

    public DbSet<User> users { get; set; }
    
    public DbSet<Recipe> recipes { get; set; }
    
    public DbSet<Step> steps { get; set; }
    
    public DbSet<Ingredient> ingredients { get; set; }
    
    public DbSet<FavoriteRecipe> favorites { get; set; }
    
    public DbSet<MealPlanItem> mealplanitems { get; set; }
}
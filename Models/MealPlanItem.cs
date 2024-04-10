using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenKitApi.Models;

public class MealPlanItem
{
    /// <summary>
    /// The ID of the meal plan item in the mealplanitems table.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The ID of the user to which the meal plan item belongs.
    /// </summary>
    [Required]
    [Column("userid")]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// The ID of the recipe that is part of this meal plan item.
    /// </summary>
    [Required]
    [Column("recipeid")]
    public Guid RecipeId { get; set; }
    
    /// <summary>
    /// The date for when the recipe is planned.
    /// </summary>
    [Required]
    [Column("date")]
    public DateTime Date { get; set; }
    
    /// <summary>
    /// The Recipe of the meal plan item.
    /// </summary>
    public Recipe Recipe { get; set; }
}
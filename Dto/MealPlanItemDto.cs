using System.ComponentModel.DataAnnotations;

namespace KitchenKitApi.Dto;

public class MealPlanItemDto
{
    /// <summary>
    /// The ID of the recipe.
    /// </summary>
    [Required]
    public Guid RecipeId { get; set; }
    
    /// <summary>
    /// The date for when the recipe is planned.
    /// </summary>
    [Required]
    public DateTime Date { get; set; }
}
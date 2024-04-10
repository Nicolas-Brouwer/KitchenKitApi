using System.ComponentModel.DataAnnotations;

namespace KitchenKitApi.Dto;

public class RecipeDto
{
    /// <summary>
    /// The name of the recipe.
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// The description of the recipe.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// The amount of servings the recipe is.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public int Servings { get; set; }

    /// <summary>
    /// A list of steps used in the recipe.
    /// </summary>
    public List<StepDto> Steps { get; set; } = new();

    /// <summary>
    /// A list of ingredients needed for the recipe.
    /// </summary>
    public List<IngredientDto> Ingredients { get; set; } = new();
}
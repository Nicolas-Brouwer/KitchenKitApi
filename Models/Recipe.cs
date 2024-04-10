using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenKitApi.Models;

public class Recipe
{
    /// <summary>
    /// The ID a recipe has in the recipes tables.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The name of a recipe.
    /// </summary>
    [Required]
    [Column("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// A description of the recipe.
    /// </summary>
    [Column("description")]
    public string Description { get; set; }

    /// <summary>
    /// The amount of servings for this recipe.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    [Column("servings")]
    public int Servings { get; set; }
    
    /// <summary>
    /// A list of steps belonging to the recipe.
    /// </summary>
    public List<Step> Steps { get; set; } = new();

    /// <summary>
    /// A list of ingredients needed for the recipe.
    /// </summary>
    public List<Ingredient> Ingredients { get; set; } = new();
}
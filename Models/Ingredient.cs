using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenKitApi.Models;

public class Ingredient
{
    /// <summary>
    /// The ID an ingredient has in the ingredients table.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The name of an ingredient.
    /// </summary>
    [Column("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// The amount of that ingredient that is needed.
    /// </summary>
    [Column("amount")]
    public float Amount { get; set; }
    
    /// <summary>
    /// The measurement used for the ingredient amount.
    /// </summary>
    [Column("measurement")]
    public Measurements Measurement { get; set; }
    
    /// <summary>
    /// The ID of the recipe to which the ingredient belongs.
    /// </summary>
    [Column("recipeid")]
    public Guid RecipeId { get; set; }
}
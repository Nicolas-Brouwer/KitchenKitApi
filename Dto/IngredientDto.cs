using System.ComponentModel.DataAnnotations;
using KitchenKitApi.Models;

namespace KitchenKitApi.Dto;

public class IngredientDto
{
    /// <summary>
    /// The name of an ingredient.
    /// </summary>
    [Required]
    public string Name { get; set; }
    
    /// <summary>
    /// The amount of the ingredient needed.
    /// </summary>
    [Required]
    public float Amount { get; set; }
    
    /// <summary>
    /// The measurement used for the ingredient.
    /// </summary>
    [Required]
    public Measurements Measurement { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenKitApi.Models;

public class Step
{
    /// <summary>
    /// The ID of the step in the steps table.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The description of the step.
    /// </summary>
    [Required]
    [Column("description")]
    public string Description { get; set; }
    
    /// <summary>
    /// The ID of the recipe to which the step belongs.
    /// </summary>
    [Required]
    [Column("recipeid")]
    public Guid RecipeId { get; set; }
}
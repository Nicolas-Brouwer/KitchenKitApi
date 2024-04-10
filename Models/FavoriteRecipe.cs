using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenKitApi.Models;

public class FavoriteRecipe
{
    /// <summary>
    /// The ID the favorite recipe has in the favorites table..
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The ID of the recipe in the recipes table.
    /// </summary>
    [Required]
    [Column("recipeid")]
    public Guid RecipeId { get; set; }
    
    /// <summary>
    /// The ID the user has in the users table.
    /// </summary>
    [Required]
    [Column("userid")]
    public Guid UserId { get; set; }
}
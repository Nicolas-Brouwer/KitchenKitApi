using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KitchenKitApi.Models;

public class User
{
    /// <summary>
    /// The ID of the user.
    /// </summary>
    [Column("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The email address of the user.
    /// </summary>
    [Column("email")]
    public string Email { get; set; }
    
    /// <summary>
    /// The password of the user.
    /// </summary>
    [JsonIgnore]
    [Column("password")]
    public string Password { get; set; }
}
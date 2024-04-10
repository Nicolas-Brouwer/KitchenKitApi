using System.ComponentModel.DataAnnotations;

namespace KitchenKitApi.Dto;

public class StepDto
{
    /// <summary>
    /// The description of a step.
    /// </summary>
    [Required]
    public string Description { get; set; }
}
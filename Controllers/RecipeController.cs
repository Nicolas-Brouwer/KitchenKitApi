using KitchenKitApi.Data.Repositories;
using KitchenKitApi.Dto;
using KitchenKitApi.Helpers;
using KitchenKitApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace KitchenKitApi.Controllers;

[ApiController]
[Route("recipes")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeRepository _repository;

    public RecipeController(IRecipeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// This endpoint retrieves a list of recipes
    /// </summary>
    /// <param name="searchString">An optional search query to filter recipes by name</param>
    /// <returns>Returns a list of Recipes</returns>
    [HttpGet]
    public IActionResult GetRecipes([FromQuery] string? searchString)
    {
        List<Recipe> recipes;
        if (string.IsNullOrEmpty(searchString))
        {
            recipes = _repository.GetAll();
        }
        else
        {
            recipes = _repository.GetByName(searchString);
        }

        return Ok(recipes);
    }

    /// <summary>
    /// This endpoint retrieves a recipe by ID
    /// </summary>
    /// <param name="recipeId">The ID by which the recipe is searched</param>
    /// <returns>Returns a single recipe if the recipe exists</returns>
    [HttpGet("{recipeId}")]
    public IActionResult GetRecipeById(Guid recipeId)
    {
        Recipe? recipe = _repository.GetById(recipeId);
        if (recipe == null)
        {
            return NotFound();
        }
        
        return Ok(recipe);
    }

    /// <summary>
    /// This endpoint allows the creation of a new recipe.
    /// </summary>
    /// <param name="recipeDto">The DataTransferObject used for creating the recipe.</param>
    /// <returns>Returns the created recipe.</returns>
    [HttpPost]
    public IActionResult CreateRecipe([FromBody] RecipeDto recipeDto)
    {
        var recipeId = Guid.NewGuid();
        var recipe = new Recipe
        {
            Id = recipeId,
            Name = recipeDto.Name,
            Description = recipeDto.Description,
            Servings = recipeDto.Servings,
            Steps = recipeDto.Steps.Select(s => new Step()
            {
                Description = s.Description,
                RecipeId = recipeId
            }).ToList(),
            Ingredients = recipeDto.Ingredients.Select(i => new Ingredient()
            {
                Name = i.Name,
                Amount = i.Amount,
                Measurement = i.Measurement,
                RecipeId = recipeId
            }).ToList()
        };

        var result = _repository.Create(recipe);

        if (result == null)
        {
            return NotFound();
        }
        
        return Created();
    }

    /// <summary>
    /// This endpoint allows the updating of an existing recipe.
    /// </summary>
    /// <param name="recipe">The data of the recipe that needs to be updated.</param>
    /// <returns>Returns the updated recipe.</returns>
    [Authorize]
    [HttpPut]
    public IActionResult UpdateRecipe([FromBody] Recipe recipe)
    {
        return Ok(_repository.Update(recipe));
    }
}
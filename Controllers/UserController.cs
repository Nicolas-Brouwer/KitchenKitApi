using KitchenKitApi.Data.Repositories;
using KitchenKitApi.Dto;
using KitchenKitApi.Helpers;
using KitchenKitApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace KitchenKitApi.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase {

    private readonly IUserRespository _repository;

    public UserController(IUserRespository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// This endpoint allows user to register to the application.
    /// </summary>
    /// <param name="userDto">The DataTransferObject used for creating a new user.</param>
    /// <returns>Returns the created Status if successful and BadRequest if the users email address is already in use</returns>
    [HttpPost]
    public IActionResult Register([FromBody] UserDto userDto)
    {
        var dbUser = _repository.GetUserByEmail(userDto.Email);

        if (dbUser != null)
        {
            return BadRequest("The email address " + userDto.Email + " is already in use");
        }
        
        var user = new User()
        {
            Email = userDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
        };

        _repository.Create(user);

        return Created();
    }

    /// <summary>
    /// This endpoint allows a user to add a recipe to their favorites list.
    /// </summary>
    /// <param name="userId">The ID of the user making the request.</param>
    /// <param name="recipeId">The ID of the recipe that is added to the user's favorite list</param>
    /// <returns>Returns a created status code.</returns>
    [Authorize]
    [HttpPost("{userId}/favorites/{recipeId}")]
    public IActionResult AddFavoriteRecipe(Guid userId, Guid recipeId)
    {
        var favoriteRecipe = new FavoriteRecipe()
        {
            RecipeId = recipeId,
            UserId = userId
        };
        
        _repository.AddFavoriteRecipe(favoriteRecipe);

        return Created();
    }
    
    /// <summary>
    /// This endpoint allows a user to get all their favorite recipes.
    /// </summary>
    /// <param name="userId">The ID of the user making the request.</param>
    /// <returns>Returns a list of favorite recipes.</returns>
    [Authorize]
    [HttpGet("{userId}/favorites")]
    public IActionResult GetFavoriteRecipes(Guid userId)
    {
        return Ok(_repository.GetAllFavoriteRecipes(userId));
    }

    /// <summary>
    /// This endpoint allows a user to delete a recipe from their favorites list.
    /// </summary>
    /// <param name="userId">The ID of the user making the request.</param>
    /// <param name="recipeId">The ID of the recipe being removed from the favorites list.</param>
    /// <returns>Returns the 200 status code.</returns>
    [Authorize]
    [HttpDelete("{userId}/favorites/{recipeId}")]
    public IActionResult RemoveFavoriteRecipe(Guid userId, Guid recipeId)
    {
        _repository.DeleteFavoriteRecipe(userId, recipeId);
        
        return Ok();
    }

    /// <summary>
    /// This endpoint allows a user to get all the recipes in their meal plan.
    /// </summary>
    /// <param name="userId">The ID of the user making the request.</param>
    /// <returns>Returns a list of all meal plan items.</returns>
    [Authorize]
    [HttpGet("{userId}/meal-plan-items")]
    public IActionResult GetMealPlanItems(Guid userId)
    {
        return Ok(_repository.GetAllMealPlanItems(userId));
    }
    
    /// <summary>
    /// This endpoint allows a user to add a recipe to their meal plan.
    /// </summary>
    /// <param name="userId">The ID of the user making the request.</param>
    /// <param name="mealPlanItemDto">The DataTransferObject containing the data of the new meal plan item.</param>
    /// <returns>Returns the created status code.</returns>
    [Authorize]
    [HttpPost("{userId}/meal-plan-items")]
    public IActionResult AddMealPlanItem(Guid userId, [FromBody] MealPlanItemDto mealPlanItemDto)
    {
        var mealPlanItem = new MealPlanItem()
        {
            UserId = userId,
            RecipeId = mealPlanItemDto.RecipeId,
            Date = mealPlanItemDto.Date
        };
        
        _repository.CreateMealPlanItem(mealPlanItem);
        return Created();
    }
    
    /// <summary>
    /// This endpoint allows a user to remove a recipe from their meal plan.
    /// </summary>
    /// <param name="userId">The ID of the user making the request.</param>
    /// <param name="recipeId">The ID of the recipe that is being removed from the meal plan.</param>
    /// <returns>Returns the 200 Ok status code.</returns>
    [Authorize]
    [HttpDelete("{userId}/meal-plan-items/{recipeId}")]
    public IActionResult GetMealPlanItems(Guid userId, Guid recipeId)
    {
        _repository.DeleteMealPlanItem(userId, recipeId);
        
        return Ok();
    }
}

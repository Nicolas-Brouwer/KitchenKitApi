using KitchenKitApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KitchenKitApi.Data.Repositories;

public class UserRepository : IUserRespository
{
    private readonly PostgresContext _context;
    
    public UserRepository(PostgresContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Get all users from the database.
    /// </summary>
    /// <returns>Returns a list of users.</returns>
    public List<User> GetAll()
    {
        return _context.users.ToList();
    }

    /// <summary>
    /// Get a user by their ID.
    /// </summary>
    /// <param name="id">The ID that is being searched for.</param>
    /// <returns>Returns a single nullable user.</returns>
    public User? GetById(Guid id)
    {
        return _context.users.FirstOrDefault(user => user.Id == id);
    }

    /// <summary>
    /// Add a new user to the database.
    /// </summary>
    /// <param name="user">The user being added to the database.</param>
    /// <returns>Returns the user that was added to the database.</returns>
    public User Create(User user)
    {
        _context.users.Add(user);
        _context.SaveChanges();

        return user;
    }
    
    /// <summary>
    /// Updates an existing user in the database.
    /// </summary>
    /// <param name="user">The user with the updated data.</param>
    /// <returns>Returns the updated user.</returns>
    public User Update(User user)
    {
        _context.users.Update(user);
        _context.SaveChanges();

        return user;
    }

    /// <summary>
    /// Get a user by their email address.
    /// </summary>
    /// <param name="email">The email address being searched for.</param>
    /// <returns>Returns a nullable user.</returns>
    public User? GetUserByEmail(string email)
    {
        return _context.users.FirstOrDefault(user => user.Email == email);
    }

    /// <summary>
    /// Add a favorite recipe to the database.
    /// </summary>
    /// <param name="favoriteRecipe">The recipe being added to the database.</param>
    public void AddFavoriteRecipe(FavoriteRecipe favoriteRecipe)
    {
        _context.favorites.Add(favoriteRecipe);
        _context.SaveChanges();
    }

    /// <summary>
    /// Get a list of favorite recipes by user ID.
    /// </summary>
    /// <param name="userId">The ID of the user for which the favorite recipes are searched.</param>
    /// <returns>Returns a list of recipes.</returns>
    public List<Recipe> GetAllFavoriteRecipes(Guid userId)
    {
        var favoriteRecipeIds = _context.favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.RecipeId)
            .ToList();

        return _context.recipes
            .Where(r => favoriteRecipeIds.Contains(r.Id))
            .Include(r => r.Steps)
            .Include(r => r.Ingredients)
            .ToList();
    }

    /// <summary>
    /// Deletes a favorite recipe from the database.
    /// </summary>
    /// <param name="userId">The ID of the user deleting the recipe.</param>
    /// <param name="recipeId">The ID of the recipe being deleted.</param>
    public void DeleteFavoriteRecipe(Guid userId, Guid recipeId)
    {
        _context.favorites
            .Where(f => f.RecipeId == recipeId && f.UserId == userId)
            .ExecuteDelete();
    }

    /// <summary>
    /// Deletes a recipe from the meal plan.
    /// </summary>
    /// <param name="userId">The ID of the user deleting the recipe.</param>
    /// <param name="recipeId">The ID of the recipe being deleted.</param>
    public void DeleteMealPlanItem(Guid userId, Guid recipeId)
    {
        _context.mealplanitems
            .Where(m => m.Recipe.Id == recipeId && m.UserId == userId)
            .ExecuteDelete();
    }

    /// <summary>
    /// Get all recipes in the meal plan of a user.
    /// </summary>
    /// <param name="userId">The ID of the user retrieving their meal plan.</param>
    /// <returns>Returns a list of meal plan items.</returns>
    public List<MealPlanItem> GetAllMealPlanItems(Guid userId)
    {
        return _context.mealplanitems
            .Where(m => m.UserId == userId)
            .Include(m => m.Recipe)
            .ThenInclude(r => r.Steps)
            .Include(m => m.Recipe)
            .ThenInclude(r => r.Ingredients)
            .ToList();
    }

    /// <summary>
    /// Adds a new meal plan item to the database.
    /// </summary>
    /// <param name="mealPlanItem">The meal plan item that is added to the database.</param>
    public void CreateMealPlanItem(MealPlanItem mealPlanItem)
    {
        _context.mealplanitems.Add(mealPlanItem);
        _context.SaveChanges();
    }
}
using KitchenKitApi.Models;
using Microsoft.EntityFrameworkCore;


namespace KitchenKitApi.Data.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly PostgresContext _context;

    public RecipeRepository(PostgresContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all recipes from the database.
    /// </summary>
    /// <returns>Returns a list of recipes.</returns>
    public List<Recipe> GetAll()
    {
        return _context.recipes
            .Include(r => r.Steps)
            .Include(r => r.Ingredients)
            .ToList();
    }

    /// <summary>
    /// Get a recipe by ID.
    /// </summary>
    /// <param name="id">The ID of the recipe that is searched for.</param>
    /// <returns>Returns a single recipe if found.</returns>
    public Recipe? GetById(Guid id)
    {
        return _context.recipes
            .Include(r => r.Steps)
            .Include(r => r.Ingredients)
            .FirstOrDefault(r => r.Id == id);
    }

    /// <summary>
    /// Add a new recipe to the database.
    /// </summary>
    /// <param name="recipe">The recipe added to the database.</param>
    /// <returns>Returns the recipe added to the database.</returns>
    public Recipe Create(Recipe recipe)
    {
        _context.recipes.Add(recipe);
        _context.steps.AddRange(recipe.Steps);
        _context.ingredients.AddRange(recipe.Ingredients);
        _context.SaveChanges();
        return recipe;
    }

    /// <summary>
    /// Update a recipe in the database.
    /// </summary>
    /// <param name="recipe">An updated instance of a recipe already stored in the database.</param>
    /// <returns>Returns the updated recipe.</returns>
    public Recipe Update(Recipe recipe)
    {
        var dbRecipe = _context.recipes
            .Include(r => r.Steps)
            .Include(r => r.Ingredients)
            .FirstOrDefault(r => r.Id == recipe.Id);
        
        if (dbRecipe == null)
        {
            return null;
        }
        
        dbRecipe.Name = recipe.Name;
        dbRecipe.Description = recipe.Description;
        dbRecipe.Servings = recipe.Servings;

        UpdateItems(
            dbRecipe.Steps.ToList(),
            recipe.Steps.ToList(),
            (Step dbStep, Step step) => dbStep.Id == step.Id,
            (Step dbStep, Step step) => dbStep.Description = step.Description,
            _context.steps
            );
        
        UpdateItems(
            dbRecipe.Ingredients.ToList(),
            recipe.Ingredients.ToList(),
            (Ingredient dbIngredient, Ingredient ingredient) => dbIngredient.Id == ingredient.Id,
            (Ingredient dbIngredient, Ingredient ingredient) =>
            {
                dbIngredient.Name = ingredient.Name;
                dbIngredient.Amount = ingredient.Amount;
                dbIngredient.Measurement = ingredient.Measurement;
            },
            _context.ingredients
        );
        
        _context.SaveChanges();
        
        return recipe;
    }

    /// <summary>
    /// Get recipes by name.
    /// </summary>
    /// <param name="searchString">The search query by which recipes will be filtered.</param>
    /// <returns>Returns a list of recipes that contain the search query in their name.</returns>
    public List<Recipe> GetByName(string searchString)
    {
        var loverCaseSearchString = searchString.ToLower();
        return _context.recipes
            .Where(r => r.Name.ToLower().Contains(loverCaseSearchString))
            .Include(r => r.Steps)
            .Include(r => r.Ingredients)
            .ToList();
    }

    /// <summary>
    /// A method used to update database tables related to a recipe.
    /// </summary>
    /// <param name="dbItems">A list of items retrieved from the database that will be updated.</param>
    /// <param name="items">A list of items that contain the updated data.</param>
    /// <param name="idComparator">A comparator used to determine if a dbItem will be removed or updated.</param>
    /// <param name="updater">The table fields that will be updated.</param>
    /// <param name="dbSet">The database table that will be updated.</param>
    /// <typeparam name="T">The data type of the items that are being updated.</typeparam>
    private void UpdateItems<T>(List<T> dbItems, List<T> items, Func<T, T, bool> idComparator, Action<T, T> updater, DbSet<T> dbSet) where T : class
    {
        foreach (var dbItem in dbItems)
        {
            if (items.All(item => !idComparator(dbItem, item)))
            {
                _context.Entry(dbItem).State = EntityState.Deleted;
            }
        }

        foreach (var item in items)
        {
            var dbItem = dbItems.FirstOrDefault(i => idComparator(i, item));
            if (dbItem != null)
            {
                updater(dbItem, item);
            }
            else
            {
                dbSet.Add(item);
            }
        }
    }
}
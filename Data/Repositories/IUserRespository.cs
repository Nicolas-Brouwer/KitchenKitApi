using KitchenKitApi.Models;

namespace KitchenKitApi.Data.Repositories;

public interface IUserRespository : IRepository<User>
{
    public User? GetUserByEmail(string email);

    public void AddFavoriteRecipe(FavoriteRecipe favoriteRecipe);

    public List<Recipe> GetAllFavoriteRecipes(Guid userId);

    public void DeleteFavoriteRecipe(Guid userId, Guid recipeId);

    public void DeleteMealPlanItem(Guid userId, Guid recipeId);
    
    public List<MealPlanItem> GetAllMealPlanItems(Guid userId);

    public void CreateMealPlanItem(MealPlanItem mealPlanItem);
}
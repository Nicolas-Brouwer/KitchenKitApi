using KitchenKitApi.Models;

namespace KitchenKitApi.Data.Repositories;

public interface IRecipeRepository : IRepository<Recipe>
{
    
    public List<Recipe> GetByName(string searchString);
}
namespace KitchenKitApi.Data.Repositories;

public interface IRepository<T>
{
    public List<T> GetAll();

    public T? GetById(Guid id);

    public T? Create(T t);

    public T Update(T t);

}
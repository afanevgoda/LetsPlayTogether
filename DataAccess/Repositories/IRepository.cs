namespace DataAccess.Repositories;

public interface IRepository<T>{

    public Task<T> Get(string id);

    public Task<List<T>> GetList(List<string> ids);

    public Task<string?> Add(T newObject);

    public void Delete(string id);

    public void UpdateList(List<T> updatedObjects);
    
    public Task Update(T updatedObject);
}
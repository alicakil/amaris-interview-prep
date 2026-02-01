using Amaris.Core.Models;

namespace Amaris.Data.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly Dictionary<int, T> _store = new();

    public T? GetById(int id)
    {
        _store.TryGetValue(id, out var entity);
        return entity;
    }

    public IEnumerable<T> GetAll() => _store.Values;

    public void Add(T entity)
    {
        if (_store.ContainsKey(entity.Id))
            throw new InvalidOperationException($"Entity with Id {entity.Id} already exists.");

        _store[entity.Id] = entity;
    }

    public void Update(T entity)
    {
        if (!_store.ContainsKey(entity.Id))
            throw new KeyNotFoundException($"Entity with Id {entity.Id} not found.");

        _store[entity.Id] = entity;
    }

    public void Delete(int id)
    {
        if (!_store.Remove(id))
            throw new KeyNotFoundException($"Entity with Id {id} not found.");
    }
}

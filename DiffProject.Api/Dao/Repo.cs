using System.Collections.Concurrent;

namespace DiffProject.Api.Dao;

public class DiffEntity
{
    public string? Left { get; set; }
    public string? Right { get; set; }
}

// just being flexible if some other type of repo would be used..
public interface IDiffRepository
{
    DiffEntity GetDiffEntity(string id);
    void UpdateDiffEntity(string id, DiffEntity entity);
}

//the InMem repo implementation
public class InMemoryRepository : IDiffRepository
{
    private readonly ConcurrentDictionary<string, DiffEntity> _db = new();
    public DiffEntity GetDiffEntity(string id) => _db.GetOrAdd(id, _ => new DiffEntity());
    public void UpdateDiffEntity(string id, DiffEntity entity) => _db[id] = entity;
}


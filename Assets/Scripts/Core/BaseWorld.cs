using System;
using System.Collections.Generic;

public interface IWorld
{
    public void Activate(WorldData data, params BaseSystem [] systems);
    public BaseEntity CreateNewObject(BaseData data);
    public void AddSystem(BaseSystem system);
    public void RemoveEntity(BaseEntity entity);
    public void RemoveSystem(BaseSystem system);
    public T GetSystem<T>();
    public void Deactivate();
}

public class BaseWorld : IWorld
{
    private Dictionary<BaseEntity, BaseData> _entities = new Dictionary<BaseEntity, BaseData>();
    private Dictionary<Type, BaseSystem> _systems = new Dictionary<Type, BaseSystem>();

    protected WorldData Data;
    
    public void Activate(WorldData data, params BaseSystem [] systems)
    {
        Data = data;

        foreach (var s in systems)
        {
            if (!_systems.ContainsKey(s.GetType()))
                _systems.Add(s.GetType(), s);
        }

        var count = data.Data.Count;
        for (var i = 0; i < count; i++) 
            CreateObject(data.Data[i]);
    }
    
    public BaseEntity CreateNewObject(BaseData data)
    {
        var entity = CreateObject(data);
        Data.Data.Add(data);

        return entity;
    }

    private BaseEntity CreateObject(BaseData data)
    {
        var entity = data.CreateEntity(this);
        _entities.Add(entity, data);

        foreach (var s in _systems)
            s.Value.AddEntity(entity);

        return entity;
    }

    public void AddSystem(BaseSystem system)
    {
        if (_systems.ContainsKey(system.GetType()))
            return;
        
        _systems.Add(system.GetType(), system);

        foreach (var entity in _entities)
            system.AddEntity(entity.Key);
    }

    public void RemoveEntity(BaseEntity entity)
    {
        foreach (var system in _systems)
            system.Value.RemoveEntity(entity);

        Data.Data.Remove(_entities[entity]);
        _entities.Remove(entity);
        
        entity.Dispose();
    }

    public void RemoveSystem(BaseSystem system)
    {
        if (_systems.ContainsKey(system.GetType()))
            _systems.Remove(system.GetType());

        foreach (var entity in _entities)
            system.RemoveEntity(entity.Key);
    }

    public T GetSystem<T>()
    {
        if (_systems[typeof(T)] is T system)
            return system;

        return default;
    }

    public virtual void Deactivate()
    {
        var baseSystems = new List<BaseSystem>();
        foreach (var system in _systems)
            baseSystems.Add(system.Value);

        foreach (var system in baseSystems)
            RemoveSystem(system);
        
        var baseEntities = new List<BaseEntity>();
        foreach (var entity in _entities)
            baseEntities.Add(entity.Key);
        
        foreach (var entity in baseEntities)
            RemoveEntity(entity);
    }
}
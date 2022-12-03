public abstract class BaseSystem
{
    public void AddEntity(BaseEntity entity)
    {
        if (entity is IActor actor)
            AddEntity(actor);
    }

    public void RemoveEntity(BaseEntity entity)
    {
        if (entity is IActor actor) 
            RemoveEntity(actor);
    }

    protected abstract void AddEntity(IActor entity);
    protected abstract void RemoveEntity(IActor entity);
}

public class BaseSystem<T> : BaseSystem
{
    protected override void AddEntity(IActor entity)
    {
        if (entity is T actor)
            AddActor(actor);
    }

    protected override void RemoveEntity(IActor entity)
    {
        if (entity is T actor)
            RemoveActor(actor);
    }

    protected virtual void AddActor(T actor)
    {
        
    }

    protected virtual void RemoveActor(T actor)
    {
        
    }
}

public class BaseSystem<T, T1> : BaseSystem<T>
{
    protected override void AddEntity(IActor entity)
    {
        base.AddEntity(entity);

        if (entity is T1 actor)
            AddActor(actor);
    }

    protected override void RemoveEntity(IActor entity)
    {
        base.RemoveEntity(entity);

        if (entity is T1 actor)
            RemoveActor(actor);
    }

    protected virtual void AddActor(T1 actor)
    {
    }

    protected virtual void RemoveActor(T1 actor)
    {
    }
}

public class BaseSystem<T, T1, T2> : BaseSystem<T, T1>
{
    protected override void AddEntity(IActor entity)
    {
        base.AddEntity(entity);

        if (entity is T1 actor)
            AddActor(actor);
    }

    protected override void RemoveEntity(IActor entity)
    {
        base.RemoveEntity(entity);

        if (entity is T1 actor)
            RemoveActor(actor);
    }

    protected virtual void AddActor(T2 actor)
    {
    }

    protected virtual void RemoveActor(T2 actor)
    {
    }
}
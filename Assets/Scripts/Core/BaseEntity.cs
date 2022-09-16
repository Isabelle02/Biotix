public abstract class BaseEntity
{
    public virtual void Dispose() { }
}

public abstract class BaseEntity<T> : BaseEntity where T : BaseData
{
    protected T Data;

    protected BaseEntity(T data)
    {
        Data = data;
    }
}
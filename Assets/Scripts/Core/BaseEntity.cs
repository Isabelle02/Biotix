public abstract class BaseEntity
{
    public virtual void Dispose() { }
}

public abstract class BaseEntity<T> : BaseEntity where T : BaseData
{
    public T Data;

    protected BaseEntity(T data)
    {
        Data = data;
    }
}
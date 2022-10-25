public interface IPage
{
    public bool IsActive { get; }
    public void Open(ViewParam viewParam);
    public void Close();
    public void OnOpenStart(ViewParam viewParam);

    public void OnCloseStart();
    
    public abstract class ViewParam
    {
    }
}
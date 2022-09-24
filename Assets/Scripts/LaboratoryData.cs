public class LaboratoryData
{
    protected readonly float BaseAttack = 1f;
    protected readonly float BaseDefence = 1f;
    protected readonly float BaseSpeed = 0.5f;
    protected readonly float BaseReproduction = 4f;
    protected readonly int BaseAdditionalInjection = 0;
    
    public virtual float Attack
    {
        get => BaseAttack;
        set {  }
    }

    public virtual float Defence
    {
        get => BaseDefence;
        set { }
    }
    
    public virtual float Speed
    {
        get => BaseSpeed;
        set { }
    }
    
    public virtual float Reproduction
    {
        get => BaseReproduction;
        set { }
    }
    
    public virtual int AdditionalInjection
    {
        get => BaseAdditionalInjection;
        set { }
    }
}
using System.Collections.Generic;

public class UnitSystem : BaseSystem<UnitEntity>
{
    private readonly List<UnitEntity> _unitEntities = new();

    protected override void AddActor(UnitEntity actor)
    {
        _unitEntities.Add(actor);
    }

    protected override void RemoveActor(UnitEntity actor)
    {
        _unitEntities.Remove(actor);
    }

    public void SetPause(bool isPaused)
    {
        foreach (var u in _unitEntities)
        {
            u.SetPause(isPaused);
        }
    }
}
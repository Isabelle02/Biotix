using System;

public class TimeController : IUpdatable
{
    private float _stepPassTime;
    
    public DateTime PassTime { get; private set; }
    
    public void Update(float delta)
    {
        _stepPassTime += delta;
        if (_stepPassTime >= 1f)
        {
            PassTime = PassTime.AddSeconds(1f);
            _stepPassTime = 0;
        }
    }
}
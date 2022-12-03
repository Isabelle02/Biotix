using System;

public class Gameplay
{
    private TimeController _timeController;

    public DateTime LevelPassTime => _timeController.PassTime;

    public async void Init(LevelData lvlData)
    {
        var worldData = new WorldData();
        
        foreach (var n in lvlData.NodesData)
        {
            worldData.Data.Add(n);
        }

        var updateSystem = new UpdateSystem();
        var unitSystem = new UnitSystem();
        var nodeSystem = new NodeSystem();

        _timeController = new TimeController();
        updateSystem.AddUpdatable(_timeController);

        WorldManager.CurrentWorld = new BaseWorld();
        WorldManager.CurrentWorld.Activate(worldData, updateSystem, unitSystem, nodeSystem);
        
        await updateSystem.Update();
    }
}
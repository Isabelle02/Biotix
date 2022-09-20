using UnityEngine;

public class Gameplay : MonoBehaviour
{
    private bool _isPaused;

    private Vector3 _mouseDownPos;
    private bool _isDragged;

    private IWorld _world;

    private async void Start()
    {
        var worldData = new WorldData();
        var lvlData = LevelManager.LevelsConfig.LevelsData[0];
        
        foreach (var t in lvlData.PlayersData)
        {
            worldData.Data.Add(t);
        }

        foreach (var t in lvlData.AisData)
        {
            worldData.Data.Add(t);
        }

        var updatableSystem = new UpdatableSystem();
        var playerSystem = new PlayerSystem();

        _world = new BaseWorld();
        _world.Activate(worldData, updatableSystem, playerSystem);

        await updatableSystem.Update();
    }
    
    private void Update()
    {
        if (_isPaused)
            return;
        
        if (Input.GetMouseButtonDown(0))
            _mouseDownPos = MouseManager.GetMousePosition(0);

        if (Input.GetMouseButtonUp(0)) 
        { 
            var node = MouseManager.GetObject<NodeView>();
            if (node != null) 
            { 
                if (_isDragged) 
                { 
                    _isDragged = false;
                    _world.GetSystem<PlayerSystem>().SendUnits(node.NodeEntity);
                }
                else
                {
                    if (!_world.GetSystem<PlayerSystem>().ActivateNode(node.NodeEntity))
                        _world.GetSystem<PlayerSystem>().SendUnits(node.NodeEntity);
                }
            } 
            else if (_isDragged) 
            { 
                _isDragged = false; 
                _world.GetSystem<PlayerSystem>().HandleSelectedNodesLines(false);
            }
            else
            {
                _world.GetSystem<PlayerSystem>().ResetSelectedNodes();
            }
        } 
 
        if (Input.GetMouseButton(0)) 
        { 
            if (_mouseDownPos != MouseManager.GetMousePosition(0)) 
            { 
                _isDragged = true; 
                var node = MouseManager.GetObject<NodeView>();
                if (node != null)
                    _world.GetSystem<PlayerSystem>().ActivateNode(node.NodeEntity);
                
                _world.GetSystem<PlayerSystem>().HandleSelectedNodesLines(true);
            } 
        } 
    }

}
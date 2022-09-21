using UnityEngine;

public class Gameplay : MonoBehaviour
{
    private bool _isPaused;

    private Vector3 _mouseDownPos;
    private bool _isDragged;

    private NodeSystem _nodeSystem;

    private async void Start()
    {
        var worldData = new WorldData();
        var lvlData = LevelManager.LevelsConfig.LevelsData[0];
        
        foreach (var t in lvlData.NodesData)
        {
            worldData.Data.Add(t);
        }

        var updateSystem = new UpdateSystem();
        _nodeSystem = new NodeSystem();

        WorldManager.CurrentWorld = new BaseWorld();
        WorldManager.CurrentWorld.Activate(worldData, updateSystem, _nodeSystem);

        await updateSystem.Update();
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
                    _nodeSystem.SendUnits(1, node.NodeEntity);
                }
                else
                {
                    if (!_nodeSystem.ActivateNode(1, node.NodeEntity))
                        _nodeSystem.SendUnits(1, node.NodeEntity);
                }
            } 
            else if (_isDragged) 
            { 
                _isDragged = false; 
                _nodeSystem.StopSearching(1);
            }
            else
            {
                _nodeSystem.DeactivateNodes(1);
            }
        } 
 
        if (Input.GetMouseButton(0)) 
        { 
            if (_mouseDownPos != MouseManager.GetMousePosition(0)) 
            { 
                _isDragged = true; 
                WorldManager.CurrentWorld.GetSystem<NodeSystem>().SearchTarget(1);
            } 
        } 
    }

}
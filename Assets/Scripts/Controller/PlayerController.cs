using System.Collections.Generic;
using UnityEngine;

public class PlayerController : TeamController, IUpdatable
{
    private readonly List<NodeEntity> _selectedNodes = new();

    private bool _isPlayHighlighting;
    private bool _isFirstNodeActivation = true;
    
    private Vector3 _mouseDownPos;
    private bool _isDragged;
    
    public NetworkPlayerController NetworkPlayerController;

    public PlayerController(int teamId) : base(teamId)
    {
        if (!LevelManager.IsNetwork)
            return;

        NetworkPlayerController = LevelManager.TeamId == teamId ? LevelManager.PlayerController : null;
    }

    public override void Init()
    {
        SetAttack(PlayerManager.AttackRate);
        SetDefence(PlayerManager.DefenceRate);
        SetSpeed(PlayerManager.SpeedRate);
        SetReproduction(PlayerManager.ReproductionRate);
        SetAdditionalInjection(PlayerManager.AdditionalInjectionRate);
    }

    public override void SetAttack(float rate)
    {
        Attack += rate;
    }

    public override void SetDefence(float rate)
    {
        Defence += rate;
    }
    
    public override void SetSpeed(float rate)
    {
        Speed += rate;
    }
    
    public override void SetReproduction(float rate)
    {
        Reproduction -= rate;
    }
    
    public override void SetAdditionalInjection(float rate)
    {
        AdditionalInjection += (int) (rate * 10);
    }
    
    public override bool ActivateNode(NodeEntity node)
    {
        if (!Nodes.Contains(node))
            return false;
        
        if (_selectedNodes.Contains(node))
            return false;

        if (_isFirstNodeActivation)
        {
            foreach (var n in Nodes)
                n.StopHighlighting();

            _isFirstNodeActivation = false;
        }

        node.SetHighlighted(true);
        _selectedNodes.Add(node);
        return true;
    }

    public override void SendUnits(NodeEntity targetNode)
    {
        if (_selectedNodes.Contains(targetNode))
        {
            targetNode.SetLineActive(false);
            targetNode.SetHighlighted(false);
            _selectedNodes.Remove(targetNode);
        }
        else
            targetNode.PlayTargetHighlighting();

        foreach (var n in _selectedNodes)
        {
            n.SendUnits(targetNode, Attack, Speed);
            n.SetLineActive(false);
            n.SetHighlighted(false);
        }
        
        _selectedNodes.Clear();
    }

    public override void SearchTarget()
    {
        var node = MouseManager.GetObject<NodeView>();
        if (node != null)
            ActivateNode(node.NodeEntity);
        
        foreach (var n in _selectedNodes)
        {
            n.SetLineActive(true);
            n.SetLineEndPosition(MouseManager.GetMousePosition(90));
        }
    }

    public override void StopSearching()
    {
        foreach (var n in _selectedNodes)
        {
            n.SetLineActive(false);
        }
    }

    public override void DeactivateNode(NodeEntity node)
    {
        if (_selectedNodes.Contains(node))
        {
            node.SetLineActive(false);
            node.SetHighlighted(false);
            _selectedNodes.Remove(node);
        }
    }

    public override void DeactivateNodes()
    {
        foreach (var n in _selectedNodes)
        {
            n.SetLineActive(false);
            n.SetHighlighted(false);
        }
        
        _selectedNodes.Clear();
    }

    public void Update(float delta)
    {
        /*if (LevelManager.IsNetwork && (NetworkPlayerController == null || !NetworkPlayerController.photonView.IsMine))
            return;*/
        
        if (LevelManager.TeamId != TeamId)
            return;

        if (_isPlayHighlighting)
        {
            foreach (var n in Nodes)
                n.PlayHighlighting();
            
            _isPlayHighlighting = false;
        }

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
                    SendUnits(node.NodeEntity);
                }
                else
                {
                    if (!ActivateNode(node.NodeEntity)) 
                        SendUnits(node.NodeEntity);
                }
            } 
            else if (_isDragged) 
            { 
                _isDragged = false; 
                StopSearching();
            }
            else
            {
                DeactivateNodes();
            }
        } 
 
        if (Input.GetMouseButton(0)) 
        { 
            if (_mouseDownPos != MouseManager.GetMousePosition(0)) 
            { 
                _isDragged = true; 
                SearchTarget();
            } 
        } 
    }
}
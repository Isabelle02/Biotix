﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UpdateSystem : BaseSystem<IUpdatable>
{
    private readonly List<IUpdatable> _updatables = new();

    private float _passTime;
    private bool _isUpdating = true;

    public void AddUpdatable(IUpdatable updatable)
    {
        _updatables.Add(updatable);
    }
    
    public void ClearUpdatables()
    {
        _updatables.Clear();
    }

    protected override void AddActor(IUpdatable actor)
    {
        _updatables.Add(actor);
    }

    protected override void RemoveActor(IUpdatable actor)
    {
        _updatables.Remove(actor);
    }
    
    public async Task SetPause(bool isPaused)
    {
        _isUpdating = !isPaused;
        if (_isUpdating)
            await Update();
    }

    public async Task Update()
    {
        while (_isUpdating)
        {
            await UniTask.DelayFrame(1);
            var delta = Time.realtimeSinceStartup - _passTime;
            _passTime = Time.realtimeSinceStartup;
            var count = _updatables.Count;
            for (var i = 0; i < count; i++)
               _updatables[i].Update(delta);
        }
    }

}
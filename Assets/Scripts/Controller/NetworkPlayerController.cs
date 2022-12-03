using System;
using System.Collections.Generic;
using Photon.Pun;

public class NetworkPlayerController : MonoBehaviourPun
{
    private Gameplay _gameplay;
    public LevelData LevelData;

    public DateTime LevelPassTime => _gameplay.LevelPassTime;

    public void Init()
    {
        _gameplay = new Gameplay();
        _gameplay.Init(LevelData);
    }

    public void OnUnitsSent(int id, List<UnitData> units, int targetId)
    {
        photonView.RPC("SendUnits", RpcTarget.AllBuffered, id,
            Storage.SerializeObject(units), targetId);
    }

    [PunRPC]
    private void SendUnits(int id, string units, int targetId)
    {
        WorldManager.CurrentWorld.GetSystem<NodeSystem>().SendUnits(id,
            Storage.DeserializeObject<List<UnitData>>(units), targetId);
    }
}
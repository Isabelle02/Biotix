using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    private bool _isConnecting;

    public event Action<bool> JoinedRoom;
    public event Action Disconnected;

    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void TryToConnect()
    {
        _isConnecting = true;

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the " + PhotonNetwork.CloudRegion + " server!");
        
        if (_isConnecting) 
            PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected due to: {cause}");
        
        Disconnected?.Invoke();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No opponents, creating a new room");

        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");

        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        var cp = PhotonNetwork.CurrentRoom.CustomProperties;
        if (!cp.ContainsKey("level"))
        {
            cp.Add("level", Storage.SerializeObject(LevelManager.GenerateLevel(2)));
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
        }

        var npc = NetworkRecycler<NetworkPlayerController>.Get();
        npc.LevelData = Storage.DeserializeObject<LevelData>((string) cp["level"]);
        LevelManager.TeamId = playerCount;
        LevelManager.PlayerController = npc;
        
        if (playerCount < 2)
        {
            Debug.Log("Waiting for an opponent");
            JoinedRoom?.Invoke(false);
        }
        else
        {
            Debug.Log("Match is ready to start");
            JoinedRoom?.Invoke(true);
            
            WindowManager.Open<NetworkGameplayWindow>();
            LevelManager.IsNetwork = true;
            LevelManager.PlayerController.Init();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        
        if (playerCount >= 2)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("Match is ready to start");
            JoinedRoom?.Invoke(true);
            
            WindowManager.Open<NetworkGameplayWindow>();
            LevelManager.IsNetwork = true;
            LevelManager.PlayerController.Init();
        }
    }

    public override void OnLeftRoom()
    {
        _isConnecting = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerCount == 1) 
            LevelManager.CompleteNetworkLevel(true);
    }
}
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayingConnectionWindow : Window
{
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _connectButton;
    [SerializeField] private Text _connectStatusText;
    
    private NetworkController _networkController;

    public override void OnOpenStart(ViewParam viewParam)
    { 
        _connectButton.interactable = false;
        _connectStatusText.text = "Loading...";
        
        _cancelButton.onClick.AddListener(OnCancelButtonClick);
        _connectButton.onClick.AddListener(OnConnectButtonClick);
    
        _networkController = FindObjectOfType<NetworkController>();
        if (_networkController == null)
            _networkController = Recycler<NetworkController>.Get();

        _networkController.ConnectedToMaster += OnConnectToMaster;
        _networkController.JoinedRoom += OnJoinedRoom;
        
        if (_networkController.IsConnectedToMaster)
            OnConnectToMaster();
    }

    private void OnConnectToMaster()
    {
        _connectStatusText.text = "Connect";
        _connectButton.interactable = true;
    }

    private void OnConnectButtonClick()
    {
        _connectButton.interactable = false;
        _connectStatusText.text = "Searching...";

        _networkController.TryToConnect();
    }

    private void OnJoinedRoom(bool isStart)
    {
        if (!isStart)
        {
            _connectStatusText.text = "Waiting For Opponent...";
        }
        else
        {
            _connectStatusText.text = "Opponent Found";
        }
    }

    private void OnCancelButtonClick()
    {
        if (LevelManager.PlayerController != null)
        {
            PhotonNetwork.LeaveRoom();
            while (PhotonNetwork.InRoom)
            {
            }
        }

        WindowManager.Open<MainMenuWindow>();
    }

    public override void OnCloseStart()
    {
        _networkController.JoinedRoom -= OnJoinedRoom;
        _connectButton.onClick.RemoveListener(OnConnectButtonClick);
        _cancelButton.onClick.RemoveListener(OnCancelButtonClick);
    }
}
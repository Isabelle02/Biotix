using Photon.Pun;
using Photon.Realtime;
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
        _networkController = FindObjectOfType<NetworkController>();
        if (_networkController == null)
            _networkController = Recycler<NetworkController>.Get();

        _networkController.JoinedRoom += OnJoinedRoom;
        _networkController.Disconnected += OnDisconnected;
        _connectButton.onClick.AddListener(OnConnectButtonClick);
        _cancelButton.onClick.AddListener(OnCancelButtonClick);
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
            _connectStatusText.text = "Waiting For Opponent";
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

    private void OnDisconnected()
    {
        _connectButton.interactable = true;
        _connectStatusText.text = "Connect";
    }

    public override void OnCloseStart()
    {
        _networkController.OnDisconnected(DisconnectCause.None);
        _networkController.JoinedRoom -= OnJoinedRoom;
        _networkController.Disconnected -= OnDisconnected;
        _connectButton.onClick.RemoveListener(OnConnectButtonClick);
        _cancelButton.onClick.RemoveListener(OnCancelButtonClick);
    }
}
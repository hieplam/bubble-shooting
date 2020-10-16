using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public GameObject BattleButton;
    public GameObject CancelButton;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//connect to master server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"Connected to {PhotonNetwork.CloudRegion} server");
        PhotonNetwork.AutomaticallySyncScene = true;
        BattleButton.SetActive(true);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join the room - {returnCode} - {message}");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to create the room - {returnCode} - {message}");
        CreateRoom();
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle clicked");
        BattleButton.SetActive(false);
        CancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel clicked");
        BattleButton.SetActive(true);
        CancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

    void CreateRoom()
    {
        var rd = Random.Range(0, 10000);
        var roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.MaxPlayers };
        PhotonNetwork.CreateRoom($"Room {rd}", roomOptions);
    }

    void Update()
    {
        
    }
}

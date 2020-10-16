using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using PhotonPlayer = Photon.Realtime.Player;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom Room;
    private PhotonView photonView;

    public bool IsGameLoaded { get; set; }
    public int CurrentScene { get; set; }
    //Player info
    private List<PhotonPlayer> players;
    public int PlayersInRoom { get; set; }
    public int MyNumberInRoom { get; set; }
    public int PlayerInGame { get; set; }

    //Delay start
    private bool readyToCount;
    private bool readyToStart;
    public float StartingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayer;
    private float timeToStart;
    // Start is called before the first frame update
    void Awake()
    {
        //singleton stuff
        if (PhotonRoom.Room == null)
        {
            PhotonRoom.Room = this;
        } else
        {
            if (PhotonRoom.Room != this)
            {
                Destroy(PhotonRoom.Room.gameObject);
                PhotonRoom.Room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene s, LoadSceneMode mode)
    {
        CurrentScene = s.buildIndex;
        if (CurrentScene == MultiplayerSetting.multiplayerSetting.MultiPlayerScene)
        {
            IsGameLoaded = true;
            if (MultiplayerSetting.multiplayerSetting.IsDelayStart)
            {
                photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene(){
        PlayerInGame++;
        if (PlayerInGame == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);

    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = StartingTime;
        atMaxPlayer = 6;
        timeToStart = StartingTime;
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        players = PhotonNetwork.PlayerList.ToList();
        PlayersInRoom = players.Count;
        MyNumberInRoom = PlayersInRoom;
        PhotonNetwork.NickName = MyNumberInRoom.ToString();
        if (MultiplayerSetting.multiplayerSetting.IsDelayStart)
        {
            Debug.Log($"Delaying start dou to max player {PlayersInRoom}, {MultiplayerSetting.MaxPlayers}");
            if (PlayersInRoom >1)
            {
                readyToCount = true;
            }
            if (PlayersInRoom == MultiplayerSetting.MaxPlayers)
            {
                readyToCount = true;
                if (!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                //close room if master client
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(PhotonPlayer newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A player has joined the room");
        players = PhotonNetwork.PlayerList.ToList();
        PlayersInRoom++;
        if (MultiplayerSetting.multiplayerSetting.IsDelayStart)
        {
            Debug.Log($"Max player reahed :{PlayersInRoom} players in room");
            if (PlayersInRoom >1)
            {
                readyToCount = true;
            }
            if (PlayersInRoom == MultiplayerSetting.MaxPlayers)
            {
                readyToCount = true;
                if (!PhotonNetwork.IsMasterClient)
                {
                    return;
                }
                //close room if master client
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }
    void RestartTimer()
    {
        lessThanMaxPlayers = StartingTime;
        timeToStart = StartingTime;
        atMaxPlayer = 6;
        readyToCount = false;
        readyToStart = false;
    }
    void StartGame()
    {
        IsGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (MultiplayerSetting.multiplayerSetting.IsDelayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.MultiPlayerScene);
    }
    
    void Update()
    {
        if (MultiplayerSetting.multiplayerSetting.IsDelayStart)
        {
            if (PlayersInRoom == 1) RestartTimer();

            if (!IsGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayer -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayer;
                    timeToStart = atMaxPlayer;
                }else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }

                if (timeToStart <= 0)
                {
                    StartGame();
                }
            }

        }
    }
}

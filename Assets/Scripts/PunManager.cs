using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PunManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform listPlayerContainer;
    [SerializeField] private GameObject playerNamePrefab;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private byte maxPlayer;
    public byte players;

    [HideInInspector]
    public static PunManager Instance;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ConnectToPun();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void AddPlayerToList(Player player)
    {
        GameObject obj = Instantiate(playerNamePrefab, listPlayerContainer);
        PlayerInfo info = obj.GetComponent<PlayerInfo>();
        if (info != null)
        {
            info.SetPlayerName(player.UserId);
        }
    }

    private void ConnectToPun()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Connected");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("ConnectToMaster");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(100, 1000);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayer });
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Host enter room");

            players++;
        }
        else
        {
            Debug.Log("Client enter room");
        }

        title.text = "Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount;
        Player[] playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
            var obj = Instantiate(playerNamePrefab, listPlayerContainer);
            PlayerInfo playerInfo = obj.GetComponent<PlayerInfo>();

            playerInfo.SetPlayerName(playerList[i].NickName);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player enter room");
        var obj = Instantiate(playerNamePrefab, listPlayerContainer);
        PlayerInfo playerInfo = obj.GetComponent<PlayerInfo>();

        players++;
        Debug.Log("ASDA" + playerInfo);
        playerInfo.SetPlayerName(newPlayer.NickName);

        title.text = "Players In Room: " + players.ToString();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        players--;
        title.text = "Players In Room: " + players.ToString();
    }
}

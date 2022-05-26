using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType<PlayerManager>();
                if (objs.Length > 0)
                {
                    _instance = objs[0];
                }
                if (objs.Length > 1)
                {
                    Debug.LogError("There is more than 1 instance of PlayerManager");
                }
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "PlayerManager";
                    _instance = obj.AddComponent<PlayerManager>();
                }
            }
            return _instance;
        }
    }

    private NetworkVariable<int> net_PlayersInGame = new NetworkVariable<int>();
    private int n_PlayersInGame;

    public int PlayersInGame
    {
        get
        {
            return net_PlayersInGame.Value;
        }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"Player {id} just enter room");
                net_PlayersInGame.Value++;
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log($"Player {id} just leave room");
                net_PlayersInGame.Value--;
            }
        };
    }

    private void Update()
    {
    }

    private void UpdateClient()
    {

    }
}

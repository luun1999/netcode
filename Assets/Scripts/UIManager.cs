using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button startClient;
    [SerializeField] private Button startHost;
    [SerializeField] private Text playersInGameText;

    // Start is called before the first frame update
    void Start()
    {
        startClient.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Start Client");
            }
            else
            {
                Debug.Log("Some thing went wrong!");
            }
        });
        startHost.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Start Host");
            }
            else
            {
                Debug.Log("Some thing went wrong!");
            }
        });
    }

    public void SubmitNewPosition()
    {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        var player = playerObject.GetComponent<PlayerController>();
        player.Move();
    }

    // Update is called once per frame
    void Update()
    {
        playersInGameText.text = "PLAYERS IN ROOM: " +
            PlayerManager.Instance.PlayersInGame.ToString();
    }
}

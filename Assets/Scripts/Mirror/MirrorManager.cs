using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MirrorManager : NetworkBehaviour
{
    [SerializeField] GameObject playerPrefab;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (isServer)
            {
                var netObj = Instantiate(playerPrefab, gameObject.transform);
                NetworkServer.Spawn(netObj);
            }
            else if (isClient)
            {
                CreateGameObject();
            }
        }
    }

    [Command]
    private void CreateGameObject()
    {
        var netObj = Instantiate(playerPrefab, gameObject.transform);
        NetworkServer.Spawn(netObj);
    }
}

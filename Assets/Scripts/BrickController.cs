using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BrickController : NetworkBehaviour
{
    private NetworkVariable<Vector3> net_Position = new NetworkVariable<Vector3>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            transform.position = net_Position.Value;
        }
        if (IsClient && IsOwner)
        {
            UpdatePositionServerRpc();
        }
    }

    [ServerRpc]
    private void UpdatePositionServerRpc()
    {
        if (net_Position.Value != transform.position)
        {
            net_Position.Value = transform.position;
        }
    }
}

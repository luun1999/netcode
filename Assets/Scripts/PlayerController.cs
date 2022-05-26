using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    public float walkSpeed;

    private NetworkVariable<Vector3> net_PlayerPosition = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<float> net_forwardBackPosition = new NetworkVariable<float>();
    [SerializeField]
    private NetworkVariable<float> net_leftRightPosition = new NetworkVariable<float>();

    //cache position
    float old_LeftRightPosition;
    float old_ForwardBackPosition;

    //On Playerr Spawn
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    private void Update()
    {
        transform.position = net_PlayerPosition.Value;

        if (IsServer)
        {
            UpdateServer();
        }
        if (IsClient && IsOwner)
        {
            UpdateClient();
        }
    }

    private void UpdateServer()
    {
        transform.position = new Vector3(transform.position.x + net_leftRightPosition.Value,
            transform.position.y,
            transform.position.z + net_forwardBackPosition.Value);
    }

    private void UpdateClient()
    {
        float leftRight = old_LeftRightPosition;
        float forwardBack = old_ForwardBackPosition;

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("left");
            leftRight -= walkSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("right");
            leftRight += walkSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("forward");
            forwardBack += walkSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("back");
            forwardBack -= walkSpeed * Time.deltaTime;
        }

        if (leftRight != old_LeftRightPosition || forwardBack != old_ForwardBackPosition)
        {
            old_LeftRightPosition = leftRight;
            old_ForwardBackPosition = forwardBack;

            //Call server update position
            UpdateClientPositionServerRpc(old_LeftRightPosition, old_ForwardBackPosition);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float leftRight, float forwardBack)
    {
        net_leftRightPosition.Value = leftRight;
        net_forwardBackPosition.Value = forwardBack;
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            transform.position = GetRandomPosition();
            net_PlayerPosition.Value = transform.position;
        }
        else
        {
            ChangePositionServerRpc();
        }
    }

    public Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-3, 3), 1f, Random.Range(-3, 3));
    }

    [ServerRpc]
    public void ChangePositionServerRpc()
    {
        net_PlayerPosition.Value = GetRandomPosition();
    }
}

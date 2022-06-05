using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject listBrick;
    private GameObject selectedObject;
    private PhotonView listPhotonView;
    private PhotonView photonView;

    private void Awake()
    {
        listPhotonView = listBrick.GetComponent<PhotonView>();
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject obj = PhotonNetwork.Instantiate("Brick_", new Vector3(0, 3, 0), Quaternion.identity, 0, null);
            PhotonView brickPhotonView = obj.GetComponent<PhotonView>();

            photonView.RPC("SetParentForBrick", RpcTarget.All, new object[] { brickPhotonView.ViewID, listPhotonView.ViewID });
        }
    }

    [PunRPC]
    private void SetParentForBrick(int brick, int parent)
    {
        Transform brickTrans = PhotonView.Find(brick).transform;
        Transform listTrans = PhotonView.Find(parent).transform;

        brickTrans.parent = listTrans;
    }
}

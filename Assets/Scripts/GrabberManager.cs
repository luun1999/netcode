using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GrabberManager : NetworkBehaviour
{
    private bool isServerStarted = false;
    public GameObject gameObjectPrefabs;

    private GameObject selectedObject;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            isServerStarted = true;
        };
    }

    private void Update()
    {
        SpawnObject();
        DragObject();
    }

    private void DragObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("Dragable"))
                    {
                        return;
                    }

                    Debug.Log("hit");
                    selectedObject = hit.collider.gameObject;
                }
            }
            else
            {
                //selectedObject = null;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;
        }

        if (selectedObject != null)
        {
            //drag object
            Vector3 position = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPos.x, 1f, worldPos.z);
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    private void SpawnObject()
    {
        if (Input.GetKeyDown(KeyCode.A) && IsServer && isServerStarted)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 7f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

            var obj = Instantiate(gameObjectPrefabs, objectPos, Quaternion.identity);
            obj.GetComponent<NetworkObject>().Spawn();
        }
        else if (Input.GetKeyDown(KeyCode.A) && IsClient && IsOwner)
        {
            SpawnObjectServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnObjectServerRpc()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 7f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);

        var obj = Instantiate(gameObjectPrefabs, objectPos, Quaternion.identity);
    }
}

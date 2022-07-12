using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkDiscovery))]
[RequireComponent(typeof(NetworkManager))]
public class AutoFindMatch : MonoBehaviour
{
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery networkDiscovery;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif

    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;
        Connect(info);
    }

    void Connect(ServerResponse info)
    {
        Debug.Log("Connect to: " + info.uri);
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }

    // Start is called before the first frame update
    void Start()
    {
        discoveredServers.Clear();
        StartCoroutine("Cor_Discovery");
    }

    private IEnumerator Cor_Discovery()
    {
        networkDiscovery.StartDiscovery();
        yield return new WaitForSeconds(5);

        if (discoveredServers.Count == 0)
        {
            networkDiscovery.StopDiscovery();
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

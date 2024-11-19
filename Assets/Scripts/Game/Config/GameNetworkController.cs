using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Config
{
  public class GameNetworkController : NetworkBehaviour
  {
    public static GameNetworkController instance { get; private set; }

    public GameObject playerPrefab;

    public Transform gameBoardTransform;

    private void Awake()
    {
      instance = this; 
    }

    public IReadOnlyList<ulong> GetPlayerIdList()
    {
      return NetworkManager.Singleton.ConnectedClientsIds;
    }
    
    public override void OnNetworkSpawn()
    {
      if (IsServer)
      {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
      }
    }

    private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) {
      foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
        GameObject playerGo = Instantiate(playerPrefab);
        playerGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        playerGo.transform.SetParent(gameBoardTransform);
      }
    }
  }
}
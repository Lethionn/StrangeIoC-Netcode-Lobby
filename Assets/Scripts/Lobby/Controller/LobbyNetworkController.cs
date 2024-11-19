using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enum;
using Common.Scene;
using Online.Controller;
using Unity.Netcode;

namespace Lobby.Controller
{
  public class LobbyNetworkController : NetworkBehaviour
  {
    public static LobbyNetworkController instance { get; private set; }

    public event EventHandler OnReadyChanged;

    private Dictionary<ulong, bool> _playerReadyDictionary;


    private void Awake()
    {
      instance = this;

      _playerReadyDictionary = new Dictionary<ulong, bool>();
    }
    

    public void SetPlayerReady(bool isReady)
    {
      SetPlayerReadyServerRpc(isReady);
    }

    public void StartGame()
    {
      StartGameServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(bool isReady, ServerRpcParams serverRpcParams = default)
    {
      SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId, isReady);

      _playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = isReady;
    }

    [ServerRpc(RequireOwnership = false)]
    private void StartGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
      if (!AreAllPlayersReady())
      {
        return;
      }

      OnlineNetworkController.instance.DeleteLobby();
      SceneLoader.LoadNetwork(SceneKey.Game);
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId, bool isReady)
    {
      _playerReadyDictionary[clientId] = isReady;

      OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool AreAllPlayersReady()
    {
      return NetworkManager.Singleton.ConnectedClientsIds.All(clientId => _playerReadyDictionary.ContainsKey(clientId) && _playerReadyDictionary[clientId]);
    }

    public bool IsPlayerReady(ulong clientId)
    {
      return _playerReadyDictionary.ContainsKey(clientId) && _playerReadyDictionary[clientId];
    }
  }
}
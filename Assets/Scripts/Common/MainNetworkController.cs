using System;
using Common.Enum;
using Common.Scene;
using Game.Vo;
using Online.Controller;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
  public class MainNetworkController : NetworkBehaviour
  {
    public static MainNetworkController instance { get; private set; }

    public NetworkList<PlayerVo> playerVoNetworkList;
    private string _playerName;
    
    public event EventHandler OnLeftLobby;
    public event EventHandler OnPlayerNetworkListChanged;

    private void Awake()
    {
      instance = this;

      DontDestroyOnLoad(gameObject);

      _playerName = PlayerPrefs.GetString(PlayerPrefKey.PlayerName, "PlayerName" + UnityEngine.Random.Range(100, 1000));

      playerVoNetworkList = new NetworkList<PlayerVo>();
      playerVoNetworkList.OnListChanged += PlayerVoNetworkListOnListChanged;
    }

    private void PlayerVoNetworkListOnListChanged(NetworkListEvent<PlayerVo> changeEvent)
    {
      OnPlayerNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public string GetPlayerName()
    {
      return _playerName;
    }


    public void SetPlayerName(string playerName)
    {
      _playerName = playerName;

      PlayerPrefs.SetString(PlayerPrefKey.PlayerName, playerName);
      SetPlayerNameServerRpc(playerName);
    }

    public void StartHost()
    {
      NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
      NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
      NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
      NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
      if (!OnlineNetworkController.instance.leftFromMenu)
      {
        if (GetPlayerDataFromClientId(clientId).playerId == OnlineNetworkController.instance.joinedLobby.HostId)
        {
          OnlineNetworkController.instance.DeleteLobby();
          return;
        }
      }

      for (int i = 0; i < playerVoNetworkList.Count; i++)
      {
        PlayerVo playerData = playerVoNetworkList[i];
        if (playerData.clientId == clientId)
        {
          playerVoNetworkList.RemoveAt(i);
        }
      }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
      playerVoNetworkList.Add(new PlayerVo()
      {
        clientId = clientId,
      });
      SetPlayerNameServerRpc(GetPlayerName());
      SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
      if (SceneManager.GetActiveScene().name != SceneKey.Lobby)
      {
        connectionApprovalResponse.Approved = false;
        connectionApprovalResponse.Reason = "Game has already started";
        return;
      }

      if (NetworkManager.Singleton.ConnectedClientsIds.Count >= GameMechanics.MaxPlayerCount)
      {
        connectionApprovalResponse.Approved = false;
        connectionApprovalResponse.Reason = "Lobby is full";
        return;
      }

      connectionApprovalResponse.Approved = true;
    }

    public void StartClient()
    {
      NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
      NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
      NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_Client_OnClientDisconnectCallback(ulong clientId)
    {
      SceneLoader.leftFromMenu = OnlineNetworkController.instance.leftFromMenu;

      OnLeftLobby?.Invoke(this, EventArgs.Empty);

      if (!OnlineNetworkController.instance.leftFromMenu)
      {
        SceneLoader.Load(SceneKey.Online);
      }
    }

    private void NetworkManager_Client_OnClientConnectedCallback(ulong clientId)
    {
      SetPlayerNameServerRpc(GetPlayerName());
      SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
      int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

      PlayerVo playerVo = playerVoNetworkList[playerDataIndex];

      playerVo.playerName = playerName;

      playerVoNetworkList[playerDataIndex] = playerVo;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
      int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

      PlayerVo playerVo = playerVoNetworkList[playerDataIndex];

      playerVo.playerId = playerId;

      playerVoNetworkList[playerDataIndex] = playerVo;
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
      return playerIndex < playerVoNetworkList.Count;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
      for (int i = 0; i < playerVoNetworkList.Count; i++)
      {
        if (playerVoNetworkList[i].clientId == clientId)
        {
          return i;
        }
      }

      return -1;
    }

    public PlayerVo GetPlayerDataFromClientId(ulong clientId)
    {
      foreach (PlayerVo playerVo in playerVoNetworkList)
      {
        if (playerVo.clientId == clientId)
        {
          return playerVo;
        }
      }

      return default;
    }

    public PlayerVo GetPlayerData()
    {
      return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public PlayerVo GetPlayerDataFromPlayerIndex(int playerIndex)
    {
      return playerVoNetworkList[playerIndex];
    }

    public void KickPlayer(ulong clientId)
    {
      NetworkManager.Singleton.DisconnectClient(clientId);
      NetworkManager_Server_OnClientDisconnectCallback(clientId);
    }
  }
}
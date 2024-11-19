using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Enum;
using Common.Scene;
using Newtonsoft.Json;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using WebSocketSharp;

namespace Online.Controller
{
  public class OnlineNetworkController : NetworkBehaviour
  {
    private const string KeyRelayJoinCode = "RelayJoinCode";

    public static OnlineNetworkController instance { get; private set; }

    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;
    public event EventHandler OnJoinStarted;
    public event EventHandler OnQuickJoinFailed;
    public event EventHandler<int> OnJoinFailed;
    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

    public class OnLobbyListChangedEventArgs : EventArgs
    {
      public List<Unity.Services.Lobbies.Models.Lobby> lobbyList;
    }

    public Unity.Services.Lobbies.Models.Lobby joinedLobby;

    private float _heartbeatTimer;
    private float _listLobbiesTimer;

    public bool leftFromMenu;

    private void Awake()
    {
      instance = this;

      DontDestroyOnLoad(gameObject);

      InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
      if (UnityServices.State != ServicesInitializationState.Initialized)
      {
        InitializationOptions initializationOptions = new();

        await UnityServices.InitializeAsync(initializationOptions);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
      }
    }

    private void Update()
    {
      HandleHeartbeat();
    }

    private void HandleHeartbeat()
    {
      if (!IsLobbyHost()) return;
      _heartbeatTimer -= Time.deltaTime;
      if (!(_heartbeatTimer <= 0f)) return;
      float heartbeatTimerMax = 15f;
      _heartbeatTimer = heartbeatTimerMax;

      LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
    }

    public bool IsLobbyHost()
    {
      return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private async Task<Allocation> AllocateRelay()
    {
      try
      {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(GameMechanics.MaxPlayerCount - 1);

        return allocation;
      }
      catch (RelayServiceException e)
      {
        Debug.Log(e);

        return default;
      }
    }

    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
      try
      {
        string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        return relayJoinCode;
      }
      catch (RelayServiceException e)
      {
        Debug.Log(e);
        return default;
      }
    }

    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
      try
      {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        return joinAllocation;
      }
      catch (RelayServiceException e)
      {
        Debug.Log(e);
        return default;
      }
    }

    public async void CreateLobby(bool isPrivate)
    {
      OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
      try
      {
        joinedLobby = await LobbyService.Instance.CreateLobbyAsync("lobby", GameMechanics.MaxPlayerCount, new CreateLobbyOptions
        {
          IsPrivate = isPrivate
        });

        Allocation allocation = await AllocateRelay();

        string relayJoinCode = await GetRelayJoinCode(allocation);

        await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
        {
          Data = new Dictionary<string, DataObject>
          {
            { KeyRelayJoinCode, new DataObject(DataObject.VisibilityOptions.Public, relayJoinCode) },
            { "bannedPlayers", new DataObject(DataObject.VisibilityOptions.Public, "[]") },
            { "lobbyCode", new DataObject(DataObject.VisibilityOptions.Public, joinedLobby.LobbyCode) }
          }
        });
        
        joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

        MainNetworkController.instance.StartHost();
        SceneLoader.LoadNetwork(SceneKey.Lobby);
      }
      catch (LobbyServiceException e)
      {
        Debug.Log(e);
        OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
      }
    }

    public async void QuickGame()
    {
      OnJoinStarted?.Invoke(this, EventArgs.Empty);

      string playerId = AuthenticationService.Instance.PlayerId; // Get the current player's ID

      try
      {
        bool foundLobby = false;

        // Fetch available lobbies
        QueryResponse availableLobbies = await LobbyService.Instance.QueryLobbiesAsync(new QueryLobbiesOptions
        {
          Filters = new List<QueryFilter>
          {
            new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT) // Only joinable lobbies
          }
        });

        foreach (Unity.Services.Lobbies.Models.Lobby lobby in availableLobbies.Results)
        {
          // Check if the player is banned

          if (lobby.Data.ContainsKey("bannedPlayers"))
          {
            string bannedListJson = lobby.Data["bannedPlayers"].Value;
            List<string> bannedList = JsonConvert.DeserializeObject<List<string>>(bannedListJson);
            
            if (bannedList!= null && bannedList.Contains(playerId))
            {
              continue; // Skip this lobby
            }
          }
          
          // Join the first suitable lobby
          JoinWithId(lobby.Id);

          foundLobby = true;
          break;
        }

        // If no suitable lobby is found, create a new one
        if (!foundLobby)
        {
          Debug.Log("No suitable lobby found. Creating a new one.");
          CreateLobby(false);
        }
      }
      catch (LobbyServiceException e)
      {
        Debug.LogError(e);

        if (e.ErrorCode == 16006) // No lobbies available
        {
          CreateLobby(false);
        }
        else
        {
          OnQuickJoinFailed?.Invoke(this, EventArgs.Empty);
        }
      }
    }


    public async void JoinWithId(string lobbyId)
    {
      OnJoinStarted?.Invoke(this, EventArgs.Empty);
      try
      {
        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

        string relayJoinCode = joinedLobby.Data[KeyRelayJoinCode].Value;

        JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

        MainNetworkController.instance.StartClient();
      }
      catch (LobbyServiceException e)
      {
        Debug.Log(e);
        OnJoinFailed?.Invoke(this, e.ErrorCode);
      }
    }

    public async void JoinWithCode(string lobbyCode)
    {
      OnJoinStarted?.Invoke(this, EventArgs.Empty);

      if (lobbyCode.IsNullOrEmpty())
      {
        OnJoinFailed?.Invoke(this, ErrorCodeKey.FaultyLobbyCodeFormat);
        return;
      }

      try
      {
        QueryResponse lobbyList = await LobbyService.Instance.QueryLobbiesAsync();

        Unity.Services.Lobbies.Models.Lobby matchingLobby = lobbyList.Results.FirstOrDefault(lobby => lobby.Data["lobbyCode"].Value == lobbyCode);

        if (matchingLobby == null)
        {
          OnJoinFailed?.Invoke(this, ErrorCodeKey.LobbyNotFound);
          return;
        }
        
        if (matchingLobby.Data.ContainsKey("bannedPlayers"))
        {
          string bannedListJson = matchingLobby.Data["bannedPlayers"].Value;
          List<string> bannedList = JsonConvert.DeserializeObject<List<string>>(bannedListJson);
            
          if (bannedList != null && bannedList.Contains(AuthenticationService.Instance.PlayerId))
          {
            OnJoinFailed?.Invoke(this, ErrorCodeKey.BannedFromLobby);
            return;
          }
        }
        
        JoinWithId(matchingLobby.Id);
      }
      catch (LobbyServiceException e)
      {
        Debug.Log(e);
        OnJoinFailed?.Invoke(this, e.ErrorCode);
      }
    }

    public async void DeleteLobby()
    {
      if (joinedLobby != null)
      {
        try
        {
          await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

          joinedLobby = null;
        }
        catch (LobbyServiceException e)
        {
          Debug.Log(e);
        }
      }
    }

    public async void LeaveLobby()
    {
      if (joinedLobby == null)
      {
        return;
      }

      try
      {
        leftFromMenu = true;
        if (joinedLobby.Players.Count <= 1 || AuthenticationService.Instance.PlayerId == joinedLobby.HostId)
        {
          CheckLobbyServerRpc();
        }
        else
        {
          await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
      }
      catch (LobbyServiceException e)
      {
        leftFromMenu = false;
        Debug.Log(e);
      }
    }

    [ServerRpc(RequireOwnership = true)]
    private void CheckLobbyServerRpc(ServerRpcParams serverRpcParams = default)
    {
      if (joinedLobby.Players.Count <= 1 || AuthenticationService.Instance.PlayerId == joinedLobby.HostId)
      {
        DeleteLobby();
      }
    }

    public async void KickPlayer(string playerId)
    {
      if (!IsLobbyHost())
      {
        return;
      }

      List<string> banList;

      if (joinedLobby.Data.ContainsKey("bannedPlayers"))
      {
        string bannedListJson = joinedLobby.Data["bannedPlayers"].Value;
        banList = JsonConvert.DeserializeObject<List<string>>(bannedListJson);
      }
      else
      {
        banList = new List<string>();
      }

      banList.Add(playerId);

      await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
      {
        Data = new Dictionary<string, DataObject>
        {
          { KeyRelayJoinCode, new DataObject(DataObject.VisibilityOptions.Public, joinedLobby.Data[KeyRelayJoinCode].Value) },
          { "bannedPlayers", new DataObject(DataObject.VisibilityOptions.Public, JsonConvert.SerializeObject(banList)) },
          { "lobbyCode", new DataObject(DataObject.VisibilityOptions.Public, joinedLobby.LobbyCode) }
        }
      });


      try
      {
        await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
      }
      catch (LobbyServiceException e)
      {
        Debug.Log(e);
      }
    }

    public Unity.Services.Lobbies.Models.Lobby GetLobby()
    {
      return joinedLobby;
    }
  }
}
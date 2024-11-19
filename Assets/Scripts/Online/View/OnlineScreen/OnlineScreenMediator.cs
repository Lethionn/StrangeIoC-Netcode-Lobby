using System.Threading.Tasks;
using Common.Enum;
using Common.Scene;
using Lobby.Model;
using Online.Enum;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using Unity.Netcode;
using Unity.Services.Lobbies;

namespace Online.View.OnlineScreen
{
  public enum OnlineScreenEvent
  {
    CREATE,
    QUICK_GAME,
    JOIN_WITH_CODE
  }

  public class OnlineScreenMediator : EventMediator
  {
    [Inject]
    public OnlineScreenView view { get; set; }

    [Inject]
    public ILobbyNetworkService lobbyNetworkService { get; set; }
    
    public override void OnRegister()
    {
      view.dispatcher.AddListener(OnlineScreenEvent.CREATE, OnCreate);
      view.dispatcher.AddListener(OnlineScreenEvent.QUICK_GAME, OnQuickGame);
      view.dispatcher.AddListener(OnlineScreenEvent.JOIN_WITH_CODE, OnJoinWithCode);

      dispatcher.AddListener(OnlineEvent.FAILED_TO_JOIN_GAME, OnFailedToJoinGame);
      dispatcher.AddListener(OnlineEvent.CREATE_LOBBY_STARTED, OnCreateLobbyStarted);
      dispatcher.AddListener(OnlineEvent.CREATE_LOBBY_FAILED, OnCreateLobbyFailed);
      dispatcher.AddListener(OnlineEvent.JOIN_STARTED, OnJoinStarted);
      dispatcher.AddListener(OnlineEvent.JOIN_FAILED, OnJoinFailed);
      dispatcher.AddListener(OnlineEvent.QUICK_JOIN_FAILED, OnQuickJoinFailed);
    }
    
    public override void OnInitialize()
    {
      view.HideMessage();

      view.playerNameInputField.text = lobbyNetworkService.GetPlayerName();

      if (SceneLoader.leftFromMenu) return;
      CheckLastLobby();
      SceneLoader.leftFromMenu = true;
    }
    
    private async void CheckLastLobby()
    {
      await LobbyCheckTask();
    }
    
    public async Task LobbyCheckTask()
    {
      try
      {
        Unity.Services.Lobbies.Models.Lobby lobby = await LobbyService.Instance.GetLobbyAsync(SceneLoader.lastLobbyId);
        view.ShowMessage("You've been removed from the lobby.", true);
      }
      catch (LobbyServiceException)
      {
        view.ShowMessage("The lobby has been removed.", true);
      }
    }

    private void OnCreate()
    {
      lobbyNetworkService.SetPlayerName(view.playerNameInputField.text);
      lobbyNetworkService.CreateLobby(view.privateToggle.isOn);
    }

    private void OnQuickGame()
    {
      lobbyNetworkService.SetPlayerName(view.playerNameInputField.text);
      lobbyNetworkService.QuickGame();
    }

    private void OnJoinWithCode(IEvent payload)
    { 
      lobbyNetworkService.SetPlayerName(view.playerNameInputField.text); 
      lobbyNetworkService.JoinWithCode((string)payload.data); 
    }

    private void OnFailedToJoinGame()
    {
      view.ShowMessage(NetworkManager.Singleton.DisconnectReason == "" ? "Failed to connect" : NetworkManager.Singleton.DisconnectReason, true);
    }

    private void OnCreateLobbyStarted()
    {
      view.ShowMessage("Creating lobby...", false);
    }

    private void OnCreateLobbyFailed()
    {
      view.ShowMessage("Failed to create a lobby!", true);
    }

    private void OnJoinStarted()
    {
      view.ShowMessage("Joining Lobby...", false);
    }

    private void OnJoinFailed(IEvent payload)
    {
      int errorCode = (int)payload.data;

      switch (errorCode)
      {
        case ErrorCodeKey.LobbyNotFound or ErrorCodeKey.FaultyLobbyCodeFormat:
          view.ShowMessage("Lobby could not be found. Check the lobby code.", true);
          break;
        case ErrorCodeKey.BannedFromLobby:
          view.ShowMessage("You can't joint back to a lobby that you were removed from.", true);
          break;
        default:
          view.ShowMessage("Failed to join the lobby.", true);
          break;
      }
    }

    private void OnQuickJoinFailed()
    {
      view.ShowMessage("Could not find a Lobby to Quick Join!", true);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(OnlineScreenEvent.CREATE, OnCreate);
      view.dispatcher.RemoveListener(OnlineScreenEvent.QUICK_GAME, OnQuickGame);
      view.dispatcher.RemoveListener(OnlineScreenEvent.JOIN_WITH_CODE, OnJoinWithCode);

      dispatcher.RemoveListener(OnlineEvent.FAILED_TO_JOIN_GAME, OnFailedToJoinGame);
      dispatcher.RemoveListener(OnlineEvent.CREATE_LOBBY_STARTED, OnCreateLobbyStarted);
      dispatcher.RemoveListener(OnlineEvent.CREATE_LOBBY_FAILED, OnCreateLobbyFailed);
      dispatcher.RemoveListener(OnlineEvent.JOIN_STARTED, OnJoinStarted);
      dispatcher.RemoveListener(OnlineEvent.JOIN_FAILED, OnJoinFailed);
      dispatcher.RemoveListener(OnlineEvent.QUICK_JOIN_FAILED, OnQuickJoinFailed);
    }
  }
}
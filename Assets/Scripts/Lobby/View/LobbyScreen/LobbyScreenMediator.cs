using Common.Scene;
using Game.Model.Player;
using Lobby.Enum;
using Lobby.Model;
using Online.Controller;
using strange.extensions.mediation.impl;

namespace Lobby.View.LobbyScreen
{
  public enum LobbyScreenEvent
  { 
    START,
    LEAVE
  }
  
  public class LobbyScreenMediator : EventMediator
  {
    [Inject]
    public LobbyScreenView view { get; set; }
    
    [Inject]
    public ILobbyNetworkService lobbyNetworkService { get; set; }
    
    [Inject]
    public IPlayerNetworkService playerNetworkService { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(LobbyScreenEvent.START, OnStartGame);
      view.dispatcher.AddListener(LobbyScreenEvent.LEAVE, OnLeaveLobby);

      dispatcher.AddListener(LobbyEvent.PLAYER_NETWORK_LIST_CHANGED, OnPlayerNetworkListChanged);
      dispatcher.AddListener(LobbyEvent.READY_CHANGED, OnReadyChanged);
    }
    
    public override void OnInitialize()
    {
      SceneLoader.lastLobbyId = lobbyNetworkService.joinedLobby.Id;
      
      view.SetOwnerState(lobbyNetworkService.joinedLobby.HostId == playerNetworkService.playerId);
      view.SetLobbyInfo(lobbyNetworkService.joinedLobby.LobbyCode, lobbyNetworkService.joinedLobby.IsPrivate);
      UpdateStartButton();
    }
    
    private void OnStartGame()
    {
      lobbyNetworkService.StartGame();
    }
    
    private void OnPlayerNetworkListChanged()
    {
      UpdateStartButton();
    }

    private void OnReadyChanged()
    {
      UpdateStartButton();
    }

    private void OnLeaveLobby()
    {
      lobbyNetworkService.LeaveLobby();
    }

    private void UpdateStartButton()
    {
      view.UpdateStartButton(lobbyNetworkService.AreAllPlayersReady(), lobbyNetworkService.playerVoNetworkList.Count);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyScreenEvent.START, OnStartGame);
      view.dispatcher.RemoveListener(LobbyScreenEvent.LEAVE, OnLeaveLobby);
      
      dispatcher.RemoveListener(LobbyEvent.PLAYER_NETWORK_LIST_CHANGED, OnPlayerNetworkListChanged);
      dispatcher.RemoveListener(LobbyEvent.READY_CHANGED, OnReadyChanged);
    }
  }
}
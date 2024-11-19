using Game.Model.Player;
using Game.Vo;
using Lobby.Enum;
using Lobby.Model;
using strange.extensions.mediation.impl;

namespace Lobby.View.LobbyScreen
{
  public enum LobbyPlayerEvent
  {
    SET_READY,
    KICK_PLAYER
  }

  public class LobbyPlayerMediator : EventMediator
  {
    [Inject]
    public LobbyPlayerView view { get; set; }

    [Inject]
    public ILobbyNetworkService lobbyNetworkService { get; set; }

    [Inject]
    public IPlayerNetworkService playerNetworkService { get; set; }

    private PlayerVo _playerVo;

    public override void OnRegister()
    {
      view.dispatcher.AddListener(LobbyPlayerEvent.SET_READY, OnSetReady);
      view.dispatcher.AddListener(LobbyPlayerEvent.KICK_PLAYER, OnKickPlayer);

      dispatcher.AddListener(LobbyEvent.PLAYER_NETWORK_LIST_CHANGED, OnPlayerNetworkListChanged);
      dispatcher.AddListener(LobbyEvent.READY_CHANGED, OnReadyChanged);
    }

    public override void OnInitialize()
    {
      UpdatePlayer();
    }

    private void OnSetReady()
    {
      lobbyNetworkService.SetPlayerReady(!lobbyNetworkService.IsPlayerReady(_playerVo.clientId));
    }

    private void OnKickPlayer()
    {
      lobbyNetworkService.KickPlayer(_playerVo.clientId, _playerVo.playerId.ToString());
    }

    private void OnPlayerNetworkListChanged()
    {
      UpdatePlayer();
    }

    private void OnReadyChanged()
    {
      UpdatePlayer();
    }

    private void UpdatePlayer()
    {
      if (lobbyNetworkService.IsPlayerIndexConnected(view.playerIndex))
      {
        _playerVo = playerNetworkService.GetPlayerDataFromPlayerIndex(view.playerIndex);
        view.ShowPlayer(_playerVo.playerName.ToString(),
          playerNetworkService.playerId == _playerVo.playerId,
          lobbyNetworkService.joinedLobby.HostId == _playerVo.playerId,
          lobbyNetworkService.joinedLobby.HostId == playerNetworkService.playerId,
          lobbyNetworkService.IsPlayerReady(_playerVo.clientId));
      }
      else
      {
        view.HidePlayer();
      }
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(LobbyPlayerEvent.SET_READY, OnSetReady);
      view.dispatcher.RemoveListener(LobbyPlayerEvent.KICK_PLAYER, OnKickPlayer);

      dispatcher.RemoveListener(LobbyEvent.PLAYER_NETWORK_LIST_CHANGED, OnPlayerNetworkListChanged);
      dispatcher.RemoveListener(LobbyEvent.READY_CHANGED, OnReadyChanged);
    }
  }
}
using Common;
using Common.Enum;
using Common.Scene;
using Game.Vo;
using Lobby.Controller;
using Online.Controller;
using Unity.Netcode;


namespace Lobby.Model
{
  public class LobbyNetworkNetworkService : ILobbyNetworkService
  {
    public Unity.Services.Lobbies.Models.Lobby joinedLobby => OnlineNetworkController.instance.joinedLobby;
    
    public NetworkList<PlayerVo> playerVoNetworkList => MainNetworkController.instance.playerVoNetworkList;

    public void SetPlayerName(string name)
    {
      MainNetworkController.instance.SetPlayerName(name);
    }
    
    public string GetPlayerName()
    {
      return MainNetworkController.instance.GetPlayerName();
    }

    public void SetPlayerReady(bool isReady)
    {
      LobbyNetworkController.instance.SetPlayerReady(isReady);
    }

    public void KickPlayer(ulong clientId, string playerId)
    {
      OnlineNetworkController.instance.KickPlayer(playerId);
      MainNetworkController.instance.KickPlayer(clientId);
    }

    
    public bool IsPlayerIndexConnected(int index)
    {
      return MainNetworkController.instance.IsPlayerIndexConnected(index);
    }
    
    public bool IsPlayerReady(ulong clientId)
    {
      return LobbyNetworkController.instance.IsPlayerReady(clientId);
    }

    public bool AreAllPlayersReady()
    {
      return LobbyNetworkController.instance.AreAllPlayersReady();
    }

    public void LeaveLobby()
    { 
      OnlineNetworkController.instance.LeaveLobby();
      NetworkManager.Singleton.Shutdown();
      SceneLoader.Load(SceneKey.Online);
    }

    public void StartGame()
    {
      LobbyNetworkController.instance.StartGame();
    }

    public void CreateLobby(bool isPrivate)
    {
      OnlineNetworkController.instance.CreateLobby(isPrivate);

    }

    public void QuickGame()
    {
      OnlineNetworkController.instance.QuickGame();
    }

    public void JoinWithCode(string lobbyCode)
    {
      OnlineNetworkController.instance.JoinWithCode(lobbyCode);
    }
  }
}
using System.Threading.Tasks;
using Game.Vo;
using Unity.Netcode;

namespace Lobby.Model
{
  public interface ILobbyNetworkService
  {
    Unity.Services.Lobbies.Models.Lobby joinedLobby { get; }

    NetworkList<PlayerVo> playerVoNetworkList { get; }

    void SetPlayerName(string name);

    string GetPlayerName();
    
    void SetPlayerReady(bool isReady);

    void KickPlayer(ulong clientId, string playerId);
    
    bool IsPlayerIndexConnected(int index);
    
    bool IsPlayerReady(ulong clientId);

    bool AreAllPlayersReady();
    
    void LeaveLobby();

    void StartGame();

    void CreateLobby(bool isPrivate);

    void QuickGame();

    void JoinWithCode(string lobbyCode);
  }
}
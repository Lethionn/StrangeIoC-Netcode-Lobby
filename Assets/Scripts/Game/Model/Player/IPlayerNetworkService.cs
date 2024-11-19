using System.Collections.Generic;
using Game.Vo;

namespace Game.Model.Player
{
  public interface IPlayerNetworkService
  {
    string playerId { get; }
    PlayerVo GetPlayerDataFromPlayerIndex(int index);
    
    PlayerVo GetPlayerDataFromClientId(ulong clientId);
  }
}
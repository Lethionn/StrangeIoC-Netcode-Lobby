using Common;
using Game.Vo;
using Unity.Services.Authentication;

namespace Game.Model.Player
{
  public class PlayerNetworkService : IPlayerNetworkService
  {
    public string playerId =>  AuthenticationService.Instance.PlayerId;
    
    public PlayerVo GetPlayerDataFromPlayerIndex(int index)
    {
      return MainNetworkController.instance.GetPlayerDataFromPlayerIndex(index);
    }

    public PlayerVo GetPlayerDataFromClientId(ulong clientId)
    {
      return MainNetworkController.instance.GetPlayerDataFromClientId(clientId);
    }
  }
}
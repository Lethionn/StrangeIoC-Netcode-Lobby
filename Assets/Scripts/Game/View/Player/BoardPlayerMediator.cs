using Game.Model.Board;
using Game.Model.Player;
using Game.Vo;
using strange.extensions.mediation.impl;
using Unity.Netcode;

namespace Game.View.Player
{
  public enum PlayerEvent
  {
  }
  
  public class BoardPlayerMediator : EventMediator
  {

    [Inject]
      public BoardPlayerView view { get; set; }
      
      [Inject]
      public IPlayerNetworkService playerNetworkService { get; set; }
      
      [Inject]
      public IBoardModel boardModel { get; set; }
    
    
      public override void OnRegister()
      {

      }

      public override void OnInitialize()
      {
        PlayerVo playerVo = playerNetworkService.GetPlayerDataFromClientId(view.networkObject.OwnerClientId);
        view.SetPlayer(playerVo.playerName.ToString());
        
        boardModel.ResetPosition(transform);
      }
      
      public override void OnRemove()
      {

      }
  }
}
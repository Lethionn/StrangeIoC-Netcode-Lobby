using strange.extensions.mediation.impl;
using TMPro;
using Unity.Netcode;

namespace Game.View.Player
{
  public class BoardPlayerView : EventView
  {
    public NetworkObject networkObject;

    public TextMeshProUGUI playerNameTmp;

    public void SetPlayer(string playerName)
    {
      playerNameTmp.text = playerName;
      
      
    }
  }
}
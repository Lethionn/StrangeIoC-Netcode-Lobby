using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.View.LobbyScreen
{
  public class LobbyPlayerView : EventView
  {
    public int playerIndex;

    public GameObject playerContainerGo;

    public GameObject ownerLabelGo;
    
    public GameObject readyButtonGo;

    public GameObject kickButtonGo;

    public GameObject readyLabelGo;
    
    public TextMeshProUGUI playerNameTmp;


    public void SetReady()
    {
      dispatcher.Dispatch(LobbyPlayerEvent.SET_READY);
    }
    
    public void KickPlayer()
    {
      dispatcher.Dispatch(LobbyPlayerEvent.KICK_PLAYER);
    }
    
    public void ShowPlayer(string playerName, bool isMe, bool isHost, bool amIHost, bool isReady)
    {
      playerContainerGo.SetActive(true);
      ownerLabelGo.SetActive(isHost);
      readyButtonGo.SetActive(isMe);
      kickButtonGo.SetActive(amIHost && !isMe);
      
      playerNameTmp.text = playerName;
      
      readyLabelGo.SetActive(isReady);
    }
    
    public void HidePlayer()
    {
      playerContainerGo.SetActive(false);
    }
  }
}
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.UI;

namespace Lobby.View.LobbyScreen
{
  public class LobbyScreenView : EventView
  {
    public Button startButton;

    public TextMeshProUGUI startWarningTmp;

    public TextMeshProUGUI lobbyCodeTmp;

    public TextMeshProUGUI lobbyTypeTmp;

    public void ClickStartGame()
    {
      dispatcher.Dispatch(LobbyScreenEvent.START);
    }

    public void ClickLeaveLobby()
    {
      dispatcher.Dispatch(LobbyScreenEvent.LEAVE);
    }

    public void SetOwnerState(bool isOwner)
    {
      startButton.gameObject.SetActive(isOwner);
    }

    public void SetLobbyInfo(string lobbyCode, bool isPrivate)
    {
      lobbyCodeTmp.text = lobbyCode;

      lobbyTypeTmp.text = isPrivate ? "Private" : "Public";
    }

    public void UpdateStartButton(bool isAllReady, int playerCount)
    {
      startWarningTmp.text = string.Empty;
      
      if (isAllReady && (playerCount > 1))
      {
        startButton.interactable = true;
      }
      else
      {
        startButton.interactable = false;
          
        if (!isAllReady)
        {
          startWarningTmp.text += "<br> All players must be ready";
        }

        if (playerCount < 2)
        {
          startWarningTmp.text += "<br> There must be at least 2 players";
        }
      }
    }
  }
}
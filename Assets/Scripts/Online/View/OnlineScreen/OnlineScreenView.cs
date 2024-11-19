using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Online.View.OnlineScreen
{
  public class OnlineScreenView : EventView
  {
    public Toggle privateToggle;
    public TextMeshProUGUI messageTmp;
    public GameObject messageContainerGo;
    public GameObject closeButton;
    public TMP_InputField lobbyCodeInputField;
    public TMP_InputField playerNameInputField;
    
    public void ClickCreate()
    {
      dispatcher.Dispatch(OnlineScreenEvent.CREATE);
    }
    
    public void ClickQuickGame()
    {
      dispatcher.Dispatch(OnlineScreenEvent.QUICK_GAME);
    }
    
    public void ClickJoinWithCode()
    {
      dispatcher.Dispatch(OnlineScreenEvent.JOIN_WITH_CODE, lobbyCodeInputField.text);
    }
    
    public void ShowMessage(string message, bool showButton) {
      messageTmp.text = message;
      messageContainerGo.SetActive(true);
      closeButton.SetActive(showButton);
    }
    

    public void HideMessage() {
      messageContainerGo.SetActive(false);
    }
  }
}
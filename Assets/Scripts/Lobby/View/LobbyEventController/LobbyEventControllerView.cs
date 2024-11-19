using Common;
using Lobby.Controller;
using Online.Controller;
using strange.extensions.mediation.impl;

namespace Lobby.View.LobbyEventController
{
    public class LobbyEventControllerView : EventView {
        
        protected override void Start() {
            base.Start();
            
            MainNetworkController.instance.OnPlayerNetworkListChanged += OnPlayerNetworkListChanged;
            LobbyNetworkController.instance.OnReadyChanged += OnReadyChanged;
        }
        
        private void OnPlayerNetworkListChanged(object sender, System.EventArgs e)
        {
            dispatcher.Dispatch(LobbyEventControllerEvent.PLAYER_NETWORK_LIST_CHANGED);
        }
        
        private void OnReadyChanged(object sender, System.EventArgs e)
        {
            dispatcher.Dispatch(LobbyEventControllerEvent.READY_CHANGED);
        }
        
        protected override void OnDestroy() {
            base.OnDestroy();
            
            MainNetworkController.instance.OnPlayerNetworkListChanged -= OnPlayerNetworkListChanged;
            LobbyNetworkController.instance.OnReadyChanged -= OnReadyChanged;
        }
    }
}
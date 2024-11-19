using Common;
using Online.Controller;
using strange.extensions.mediation.impl;

namespace Online.View.OnlineEventController
{
    public class OnlineEventControllerView : EventView {
        
        protected override void Start() {
            base.Start();
            
            MainNetworkController.instance.OnLeftLobby += OnFailedToJoinGame;
            OnlineNetworkController.instance.OnCreateLobbyStarted += OnCreateLobbyStarted;
            OnlineNetworkController.instance.OnCreateLobbyFailed += OnCreateLobbyFailed;
            OnlineNetworkController.instance.OnJoinStarted += OnJoinStarted;
            OnlineNetworkController.instance.OnJoinFailed += OnJoinFailed;
            OnlineNetworkController.instance.OnQuickJoinFailed += OnQuickJoinFailed;
        }
        
        private void OnQuickJoinFailed(object sender, System.EventArgs e) {
            dispatcher.Dispatch(OnlineEventControllerEvent.QUICK_JOIN_FAILED);
        }

        private void OnJoinFailed(object sender, int errorCode) {
            dispatcher.Dispatch(OnlineEventControllerEvent.JOIN_FAILED, errorCode);
        }

        private void OnJoinStarted(object sender, System.EventArgs e) {
            dispatcher.Dispatch(OnlineEventControllerEvent.JOIN_STARTED);
        }

        private void OnCreateLobbyFailed(object sender, System.EventArgs e) {           
            dispatcher.Dispatch(OnlineEventControllerEvent.CREATE_LOBBY_FAILED);
        }

        private void OnCreateLobbyStarted(object sender, System.EventArgs e) {
            dispatcher.Dispatch(OnlineEventControllerEvent.CREATE_LOBBY_STARTED);
        }

        private void OnFailedToJoinGame(object sender, System.EventArgs e) {
            dispatcher.Dispatch(OnlineEventControllerEvent.FAILED_TO_JOIN_GAME);
        }
        

        protected override void OnDestroy() {
            base.OnDestroy();
            
            MainNetworkController.instance.OnLeftLobby -= OnFailedToJoinGame;
            OnlineNetworkController.instance.OnCreateLobbyStarted -= OnCreateLobbyStarted;
            OnlineNetworkController.instance.OnCreateLobbyFailed -= OnCreateLobbyFailed;
            OnlineNetworkController.instance.OnJoinStarted -= OnJoinStarted;
            OnlineNetworkController.instance.OnJoinFailed -= OnJoinFailed;
            OnlineNetworkController.instance.OnQuickJoinFailed -= OnQuickJoinFailed;
        }
    }
}
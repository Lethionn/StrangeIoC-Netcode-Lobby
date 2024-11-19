using Online.Enum;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

namespace Online.View.OnlineEventController
{
    public enum OnlineEventControllerEvent
    {
       FAILED_TO_JOIN_GAME,
       CREATE_LOBBY_STARTED,
       CREATE_LOBBY_FAILED,
       JOIN_STARTED,
       JOIN_FAILED,
       QUICK_JOIN_FAILED
    }
    
    public class OnlineEventControllerMediator : EventMediator {

        [Inject]
        public OnlineEventControllerView controllerView { get; set; }

        public override void OnRegister()
        {
            controllerView.dispatcher.AddListener(OnlineEventControllerEvent.FAILED_TO_JOIN_GAME, OnFailedToJoinGame);
            controllerView.dispatcher.AddListener(OnlineEventControllerEvent.CREATE_LOBBY_STARTED, OnCreateLobbyStarted);
            controllerView.dispatcher.AddListener(OnlineEventControllerEvent.CREATE_LOBBY_FAILED, OnCreateLobbyFailed);
            controllerView.dispatcher.AddListener(OnlineEventControllerEvent.JOIN_STARTED, OnJoinStarted);
            controllerView.dispatcher.AddListener(OnlineEventControllerEvent.JOIN_FAILED, OnJoinFailed);
            controllerView.dispatcher.AddListener(OnlineEventControllerEvent.QUICK_JOIN_FAILED, OnQuickJoinFailed);
        }
        
        private void OnFailedToJoinGame()
        {
            dispatcher.Dispatch(OnlineEvent.FAILED_TO_JOIN_GAME);
        }
        
        private void OnCreateLobbyStarted()
        {
            dispatcher.Dispatch(OnlineEvent.CREATE_LOBBY_STARTED);
        }
        
        private void OnCreateLobbyFailed()
        {
            dispatcher.Dispatch(OnlineEvent.CREATE_LOBBY_FAILED);
        }
        
        private void OnJoinStarted()
        {
            dispatcher.Dispatch(OnlineEvent.JOIN_STARTED);
        }

        private void OnJoinFailed(IEvent payload)
        {
            dispatcher.Dispatch(OnlineEvent.JOIN_FAILED, (int)payload.data);
        }

        private void OnQuickJoinFailed()
        {
            dispatcher.Dispatch(OnlineEvent.QUICK_JOIN_FAILED);
        }
        
        public override void OnRemove()
        {
            controllerView.dispatcher.RemoveListener(OnlineEventControllerEvent.FAILED_TO_JOIN_GAME, OnFailedToJoinGame);
            controllerView.dispatcher.RemoveListener(OnlineEventControllerEvent.CREATE_LOBBY_STARTED, OnCreateLobbyStarted);
            controllerView.dispatcher.RemoveListener(OnlineEventControllerEvent.CREATE_LOBBY_FAILED, OnCreateLobbyFailed);
            controllerView.dispatcher.RemoveListener(OnlineEventControllerEvent.JOIN_STARTED, OnJoinStarted);
            controllerView.dispatcher.RemoveListener(OnlineEventControllerEvent.JOIN_FAILED, OnJoinFailed);
            controllerView.dispatcher.RemoveListener(OnlineEventControllerEvent.QUICK_JOIN_FAILED, OnQuickJoinFailed);
        }
    }
}
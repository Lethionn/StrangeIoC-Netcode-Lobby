using System.Collections.Generic;
using Common.Enum;
using Common.Scene;
using Lobby.Enum;
using Lobby.Model;
using strange.extensions.mediation.impl;
using Unity.Netcode;
using Unity.Services.Lobbies;

namespace Lobby.View.LobbyEventController
{
    public enum LobbyEventControllerEvent
    {
        PLAYER_NETWORK_LIST_CHANGED,
        READY_CHANGED,
    }
    
    public class LobbyEventControllerMediator : EventMediator {

        [Inject]
        public LobbyEventControllerView view { get; set; }
        
        [Inject]
        public ILobbyNetworkService lobbyNetworkService { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(LobbyEventControllerEvent.PLAYER_NETWORK_LIST_CHANGED, OnPlayerNetworkListChanged);
            view.dispatcher.AddListener(LobbyEventControllerEvent.READY_CHANGED, OnReadyChanged);
        }
        
        private void OnPlayerNetworkListChanged()
        {
            dispatcher.Dispatch(LobbyEvent.PLAYER_NETWORK_LIST_CHANGED);
        }

        private void OnReadyChanged()
        {
            dispatcher.Dispatch(LobbyEvent.READY_CHANGED);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(LobbyEventControllerEvent.PLAYER_NETWORK_LIST_CHANGED, OnPlayerNetworkListChanged);
            view.dispatcher.RemoveListener(LobbyEventControllerEvent.READY_CHANGED, OnReadyChanged);
        }
    }
}
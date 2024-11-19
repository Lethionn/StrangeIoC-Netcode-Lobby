using Common;
using Game.Model.Player;
using Game.View.Board;
using Lobby.Model;
using Lobby.View;
using Lobby.View.LobbyEventController;
using Lobby.View.LobbyScreen;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Lobby.Config
{
  public class LobbyContext : MVCSContext
  {
    public LobbyContext(MonoBehaviour view)
      : base(view)
    {
    }

    public LobbyContext(MonoBehaviour view, ContextStartupFlags flags)
      : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      injectionBinder.Bind<ILobbyNetworkService>().To<LobbyNetworkNetworkService>().ToSingleton();
      injectionBinder.Bind<IPlayerNetworkService>().To<PlayerNetworkService>().ToSingleton();

      mediationBinder.Bind<LobbyEventControllerView>().To<LobbyEventControllerMediator>();
      mediationBinder.Bind<LobbyScreenView>().To<LobbyScreenMediator>();
      mediationBinder.Bind<LobbyPlayerView>().To<LobbyPlayerMediator>();
    }
  }
}
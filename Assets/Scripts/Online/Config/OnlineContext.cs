using Lobby.Model;
using Online.View.OnlineEventController;
using Online.View.OnlineScreen;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Online.Config
{
  public class OnlineContext : MVCSContext
  {
    public OnlineContext(MonoBehaviour view) : base(view)
    {
    }

    public OnlineContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      injectionBinder.Bind<ILobbyNetworkService>().To<LobbyNetworkNetworkService>().ToSingleton();
      
      mediationBinder.Bind<OnlineEventControllerView>().To<OnlineEventControllerMediator>();
      mediationBinder.Bind<OnlineScreenView>().To<OnlineScreenMediator>();
    }
  }
}
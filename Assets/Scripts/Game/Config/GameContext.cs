using Game.Model.Board;
using Game.Model.Player;
using Game.View.Board;
using Game.View.Player;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Game.Config
{
  public class GameContext : MVCSContext
  {
    public GameContext(MonoBehaviour view)
      : base(view)
    {
    }

    public GameContext(MonoBehaviour view, ContextStartupFlags flags)
      : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      injectionBinder.Bind<IPlayerNetworkService>().To<PlayerNetworkService>().ToSingleton();
      injectionBinder.Bind<IBoardModel>().To<BoardModel>().ToSingleton();
      injectionBinder.Bind<IPlayerModel>().To<PlayerModel>().ToSingleton();

      mediationBinder.Bind<BoardControllerView>().To<BoardControllerMediator>();
      mediationBinder.Bind<BoardPlayerView>().To<BoardPlayerMediator>();
    }
  }
}
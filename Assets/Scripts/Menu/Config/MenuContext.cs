using Common;
using Menu.Model;
using Menu.View;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Menu.Config
{
  public class MenuContext : MVCSContext
  {
    public MenuContext(MonoBehaviour view)
      : base(view)
    {
    }

    public MenuContext(MonoBehaviour view, ContextStartupFlags flags)
      : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      mediationBinder.Bind<MenuScreenView>().To<MenuScreenMediator>();

      injectionBinder.Bind<IMenuModel>().To<MenuModel>();
    }
  }
}
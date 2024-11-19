using Menu.Model;
using strange.extensions.mediation.impl;

namespace Menu.View
{
  public enum MenuScreenEvent
  {
    ONLINE,
    LOCAL,
    SINGLE
  }

  public class MenuScreenMediator : EventMediator
  {
    [Inject]
    public MenuScreenView view { get; set; }
    
    [Inject]
    public IMenuModel menuModel { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(MenuScreenEvent.ONLINE, OnOnline);
      view.dispatcher.AddListener(MenuScreenEvent.LOCAL, OnOnline);
      view.dispatcher.AddListener(MenuScreenEvent.SINGLE, OnOnline);
    }

    private void OnOnline()
    {
      menuModel.StartOnlineGame();
    }

    private void OnLocal()
    {
    }

    private void OnSingle()
    {
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(MenuScreenEvent.ONLINE, OnOnline);
      view.dispatcher.RemoveListener(MenuScreenEvent.LOCAL, OnOnline);
      view.dispatcher.RemoveListener(MenuScreenEvent.SINGLE, OnOnline);
    }
  }
}
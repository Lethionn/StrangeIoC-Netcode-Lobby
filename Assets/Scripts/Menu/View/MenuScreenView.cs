using strange.extensions.mediation.impl;

namespace Menu.View
{
  public class MenuScreenView : EventView
  {
    public void ClickOnline()
    {
      dispatcher.Dispatch(MenuScreenEvent.ONLINE);
    }

    public void ClickLocal()
    {
      dispatcher.Dispatch(MenuScreenEvent.LOCAL);

    }

    public void ClickSingle()
    {
      dispatcher.Dispatch(MenuScreenEvent.SINGLE);
    }
  }
}
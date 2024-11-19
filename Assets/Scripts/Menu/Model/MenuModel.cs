using Common;
using Common.Enum;
using Common.Scene;

namespace Menu.Model
{
  public class MenuModel : IMenuModel
  {
    public void StartOnlineGame()
    {
      SceneLoader.Load(SceneKey.Online);
    }
  }
}
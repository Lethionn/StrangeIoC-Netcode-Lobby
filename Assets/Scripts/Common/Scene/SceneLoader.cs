using Common.Enum;
using Online.Controller;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Scene
{
  public static class SceneLoader
  {
    private static string _targetScene;
    private static string _currentScene = SceneKey.Menu;

    public static bool leftFromMenu = true;

    public static string lastLobbyId; 
    
    public static void Load(string sceneName)
    {
      _targetScene = sceneName;
      
      SceneManager.LoadScene(SceneKey.Loading);
    }

    public static void LoadNetwork(string sceneName)
    {
      _targetScene = sceneName;

      ClearNetwork();

      NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

      _currentScene = sceneName;
    }

    public static void LoaderCallback()
    {
      ClearNetwork();

      SceneManager.LoadScene(_targetScene);

      _currentScene = _targetScene;
    }

    private static void ClearNetwork()
    {
      if (_targetScene == SceneKey.Menu)
      {
        NetworkManager.Singleton.Shutdown();
      }
      
      switch (_currentScene)
      {
        case SceneKey.Menu:
        case SceneKey.Online when _targetScene == SceneKey.Lobby:
        case SceneKey.Lobby when _targetScene == SceneKey.Game:
          return;
        default:
          ClearNetworkControllers();
          break;
      }
    }

    private static void ClearNetworkControllers()
    {
      Object.Destroy(NetworkManager.Singleton.gameObject);
      
      Object.Destroy(MainNetworkController.instance.gameObject);
      
      Object.Destroy(OnlineNetworkController.instance.gameObject);
    }
  }
}
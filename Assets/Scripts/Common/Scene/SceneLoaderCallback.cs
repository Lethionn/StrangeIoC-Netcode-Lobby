using UnityEngine;

namespace Common.Scene
{
  public class SceneLoaderCallback : MonoBehaviour {


    private bool _isFirstUpdate = true;

    private void Update()
    {
      if (!_isFirstUpdate) return;
      _isFirstUpdate = false;

      SceneLoader.LoaderCallback();
    }

  }
}
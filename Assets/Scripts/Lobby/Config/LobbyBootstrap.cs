using Game.Config;
using strange.extensions.context.impl;

namespace Lobby.Config
{
    public class LobbyBootstrap : ContextView
    {
        private void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new LobbyContext(this);
        }
    }
}

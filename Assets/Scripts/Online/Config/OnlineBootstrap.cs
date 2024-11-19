using strange.extensions.context.impl;

namespace Online.Config
{
    public class OnlineBootstrap : ContextView
    {
        private void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new OnlineContext(this);
        }
    }
}

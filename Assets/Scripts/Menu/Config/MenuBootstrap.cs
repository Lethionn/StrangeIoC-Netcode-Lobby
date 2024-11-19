using strange.extensions.context.impl;

namespace Menu.Config
{
    public class MenuBootstrap : ContextView
    {
        private void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new MenuContext(this);
        }
    }
}

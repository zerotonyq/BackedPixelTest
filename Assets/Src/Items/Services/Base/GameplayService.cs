using Zenject;

namespace Src.Items.Services.Base
{
    public abstract class GameplayService : IInitializable
    {
        [Inject] protected SignalBus SignalBus;
        
        public abstract void Initialize();

        public abstract void Dispose();
    }
}
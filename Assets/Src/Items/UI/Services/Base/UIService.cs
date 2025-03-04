using UnityEngine;
using Zenject;

namespace Src.Items.UI.Services.Base
{
    public abstract class UIService : MonoBehaviour, IInitializable
    {
        [Inject] protected SignalBus SignalBus;

        public abstract void Initialize();
    }
}
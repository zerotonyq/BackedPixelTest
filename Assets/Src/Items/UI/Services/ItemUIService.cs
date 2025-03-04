using Src.Items.UI.Services.Base;
using Src.Signals.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Src.Items.UI.Services
{
    public class ItemUIService : UIService
    {
        [SerializeField] private Button addRandomItemButton;
        [SerializeField] private Button removeRandomItemButton;

        [Inject]
        public override void Initialize()
        {
            addRandomItemButton.onClick.AddListener(() => SignalBus.Fire<AddRandomItemRequestSignal>());
            removeRandomItemButton.onClick.AddListener(() => SignalBus.Fire<RemoveRandomItemSignal>());
        }
    }
}
using Src.Items.UI.Services.Base;
using Src.Signals.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Src.Items.UI.Services
{
    public class GunUIService : UIService
    {
        [SerializeField] private Button fireButton;
        [SerializeField] private Button addAmmoButton;

        [Inject]
        public override void Initialize()
        {
            fireButton.onClick.AddListener(() => SignalBus.Fire<FireRequestSignal>());
            addAmmoButton.onClick.AddListener(() => SignalBus.Fire<AddAmmoRequestSignal>());
        }
    }
}
using Src.Items.Config;
using Src.Items.Services.Base;
using Src.Items.Services.ConfigProvider;
using Src.Items.Services.Inventory;
using Src.Signals.UI;
using Zenject;

namespace Src.Items.Services.Gun
{
    public class GunItemService : GameplayService, IInitializable
    {
        [Inject] private InventoryService _inventoryService;
        
        [Inject] private ItemConfigProvider _itemConfigProvider;
        
        
        public override void Initialize()
        {
            SignalBus.Subscribe<FireRequestSignal>(OnFireRequested);
            SignalBus.Subscribe<AddAmmoRequestSignal>(OnAddAmmoRequested);
        }

        private void OnAddAmmoRequested()
        {
            var itemConfig = _itemConfigProvider.GetRandomItem<AmmoItemConfig>();

            if (itemConfig.config == null)
                return;
            
            _inventoryService.TryAddToInventory(itemConfig.config, itemConfig.guid);
        }

        private void OnFireRequested() => _inventoryService.ChangeRandomItemTypeStackCount<AmmoItemConfig>();


        public override void Dispose()
        {
            SignalBus.Unsubscribe<FireRequestSignal>(OnFireRequested);
            SignalBus.Unsubscribe<FireRequestSignal>(OnAddAmmoRequested);
        }
    }
}
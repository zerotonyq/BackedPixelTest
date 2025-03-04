using Src.Items.Services.ConfigProvider;
using Src.Items.Services.Gun;
using Src.Items.Services.Inventory;
using Src.Items.Services.Inventory.Config;
using Src.Items.Services.Provider.Config;
using Src.Items.UI.Services;
using Src.Items.UI.Services.Inventory;
using Src.Signals;
using Src.Signals.UI;
using UnityEngine;
using Zenject;

namespace Src.DI
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("UI Services"), Space]
        [SerializeField] private GunUIService gunUIService;
        [SerializeField] private ItemUIService itemUIService;
        [SerializeField] private InventoryUIService inventoryUIService;

        [Header("Configs"), Space]
        [SerializeField] private InventoryServiceConfig inventoryServiceConfig;
        [SerializeField] private ItemConfigProviderConfig itemConfigProviderConfig;
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            DeclareSignals();
            
            Container.BindInterfacesAndSelfTo<ItemConfigProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GunItemService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InventoryService>().AsSingle().NonLazy();
            
            Container.BindInstance(gunUIService).AsSingle().NonLazy();
            Container.BindInstance(itemUIService).AsSingle().NonLazy();
            Container.BindInstance(inventoryUIService).AsSingle().NonLazy();
            
            Container.BindInstance(inventoryServiceConfig).AsSingle().NonLazy();
            Container.BindInstance(itemConfigProviderConfig).AsSingle().NonLazy();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<AddAmmoRequestSignal>();
            Container.DeclareSignal<AddRandomItemRequestSignal>();
            Container.DeclareSignal<FireRequestSignal>();
            Container.DeclareSignal<RemoveRandomItemSignal>();
            Container.DeclareSignal<ItemChangedSignal>();
            Container.DeclareSignal<ItemConfigProviderInitializedSignal>();
        }
    }
}
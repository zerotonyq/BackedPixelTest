using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Src.Items.Config.Base;
using Src.Items.Services.Base;
using Src.Items.Services.Provider.Config;
using Src.Signals;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Src.Items.Services.ConfigProvider
{
    public class ItemConfigProvider : GameplayService
    {
        [Inject] private ItemConfigProviderConfig _itemConfigProviderConfig;


        private readonly Dictionary<string, ItemConfig> _itemConfigs = new();
        
        
        public override async void Initialize()
        {
            foreach (var availableItemConfig in _itemConfigProviderConfig.availableItemConfigs)
            {
                _itemConfigs.TryAdd(
                    availableItemConfig.AssetGUID,
                    await Addressables.LoadAssetAsync<ItemConfig>(availableItemConfig));
            }
            
            SignalBus.Fire<ItemConfigProviderInitializedSignal>();
        }


        public (string guid, ItemConfig config) GetRandomItem<T>() where T : ItemConfig
        {
            var items = _itemConfigs.Where(a => a.Value is T).ToList();

            var item = items[Random.Range(0, items.Count)];

            return (item.Key, item.Value);
        }

        public ItemConfig GetItem(string key)
        {
            return string.IsNullOrEmpty(key) ? null : _itemConfigs.GetValueOrDefault(key);
        }
        
        public override void Dispose() { }
    }
}
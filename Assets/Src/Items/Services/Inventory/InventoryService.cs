using System;
using System.Collections.Generic;
using System.Linq;
using Src.DataPersistence;
using Src.Items.Config;
using Src.Items.Config.Base;
using Src.Items.Services.Base;
using Src.Items.Services.ConfigProvider;
using Src.Items.Services.Inventory.Config;
using Src.Signals;
using Src.Signals.UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Src.Items.Services.Inventory
{
    public class InventoryService : GameplayService
    {
        private List<ItemInternal> _items;
        public IReadOnlyList<ItemInternal> Items => _items;

        [Inject] private InventoryServiceConfig _inventoryServiceConfig;
        [Inject] private ItemConfigProvider _itemConfigProvider;

        [Serializable]
        public class ItemInternal
        {
            public int stackCount;
            [NonSerialized] public ItemConfig Config;
            public string configGuid;
        }

        public override void Initialize()
        {
            _items = Enumerable.Repeat(new ItemInternal(), _inventoryServiceConfig.slotsCount).ToList();
            
            SignalBus.Subscribe<AddRandomItemRequestSignal>(OnAddRandomItemRequested);
            SignalBus.Subscribe<RemoveRandomItemSignal>(OnRemoveRandomItemRequested);
            SignalBus.Subscribe<ItemConfigProviderInitializedSignal>(LoadItems);
        }

        private void LoadItems()
        {

            var loaded = DataPersistenceUtility.LoadData<List<ItemInternal>>();

            for (var i = 0; i < loaded.Count; i++)
            {
                var config = _itemConfigProvider.GetItem(loaded[i].configGuid);
                
                if (config == null)
                    continue;
                
                loaded[i].Config = config;

                TryAddToInventoryAtPosition(config, loaded[i].configGuid, i, loaded[i].stackCount);
            }
        }

        private void OnRemoveRandomItemRequested()
        {
            var item = GetRandomItemAndIndexOfType<ItemConfig>();

            if (item.itemInternal == null)
                return;

            ClearItem(item.itemInternal, item.index);
        }

        private void OnAddRandomItemRequested()
        {
            var item = _itemConfigProvider.GetRandomItem<EquipmentItemConfig>();

            TryAddToInventory(item.config, item.guid);
        }

        public void TryAddToInventory(ItemConfig itemConfig, string itemGuid, int stackCount = -1)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Config != null)
                    continue;

                _items[i] = new ItemInternal()
                {
                    Config = itemConfig,
                    stackCount = stackCount == -1 ? itemConfig.stackSize : stackCount,
                    configGuid = itemGuid
                };

                SignalBus.Fire(new ItemChangedSignal
                {
                    Color = _items[i].Config.color,
                    Index = i,
                    Sprite = _items[i].Config.sprite,
                    StackCount = _items[i].stackCount
                });

                DataPersistenceUtility.SaveData(_items);

                return;
            }
            
            Debug.LogError("There is no place in inventory");
        }

        public void TryAddToInventoryAtPosition(ItemConfig itemConfig, string itemGuid,  int i, int stackCount = -1)
        {
            if (_items[i].Config != null)
            {
                Debug.LogError("Busy position in inventory");
                return;
            }

            _items[i] = new ItemInternal()
            {
                Config = itemConfig,
                stackCount = stackCount == -1 ? itemConfig.stackSize : stackCount,
                configGuid = itemGuid
            };

            SignalBus.Fire(new ItemChangedSignal
            {
                Color = _items[i].Config.color,
                Index = i,
                Sprite = _items[i].Config.sprite,
                StackCount = _items[i].stackCount
            });

            DataPersistenceUtility.SaveData(_items);
        }

        public void ChangeRandomItemTypeStackCount<T>() where T : ItemConfig
        {
            var (itemInternal, i) = GetRandomItemAndIndexOfType<T>();

            if (itemInternal == null)
                return;

            --itemInternal.stackCount;

            if (itemInternal.stackCount <= 0)
            {
                ClearItem(itemInternal, i);
                return;
            }

            SignalBus.Fire(new ItemChangedSignal
            {
                Color = _items[i].Config.color,
                Index = i,
                Sprite = _items[i].Config.sprite,
                StackCount = _items[i].stackCount
            });

            DataPersistenceUtility.SaveData(_items);
        }

        private (ItemInternal itemInternal, int index) GetRandomItemAndIndexOfType<T>() where T : ItemConfig
        {
            var items = _items.FindAll(a => a.Config is T);

            if (items.Count == 0)
            {
                Debug.LogError("All slots are empty");
                return (null, -1);
            }

            var item = items[Random.Range(0, items.Count)];

            return (item, _items.IndexOf(item));
        }

        private void ClearItem(ItemInternal item, int i)
        {
            item.Config = null;
            item.stackCount = 0;
            item.configGuid = string.Empty;

            SignalBus.Fire(new ItemChangedSignal
            {
                Color = Color.white,
                Index = i,
                Sprite = null,
                StackCount = 0
            });

            DataPersistenceUtility.SaveData(_items);
        }

        public override void Dispose()
        {
            SignalBus.Unsubscribe<AddRandomItemRequestSignal>(OnAddRandomItemRequested);
            SignalBus.Unsubscribe<RemoveRandomItemSignal>(OnRemoveRandomItemRequested);
            SignalBus.Unsubscribe<ItemConfigProviderInitializedSignal>(LoadItems);
        }
    }
}
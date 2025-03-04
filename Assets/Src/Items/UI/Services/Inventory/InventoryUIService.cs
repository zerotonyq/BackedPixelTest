using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Src.Items.Services.Inventory.Config;
using Src.Items.UI.Services.Base;
using Src.Signals;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Src.Items.UI.Services.Inventory
{
    public class InventoryUIService : UIService
    {
        [SerializeField] private ScrollRect inventoryScroll;
        [SerializeField] private AssetReferenceGameObject inventoryTileReference;
        
        [Inject] private InventoryServiceConfig _inventoryServiceConfig;

        private List<InventoryTile> _tiles = new();

        [Inject]
        public override async void Initialize()
        {
            if (!inventoryScroll)
                Debug.LogError("No scroll provided for ui service " + nameof(InventoryUIService));

            if (inventoryTileReference == null)
                Debug.LogError("No tile prefab for ui service " + nameof(InventoryUIService));

            for (int i = 0; i < _inventoryServiceConfig.slotsCount; i++)
            {
                var tile = (await Addressables.InstantiateAsync(inventoryTileReference))
                    .GetComponent<InventoryTile>();

                _tiles.Add(tile);

                tile.transform.SetParent(inventoryScroll.content);
                tile.transform.localScale = Vector3.one;
            }
            
            SignalBus.Subscribe<ItemChangedSignal>(OnItemChanged);
        }

        private void OnItemChanged(ItemChangedSignal obj)
        {
            SetTileStackCount(obj.Index, obj.StackCount);
            SetTileSpriteAndColor(obj.Index, obj.Sprite, obj.Color);
        }

        private  void SetTileStackCount(int index, int count)
        {
            if (!ValidateTileIndex(index))
                return;

            _tiles[index].SetStackCount(count);
        }

        private void SetTileSpriteAndColor(int index, Sprite sprite, Color color)
        {
            if (!ValidateTileIndex(index))
                return;

            _tiles[index].SetSpriteAndColor(sprite, color);
        }

        private bool ValidateTileIndex(int index)
        {
            if (index < 0 || index >= _tiles.Count)
            {
                Debug.LogError("Wrong index of UI tile");
                return false;
            }

            return true;
        }
    }
}
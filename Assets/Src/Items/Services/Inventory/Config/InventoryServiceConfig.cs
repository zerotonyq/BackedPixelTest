using UnityEngine;

namespace Src.Items.Services.Inventory.Config
{
    [CreateAssetMenu(menuName = "ServiceConfig/" + nameof(InventoryServiceConfig), fileName = nameof(InventoryServiceConfig))]
    public class InventoryServiceConfig : ScriptableObject
    {
        public int slotsCount;
    }
}
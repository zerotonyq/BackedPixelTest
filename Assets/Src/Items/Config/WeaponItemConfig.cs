using UnityEngine;

namespace Src.Items.Config
{
    [CreateAssetMenu(menuName = "ItemConfig/" + nameof(WeaponItemConfig), fileName = nameof(WeaponItemConfig))]

    public class WeaponItemConfig : EquipmentItemConfig
    {
        public AmmoItemConfig ammoItemConfig;
        public int damage;
    }
}
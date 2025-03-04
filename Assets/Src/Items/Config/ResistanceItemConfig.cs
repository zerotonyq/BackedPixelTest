using UnityEngine;

namespace Src.Items.Config
{
    [CreateAssetMenu(menuName = "ItemConfig/" + nameof(ResistanceItemConfig), fileName = nameof(ResistanceItemConfig))]

    public class ResistanceItemConfig : EquipmentItemConfig
    {
        public int resistancePoints;
        public ResistanceType resistanceType;
    }

    public enum ResistanceType
    {
        None,
        Head,
        Torse
    }
}
using System.Collections.Generic;
using Src.Items.Config.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Src.Items.Services.Provider.Config
{
    [CreateAssetMenu(menuName = "ServiceConfig/" + nameof(ItemConfigProviderConfig), fileName = nameof(ItemConfigProviderConfig))]
    public class ItemConfigProviderConfig : ScriptableObject
    {
        public List<AssetReference> availableItemConfigs = new();

    }
}
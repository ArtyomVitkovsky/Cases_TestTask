using System;
using Setup.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Setup
{
    [Serializable]
    public class AssetReferenceItemsSetup : AssetReferenceT<ItemsSetup>
    {
        public AssetReferenceItemsSetup(string guid) : base(guid) { }
    }
    
    [CreateAssetMenu(menuName = "Setup/Game Setup", fileName = "Game_Setup")]
    public class GameSetup : ScriptableObject
    {
        [SerializeField] private AssetReferenceItemsSetup itemsSetupReference;

        public AssetReferenceItemsSetup ItemsSetupReference => itemsSetupReference;
    }
}
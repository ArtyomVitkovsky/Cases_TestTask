using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Setup.Items
{
    [Serializable]
    public class ItemsByRarity
    {
        [SerializeField] private Rarity rarity;
        [SerializeField] private Color borderColor = Color.white;
        [SerializeField] private ItemSetup[] itemSetups;

        public Rarity Rarity => rarity;

        public ItemSetup[] ItemSetups => itemSetups;

        public Color BorderColor => borderColor;
    }
    
    [CreateAssetMenu(menuName = "Setup/Items Setup", fileName = "Items_Setup")]
    public class ItemsSetup : ScriptableObject
    {
        [SerializeField] private ItemsByRarity[] itemsByRarities;

        public ItemSetup GetRandomItem()
        {
            ItemsByRarity itemsByRarity = GetItemsByRarity(GetRarity());
            
            var randomIndex = Random.Range(0, itemsByRarity.ItemSetups.Length);

            var item = itemsByRarity.ItemSetups[randomIndex];
            item.SetColor(itemsByRarity.BorderColor);
            
            return item;
        }

        private ItemsByRarity GetItemsByRarity(Rarity rarity)
        {
            ItemsByRarity itemsByRarity = null;

            for (int i = 0; i < itemsByRarities.Length; i++)
            {
                if (itemsByRarities[i].Rarity == rarity)
                {
                    itemsByRarity = itemsByRarities[i];
                    break;
                }
            }

            return itemsByRarity ?? itemsByRarities[0];
        }

        private Rarity GetRarity()
        {
            var randomProbability = Random.Range(0, 100);

            if (randomProbability - Rarity.Legendary < 0)
            {
                return Rarity.Legendary;
            }
            if (randomProbability - Rarity.Epic < 0)
            {
                return Rarity.Epic;
            }
            if (randomProbability - Rarity.Rare < 0)
            {
                return Rarity.Rare;
            }
            if (randomProbability - Rarity.Uncommon < 0)
            {
                return Rarity.Uncommon;
            }

            return Rarity.Common;
        }
    }
}
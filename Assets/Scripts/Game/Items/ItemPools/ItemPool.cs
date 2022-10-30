using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Items.ItemPools
{
    public class ItemPool : MonoBehaviour
    {
        public bool removeItemOnGet;

        public ItemPoolController.PoolType poolType;

        public List<Item> items;

        public UnityEvent<Item> itemPooled;

        public Item GetRandomItem()
        {
            if(items.Count == 0)
            {
                return null;
            }

            int index = Random.Range(0, items.Count);
            Item item = items[index];

            itemPooled.Invoke(item);

            return item;
        }

        public void RemoveItem(Item item)
        {
            if(removeItemOnGet)
            items.Remove(item);
        }
    }
}
using Assets.Scripts.Game.PlayerController;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemPools
{
    public class ItemPoolController : MonoBehaviour
    {
        [SerializeField]
        List<ItemPool> itemPools;

        [SerializeField]
        ItemPool breakfastPool;

        public Transform itemHolder;

        private void Awake()
        {
            foreach(ItemPool pool in itemPools)
            {
                pool.itemPooled.AddListener(OnItemPooled);
            }
        }

        public void AddItemToPools(Item item)
        {
            foreach (PoolType type in item.pools)
            {
                foreach (ItemPool pool in itemPools)
                {
                    if (pool.poolType == type && pool.removeItemOnGet)
                    {
                        pool.AddItem(item);
                    }
                }
            }
        }

        public ItemPool GetPool(PoolType poolType)
        {
            return itemPools.Find((ItemPool pool)=> pool.poolType == poolType);
        }

        public Item GetItemFromPool(PoolType type)
        {
            ItemPool pool = itemPools.Find((pool) => pool.poolType == type);

            if(pool != null)
            {
                Item found = pool.GetRandomItem();

                if(found != null)
                {
                    return Instantiate(found, Player.instance.itemsController.transform);
                }
                else
                {
                    return Instantiate(breakfastPool.GetRandomItem(), Player.instance.itemsController.transform);
                }
            }
            else
            {
                return Instantiate(breakfastPool.GetRandomItem(), Player.instance.itemsController.transform);
            }
        }

        public void OnItemPooled(Item item)
        {
            foreach(PoolType type in item.pools)
            {
                foreach(ItemPool pool in itemPools)
                {
                    if(pool.poolType == type && pool.removeItemOnGet)
                    {
                        pool.RemoveItem(item);
                    }
                }
            }
        }

        public enum PoolType
        {
            trasure, shop
        }
    }
}
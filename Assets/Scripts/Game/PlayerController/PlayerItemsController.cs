using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game.Items;

namespace Assets.Scripts.Game.PlayerController
{
    public class PlayerItemsController : MonoBehaviour
    {
        public int maxACtiveItemsCount;

        public int CurentActiveItems
        {
            get { return ActiveItems.Count; }
        }

        public List<Item> passiveItems;
        
        public List<Item> ActiveItems;

        public int selectedActiveItem;

        public ItemUseEvent OnItemUsed;

        public ItemEvent ActiveItemSwitched;

        public ItemEvent ItemAdded;

        public ItemEvent ItemRemoved;


        private void Start()
        {
            ChargeAllActive();

            GameMap.MainGameLevelMapController.Instance.SetUpComplete.AddListener(ChargeAllActive);
        }

        private void ChargeAllActive()
        {
            foreach(Item item in ActiveItems)
            {
                if (item.isChargeItem)
                {
                    item.Recharge();
                }
            }

            ActiveItemSwitched.Invoke(null);
        }

        public void SwitchActiveItem()
        {
            selectedActiveItem++;

            if(selectedActiveItem >= ActiveItems.Count)
            {
                selectedActiveItem = 0;
            }

            Item item = ActiveItems[selectedActiveItem];


            ActiveItemSwitched.Invoke(item);
        }

        public void AddItem(Item item, Item.ItemExternalEventArgs args)
        {
            if(item == null)
            {
                return;
            }

            if (item.isUseItem)
            {
                if(ActiveItems.Count < maxACtiveItemsCount)
                {
                    ActiveItems.Add(item);
                    item.OnItemAdd(args);
                }
                else
                {
                    ActiveItems[1].OnItemRemove(args);
                    ActiveItems.Add(item);
                    item.OnItemAdd(args);
                }
            }
            else
            {
                passiveItems.Add(item);
                item.OnItemAdd(args);
            }
        }

        public void RemoveItem(Item item, Item.ItemExternalEventArgs args)
        {
            if (item == null)
            {
                return;
            }

            if (item.isUseItem)
            {
                if(ActiveItems.Remove(item))
                item.OnItemRemove(args);
            }
            else
            {
                if(passiveItems.Remove(item))
                item.OnItemRemove(args);
            }
        }

        public void UseItem(Item.ItemExternalEventArgs args)
        {
            Item item = ActiveItems[selectedActiveItem];
            if (item.TryUseItem(args))
            {
                OnItemUsed.Invoke(item, args);
            }

            
        }

        [System.Serializable]
        public class ItemUseEvent : UnityEvent<Item, Item.ItemExternalEventArgs> { }

        [System.Serializable]
        public class ItemEvent : UnityEvent<Item> { }
       
    }
}
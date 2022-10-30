using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game.Items;
using Assets.Scripts.InputManager;
using Assets.Scripts.Game.GameMap;

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

        private GameLevelInputManager input;

        private MainGameLevelMapController main;

        private void Start()
        {
            ChargeAllActive();

            main = GameMap.MainGameLevelMapController.Instance;
            main.SetUpComplete.AddListener(ChargeAllActive);

            input = main.GameLevelInputManager;
            input.TilePositionChanged.AddListener(OnTilePositionChanged);
        }

        private void OnTilePositionChanged()
        {
            ActiveItems[selectedActiveItem].OnTilePosChanged(new Item.ItemExternalEventArgs()
            {
                mainGameController = main,
                player = Player.instance,
                tilePos = input.CurentTileMousePosition
            });
        }

        public void ChangeMaxActiveCount(int ammount)
        {
            if(ammount < 0)
            {
                maxACtiveItemsCount += ammount;

                if(maxACtiveItemsCount < 1)
                {
                    maxACtiveItemsCount = 1;
                }

                int dif = CurentActiveItems - maxACtiveItemsCount;

                for(int i = 0; i < dif; i++)
                {
                    ActiveItems.RemoveAt(ActiveItems.Count - 1);
                }
            }
            else
            {
                maxACtiveItemsCount += ammount;
            }
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

        public void SwitchActiveItem(Item.ItemExternalEventArgs args)
        {
            ActiveItems[selectedActiveItem].OnDeSelect(args);

            selectedActiveItem++;

            if(selectedActiveItem >= ActiveItems.Count)
            {
                selectedActiveItem = 0;
            }

            Item item = ActiveItems[selectedActiveItem];

            item.OnSelect(args);

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
                    ActiveItems.RemoveAt(1);
                    ActiveItems.Add(item);
                    item.OnItemAdd(args);
                    
                }

                ActiveItemSwitched.Invoke(item);
            }
            else
            {
                passiveItems.Add(item);
                item.OnItemAdd(args);
                
            }

            ItemAdded.Invoke(item);
        }

        public void RemoveItem(Item item, Item.ItemExternalEventArgs args)
        {
            if (item == null)
            {
                return;
            }

            if (item.isUseItem)
            {
                if(selectedActiveItem > 0)
                {
                    selectedActiveItem--;
                }

                if(ActiveItems.Remove(item))
                item.OnItemRemove(args);
                ActiveItemSwitched.Invoke(item);
            }
            else
            {
                if(passiveItems.Remove(item))
                item.OnItemRemove(args);
                
            }

            ItemRemoved.Invoke(item);
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
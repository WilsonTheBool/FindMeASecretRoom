using UnityEditor;
using UnityEngine;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.GameMap;
using System;

namespace Assets.Scripts.Game.Items
{
    public class Item : MonoBehaviour
    {
        public bool isUseItem;
        public bool isChargeItem;

        public string Name;
        public string Description;
        public Sprite Sprite;

        public ItemUseBehaviour[] useBehaviours;

        public ItemEffect[] onAddItemEffect;

        public ItemEffect[] onUseItemEffect;

        public int maxCharge;

        [HideInInspector]
        public int curentCharge;

        public virtual void OnItemAdd(ItemExternalEventArgs args)
        {
            foreach (ItemEffect itemEffect in onUseItemEffect)
            {
                itemEffect.OnEffectRemove(CreateEventArgs(args));
            }
        }

        private ItemInternalEventArgs CreateEventArgs(ItemExternalEventArgs args)
        {
            return new ItemInternalEventArgs() { item = this, external = args };
        }

        public bool TryUseItem(ItemExternalEventArgs args)
        {
            if (!isUseItem)
            {
                return false;
            }

            if(isChargeItem && curentCharge <= 0)
            {
                return false;
            }

            foreach (ItemUseBehaviour behaviour in useBehaviours)
            {
                if(behaviour == null || !behaviour.CanUse(CreateEventArgs(args)))
                {
                    return false;
                }
            }



            foreach (ItemEffect itemEffect in onUseItemEffect)
            {
                itemEffect.OnEffectAdd(CreateEventArgs(args));
            }

            if (isChargeItem)
            {
                curentCharge--;
            }

            return true;
        }

        public void Recharge()
        {
            if(isChargeItem)
            curentCharge = maxCharge;
        }

        public virtual void OnItemRemove(ItemExternalEventArgs args) 
        {
            foreach (ItemEffect itemEffect in onAddItemEffect)
            {
                itemEffect.OnEffectRemove(CreateEventArgs(args));
            }
        }

        /// <summary>
        /// Args for all event inside an item or its children
        /// </summary>
        public class ItemInternalEventArgs: EventArgs
        {
            public Item item;

            public ItemExternalEventArgs external;
        }


        /// <summary>
        /// Args for all event inside an item or its children
        /// </summary>
        public class ItemExternalEventArgs : EventArgs
        {
            public Player player;

            public MainGameLevelMapController mainGameController;

            public Vector2Int tilePos;
        }
    }
}
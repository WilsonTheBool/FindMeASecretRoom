using UnityEditor;
using UnityEngine;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.GameMap;
using System;
using Assets.Scripts.Game.Items.ItemPools;

namespace Assets.Scripts.Game.Items
{
    public class Item : MonoBehaviour
    {
        public bool isUseItem;
        public bool isChargeItem;
        public bool hasAltMode;

        public string Name;
        public string Description;
        public Sprite Sprite;

        public ItemUseBehaviour[] useBehaviours;

        public ItemEffect[] onAddItemEffect;

        public int maxCharge;

        [HideInInspector]
        public int curentCharge;

        public ItemPoolController.PoolType[] pools;

        public virtual void OnItemAdd(ItemExternalEventArgs args)
        {
            foreach (ItemEffect itemEffect in onAddItemEffect)
            {
                itemEffect.OnEffectAdd(CreateEventArgs(args));
            }
        }

        private ItemInternalEventArgs CreateEventArgs(ItemExternalEventArgs args)
        {
            return new ItemInternalEventArgs() { item = this, external = args };
        }

        public bool CanUseItem_AtAll()
        {
            if (!isUseItem)
            {
                return false;
            }
            else
            {
                if(isChargeItem)
                {
                    if(curentCharge > 0)
                    return true;
                    else
                    {
                        return false;
                    }
                }


                return true;
            }
        }

        public bool TryUseItem(ItemExternalEventArgs args)
        {
            if (!isUseItem)
            {
                return false;
            }

            ItemInternalEventArgs iArgs = CreateEventArgs(args);

            if (isChargeItem && curentCharge <= 0)
            {
                return false;
            }

            foreach (ItemUseBehaviour behaviour in useBehaviours)
            {
                if(behaviour == null || !behaviour.CanUse(iArgs))
                {
                    return false;
                }
            }



            foreach (ItemUseBehaviour behaviour in useBehaviours)
            {
                behaviour.OnUse(iArgs);
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

        public void Charge(int ammount)
        {
            if (isChargeItem)
                curentCharge += ammount;
            if(curentCharge > maxCharge)
            {
                curentCharge = maxCharge;
            }
        }

        public virtual void OnItemRemove(ItemExternalEventArgs args) 
        {
            ItemInternalEventArgs iArgs = CreateEventArgs(args);

            foreach (ItemEffect itemEffect in onAddItemEffect)
            {
                itemEffect.OnEffectRemove(iArgs);
            }
        }

        public void OnTilePosChanged(ItemExternalEventArgs args)
        {
            ItemInternalEventArgs iArgs = CreateEventArgs(args);

            foreach (ItemUseBehaviour use in useBehaviours)
            {
                use.OnTilePosChnaged(iArgs);
            }
        }

        public void OnSelect(ItemExternalEventArgs args)
        {
            ItemInternalEventArgs iArgs = CreateEventArgs(args);

            foreach (ItemUseBehaviour use in useBehaviours)
            {
                use.OnSelected(iArgs);
            }
        }

        public void OnDeSelect(ItemExternalEventArgs args)
        {
            ItemInternalEventArgs iArgs = CreateEventArgs(args);

            foreach (ItemUseBehaviour use in useBehaviours)
            {
                use.OnDeselected(iArgs);
            }
        }
        
        public void OnAltUse(ItemExternalEventArgs args)
        {
            ItemInternalEventArgs iArgs = CreateEventArgs(args);

            foreach (ItemUseBehaviour use in useBehaviours)
            {
                use.OnAlternativeUse(iArgs);
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
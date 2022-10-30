using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class CarBattery_Effect : ItemEffect
    {
        public List<ItemEffect> effects;

        private Item.ItemInternalEventArgs args;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            this.args = args;
            args.external.player.itemsController.ItemAdded.AddListener(OnItemAdd);
            args.external.player.itemsController.ItemRemoved.AddListener(OnItemRemove);

            foreach(Item item in args.external.player.itemsController.passiveItems)
            {
                OnItemAdd(item);
            }
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.player.itemsController.ItemAdded.RemoveListener(OnItemAdd);
            args.external.player.itemsController.ItemRemoved.RemoveListener(OnItemRemove);

            foreach(ItemEffect effect in effects)
            {
                effect.OnEffectRemove(args);
            }

            effects.Clear();
        }

        public void OnItemAdd(Item item)
        {
            if (!item.isUseItem)
            {
                foreach(ItemEffect itemEffect in item.onAddItemEffect)
                {
                    if(itemEffect is CarBattery_Effect)
                    {
                        return;
                    }

                    effects.Add(itemEffect);

                    itemEffect.OnEffectAdd(args);
                }
            }
        }

        public void OnItemRemove(Item item)
        {
            foreach(ItemEffect effect in item.onAddItemEffect)
            {
                int found = effects.IndexOf(effect);

                if(found != -1)
                {
                    effects[found].OnEffectRemove(args);
                    effects.RemoveAt(found);
                }

            }
        }
    }
}
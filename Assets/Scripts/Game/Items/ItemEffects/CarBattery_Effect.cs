using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class CarBattery_Effect : ItemEffect
    {
        public List<ItemEffect> effects;

        private Item.ItemInternalEventArgs args;

        public List<Item> excludingItems;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            if (excludingItems.Exists((Item i) => i.Name == args.item.Name))
            {
                return;
            }

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
            if (excludingItems.Exists((Item i) => i.Name == args.item.Name))
            {
                return;
            }

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

                    Item.ItemInternalEventArgs args2 = new Item.ItemInternalEventArgs() { item = item, external = args.external };

                    itemEffect.OnEffectAdd(args2);
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
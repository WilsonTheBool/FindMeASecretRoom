using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class TheBattery_Effect : ItemEffect
    {
        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.player.itemsController.ItemAdded.AddListener(TryAddCharge);

            foreach (Item item in args.external.player.itemsController.ActiveItems)
            {
                TryAddCharge(item);
            }
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.player.itemsController.ItemAdded.RemoveListener(TryAddCharge);

            foreach (Item item in args.external.player.itemsController.ActiveItems)
            {
                TryRemoveCharge(item);
            }
        }

        private void TryAddCharge(Item item)
        {
            if(item.isUseItem && item.isChargeItem)
            {
                item.maxCharge++;
            }
        }

        private void TryRemoveCharge(Item item)
        {
            if (item.isUseItem && item.isChargeItem)
            {
                item.maxCharge--;
            }
        }
    }
}
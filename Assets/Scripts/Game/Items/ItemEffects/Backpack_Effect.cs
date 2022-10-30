using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class Backpack_Effect : ItemEffect
    {

        public int slotsToAdd = 1;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.player.itemsController.ChangeMaxActiveCount(slotsToAdd);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.player.itemsController.ChangeMaxActiveCount(-slotsToAdd);
        }
    }
}
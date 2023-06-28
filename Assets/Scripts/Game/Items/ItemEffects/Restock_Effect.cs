using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class Restock_Effect : ItemEffect
    {
        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.progression.ShopRoomController.itemsRestock = true;
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.progression.ShopRoomController.itemsRestock = false;
        }

      
    }
}
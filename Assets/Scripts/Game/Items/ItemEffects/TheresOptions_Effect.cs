using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class TheresOptions_Effect : ItemEffect
    {
        public int numOfItemsAdd;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.progression.ShopRoomController.shopItemsCount += numOfItemsAdd;
            
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.progression.ShopRoomController.shopItemsCount -= numOfItemsAdd;
        }
    }
}
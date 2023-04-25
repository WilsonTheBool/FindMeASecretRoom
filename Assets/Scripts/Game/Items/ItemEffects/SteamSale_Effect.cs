using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class SteamSale_Effect : ItemEffect
    {
        public float saleChanceAdd;
        public int maxNumOfSales;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.progression.ShopRoomController.saleChance += saleChanceAdd;
            args.external.mainGameController.progression.ShopRoomController.maxSaleItems += maxNumOfSales;
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.progression.ShopRoomController.saleChance -= saleChanceAdd;
            args.external.mainGameController.progression.ShopRoomController.maxSaleItems -= maxNumOfSales;
        }
    }
}
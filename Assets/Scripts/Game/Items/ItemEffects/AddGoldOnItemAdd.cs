using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class AddGoldOnItemAdd : ItemEffect
    {
        public int AddGoldOnStart;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.player.goldController.AddGold(AddGoldOnStart);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            
        }
    }
}
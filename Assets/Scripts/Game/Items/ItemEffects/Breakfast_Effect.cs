using System.Collections;
using UnityEngine;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class Breakfast_Effect : ItemEffect
    {
        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.player.playerHPController.RequestAddContainer(new PlayerHPController.HpEventArgs(1, args.item.gameObject));

            args.external.player.playerHPController.HealRedHP(1);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            
        }
    }
}
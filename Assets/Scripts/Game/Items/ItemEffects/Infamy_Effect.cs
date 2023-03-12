using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class Infamy_Effect : ItemEffect
    {
        [Range(0f, 1f)]
        public float chanceToBlock;

        PlayerPassiveItemsUIController passive;
        Item item;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.player.playerHPController.beforeTakeDamage.AddListener(OnDamageTake);
            passive = GameUIController.Instance.PlayerPassiveItemsUIController;
            item = args.item;
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.player.playerHPController.beforeTakeDamage.RemoveListener(OnDamageTake);
        }

        private void OnDamageTake(PlayerHPController.HpEventArgs args)
        {
            if(Random.Range(0f,1f) < chanceToBlock)
            {
                args.change = -100;
                passive.StartHIghlightAnim(item);
            }
        }
    }
}
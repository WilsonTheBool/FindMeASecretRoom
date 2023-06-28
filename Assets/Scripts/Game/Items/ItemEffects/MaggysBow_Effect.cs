using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class MaggysBow_Effect : ItemEffect
    {
        public HpObject redHeart;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.player.playerHPController.RequestAddContainer_Empty(new PlayerController.PlayerHPController.HpEventArgs(1, args.item.gameObject));
            args.external.player.playerHPController.beforePickUpHeart.AddListener(OnHeal);
        }

        private void OnHeal(PlayerHPController.HpEventArgs args)
        {
            if(args.HpObject == redHeart)
            {
                args.change += 1;
            }
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.player.playerHPController.RemoveContainer(new PlayerController.PlayerHPController.HpEventArgs(1, args.item.gameObject));
            args.external.player.playerHPController.beforePickUpHeart.RemoveListener(OnHeal);
        }
    }
}
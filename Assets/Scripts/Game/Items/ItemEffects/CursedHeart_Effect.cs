using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class CursedHeart_Effect : ItemEffect
    {
        public int hpAdd = 2; 

        private PlayerHPController hpController;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            hpController = args.external.player.playerHPController;
            args.external.player.playerHPController.RequestAddContainer(new PlayerController.PlayerHPController.HpEventArgs(2, args.item.gameObject));
            args.external.mainGameController.levelStarted.AddListener(OnNewLevel);

            hpController.HealToFullRedHP();
        }

        private void OnNewLevel()
        {
            hpController.RequestTakeDamage(new PlayerHPController.HpEventArgs(1, this.gameObject));
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.player.playerHPController.RemoveContainer(new PlayerController.PlayerHPController.HpEventArgs(2, args.item.gameObject));
            args.external.mainGameController.levelStarted.RemoveListener(OnNewLevel);
        }
    }
}
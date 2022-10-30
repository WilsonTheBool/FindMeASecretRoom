using Assets.Scripts.Game.PlayerController;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class HolyMental_Effect : ItemEffect
    {
        bool canUse;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            canUse = true;

            args.external.mainGameController.levelStarted.AddListener(OnVictory);

            args.external.player.playerHPController.beforeTakeDamage.AddListener(OnTakeDamage);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.levelStarted.RemoveListener(OnVictory);

            args.external.player.playerHPController.beforeTakeDamage.RemoveListener(OnTakeDamage);
        }

        private void OnVictory()
        {
            canUse = true;
        }

        private void OnTakeDamage(PlayerHPController.HpEventArgs args)
        {
            if (canUse)
            {
                print(args.change);

                if (args.change <= 0)
                {
                    return;
                }

                args.change = -100;
                canUse = false;

                OnEffectActivated.Invoke();
            }
        }
    }
}
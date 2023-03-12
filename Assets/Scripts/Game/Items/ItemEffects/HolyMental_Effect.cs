using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class HolyMental_Effect : ItemEffect
    {
        bool canUse;

        private Item item;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            item = args.item;
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
            Debug.Log(item.Name);
            GameUIController.Instance.PlayerPassiveItemsUIController.SetHighlight(item, true);
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
                GameUIController.Instance.PlayerPassiveItemsUIController.SetHighlight(item, false);
                OnEffectActivated.Invoke();
            }
        }
    }
}
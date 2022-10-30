using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class PiggyBank_effect : ItemEffect
    {
        public int minGold;
        public int maxGold;

        public Item prefab;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {

            args.external.player.playerHPController.beforeTakeDamage.AddListener(OnTakeDamage);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {

            args.external.player.playerHPController.beforeTakeDamage.RemoveListener(OnTakeDamage);
        }


        private void OnTakeDamage(PlayerHPController.HpEventArgs args)
        {
            if(args.change <= 0)
            {
                return;
            }

            int randomValue = Random.Range(minGold, maxGold + 1);

            Player.instance.goldController.AddGold(randomValue);

            GameUIController mainUi = GameUIController.Instance;

            GameUIController.Instance.CreateTransferAnimation(new GameUIController.TransferAnimData
            {
                origin = mainUi.GetPosition_Ui_To_World(mainUi.PlayerPassiveItemsUIController.FindElement(prefab.Name).transform.position),
                destination = mainUi.GetPosition_Ui_To_World(mainUi.playerGoldUIController.transform.position),
                startScale = new Vector3(0.5f, 0.5f),
                endScale = new Vector3(0.5f, 0.5f),
                prefab = mainUi.gold_prefab,
            }, randomValue);

            OnEffectActivated.Invoke();
        }
    }
}
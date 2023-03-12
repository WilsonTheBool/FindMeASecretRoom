using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class IVBag_Behaviour : ItemUseBehaviour
    {
        public int minGold;
        public int maxGold;
        public int damageTaken = 1;

        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            return true;
        }

        public override void OnAlternativeUse(Item.ItemInternalEventArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselected(Item.ItemInternalEventArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelected(Item.ItemInternalEventArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTilePosChnaged(Item.ItemInternalEventArgs args)
        {
            throw new System.NotImplementedException();
        }

        public override void OnUse(Item.ItemInternalEventArgs args)
        {
            Player player = Player.instance;

            player.playerHPController.RequestTakeDamage(new PlayerHPController.HpEventArgs(damageTaken, this.gameObject));

            int randomValue = Random.Range(minGold, maxGold + 1);

            Player.instance.goldController.AddGold(randomValue);

            GameUIController mainUi = GameUIController.Instance;

            GameUIController.Instance.CreateTransferAnimation(new GameUIController.TransferAnimData
            {
                origin = mainUi.GetPosition_Ui_To_World(mainUi.PlayerTrinketUI.transform.position),
                destination = mainUi.GetPosition_Ui_To_World(mainUi.playerGoldUIController.transform.position),
                startScale = new Vector3(0.5f, 0.5f),
                endScale = new Vector3(0.5f, 0.5f),
                prefab = mainUi.gold_prefab,
            }, randomValue);
        }
    }
}
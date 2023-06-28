using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class AQuarter_Effect : ItemEffect
    {
        public int goldAdd = 25;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            int randomValue = goldAdd;

            Player.instance.goldController.AddGold(randomValue);

            GameUIController mainUi = GameUIController.Instance;

            mainUi.CreateTransferAnimation(new GameUIController.TransferAnimData
            {
                origin = mainUi.GetPosition_Ui_To_World(mainUi.TresureRoomUIController.itemsHolder.transform.position),
                destination = mainUi.GetPosition_Ui_To_World(mainUi.playerGoldUIController.transform.position),
                startScale = new Vector3(0.5f, 0.5f),
                endScale = new Vector3(0.5f, 0.5f),
                prefab = mainUi.gold_prefab,
            }, randomValue);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            
        }
    }
}
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class WoodenNickel_Effect : ItemEffect
    {
        public float coinDropChance = 0.5f;

        private Player Player;
        private GameUIController main;
        Item item;
        public TransferAnimationUIObject coinAnim;
        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            main = GameUIController.Instance;
            item = args.item;
            args.external.mainGameController.GameMapRoomUnlockController.roomUnlocked.AddListener(TryAddCoinOnRoomUnlock);
            Player = Player.instance;
        }

        private void TryAddCoinOnRoomUnlock(LevelGeneration.Room room)
        {
            if(room.type != null && room.type.isSecretRoom)
            if(Random.Range(0f,1f) < coinDropChance)
            {
                Player.goldController.AddGold(1);
                main.CreateTransferAnimation(new GameUIController.TransferAnimData()
                {
                    origin = main.GetPosition_Ui_To_World(main.PlayerPassiveItemsUIController.FindElement(item).transform.position),
                    destination = main.GetPosition_Ui_To_World(main.playerGoldUIController.transform.position),
                    prefab = coinAnim,
                    startScale = new Vector3(0.5f, 0.5f, 1),
                    endScale = new Vector3(0.5f, 0.5f, 1)
                }, 1);
            }
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.GameMapRoomUnlockController.roomUnlocked.RemoveListener(TryAddCoinOnRoomUnlock);
        }
    }
}
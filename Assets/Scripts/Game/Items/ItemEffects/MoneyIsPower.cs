using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class MoneyIsPower : ItemEffect
    {
        public int goldPerTrigger = 30;

        private MainGameLevelMapController main;

        //GameUIController mainUI;
        //public HpObject redHeart;
        //PlayerHPController playerHPController;

        //Item item;

        //public TransferAnimationUIObject hpAnim;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            //mainUI = GameUIController.Instance;
            //item = args.item;
            //playerHPController = args.external.player.playerHPController;
            //args.external.mainGameController.levelStarted.AddListener(HealHP);

            main = args.external.mainGameController;
            args.external.mainGameController.levelStarted.AddListener(DoAction);
        }

        private void DoAction()
        {
            int count = Player.instance.goldController.gold / goldPerTrigger;

            for(int i = 0; i < count; i++)
            {
                UnlockSecretRoom();
                //HealHP();
            }
        }

        private void UnlockSecretRoom()
        {
            List<Room> secretRooms = new List<Room>();

            foreach (Room room in main.LevelMap.rooms)
            {
                if (room.type != null && room.type.isSecretRoom && !main.GameMapRoomUnlockController.IsUnlocked(room.position))
                {
                    secretRooms.Add(room);
                }
            }

            if (secretRooms.Count > 0)
            {
                main.GameMapRoomUnlockController.TryUnlockRoom(secretRooms[Random.Range(0, secretRooms.Count)]);
            }
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.levelStarted.RemoveListener(UnlockSecretRoom);
        }

        //private void HealHP()
        //{
        //    if (playerHPController.CanPickUpHeart(redHeart))
        //    {
        //        playerHPController.RequestPickUpHeart(new PlayerHPController.HpEventArgs(1, redHeart, this.gameObject));

        //        mainUI.CreateTransferAnimation(new GameUIController.TransferAnimData()
        //        {
        //            origin = mainUI.GetPosition_Ui_To_World(mainUI.PlayerPassiveItemsUIController.FindElement(item).transform.position),
        //            destination = mainUI.GetPosition_Ui_To_World(mainUI.playerHpUIController.transform.position),
        //            prefab = hpAnim,
        //            startScale = new Vector3(0.3f, 0.3f, 1),
        //            endScale = new Vector3(0.3f, 0.3f, 1)
        //        }, 1);
        //    }



        //}
    }
}
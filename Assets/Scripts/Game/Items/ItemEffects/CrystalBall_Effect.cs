using Assets.Scripts.Game.GameMap;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LevelGeneration;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class CrystalBall_Effect : ItemEffect
    {
        private MainGameLevelMapController main;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            main = args.external.mainGameController;
            args.external.mainGameController.levelStarted.AddListener(UnlockSecretRoom);
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
    }
}
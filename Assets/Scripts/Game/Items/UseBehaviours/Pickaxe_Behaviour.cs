using UnityEditor;
using UnityEngine;
using Assets.Scripts.Game.Items;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class Pickaxe_Behaviour : ItemUseBehaviour
    {
        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            if (args.external.mainGameController.GameMapRoomUnlockController.IsUnlocked(args.external.tilePos))
            {
                return false;
            }

            return args.external.mainGameController.GameMapRoomUnlockController.CanCheckToUnlock(args.external.tilePos);
        }
    }
}
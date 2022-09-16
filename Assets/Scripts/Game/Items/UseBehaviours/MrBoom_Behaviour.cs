using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class MrBoom_Behaviour: ItemUseBehaviour
    {
        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            return args.external.mainGameController.GameMapRoomUnlockController.CanCheckToUnlock(args.external.tilePos);
        }
    }
}

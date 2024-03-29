﻿using Assets.Scripts.Game.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Achievements.Actions
{
    public class Action_UnlockItem: AchievmentAction
    {
        public Item itemToUnlock;

        public bool invokeEvents;

        public Action_UnlockItem(Item itemToUnlock, bool invokeEvents)
        {
            this.itemToUnlock = itemToUnlock;
            this.invokeEvents = invokeEvents;
        }

        public override void DoAction(AchivementArgs args)
        {
            args.challenges.unlockController.AddUnlockedItems(itemToUnlock, invokeEvents);
        }
    }
}

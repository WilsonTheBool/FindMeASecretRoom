using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Unlocks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression.Actions
{
    [CreateAssetMenu(menuName = "Progression/Add unlocks")]
    public class Progression_AddUnlocksToPools : ProgressionAction
    {
        public UnlockControllerData_SO UnlockControllerData_SO;
        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            foreach(Item item in UnlockControllerData_SO.UnlockedItems)
            {
                progression.ItemPoolController.AddItemToPools(item);
                Debug.Log("Item: " + item.Name + " Added to pool");
            }

            progression.LoadNextStep();
        }
    }
}
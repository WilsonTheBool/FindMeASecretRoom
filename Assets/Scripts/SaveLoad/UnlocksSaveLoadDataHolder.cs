using Assets.Scripts.Unlocks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class UnlocksSaveLoadDataHolder : SaveLoadComponent<SaveData>
    {
        [SerializeField]
        UnlockControllerData_SO UnlockControllerData_SO;
        public override void SetUp(SaveLoadController controller)
        {
            this.awakeOrder = -1;
            UnlockControllerData_SO.ClearUnlocks();
        }
    }
}
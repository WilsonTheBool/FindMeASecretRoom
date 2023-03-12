using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Unlocks
{
    public class UnlockController : MonoBehaviour
    {
        public UnlockControllerData_SO UnlockController_SO;

        public ItemUnlockedUI prefab;

        private GameUIController mainUI;
        private void Start()
        {
            UnlockController_SO.OnNewUnlock += UnlockController_SO_OnNewUnlock;
            mainUI = GameUIController.Instance;
        }

        private void OnDestroy()
        {
            UnlockController_SO.OnNewUnlock -= UnlockController_SO_OnNewUnlock;
        }

        private void UnlockController_SO_OnNewUnlock(Item arg0)
        {
            var ui = Instantiate(prefab, mainUI.transform);

            ui.SetUp(arg0);
        }
    }
}
using Assets.Scripts.Game.GameMap;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression.Actions
{
    

    [CreateAssetMenu(menuName = "Progression/Load Shop")]
    public class Progression_LoadShop : ProgressionAction
    {
        GameProgressionController controller;
        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            controller = progression;
            progression.transitionFadeInEnd.AddListener(OnTransitionMiddle);

        }

        private void OnTransitionMiddle(GameProgressionController progression)
        {
            progression.ShopRoomController.CreateShopWindow();

            progression.ShopRoomController.OnClose.AddListener(OnShopClosed);

            progression.transitionFadeInEnd.RemoveListener(OnTransitionMiddle);
        }

        private void OnShopClosed()
        {
            controller.ShopRoomController.OnClose.RemoveListener(OnShopClosed);
            controller.LoadNextStep();
        }

        public override string GetTransitionName(GameProgressionController progression)
        {
            return "Shop";
        }
    }

}
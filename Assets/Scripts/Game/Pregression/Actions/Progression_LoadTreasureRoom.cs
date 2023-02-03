using Assets.Scripts.Game.GameMap;
using Assets.Scripts.LevelGeneration;
using UnityEditor;
using UnityEngine;



namespace Assets.Scripts.Game.Pregression.Actions
{
    [CreateAssetMenu(menuName = "Progression/Load Treasure Room")]
    public class Progression_LoadTreasureRoom : ProgressionAction
    {
        GameProgressionController controller;


        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            controller = progression;
            progression.transitionFadeInEnd.AddListener(OnTransitionMiddle);

        }

        private void OnTransitionMiddle(GameProgressionController progression)
        {
            progression.TreasureRoomController.CreateWindow();

            progression.TreasureRoomController.ItemSelected.AddListener(OnTreasureClosed);

            progression.transitionFadeInEnd.RemoveListener(OnTransitionMiddle);
        }

        private void OnTreasureClosed()
        {
            controller.TreasureRoomController.CloseWindow();
            controller.TreasureRoomController.ItemSelected.RemoveListener(OnTreasureClosed);
            controller.LoadNextStep();
        }
        

        public override string GetTransitionName(GameProgressionController progression)
        {
            return "Treasure room";
        }
    }
}
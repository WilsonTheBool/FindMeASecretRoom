using Assets.Scripts.Game.GameMap;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.UI.GameOverScreen
{
    public class GameOverHandler : MonoBehaviour
    {

        private StatisticsController StatisticsController;

        private GameUIController GameUIController;

        private MainGameLevelMapController MainGameLevelMapController;

        private void Start()
        {
            StatisticsController = StatisticsController.Instance;
            GameUIController = GameUIController.Instance;
            MainGameLevelMapController = MainGameLevelMapController.Instance;
            MainGameLevelMapController.onDefeat.AddListener(OpenGameOverWindow);
        }

        private void OpenGameOverWindow()
        {
            GameUIController.GameOverScreenController.SetUp(StatisticsController.ItemsCollected.ToArray(), StatisticsController.roomsUnlocked.ToArray(),
                StatisticsController.levelsCompleted, StatisticsController.maxLevelCount);
        }
    }
}
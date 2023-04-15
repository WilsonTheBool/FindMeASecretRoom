using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Gameplay.BossRush
{
    public class BossRush_Counter : MonoBehaviour
    {
        public TMPro.TMP_Text text;

        BossRushController bossRushController;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void SetUp(BossRushController bossRushController)
        {
            this.bossRushController = bossRushController;
            bossRushController.anyLevelFinished.AddListener(UpdateText);
            this.gameObject.SetActive(true);
            UpdateText();
        }

        void UpdateText()
        {
            text.text = bossRushController.completedLevels + "/" + bossRushController.maxLevels;
        }
    }
}
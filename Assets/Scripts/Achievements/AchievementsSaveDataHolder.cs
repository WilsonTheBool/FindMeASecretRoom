using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SaveLoad;
using Assets.Scripts.Challenges;
using Assets.Scripts.Unlocks;

namespace Assets.Scripts.Achievements
{
    public class AchievementsSaveDataHolder : SaveLoadComponent<AchievementSaveData>
    {
        private const string SaveDataName = "Achievements";
        private const int order = 1;

        private SaveLoadController controller;

        public AchievementsController AchivementsController;

        private void OnNewAchievementUnlock(Achievment achievment)
        {
            UpdateUnlockedAchievements(AchivementsController.unlockedAchivements);
            Save_SaveData(controller);
        }

        public void UpdateUnlockedAchievements(List<Achievment> unlocked)
        {
            int[] ids = new int[unlocked.Count];

            for (int i = 0; i < unlocked.Count; i++)
            {
                ids[i] = unlocked[i].id;
            }

            SaveData.achievementsUnlocked = ids;
        }

        public void ClearAllData()
        {
            SaveData.achievementsUnlocked = new int[0];
            Save_SaveData(controller);
        }

        public override void OnAwake(SaveLoadController controller)
        { 
            AchivementsController.LoadAchivements(SaveData);

            AchivementsController.OnNewAchievmentUnlocked.AddListener(OnNewAchievementUnlock);

            base.OnAwake(controller);
        }

        public override void OnStart(SaveLoadController controller)
        {

            AchivementsController.AwakeUnlockedAchievements();

            base.OnStart(controller);
        }

        public override void SetUp(SaveLoadController controller)
        {
            this.controller = controller;
            this.awakeOrder = order;
            this.saveName = SaveDataName;
        }
    }

    [System.Serializable]
    public class AchievementSaveData:SaveData
    {
        public int[] achievementsUnlocked = new int[0];
    }
}
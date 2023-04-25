using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.SaveLoad;
using Assets.Scripts.Challenges;
using Assets.Scripts.Unlocks;

namespace Assets.Scripts.Achievements
{
    public class AchievementsSaveDataHolder : MonoBehaviour
    {
        private const string SaveDataName = "Achievements";
        public AchievementSaveData SaveData;

        private SaveLoadController controller;

        public AchievementsController AchivementsController;

        private void LoadSaveData()
        {
            SaveData = controller.LoadObject<AchievementSaveData>(SaveDataName);

            SaveData ??= new AchievementSaveData();
        }

        public void WriteSaveData()
        {
            controller.SaveObject<AchievementSaveData>(SaveData, SaveDataName);
        }

        private void OnNewAchievementUnlock(Achievment achievment)
        {
            UpdateUnlockedAchievements(AchivementsController.unlockedAchivements);
            WriteSaveData();
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
            WriteSaveData();
        }

        public void Awake()
        {
            controller = SaveLoadController.Instance;

            if (controller == null)
            {
                controller = GetComponentInParent<SaveLoadController>();
            }

            LoadSaveData();

            AchivementsController.LoadAchivements(SaveData);

            AchivementsController.OnNewAchievmentUnlocked.AddListener(OnNewAchievementUnlock);
        }

        private void Start()
        {
            AchivementsController.AwakeUnlockedAchievements();
        }

    }

    [System.Serializable]
    public class AchievementSaveData
    {
        public int[] achievementsUnlocked = new int[0];
    }
}
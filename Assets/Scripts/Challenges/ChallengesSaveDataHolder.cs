using Assets.Scripts.SaveLoad;
using Assets.Scripts.Unlocks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Challenges
{
    public class ChallengesSaveDataHolder : MonoBehaviour
    {

        private const string SaveDataName = "Challenges";
        public ChallengesSaveData SaveData;

        private SaveLoadController controller;

        public ChallengeRunController ChallengeRunController;
        public UnlockControllerData_SO UnlockController;

        private void LoadSaveData()
        {
            SaveData = controller.LoadObject<ChallengesSaveData>(SaveDataName);

            SaveData ??= new ChallengesSaveData();
        }

        public void WriteSaveData()
        {

            controller.SaveObject<ChallengesSaveData>(SaveData, SaveDataName);

        }

        public void UpdateCompleteedChalanges(ChallengeRunData[] completed)
        {
            int[] ids = new int[completed.Length];

            for (int i = 0; i < completed.Length; i++)
            {
                ids[i] = completed[i].id;
            }

            SaveData.challengesCompleted = ids;
        }

        public void ClearAllData()
        {
            SaveData.challengesCompleted = new int[0];
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

            ChallengeRunController.LoadCompletedChallenges(SaveData);
            UnlockController.SetUnlockedItems(SaveData);
        }

    }

    [System.Serializable]
    public class ChallengesSaveData
    {
        public int[] challengesCompleted = new int[0];
    }
}
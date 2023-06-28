using Assets.Scripts.SaveLoad;
using Assets.Scripts.Unlocks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Challenges
{
    public class ChallengesSaveDataHolder : SaveLoadComponent<ChallengesSaveData>
    {

        private const string SaveDataName = "Challenges";
        private const int order = 2;

        private SaveLoadController controller;

        public ChallengeRunController ChallengeRunController;
        public UnlockControllerData_SO UnlockController;

        public void UpdateUnlocks(ChallengeRunData[] unlocked)
        {
            int[] ids = new int[unlocked.Length];

            for (int i = 0; i < unlocked.Length; i++)
            {
                ids[i] = unlocked[i].id;
            }

            SaveData.challengesUnlocked = ids;
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
            SaveData.challengesUnlocked = new int[0];
            Save_SaveData(controller);
        }

        public override void OnAwake(SaveLoadController controller)
        {
            ChallengeRunController.LoadCompletedChallenges(this);

            base.OnAwake(controller);
        }

        public override void OnStart(SaveLoadController controller)
        {
            UnlockController.AddUnlockedItems(ChallengeRunController.GetChallengesUnlockItems(SaveData.challengesCompleted), false);

            base.OnAfterAwake(controller);
        }


        public override void SetUp(SaveLoadController controller)
        {
            this.controller = controller;
            this.awakeOrder = order;
            saveName = SaveDataName;
        }


    }

    [System.Serializable]
    public class ChallengesSaveData:SaveData
    {
        public int[] challengesCompleted = new int[0];
        public int[] challengesUnlocked = new int[0];
    }
}
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.SaveLoad;
using Assets.Scripts.Unlocks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Challenges
{
    [CreateAssetMenu(menuName = "Challenges/Challenge Controller")]
    public class ChallengeRunController : ScriptableObject
    {
        public UnlockControllerData_SO unlockController;

        public ChallengeRunData CurentChallenge;

        public ChallengeRunData[] allChalanges;

        public CompainLevelsData_SO default_compain;

        public List<ChallengeRunData> completedChallenges;

        public List<ChallengeRunData> unlockedChallenges;

        [HideInInspector]
        public ChallengesSaveDataHolder ChallengesSaveDataHolder;

        public UnityEvent<ChallengeRunData> ChallengeStarted;
        public UnityEvent<ChallengeRunData> ChallengeVictory;
        public UnityEvent<ChallengeRunData> ChallengeFail;

        public void StartChallenge(GameProgressionController progression)
        {
            ChallengesSaveDataHolder = SaveLoadController.Instance.challengesSaveData;
            progression.OnRunCompleted.AddListener(OnChallengeVictory);
            progression.OnRunFailed.AddListener(OnChallengeFail);
            ChallengeStarted.Invoke(CurentChallenge);

            if(CurentChallenge != null)
            {
                CurentChallenge.OnSetUp(progression, MainGameLevelMapController.Instance);
                
            }


        }

        public void OnChallengeFail(GameProgressionController progression)
        {
            progression.OnRunCompleted.RemoveListener(OnChallengeVictory);
            progression.OnRunFailed.RemoveListener(OnChallengeFail);

            if(CurentChallenge != null)
            {
                ChallengeFail.Invoke(CurentChallenge);
                CurentChallenge = null;
            }
            

            
        }

        public bool IsChallangeUnlocked(ChallengeRunData data)
        {
            if (data == null)
            {
                return false;
            }

            return unlockedChallenges.Contains(data);
        }

        public void UnlockChallenge(ChallengeRunData challenge)
        {
            if (unlockedChallenges.Contains(challenge))
            {
                return;
            }
            else
            {
                unlockedChallenges.Add(challenge);
                ChallengesSaveDataHolder.UpdateUnlocks(unlockedChallenges.ToArray());
            }
        }

        public void OnChallengeVictory(GameProgressionController progression)
        {
            progression.OnRunCompleted.RemoveListener(OnChallengeVictory);
            progression.OnRunFailed.RemoveListener(OnChallengeFail);

            if (CurentChallenge != null && !completedChallenges.Contains(CurentChallenge))
            {
                completedChallenges.Add(CurentChallenge);

                ChallengesSaveDataHolder.UpdateCompleteedChalanges(completedChallenges.ToArray());

                if (CurentChallenge.hasUnlockReward)
                {
                    new Achievements.Actions.Action_UnlockItem() { itemToUnlock = CurentChallenge.itemUnlock }.DoAction(
                        new Achievements.AchievmentAction.AchivementArgs(Assets.Scripts.Game.PlayerController.Player.instance,
                        MainGameLevelMapController.Instance, progression, Assets.Scripts.Game.UI.GameUIController.Instance, this));
                }
            }
            ChallengeVictory.Invoke(CurentChallenge);

            ChallengesSaveDataHolder.WriteSaveData();

            CurentChallenge = null;

            
        }

        public ChallengeRunData[] GetChallenges(int[] ids)
        {
            ChallengeRunData[] result = new ChallengeRunData[ids.Length];

            for(int i = 0; i < ids.Length; i++)
            {
                result[i] = GetChallenge(ids[i]);
            }

            return result;
        }

        public Item[] GetChallengesUnlockItems(int[] ids)
        {
            List<Item> result = new List<Item>();

            foreach(int id in ids)
            {
                ChallengeRunData run = GetChallenge(id);

                if(run != null && run.hasUnlockReward)
                {
                    result.Add(run.itemUnlock);
                }
            }

            return result.ToArray();
        }

        public ChallengeRunData GetChallenge(int id)
        {
            foreach(ChallengeRunData data in allChalanges)
            {
                if(data.id == id)
                {
                    return data;
                }
            }

            return null;
        }

        public void LoadCompletedChallenges(ChallengesSaveData saveData)
        {
            ChallengeRunData[] result = new ChallengeRunData[saveData.challengesCompleted.Length];

            unlockedChallenges = new List<ChallengeRunData>(saveData.challengesUnlocked.Length);

            for(int i = 0; i < result.Length; i++)
            {
                result[i] = GetChallenge(saveData.challengesCompleted[i]);
                
            }

            for(int i = 0; i < saveData.challengesUnlocked.Length; i++)
            {
                unlockedChallenges.Add(GetChallenge(saveData.challengesUnlocked[i]));
            }
            completedChallenges = new List<ChallengeRunData>(result);
        }

        public bool IsChallengeComplete(ChallengeRunData data)
        {
            if(data == null)
            {
                return false;
            }

            return completedChallenges.Contains(data);
        }

        public void OnChallengeRunStart(ChallengeRunData data)
        {
            CurentChallenge = data;
        }
    }
}
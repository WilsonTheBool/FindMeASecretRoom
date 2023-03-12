using Assets.Scripts.Game.GameMap;
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

        public List<ChallengeRunData> completedChallenges;

        [HideInInspector]
        public ChallengesSaveDataHolder ChallengesSaveDataHolder;

        public UnityEvent<ChallengeRunData> ChallengeStarted;
        public UnityEvent<ChallengeRunData> ChallengeVictory;
        public UnityEvent<ChallengeRunData> ChallengeFail;

        public void StartChallenge(ChallengeRunData data, GameProgressionController progression)
        {
            ChallengesSaveDataHolder = SaveLoadController.Instance.challengesSaveData;
            CurentChallenge = data;
            progression.OnRunCompleted.AddListener(OnChallengeVictory);
            progression.OnRunFailed.AddListener(OnChallengeFail);
            ChallengeStarted.Invoke(data);
            Debug.Log("Chellange: " + CurentChallenge.id + " Started");

        }

        public void OnChallengeFail(GameProgressionController progression)
        {
            progression.OnRunCompleted.RemoveListener(OnChallengeVictory);
            progression.OnRunFailed.RemoveListener(OnChallengeFail);

            ChallengeFail.Invoke(CurentChallenge);
            Debug.Log("Chellange: " + CurentChallenge.id + " Fail");
            CurentChallenge = null;

            
        }

        public void OnChallengeVictory(GameProgressionController progression)
        {
            progression.OnRunCompleted.RemoveListener(OnChallengeVictory);
            progression.OnRunFailed.RemoveListener(OnChallengeFail);

            completedChallenges.Add(CurentChallenge);

            ChallengeVictory.Invoke(CurentChallenge);

            ChallengesSaveDataHolder.UpdateCompleteedChalanges(completedChallenges.ToArray());

            ChallengesSaveDataHolder.WriteSaveData();
            Debug.Log("Chellange: " + CurentChallenge.id + " Victory");
            CurentChallenge = null;

            
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

            for(int i = 0; i < result.Length; i++)
            {
                result[i] = GetChallenge(saveData.challengesCompleted[i]);
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
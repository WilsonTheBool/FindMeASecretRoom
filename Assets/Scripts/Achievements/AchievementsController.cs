using Assets.Scripts.Challenges;
using Assets.Scripts.Game.PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Achievements
{
    [CreateAssetMenu(menuName = "Achievements/Achievement Controller")]
    public class AchievementsController : ScriptableObject
    {
        [SerializeField]
        Achievment[] all_achievements;

        public ChallengeRunController ChallengeRunController;

        public List<Achievment> activeAchivements;
        public List<Achievment> unlockedAchivements;

        public UnityEvent<Achievment> OnNewAchievmentUnlocked;

        public void LoadAchivements(AchievementSaveData saveData)
        {
            ChallengeRunController.ChallengeStarted.AddListener(RunStarted);
            ChallengeRunController.ChallengeVictory.AddListener(RunVictory);
            ChallengeRunController.ChallengeFail.AddListener(RunFail);

            activeAchivements = new List<Achievment>(all_achievements);
            unlockedAchivements = new List<Achievment>();

            foreach(int id in saveData.achievementsUnlocked)
            {
                Achievment achievement = GetAchievment(id);

                activeAchivements.Remove(achievement);
                unlockedAchivements.Add(achievement);
            }

            foreach(Achievment achievment in activeAchivements)
            {
                achievment.onAchivementUnlock.AddListener(OnUnlockAchivement);
            }
        }

        public void AwakeUnlockedAchievements()
        {
            foreach (Achievment achievment in unlockedAchivements)
            {
                achievment.OnAwakeWhenUnlocked(CreateAchivementArgs());
            }
        }

        public Achievment GetAchievment(int id)
        {
            foreach(Achievment achievement in all_achievements)
            {
                if(achievement.id == id)
                {
                    return achievement;
                }
            }

            return null;
        }

        public bool IsUnlocked(Achievment achievment)
        {
            return !activeAchivements.Contains(achievment);
        }

        public void OnUnlockAchivement(Achievment achievment)
        {
            if (activeAchivements.Contains(achievment))
            {
                activeAchivements.Remove(achievment);

                unlockedAchivements.Add(achievment);

                achievment.onAchivementUnlock.RemoveListener(OnUnlockAchivement);
                OnNewAchievmentUnlocked.Invoke(achievment);
            }

        }

        private void RunStarted(ChallengeRunData challenge)
        {
            if(challenge == null)
            {
                AchievmentAction.AchivementArgs args = CreateAchivementArgs();

                //Call OnRunStart on all active achivements;
                foreach (Achievment achievment in activeAchivements.ToArray())
                {
                    achievment.OnRunStarted(args);
                }
            }
           
        }

        private void RunVictory(ChallengeRunData challenge)
        {
            if (challenge == null)
            {
                AchievmentAction.AchivementArgs args = CreateAchivementArgs();

                foreach (Achievment achievment in activeAchivements.ToArray())
                {
                    achievment.OnRunVictory(args);
                }
            }
        }

        private void RunFail(ChallengeRunData challenge)
        {
            if (challenge == null)
            {
                AchievmentAction.AchivementArgs args = CreateAchivementArgs();

                foreach (Achievment achievment in activeAchivements.ToArray())
                {
                    achievment.OnRunDefeat(args);
                }
            }
        }

        private AchievmentAction.AchivementArgs CreateAchivementArgs()
        {
            return new AchievmentAction.AchivementArgs(Player.instance,
                Game.GameMap.MainGameLevelMapController.Instance, Game.Pregression.GameProgressionController.Instance,
                Game.UI.GameUIController.Instance, ChallengeRunController);
        }
    }
}
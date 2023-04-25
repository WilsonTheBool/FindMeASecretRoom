using Assets.Scripts.Achievements;
using Assets.Scripts.Challenges;
using Assets.Scripts.Game.Pregression;
using System;


namespace Assets.Scripts.MainMenu
{
    public class StartChallengeButton_Normal: StartChallengeButton
    {
        public CompainLevelsData_SO shortCompain;
        public CompainLevelsData_SO longCompain;

        public ChallengeRunController ChallengeRunController;

        public AchievementsController AchievementsController;

        public Achivement_OnShortCompain_Vicotry victoryAchivment;

        private void Start()
        {
            if (AchievementsController.IsUnlocked(victoryAchivment))
            {
                ChallengeRunController.default_compain = longCompain;
            }
            else
            {
                ChallengeRunController.default_compain = shortCompain;
            }
        }
    }
}

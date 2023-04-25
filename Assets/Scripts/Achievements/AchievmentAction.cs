using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Achievements
{
    public abstract class AchievmentAction
    {
        public abstract void DoAction(AchivementArgs args);

        public class AchivementArgs
        {
            public Player Player;

            public MainGameLevelMapController MainGame;

            public GameProgressionController progression;

            public GameUIController GameUIController;

            public Challenges.ChallengeRunController challenges;

            public AchivementArgs(Player player, MainGameLevelMapController mainGame, GameProgressionController progression, GameUIController gameUIController, Challenges.ChallengeRunController challenges)
            {
                Player = player;
                MainGame = mainGame;
                this.progression = progression;
                GameUIController = gameUIController;
                this.challenges = challenges;
            }
        }
    }
}
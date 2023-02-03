using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.SaveLoad;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.GameDebug.Cheats
{
    
    public abstract class Cheat : ScriptableObject
    {
        public abstract bool CanActivate(CheatActivationData data);

        public abstract void Activate(CheatActivationData data);

        public abstract void Deactivate(CheatActivationData data);


        public class CheatActivationData
        {
            public CheatsData cheatsData;

            public MainGameLevelMapController main;

            public Player player;

            public CheatActivationData(CheatsData cheatsData, MainGameLevelMapController main, Player player)
            {
                this.cheatsData = cheatsData;
                this.main = main;
                this.player = player;
            }
        }
    }
}
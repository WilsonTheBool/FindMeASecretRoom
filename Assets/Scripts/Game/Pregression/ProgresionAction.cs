using Assets.Scripts.Game.GameMap;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression
{
    public class ProgresionAction : ScriptableObject
    {
        public virtual void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
        }

        public virtual string GetTransitionName(GameProgressionController progression)
        {
            return "";
        }
    }
}
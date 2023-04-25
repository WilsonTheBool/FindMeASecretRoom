using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression
{
    [CreateAssetMenu(menuName = "Progression/Level to Transition Interpreter")]
    public class LevelToTransitionObject : ScriptableObject
    {
        public InterpreterData[] interpreterDatas;

        public TransitionRoom defaultRoom;

        public TransitionRoom bossRoom;

        public TransitionRoom GetTransitionRoom(ProgressionAction action)
        {
            foreach(InterpreterData interpreterData in interpreterDatas)
            {
                if(interpreterData.action == action)
                {
                    return interpreterData.TransitionRoom;
                }
            }

            return null;
        }

        [System.Serializable]
        public struct InterpreterData
        {
            public ProgressionAction action;

            public TransitionRoom TransitionRoom;
        }

    }
}
using Assets.Scripts.Game.GameMap;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.VictoryController
{
    public class GameVictoryController : MonoBehaviour
    {
        public static GameVictoryController instance;

        public static UnityEvent OnVictory;

        public VictoryBehaviour VictoryBehaviour;

        public void SetBehaviour(VictoryBehaviour victoryBehaviour)
        {
            if(this.VictoryBehaviour != null)
            {
                this.VictoryBehaviour.OnRemove();
            }

            this.VictoryBehaviour = victoryBehaviour;

            if (this.VictoryBehaviour != null)
            {
                this.VictoryBehaviour.OnAdd();
            }
        }

        public static void InvokeVictory()
        {
            //Debug.Log("VICOTRY INVOKE");

            OnVictory?.Invoke();
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                if (OnVictory == null)
                {
                    OnVictory = new UnityEvent();
                }


            }
            else
            {
                Destroy(this);
            }
           

            
        }

        public void Start()
        {
            if (VictoryBehaviour == null)
            {
                SetBehaviour(new Normal_VictoryBhaviour(MainGameLevelMapController.Instance.GameRoomCounter));
            }
        }

        private void OnDestroy()
        {
            if(instance == this)
            {
                OnVictory?.RemoveAllListeners();
                instance = null;
            }
        }
    }
}
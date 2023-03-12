using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items.ItemPools;
using Assets.Scripts.Game.Pregression.Actions;
using Assets.Scripts.Game.UI;
using Assets.Scripts.InputManager;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Pregression
{
    public class GameProgressionController : MonoBehaviour
    {
        public static GameProgressionController Instance { get; private set; }

        public InputListener TransitionScreenInput;

        private GameUIController GameUIController;
        private ScreenTransitionController ScreenTransitionController;
        public MainGameLevelMapController MainGameLevelMapController;

        public ItemPoolController ItemPoolController;
        public TreasureRoomController TreasureRoomController;
        public ShopRoomController ShopRoomController;

        public ProgressEvent transitionEnded;
        public ProgressEvent transitionFadeInEnd;

        public ProgressEvent OnRunCompleted;
        public ProgressEvent OnRunFailed;

        public ProgressionAction[] actions;

        public LevelData[] levels;

        public int actionCount = 0;
        private int actionsForTransition = 0;

        public int leveldataCount = 0;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            if(ItemPoolController == null)
            {
                ItemPoolController = GetComponentInChildren<ItemPoolController>();
            }
            if (TreasureRoomController == null)
            {
                TreasureRoomController = GetComponentInChildren<TreasureRoomController>();
            }
            if (ShopRoomController == null)
            {
                ShopRoomController = GetComponentInChildren<ShopRoomController>();
            }

            if(TransitionScreenInput == null)
            {
                var inputs = FindObjectsOfType<InputListener>(true);

                foreach(InputListener listener in inputs)
                {
                    if(listener.ListenerName == "Transition")
                    {
                        TransitionScreenInput = listener;
                        break;
                    }
                }
            }

            TransitionScreenInput.enabled = false;
            TransitionScreenInput.OnActivate.AddListener(OnActivate);
        }

        private void Start()
        {
            GameUIController = GameUIController.Instance;
            ScreenTransitionController = GameUIController.ScreenTransitionController;
            ScreenTransitionController.FadeOutEnded.AddListener(TransitionComplete);
            ScreenTransitionController.FadeInEnded.AddListener(() => transitionFadeInEnd.Invoke(this));

            MainGameLevelMapController = MainGameLevelMapController.Instance;
            MainGameLevelMapController.onVictory.AddListener(LoadNextStep);

            MainGameLevelMapController.onDefeat.AddListener(() => OnRunFailed.Invoke(this));

            LoadNextStep();
        }

        private void TransitionComplete()
        {
            transitionEnded.Invoke(this);
        }

        public void LoadNextStep()
        {
           if(actionCount < 0 || actionCount >= actions.Length)
           {
                return;
           }

            if(GetCurent().needTransition)
           StartTransitionAnimation(GetCurent().GetTransitionName(this));

           ActivateNextAction();

        }

        private void OnActivate()
        {
            if (ScreenTransitionController.isFadeInCompleted)
                ScreenTransitionController.CancelFadeOut();

            else
            {
                ScreenTransitionController.FadeInEnded.AddListener(CancelTransition_Event);
            }


        }

        private ProgressionAction GetCurent()
        {
            if (actionCount < 0 || actionCount >= actions.Length)
            {
                return null;
            }

            return actions[actionCount];
        }

        private void ActivateNextAction()
        {
            var action = GetCurent();

            if (action.needTransition)
            {
                actionsForTransition++;
            }

            ScreenTransitionController.FadeInEnded.RemoveListener(ActivateNextAction);
            actionCount++;

            action.DoAction(this, MainGameLevelMapController);
        }

        private void CancelTransition_Event()
        {
            ScreenTransitionController.CancelFadeOut();
            ScreenTransitionController.FadeInEnded.RemoveListener(CancelTransition_Event);
        }

        public void StartTransitionAnimation(string levelName)
        {
            TransitionScreenInput.enabled = true;
            ScreenTransitionController.StartFadeIn(actionsForTransition, levelName);
        }

        public LevelData GetCurentLevel()
        {
            if(leveldataCount >= levels.Length)
            {
                return null;
            }

            return levels[leveldataCount - 1];
        }

        public LevelData GetNextLevel()
        {
            leveldataCount++;
            return levels[leveldataCount - 1];
        }


        [System.Serializable]
        public class ProgressEvent: UnityEvent<GameProgressionController>
        {

        }
    }
}
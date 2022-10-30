﻿using Assets.Scripts.Game.GameMap;
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

        public ProgresionAction[] actions;

        public LevelData[] levels;

        public int actionCount = 0;

        public int leveldataCount = 0;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
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

            LoadNextStep();
        }

        private void TransitionComplete()
        {
            transitionEnded.Invoke(this);
        }

        public void LoadNextStep()
        {
            
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

        private ProgresionAction GetCurent()
        {
            return actions[actionCount];
        }

        private void ActivateNextAction()
        {
            GetCurent().DoAction(this, MainGameLevelMapController);
            ScreenTransitionController.FadeInEnded.RemoveListener(ActivateNextAction);
            actionCount++;
        }

        private void CancelTransition_Event()
        {
            ScreenTransitionController.CancelFadeOut();
            ScreenTransitionController.FadeInEnded.RemoveListener(CancelTransition_Event);
        }

        public void StartTransitionAnimation(string levelName)
        {
            TransitionScreenInput.enabled = true;
            ScreenTransitionController.StartFadeIn(actionCount, levelName);
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
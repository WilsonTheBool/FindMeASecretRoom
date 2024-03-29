﻿using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.VictoryController;
using Assets.Scripts.InputManager;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Game.GameMap
{
    public class MainGameLevelMapController : MonoBehaviour
    {
        public static MainGameLevelMapController Instance { get; private set; }

        public LevelMapRenderer LevelMapRenderer;

        public LevelGenerationController LevelGenerationController;

        public GameMapRoomUnlockController GameMapRoomUnlockController;

        public GameRoomCounter GameRoomCounter;

        public LevelMap LevelMap;

        public ExplosionController ExplosionController;

        [HideInInspector]
        public Player Player;

        public GameLevelInputManager GameLevelInputManager;

        public GameSelectTileController GameSelectTileController;

        public GameTilemapController GameTilemapController;


        [HideInInspector]
        public GameMapSizeController GameMapSizeController;

        [HideInInspector]
        public StatisticsController StatisticsController;

        public UnityEvent SetUpComplete;
        public UnityEvent levelStarted;

        [HideInInspector]
        public GameProgressionController progression;

        public UnityEvent onLevelOver;
        public UnityEvent onVictory;
        public UnityEvent onDefeat;

        //public Grid grid;
        public GridMap grid;

        public GameObject selectTile;

        public Camera cam;

        [HideInInspector]
        public bool isSetUpComplete;

        private void Awake()
        {
            isSetUpComplete = false;

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
                Instance = this;
            }

            if (LevelGenerationController == null)
            {
                LevelGenerationController = GetComponentInChildren<LevelGenerationController>();
            }

            if (GameMapRoomUnlockController == null)
            {
                GameMapRoomUnlockController = GetComponentInChildren<GameMapRoomUnlockController>();
            }

            if (GameRoomCounter == null)
            {
                GameRoomCounter = GetComponentInChildren<GameRoomCounter>();
            }

            if (ExplosionController == null)
            {
                ExplosionController = GetComponentInChildren<ExplosionController>();
            }

            if (GameLevelInputManager == null)
            {
                GameLevelInputManager = GetComponentInChildren<GameLevelInputManager>();
            }

            if (GameSelectTileController == null)
            {
                GameSelectTileController = GetComponentInChildren<GameSelectTileController>();
            }


            if (GameTilemapController == null)
            {
                GameTilemapController = GetComponentInChildren<GameTilemapController>();
            }

            if (GameMapSizeController == null)
            {
                GameMapSizeController = GetComponentInChildren<GameMapSizeController>();
            }

            cam = Camera.main;

            GameLevelInputManager.Listener.OnAccept.AddListener(SwitchActiveItem);
            GameLevelInputManager.Listener.OnActivate.AddListener(UseCurentItem);
            GameLevelInputManager.Listener.OnTrinketUse.AddListener(UseTrinket);
            GameLevelInputManager.Listener.OnAlternativeAction.AddListener(UseCurentItem_Alt);

        }

        public void SetUpLevel()
        {
            GameTilemapController.beckgroundTilemap.gameObject.SetActive(true);

            GameSelectTileController.selectTile.SetActive(true);

            LevelMap = LevelGenerationController.Generate();

            GameMapRoomUnlockController.SetUp(LevelMap);

            GameRoomCounter.SetUp(LevelMap);

            isSetUpComplete = true;
            SetUpComplete.Invoke();

            cam.transform.position = grid.GetCellCenter(GameMapSizeController.curentMapSize / 2) + new Vector3(0, 0, -10);
        }

        public void SetUpLevel(LevelMap level, bool invokeCompleteEveents)
        {
            GameTilemapController.beckgroundTilemap.gameObject.SetActive(true);

            GameSelectTileController.selectTile.SetActive(true);

            LevelMap = level;

            GameMapRoomUnlockController.SetUp(LevelMap);

            GameRoomCounter.SetUp(LevelMap);

            if (invokeCompleteEveents)
            {
                isSetUpComplete = true;
                SetUpComplete.Invoke();
            }

            cam.transform.position = grid.GetCellCenter(GameMapSizeController.curentMapSize / 2) + new Vector3(0, 0, -10);
        }

        public void SwitchActiveItem()
        {
            Player.itemsController.SwitchActiveItem(GetItemArgs());
        }

        private Items.Item.ItemExternalEventArgs GetItemArgs()
        {
            return new Items.Item.ItemExternalEventArgs()
            {
                mainGameController = this,
                player = Player,
                tilePos = GameLevelInputManager.CurentTileMousePosition
            };
        }

        private void UseTrinket()
        {
            Player.trinketController.UseTrinket(GetItemArgs());
        }

        private void UseCurentItem()
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            Player.itemsController.UseItem(GetItemArgs());
        }

        private void UseCurentItem_Alt()
        {
            Player.itemsController.UseItem_Alt(GetItemArgs());
        }

        private void Start()
        {
            StatisticsController = StatisticsController.Instance;

            Player = Player.instance;

            progression = GameProgressionController.Instance;

            Player.playerHPController.afterDeath.AddListener(OnPlayerDeath);

            GameVictoryController.OnVictory.AddListener(OnVictory);

            //SetUpLevel();
        }

        private void OnPlayerDeath(PlayerHPController.DeathEventArgs args)
        {
            //onLevelOver.Invoke();

            onDefeat.Invoke();

            
        }

        public void RestartLevel()
        {
            Player.playerHPController.HealToFullRedHP();

            LevelMapRenderer.ClearAll();

            SetUpLevel();

            LevelMapRenderer.StartRenderMap(LevelMap);
        }

        [SerializeField]
        GameObject victoryText;

        private void OnVictory()
        {
            StartCoroutine(VictoryCo());
        }

        IEnumerator VictoryCo()
        {
            
            if(victoryText != null)
                victoryText.gameObject.SetActive(true);

            if(victoryText != null)
                yield return new WaitForSeconds(2);
            else
                yield return new WaitForSeconds(1);

            if (victoryText != null)
                victoryText.gameObject.SetActive(false);

            GameTilemapController.beckgroundTilemap.gameObject.SetActive(false);
            GameSelectTileController.selectTile.SetActive(false);

            onLevelOver.Invoke();

            onVictory.Invoke();


        }

        public class LevelSetUpArgs
        {
            public bool isGenerateLevel;

            public LevelMap level;

            public LevelGeneration.LevelGeneratorParams LevelGeneratorParams;

            public bool isSetUpRoomCounter;

            public bool isSetUpRoomUnlocks;
        }
    }
}
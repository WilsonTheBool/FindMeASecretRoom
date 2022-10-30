using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.InputManager;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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

        public Player Player;

        public GameLevelInputManager GameLevelInputManager;

        public GameSelectTileController GameSelectTileController;

        public GameTilemapController GameTilemapController;

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

            cam = Camera.main;

            GameLevelInputManager.Listener.OnAccept.AddListener(SwitchActiveItem);
            GameLevelInputManager.Listener.OnActivate.AddListener(UseCurentItem);

            GameRoomCounter.onVictory.AddListener(OnVictory);
        }

        public void SetUpLevel()
        {
            GameTilemapController.beckgroundTilemap.gameObject.SetActive(true);

            GameSelectTileController.selectTile.SetActive(true);

            LevelMap = LevelGenerationController.Generate();

            GameMapRoomUnlockController.SetUp(LevelMap);

            GameRoomCounter.SetUp(LevelMap);

            ExplosionController.SetUp(LevelMap);

            isSetUpComplete = true;
            SetUpComplete.Invoke();

            cam.transform.position = grid.GetCellCenter(new Vector2Int(7, 7)) + new Vector3(0, 0, -10);
        }

        private void SwitchActiveItem()
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

        private void UseCurentItem()
        {
            Player.itemsController.UseItem(GetItemArgs());
        }

        private void Start()
        {
            Player = Player.instance;

            progression = GameProgressionController.Instance;

            Player.playerHPController.afterDeath.AddListener(OnPlayerDeath);

            //SetUpLevel();
        }

        private void OnPlayerDeath(PlayerHPController.DeathEventArgs args)
        {
            onLevelOver.Invoke();
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
            

            victoryText.gameObject.SetActive(true);

            yield return new WaitForSeconds(2);

            victoryText.gameObject.SetActive(false);

            onLevelOver.Invoke();

            onVictory.Invoke();

            GameTilemapController.beckgroundTilemap.gameObject.SetActive(false);
            GameSelectTileController.selectTile.SetActive(false);
        }
    }
}
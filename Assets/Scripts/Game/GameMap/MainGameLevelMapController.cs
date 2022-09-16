using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.PlayerController;
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

        public UnityEvent SetUpComplete;

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
                Destroy(this.gameObject);
            }

            cam = Camera.main;

            GameLevelInputManager.Listener.OnAccept.AddListener(SwitchActiveItem);
            GameLevelInputManager.Listener.OnActivate.AddListener(UseCurentItem);
        }

        private void SetUpLevel()
        {
            LevelMap = LevelGenerationController.Generate();

            GameMapRoomUnlockController.SetUp(LevelMap);

            LevelMapRenderer.StartRenderMap(LevelMap);

            GameRoomCounter.SetUp(LevelMap);

            ExplosionController.SetUp(LevelMap);


            isSetUpComplete = true;
            SetUpComplete.Invoke();
        }

        private void SwitchActiveItem()
        {
            Player.itemsController.SwitchActiveItem();
        }

        private void UseCurentItem()
        {
            Player.itemsController.UseItem(new Items.Item.ItemExternalEventArgs()
            {
                mainGameController = this,
                player = Player,
                tilePos = GameLevelInputManager.CurentTileMousePosition
            });
        }

        private void Start()
        {
            Player = Player.instance;

            Player.playerHPController.afterDeath.AddListener(OnPlayerDeath);

            SetUpLevel();
        }

        private void OnPlayerDeath(PlayerHPController.DeathEventArgs args)
        {
            Restart();
        }

        private void Restart()
        {
            Player.playerHPController.HealToFullRedHP();

            LevelMapRenderer.ClearAll();

            SetUpLevel();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Restart();
            }

            cam.transform.position = grid.GetCellCenter(new Vector2Int(7, 7)) + new Vector3(0, 0, -10);
        }
    }
}
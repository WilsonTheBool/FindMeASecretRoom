using Assets.Scripts.LevelGeneration;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Game.GameMap
{
    public class GameMapSizeController : MonoBehaviour
    {
        public static GameMapSizeController Instance { get; private set; }

        GameTilemapController tilemapController;
        LevelGenerationController LevelGenerationController;


        public Tile normalTile;
        public Tile redTile;

        [HideInInspector]
        public Vector2Int curentMapSize = new Vector2Int(13,13);

        public void UpdateToSize(Vector2Int size)
        {
            if(LevelGenerationController == null || tilemapController == null)
            {
                Start();
            }

            LevelGenerationController.isLargeMap = true;
            LevelGenerationController.LargeMapSize = size;
            LevelGenerationController.LargeMapStart = size / 2;

            UpdateTilemap(size);
        }
            

        private void UpdateTilemap(Vector2Int size)
        {
            curentMapSize = size;

            tilemapController.beckgroundTilemap.ClearAllTiles();
            GridMap grid = MainGameLevelMapController.Instance.grid;

            for(int x = -1; x <= size.x; x++)
            {
                for(int y = -1; y <= size.y; y++)
                {
                    if(x == -1 || y == -1 || x == size.x|| y == size.y)
                    {
                        tilemapController.beckgroundTilemap.SetTile(tilemapController.beckgroundTilemap.WorldToCell(grid.GetCellCenter(new Vector2Int(x, y))),redTile);
                    }
                    else
                    {
                        tilemapController.beckgroundTilemap.SetTile(tilemapController.beckgroundTilemap.WorldToCell(grid.GetCellCenter(new Vector2Int(x, y))), normalTile);
                    }
                }
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDisable()
        {
            if(Instance == this)
            {
                Instance = null;
            }
        }

        

        public void Start()
        {
            LevelGenerationController = GetComponent<LevelGenerationController>();
            tilemapController = GetComponent<GameTilemapController>();
        }
    }
}
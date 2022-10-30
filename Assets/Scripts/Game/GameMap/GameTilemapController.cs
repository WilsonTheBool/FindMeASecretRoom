using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Game.GameMap
{
    public class GameTilemapController : MonoBehaviour
    {
        [HideInInspector]
        public MainGameLevelMapController mainGameLevelMapController;

        public Tilemap beckgroundTilemap;

        public Tilemap checkedMarkerTilemap;

        public Tile markerTile;

        public List<Tilemap> tilemaps;

        public Grid grid;

        private void Awake()
        {
            if(grid == null)
            {
                grid = FindObjectOfType<Grid>();
            }
        }

        private void Start()
        {
            mainGameLevelMapController = MainGameLevelMapController.Instance;

            mainGameLevelMapController.onLevelOver.AddListener(ClearAll);
        }

        public void SetMarker(Vector2Int pos)
        {
            checkedMarkerTilemap.SetTile(new Vector3Int(pos.x,pos.y,0), markerTile);
        }

        public void RemoveMarker(Vector2Int pos)
        {
            checkedMarkerTilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
        }

        public bool IsMarked(Vector2Int pos)
        {
            return checkedMarkerTilemap.HasTile(new Vector3Int(pos.x, pos.y, 0));
        }

        public void ClearAll()
        {
            checkedMarkerTilemap.ClearAllTiles();

            while(tilemaps.Count > 0)
            {
                Tilemap tilemap = tilemaps[0];
                tilemaps.RemoveAt(0);
                Destroy(tilemap.gameObject);
            }
        }

        public Tilemap AddTilemap(Tilemap prefab, string name)
        {
            var map = Instantiate(prefab, grid.transform);
            map.name = name;
            tilemaps.Add(map);

            return map;
        }

        public void ShowTilemap(string name)
        {
            Tilemap tilemap = tilemaps.Find(x => x.name == name);

            if(tilemap!= null)
            {
                tilemap.gameObject.SetActive(true);
            }
        }

        public void HideTilemap(string name)
        {
            Tilemap tilemap = tilemaps.Find(x => x.name == name);

            if (tilemap != null)
            {
                tilemap.gameObject.SetActive(false);
            }
        }

        public Tilemap GetTilemap(string name)
        {
            return tilemaps.Find(x => x.name == name);
        }
    }
}
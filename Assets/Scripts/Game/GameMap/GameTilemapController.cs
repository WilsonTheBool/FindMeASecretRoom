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

        private List<Vector2Int> markedPos = new List<Vector2Int>();

        private Dictionary<Vector2Int, GameObject> flags = new Dictionary<Vector2Int, GameObject>();

        [SerializeField]
        private GameObject flag_prefab;


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
            markedPos.Add(pos);
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
            markedPos.Clear();
            DeleteFlags();
            flags.Clear();

            while (tilemaps.Count > 0)
            {
                Tilemap tilemap = tilemaps[0];
                tilemaps.RemoveAt(0);
                Destroy(tilemap.gameObject);
            }
        }

        private void DeleteFlags()
        {
            foreach (GameObject value in flags.Values)
            {
                Destroy(value);
            }

            flags.Clear();
        }

        public void ClearMarkers()
        {
            checkedMarkerTilemap.ClearAllTiles();
            markedPos.Clear();
            DeleteFlags();
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

        public void AddFlag(Vector2Int tilePos)
        {
            var flag = Instantiate(flag_prefab, mainGameLevelMapController.grid.GetCellCenter(tilePos), Quaternion.Euler(0, 0, 0), this.transform);

            flags.Add(tilePos, flag);
        }

        public bool CanPlaceFlag(Vector2Int tilePos)
        {
            return !flags.ContainsKey(tilePos) && mainGameLevelMapController.LevelMap.IsInRange(tilePos);
        }

        public void RemoveFlag(Vector2Int tilePos)
        {
            if (flags.TryGetValue(tilePos, out GameObject flag))
            {
                Destroy(flag);
                flags.Remove(tilePos);
            }
        }

        public void Save(ref SaveData saveData)
        {

            saveData.markedPos = new List<Vector2Int>(this.markedPos);

            saveData.flags = new List<Vector2Int>(flags.Keys);
        }

        public bool Load(ref SaveData data)
        {
            if (data == null || data.markedPos == null)
            {
                return false;
            }

            ClearMarkers();


            foreach(Vector2Int pos in data.markedPos)
            {
                SetMarker(pos);
            }

            foreach (Vector2Int pos in data.flags)
            {
                AddFlag(pos);
            }

            return true;
        }

        public class SaveData
        {
            public List<Vector2Int> markedPos;

            public List<Vector2Int> flags;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.LevelGeneration
{
    public class LevelGenerationController : MonoBehaviour
    {

        public Rebirth_LevelGenerator levelGenerator;

        public LevelGeneratorParams LevelGeneratorParams;

        public int curentRoomCount;

        public int roomQueueCount;

        public Grid grid;
        //public Room_GM roomPrefab;

        private void Start()
        {
            rooms = new List<Room_GM>();
            Generate();
        }

        private List<Room_GM> rooms;

        public void Generate()
        {
            LevelMap levelMap;

            foreach (Room_GM room in rooms)
            {
                Destroy(room.gameObject);
            }

            rooms.Clear();

            int loopCount = 0;

            do
            {
                loopCount++;
                levelMap = new LevelMap();
                Debug.Log("Generation End");

                if (loopCount > 30)
                {
                    Debug.Log("Main Loop fail");
                    return;
                }
            }
            while ((!levelGenerator.GenerateLevel(levelMap, LevelGeneratorParams)));



            foreach (Room room in levelMap.rooms)
            {
                Room_GM roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);

                Vector3Int pos = new Vector3Int(room.position.x, room.position.y, 0);
                Room_GM gm = Instantiate(roomPrefab, grid.GetCellCenterWorld(pos), Quaternion.Euler(0, 0, 0));
                rooms.Add(gm);
                if (room.type != null)
                    gm.SetIcon(room.type.icon);

            }

            Vector2Int max = new Vector2Int(-1, -1);
            Vector2Int min = new Vector2Int(100, 100);

            foreach (Room room in levelMap.rooms)
            {
                min = Vector2Int.Min(min, room.position);
                max = Vector2Int.Max(max, room.position);
            }

            Vector2Int middle = (min + (max - min) / 2);

            Vector3Int vector3Int = new Vector3Int(middle.x, middle.y, 0);

            Camera.main.transform.position = grid.CellToWorld(vector3Int) + new Vector3(0,0, -10f);
        }

        private void FixedUpdate()
        {
            curentRoomCount = levelGenerator.curentRoomCount;
            roomQueueCount = levelGenerator.roomQueue.Count;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Generate();
            }
        }
    }
}
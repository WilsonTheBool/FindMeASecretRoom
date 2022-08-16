using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Assets.Scripts.LevelGeneration
{
    public class LevelGenerationController : MonoBehaviour
    {

        public Rebirth_LevelGenerator levelGenerator;

        public LevelGeneratorParams LevelGeneratorParams;

        public bool hideSecretRooms;

        public int curentRoomCount;

        public int roomQueueCount;

        //public Grid grid;
        public GridMap grid;

        public GameObject selectTile;

        public Camera cam;

        public float waitTimeTileGeneration;
        //public Room_GM roomPrefab;

        private void Start()
        {
            rooms = new List<Room_GM>();
            cam = Camera.main;
            Generate();

            
        }

        private List<Room_GM> rooms;

        LevelMap levelMap;

        private SecretRoomGenerationRule.RuleCostMap costMap;

        public void Generate()
        {
            

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


            RoomType SecretRooms = LevelGeneratorParams.GetSecretRoomTypes(10, out int secretCount);
            costMap = (SecretRooms.rule as SecretRoomRules.NormalSecretRoom_Rule).costMap;


            Vector2Int max = new Vector2Int(-1, -1);
            Vector2Int min = new Vector2Int(100, 100);

            foreach (Room room in levelMap.rooms)
            {
                min = Vector2Int.Min(min, room.position);
                max = Vector2Int.Max(max, room.position);
            }

            Vector2Int middle = (min + (max - min) / 2);

           

            cam.transform.position = grid.CellToWorld(middle) + new Vector3(0,0, -10f);

            StartCoroutine(GenCorutine());
        }

        IEnumerator GenCorutine()
        {

            foreach (Room room in levelMap.rooms)
            {
                if (room.type != null && (hideSecretRooms && room.type.isSecretRoom))
                {
                    continue;
                }

                Room_GM roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);

                Vector2Int pos = new Vector2Int(room.position.x, room.position.y);
                Room_GM gm = Instantiate(roomPrefab, grid.GetCellCenter(pos), Quaternion.Euler(0, 0, 0));
                rooms.Add(gm);
                if (room.type != null)
                {
                    gm.SetIcon(room.type.icon);

                    if (room.type.overridesColor)
                    {
                        gm.SetColor_Base(room.type.colorOfBase);
                    }
                }
                    

                

                yield return new WaitForSeconds(waitTimeTileGeneration);
            }



        }

        private void FixedUpdate()
        {
            curentRoomCount = levelGenerator.curentRoomCount;
            roomQueueCount = levelGenerator.roomQueue.Count;
        }

        public TMPro.TMP_Text tileDataDebugText;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Generate();
            }

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos += new Vector3(0,0, -10);

            Vector2Int tilePos = grid.WorldToCell(mousePos);

            if (costMap != null)
            {
                tileDataDebugText.text = costMap.GetTileText(new Vector2Int(tilePos.x, tilePos.y));

                if(levelMap.GetRoom(new Vector2Int(tilePos.x, tilePos.y)) != null)
                tileDataDebugText.text += "\n"+levelMap.GetRoom(new Vector2Int(tilePos.x, tilePos.y )).Figure.name;
            }
            

            if (levelMap != null)
            {
                selectTile.transform.position = grid.GetCellCenter(tilePos);
            }
            else
            {
                selectTile.transform.position = new Vector3(-1000, -1000, -1000);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Room room = levelMap.GetRoom(new Vector2Int(tilePos.x, tilePos.y));

                if(room != null && room.type != null && room.type.isSecretRoom)
                {
                    Room_GM roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);

                    Vector2Int pos = new Vector2Int(room.position.x, room.position.y);
                    Room_GM gm = Instantiate(roomPrefab, grid.GetCellCenter(pos), Quaternion.Euler(0, 0, 0));
                    rooms.Add(gm);
                    if (room.type != null)
                        gm.SetIcon(room.type.icon);
                }
            }
        }

        public void SetHideSecretRooms(bool value)
        {
            hideSecretRooms = value;
        }
    }
}
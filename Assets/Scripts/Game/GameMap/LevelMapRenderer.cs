using System.Collections;
using UnityEngine;
using Assets.Scripts.LevelGeneration;
using UnityEditor;
using System.Collections.Generic;

namespace Assets.Scripts.Game.GameMap
{
    public class LevelMapRenderer : MonoBehaviour
    {
        LevelMap levelMap;

        public Rebirth_LevelGenerator levelGenerator;

        public LevelGeneratorParams LevelGeneratorParams;

        public bool hideSecretRooms;

        //public Grid grid;
        public GridMap grid;

        public GameObject selectTile;

        public Camera cam;

        public float waitTimeTileGeneration;

        public GameMapRoomUnlockController roomUnlockController;


        /// <summary>
        /// Rooms (Game Objects) rendered in game
        /// </summary>
        private List<Room_GM> rooms = new List<Room_GM>();

        private void Awake()
        {
            if(roomUnlockController == null)
            {
                roomUnlockController = GetComponent<GameMapRoomUnlockController>();
            }
        }

        public void StartRenderMap(LevelMap map)
        {
            levelMap = map;

            StartCoroutine(RenderCo());
        }

        public void TryRemoveRoom(Room room)
        {
            var found = rooms.FindAll((r) => r.position == room.position);

            foreach(Room_GM room_GM in found)
            {
                rooms.Remove(room_GM);
            }
            

        }

        
        public void RenderRoom_Unsafe(Room room)
        {
            Room_GM roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);
            Vector2Int pos = new Vector2Int(room.position.x, room.position.y);
            Room_GM gm = Instantiate(roomPrefab, grid.GetCellCenter(pos), Quaternion.Euler(0, 0, 0));
            gm.position = room.position;
            rooms.Add(gm);

            if (room.type != null)
            {
                gm.SetIcon(room.type.icon);

                if (room.type.overridesColor)
                {
                    gm.SetColor_Base(room.type.colorOfBase);
                }
            }
        }

        public void RenderRoom(Room room)
        {
            if (roomUnlockController.IsUnlocked(room))
            {
                Room_GM roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);
                Vector2Int pos = new Vector2Int(room.position.x, room.position.y);
                Room_GM gm = Instantiate(roomPrefab, grid.GetCellCenter(pos), Quaternion.Euler(0, 0, 0));
                gm.position = room.position;
                rooms.Add(gm);

                if (room.type != null)
                {
                    gm.SetIcon(room.type.icon);

                    if (room.type.overridesColor)
                    {
                        gm.SetColor_Base(room.type.colorOfBase);
                    }
                }
            }
        }

        public GameObject CreateObject(GameObject prefab, Vector2Int pos)
        {
            return Instantiate(prefab, grid.GetCellCenter(pos), Quaternion.Euler(0, 0, 0));
        }

        public GameObject CreateObject(GameObject prefab, Vector2Int pos, Vector3 size)
        {
            var gm = Instantiate(prefab, grid.GetCellCenter(pos), Quaternion.Euler(0, 0, 0));
            gm.transform.localScale = size;

            return gm;

        }

        public void RenderRoom(Vector2Int globalPos)
        {

            Room room = levelMap.GetRoom(globalPos);

            if (room != null)
                RenderRoom(room);


        }

        public void ClearAll()
        {
            foreach(Room_GM room in rooms)
            {
                Destroy(room.gameObject);
            }

            rooms.Clear();
        }

        IEnumerator RenderCo()
        {
            foreach (Room room in levelMap.rooms)
            {
                RenderRoom(room);

                yield return new WaitForSeconds(waitTimeTileGeneration);
            }
        }
    }
}
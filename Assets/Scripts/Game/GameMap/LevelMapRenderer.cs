using System.Collections;
using UnityEngine;
using Assets.Scripts.LevelGeneration;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.InputManager;
using UnityEngine.Events;
using Assets.Scripts.Game.Pregression;

namespace Assets.Scripts.Game.GameMap
{
    public class LevelMapRenderer : MonoBehaviour
    {
        LevelMap levelMap;

        public LevelGeneratorParams LevelGeneratorParams;

        public bool renderSecretRoomsOnStart;

        //public Grid grid;
        public GridMap grid;

        public GameObject selectTile;

        public Camera cam;

        public float waitTimeTileGeneration;

        public GameMapRoomUnlockController roomUnlockController;

        public InputListener InputListener;

        public Color baseRenderColor = Color.white;

        public Color StartRoomBaseColor = Color.green;

        public UnityEvent onRenderEnded;
        public UnityEvent onRenderStarted;

        public UnityEvent<Room_GM> onRoomRender;

        /// <summary>
        /// Rooms (Game Objects) rendered in game
        /// </summary>
        private List<Room_GM> rooms = new List<Room_GM>();
        

        private void Awake()
        {
            

            if (roomUnlockController == null)
            {
                roomUnlockController = GetComponentInChildren<GameMapRoomUnlockController>();
            }

           
        }

        private void Start()
        {
            MainGameLevelMapController.Instance.onLevelOver.AddListener(ClearAll);
            MainGameLevelMapController.Instance.onDefeat.AddListener(RenderNotUnlockedAllSecretRooms);
            InputListener.enabled = false;
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

       public void RenderNotUnlockedAllSecretRooms()
        {
            if(levelMap == null)
            {
                return;
            }

            foreach(Room room in levelMap.rooms)
            {
                if (room.type != null && room.type.isSecretRoom && !roomUnlockController.IsUnlocked(room.position))
                {
                    RenderRoom_Unsafe(room);
                }
            }
        }

        public void RenderRoom_Unsafe(Room room)
        {
            
            Room_GM roomPrefab;
            if (room.type != null && room.type.customRenderObject)
            {
                roomPrefab = room.type.renderObject;
            }
            else
            {
                roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);
            }

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
                else
                {
                    gm.SetColor_Base(baseRenderColor);
                }
            }

            onRoomRender.Invoke(gm);
        }

        public void RenderRoom(Room room)
        {
            if (renderSecretRoomsOnStart && room.type != null && room.type.isSecretRoom)
            {
                Room_GM roomPrefab;
                if (room.type != null && room.type.customRenderObject)
                {
                    roomPrefab = room.type.renderObject;
                }
                else
                {
                    roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);
                }
                Vector2Int pos = new Vector2Int(room.position.x, room.position.y);
                Room_GM gm = Instantiate(roomPrefab, grid.GetCellCenter(pos), Quaternion.Euler(0, 0, 0));
                gm.position = room.position;
                rooms.Add(gm);

                gm.SetIcon(room.type.icon);

                if (room.type.overridesColor)
                {
                    gm.SetColor_Base(room.type.colorOfBase);
                }
                else
                {
                    gm.SetColor_Base(baseRenderColor);
                }

                onRoomRender.Invoke(gm);
            }

            if (roomUnlockController.IsUnlocked(room))
            {
                Room_GM roomPrefab;
                if (room.type != null && room.type.customRenderObject)
                {
                    roomPrefab = room.type.renderObject;
                }
                else
                {
                    roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);
                }
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
                    else
                    {
                        gm.SetColor_Base(baseRenderColor);
                    }
                }
                else
                {
                    gm.SetColor_Base(baseRenderColor);
                }

                onRoomRender.Invoke(gm);
            }
        }

        public void RenderRoom(Room room, bool isStart)
        {
            if (!isStart)
            {
                RenderRoom(room);
                return;
            }

            if (roomUnlockController.IsUnlocked(room))
            {
                Room_GM roomPrefab = LevelGeneratorParams.RoomLayoutPicker.GetRoomObjectFromLayout(room.Figure, room.type);
                Vector2Int pos = new Vector2Int(room.position.x, room.position.y);
                Room_GM gm = Instantiate(roomPrefab, grid.GetCellCenter(pos), Quaternion.Euler(0, 0, 0));
                gm.position = room.position;
                rooms.Add(gm);

                gm.SetColor_Base(StartRoomBaseColor);

                onRoomRender.Invoke(gm);
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

        //public void RenderRoom(Vector2Int globalPos)
        //{

        //    Room room = levelMap.GetRoom(globalPos);

        //    if (room != null)
        //        RenderRoom(room);


        //}

        public void ClearAll()
        {
            foreach(Room_GM room in rooms)
            {
                Destroy(room.gameObject);
            }
            rooms.Clear();
        }

        public void RenderAll(LevelMap map)
        {
            ClearAll();

            onRenderStarted.Invoke();

            LevelData level = GameProgressionController.Instance.GetCurentLevel();

            List<Vector2Int> startRooms = new List<Vector2Int>(level.generatorParams.LevelGenerator.GetStartRooms());

            if (level != null)
            {
                baseRenderColor = level.baseColor;
            }


            foreach (Room room in map.rooms)
            {

                if (startRooms.Contains(room.position))
                {
                    RenderRoom(room, true);
                }
                else
                {
                    RenderRoom(room, false);
                }
            }


            //onRenderEnded.Invoke();

            InputListener.enabled = false;
        }

        public void RenderAll(LevelMap map, LevelData level)
        {
            ClearAll();

            onRenderStarted.Invoke();

            List<Vector2Int> startRooms = new List<Vector2Int>(level.generatorParams.LevelGenerator.GetStartRooms());

            if (level != null)
            {
                baseRenderColor = level.baseColor;
            }


            foreach (Room room in map.rooms)
            {

                if (startRooms.Contains(room.position))
                {
                    RenderRoom(room, true);
                }
                else
                {
                    RenderRoom(room, false);
                }
            }


            onRenderEnded.Invoke();

            InputListener.enabled = false;
        }

        IEnumerator RenderCo()
        {
            InputListener.enabled = true;

            onRenderStarted.Invoke();

            LevelData level = GameProgressionController.Instance.GetCurentLevel();

            List<Vector2Int> startRooms = new List<Vector2Int>(level.generatorParams.LevelGenerator.GetStartRooms());

            if(level != null)
            {
                baseRenderColor = level.baseColor;
            }
           

            foreach (Room room in levelMap.rooms)
            {

                if (startRooms.Contains(room.position))
                {
                    RenderRoom(room, true);
                }
                else
                {
                    RenderRoom(room,false);
                }

                yield return new WaitForSeconds(waitTimeTileGeneration);
            }
                

            onRenderEnded.Invoke();

            InputListener.enabled = false;
        }

        public bool TryGetRoom_GM(Vector2Int position, out Room_GM room)
        {   
            foreach(Room_GM room_GM in rooms)
            {
                if(room_GM.position == position)
                {
                    room = room_GM;
                    return true;
                }
            }
            room = null;
            return false;
        }
    }
}
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Game.ErrorRoom.ErrorRoomController;

namespace Assets.Scripts.Game.ErrorRoom
{
    public class ErrorRoomController : MonoBehaviour
    {
        public static ErrorRoomController Instance { get; private set; }

        private MainGameLevelMapController main;

        public ErrorRoom_GM gm_prefab;

        public GameObject error_shadow_prefab;
        public GameObject error_second_shadow_prefab;

        public ErrorRoom_UICounter uiCounter_prefab;

        public List<ErrorRoomData> errorRoomDatas;

        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void AddErrorRoom(Room errorRoom, Vector2Int[] roomPositions)
        {
            errorRoomDatas ??= new List<ErrorRoomData>();

            ErrorRoomData data = new ErrorRoomData(new ErrorRoomCycle(roomPositions, errorRoom), null, errorRoom, null);
            
            errorRoomDatas.Add(data);

        }

        private void Start()
        {
            main = MainGameLevelMapController.Instance;
            main.onLevelOver.AddListener(OnLevelOver);
            main.LevelMapRenderer.onRenderStarted.AddListener(OnRenderStart);
            main.LevelMapRenderer.onRoomRender.AddListener(OnRoomRender);
            PlayerActionController.ActionPerformed.AddListener(OnPlayerAction);

        }

        private void OnPlayerAction()
        {
            //Change secret room position
            foreach(ErrorRoomData data in errorRoomDatas)
            {
                if (data.justUnlocked)
                {
                    data.justUnlocked = false;
                }
                else
                {
                    MoveErrorRoom(data);
                }
            }


        }

        private void OnRoomRender(Room_GM room)
        {
            foreach(ErrorRoomData data in errorRoomDatas)
            {
                if(room.position == data.errorRoom.position)
                {
                    if(main.LevelMapRenderer.TryGetRoom_GM(room.position, out Room_GM room_gm))
                    {
                        data.gm = room_gm as ErrorRoom_GM;

                        data.justUnlocked = true;

                        Debug.Log("On Secret Room Render");
                    }
                }
            }
        }

        private void MoveErrorRoom(ErrorRoomData errorRoomData)
        {
            Vector2Int newPos = errorRoomData.errorRoomCycle.GetNextPos(); //Get next from cycle and push;

            //Remove marker if present
            if (main.GameTilemapController.IsMarked(newPos))
            {
                main.GameTilemapController.RemoveMarker(newPos);
            }

            if (main.GameMapRoomUnlockController.IsUnlocked(errorRoomData.errorRoom))
            {
                main.GameMapRoomUnlockController.LockPosition_Unsafe(errorRoomData.errorRoom.position);
                main.GameMapRoomUnlockController.UnlockPosition_Unsafe(newPos);
            }

            //move Room (in level map)
            main.LevelMap.TryMoveRoom(errorRoomData.errorRoom, newPos);

            //move shadow (or create if not exist)
            if (errorRoomData.shadow != null)
            {
                errorRoomData.shadow.transform.position = main.grid.GetCellCenter(errorRoomData.errorRoomCycle.PeekPosition(-1));

                if(errorRoomData.second_shadow != null)
                {
                    errorRoomData.second_shadow.transform.position = main.grid.GetCellCenter(errorRoomData.errorRoomCycle.PeekPosition(-2));
                }
                else
                {
                    errorRoomData.second_shadow = Instantiate(error_second_shadow_prefab, main.grid.GetCellCenter(errorRoomData.errorRoomCycle.PeekPosition(-2)),
                    Quaternion.Euler(0, 0, 0), this.transform);
                }
            }
            else
            {
                errorRoomData.shadow = Instantiate(error_shadow_prefab, main.grid.GetCellCenter(errorRoomData.errorRoomCycle.PeekPosition(-1)),
                    Quaternion.Euler(0, 0, 0), this.transform);
            }

            //move room_gm if rendered (if exists)
            if(errorRoomData.gm != null)
            {
                errorRoomData.gm.transform.position = main.grid.GetCellCenter(errorRoomData.errorRoomCycle.PeekPosition(0));
            }
        }
        

        private void OnLevelOver()
        {
            Destroy(this.gameObject);
        }

        private void OnRenderStart()
        {
            foreach(ErrorRoomData data in errorRoomDatas)
            {
                Vector2Int newPos = data.errorRoomCycle.PeekPosition(0);
                main.LevelMap.TryMoveRoom(data.errorRoom, newPos);
            }
        }

        public void OnDestroy()
        {
            if(Instance = this)
            {
                Instance = null;
            }
        }

        public class ErrorRoomData
        {
            public int error_room_cycle_index;

            public ErrorRoomCycle errorRoomCycle;

            public ErrorRoom_GM gm;

            public Room errorRoom;

            public GameObject shadow;

            public GameObject second_shadow;

            public bool justUnlocked = false;

            public ErrorRoomData(ErrorRoomCycle errorRoomCycle, ErrorRoom_GM gm, Room errorRoom, GameObject shadow)
            {
                this.errorRoomCycle = errorRoomCycle;
                this.gm = gm;
                this.errorRoom = errorRoom;
                this.shadow = shadow;
            }
        }


        public class ErrorRoomCycle
        {
            public Vector2Int[] cycle;

            public Room errorRoom;

            private int curentIndex;

            /// <summary>
            /// Returns cycle next position and pushes the cycle
            /// </summary>
            /// <returns></returns>
            public Vector2Int GetNextPos()
            {
                curentIndex++;

                if(curentIndex >= cycle.Length)
                {
                    curentIndex = 0;
                }

                return cycle[curentIndex];
            }

            /// <summary>
            /// Returns position in a cycle with an offset  
            /// (negative offset = previous positions, positive offset = next positions)
            /// </summary>
            /// <param name="offset"></param>
            /// <returns></returns>
            public Vector2Int PeekPosition(int offset)
            {
                int index = curentIndex + offset;

                bool fl = false;

                while (!fl)
                {
                    if(index >= cycle.Length)
                    {
                        index -= cycle.Length;
                        
                    }
                    else
                    {
                        if (index < 0)
                        {
                            index = cycle.Length + index;
                            
                        }
                        else
                        {
                            fl = true;
                        }
                    }
                    
                }

                return cycle[index];
            }

            public ErrorRoomCycle(Vector2Int[] positions, Room errorRoom)
            {
                //Shuffle positions
                cycle = Shuffle(positions);

                this.errorRoom = errorRoom;

                this.curentIndex = 0;
            }

            private Vector2Int[] Shuffle(Vector2Int[] array)
            {
                Vector2Int[] resultArray = new Vector2Int[array.Length];
                array.CopyTo(resultArray, 0);

                for(int i = array.Length - 1; i >= 0; i--)
                {
                    int rand = Random.Range(0, i);

                    Vector2Int temp = resultArray[i];
                    resultArray[i] = resultArray[rand];
                    resultArray[rand] = temp;
                }

                return resultArray;
            }
        }
    }
}
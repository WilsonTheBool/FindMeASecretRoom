using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Assets.Scripts.LevelGeneration
{
    /// <summary>
    /// Room figure class containing room geometry and all exits;
    /// Ex of 1-tile room w\ 4 exits: RoomTiles = [(0,0)], RoomExits = [(-1,0),(1,0),(0,1),(0,-1)]
    /// </summary>
    [CreateAssetMenu(menuName = "LevelGeneration/RoomFigure")]
    public class Room_Figure : ScriptableObject
    {
        public Vector2Int[] RoomTiles;

        public Vector2Int[] RoomExits;

        public Vector2Int[] BlockedExits;

        public ExitDistanceData[] exitDistances;

        public bool isLarge;

        public bool IsRoomContainsPos(Vector2Int localPos)
        {
            foreach(Vector2Int tile in RoomTiles)
            {
                if(tile == localPos)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsExitsContainsPos(Vector2Int localPos)
        {
            foreach (Vector2Int tile in RoomExits)
            {
                if (tile == localPos)
                {
                    return true;
                }
            }

            return false;
        }

        public Vector2Int[] GetRoomExitsOfTile(Vector2Int local)
        {
            List<Vector2Int> roomExits = new List<Vector2Int>();

            foreach(Vector2Int exit in RoomExits)
            {
                if(Vector2Int.Distance(exit,local) == 1)
                {
                    roomExits.Add(exit);
                }
            }

            return roomExits.ToArray();
        }

        public int GetDistance_FromLocal(Vector2Int exit1_local, Vector2Int exit2_local)
        {
            if (isLarge)
            {
                foreach(ExitDistanceData exitDistanceData in exitDistances)
                {
                    if((exitDistanceData.exit1 == exit1_local && exitDistanceData.exit2 == exit2_local) || 
                        (exitDistanceData.exit1 == exit2_local && exitDistanceData.exit2 == exit1_local))
                    {
                        return exitDistanceData.distance;
                    }
                }

                return -1;
            }
            return 1;
        }

        public Vector2Int[] GetRoomExitsOfTileRandomized(Vector2Int localPos)
        {
            return LevelMath.ShuffleArrayCreateNew(in RoomExits);
        }

        public Vector2Int GetRandomRoomTile()
        {
            return RoomTiles[Random.Range(0,RoomTiles.Length)];
        }

        public bool CanConnect(Room_Figure owner, Vector2Int curentExit)
        {
            foreach (Vector2Int exit in RoomExits)
            {
                if(owner.IsRoomContainsPos(curentExit + exit))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanConnect(Room_Figure owner, Vector2Int ownerGlobalPos, Vector2Int roomGlobalPos)
        {
            Vector2Int ownerPos = roomGlobalPos - ownerGlobalPos;

            

            foreach (Vector2Int exit in RoomExits)
            {
                if (owner.IsRoomContainsPos(exit + ownerPos))
                {
                    
                    return true;
                }

            }

            return false;

        }

        [System.Serializable]
        public class ExitDistanceData
        {
            public Vector2Int exit1;
            public Vector2Int exit2;

            public int distance;
        }
    }
}
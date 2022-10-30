using Assets.Scripts.LevelGeneration;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Assets.Scripts.Game.GameMap
{
    public class GameMapRoomUnlockController : MonoBehaviour
    {
        /// <summary>
        /// map of tiles that is rendered already
        /// </summary>
        bool[,] isUnlockedMap;

        public bool unlockSecretRoomsOnStart;

        public RoomUnlockEvent roomUnlocked;

        public RoomUnlockEvent roomLocked;

        

        public void SetUp(LevelMap levelMap)
        {
            isUnlockedMap = new bool[levelMap.level.GetLength(0), levelMap.level.GetLength(1)];

            foreach (Room room in levelMap.rooms)
            {
                if(room.type == null || !room.type.isSecretRoom || (unlockSecretRoomsOnStart && room.type.isSecretRoom))
                {
                    foreach (Vector2Int tile in room.Figure.RoomTiles)
                    {
                        isUnlockedMap[room.position.x + tile.x, room.position.y + tile.y] = true;
                    }

                    roomUnlocked.Invoke(room);
                }
            }
        }

        public bool TryUnlockRoom(Room room)
        {
            if (IsUnlocked(room.position))
            {
                return false;
            }
            else
            {
                if(LevelMath.IsInRange(room.position, isUnlockedMap))
                {
                    foreach (Vector2Int tile in room.Figure.RoomTiles)
                    {
                        isUnlockedMap[room.position.x + tile.x, room.position.y + tile.y] = true;
                    }

                    roomUnlocked.Invoke(room);
                }
                return true;
            }
        }

        public bool TryLockRoom(Room room)
        {
            if (!IsUnlocked(room.position))
            {
                return false;
            }
            else
            {
                if (LevelMath.IsInRange(room.position, isUnlockedMap))
                {
                    foreach (Vector2Int tile in room.Figure.RoomTiles)
                    {
                        isUnlockedMap[room.position.x + tile.x, room.position.y + tile.y] = false;
                    }

                    roomLocked.Invoke(room);
                }
                return true;
            }
        }

        public bool IsInMapRange(Vector2Int position)
        {
            return LevelMath.IsInRange(position, isUnlockedMap);
        }

        public bool IsUnlocked(Vector2Int position)
        {
            return LevelMath.IsInRange(position, isUnlockedMap) && isUnlockedMap[position.x, position.y];
        }

        public bool IsUnlocked(Room room)
        {
            return LevelMath.IsInRange(room.position, isUnlockedMap) && isUnlockedMap[room.position.x, room.position.y];
        }

        public Vector2Int[] GetLockedRoomsPos(Vector2Int[] allPos)
        {
            List<Vector2Int> result = new List<Vector2Int>();

            foreach(Vector2Int pos in allPos)
            {
                if (IsInMapRange(pos) && !IsUnlocked(pos))
                {
                    result.Add(pos);
                }
            }

            return result.ToArray();
        }

        public bool CanCheckToUnlock(Vector2Int position)
        {
            if (!IsInMapRange(position))
            {
                return false;
            }

            Vector2Int neighbour = position + new Vector2Int(1, 0);
            if (IsUnlocked(neighbour))
            {
                return true;
            }

            neighbour = position + new Vector2Int(-1, 0);
            if (IsUnlocked(neighbour))
            {
                return true;
            }

            neighbour = position + new Vector2Int(0, 1);
            if (IsUnlocked(neighbour))
            {
                return true;
            }

            neighbour = position + new Vector2Int(0, -1);
            if (IsUnlocked(neighbour))
            {
                return true;
            }


            return false;
        }
    }

    [System.Serializable]
    public class RoomUnlockEvent: UnityEvent<Room>
    {

    }
}
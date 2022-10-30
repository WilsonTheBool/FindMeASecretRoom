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

        public Vector2Int[] GetRoomExitsOfTile(Vector2Int localPos)
        {
            return RoomExits;
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

            //Debug.Log("Type = "+ name + "Owner_Type = " + owner.name + "| RelativePos = " + ownerPos + "| Global: = " + ownerGlobalPos);

            foreach (Vector2Int exit in RoomExits)
            {
                if (owner.IsRoomContainsPos(exit + ownerPos))
                {
                    //Debug.Log("Trying Exit = " + exit + " Succses");
                    return true;
                }
                else
                {
                    //Debug.Log("Trying Exit = " + exit + " FAIL");
                }
            }

            return false;

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public class LevelMap
    {
        public int LevelX = 13;
        public int LevelY = 13;

        public int StartRoomX = 7;
        public int StartRoomY = 7;

        public Room[,] level;

        public Vector2Int[] startRooms;

        public List<Room> rooms;
        public List<Room> curentGenRooms;

        public Room GetNeighbour(Room origin, Vector2Int globalPos)
        {
            Room room = GetRoom(globalPos);

            if (room == origin)
            {
                return null;
            }

            return room;
        }

        public Room FindFirstNeighbour(Room room)
        {
            Vector2Int globalPos = room.position;

            if (!IsInRange(globalPos)) { return null; }

            Vector2Int neighbourPos;
            Room neighbour;
            neighbourPos = globalPos + new Vector2Int(1, 0);
            if (IsInRange(neighbourPos))
            {
                neighbour = GetNeighbour(room, neighbourPos);
                if (neighbour != null)
                {
                    return neighbour;
                }
            }

            neighbourPos = globalPos + new Vector2Int(-1, 0);
            if (IsInRange(neighbourPos))
            {
                neighbour = GetNeighbour(room, neighbourPos);
                if (neighbour != null)
                {
                    return neighbour;
                }
            }

            neighbourPos = globalPos + new Vector2Int(0, 1);
            if (IsInRange(neighbourPos))
            {
                neighbour = GetNeighbour(room, neighbourPos);
                if (neighbour != null)
                {
                    return neighbour;
                }
            }

            neighbourPos = globalPos + new Vector2Int(0, -1);
            if (IsInRange(neighbourPos))
            {
                neighbour = GetNeighbour(room, neighbourPos);
                if (neighbour != null)
                {
                    return neighbour;
                }
            }

            return null;
        }

        public bool CanFitRoom(Room room, Vector2Int pos)
        {
            if (room == null || room.Figure == null)
            {
                return false;
            }

            foreach (Vector2Int tile in room.Figure.RoomTiles)
            {
                Vector2Int p = pos + tile;

                if (IsInRange(p))
                {
                    if (GetRoom_Unsafe(p) != null)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            //Check if connected to blocked exits
            if (room.Figure.isLarge)
            {
                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int p = pos + exit;
                    Room neighbour = GetRoom(p);
                    if (neighbour != null)
                    {
                        foreach (Vector2Int blocked in neighbour.Figure.BlockedExits)
                        {
                            if (room.Figure.IsRoomContainsPos(blocked + exit))
                            {
                                return false;
                            }
                        }
                    }
                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int p = pos + exit;
                    Room neighbour = GetRoom(p);
                    if (neighbour != null)
                    {
                        foreach (Vector2Int blocked in neighbour.Figure.BlockedExits)
                        {
                            if (room.Figure.IsRoomContainsPos(blocked + exit))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }



        //public bool IsTileFreeToPlaceRoom(Room_Figure figure, Vector2Int pos)
        //{
        //    if(figure == null)
        //    {
        //        return false;
        //    }

        //    foreach (Vector2Int exit in figure.RoomExits)
        //    {
        //        Vector2Int p = pos + exit;


        //    }
        //}

        public int GetNeighbourCount(Room room, Vector2Int globalPos)
        {
            int count = 0;

            if (!IsInRange(globalPos)) { return count; }

            Vector2Int neighbourPos;

            neighbourPos = globalPos + new Vector2Int(1, 0);
            if (IsInRange(neighbourPos))
            {
                if (GetNeighbour(room, neighbourPos) != null)
                {
                    count++;
                }
            }

            neighbourPos = globalPos + new Vector2Int(-1, 0);
            if (IsInRange(neighbourPos))
            {
                if (GetNeighbour(room, neighbourPos) != null)
                {
                    count++;
                }
            }

            neighbourPos = globalPos + new Vector2Int(0, 1);
            if (IsInRange(neighbourPos))
            {
                if (GetNeighbour(room, neighbourPos) != null)
                {
                    count++;
                }
            }

            neighbourPos = globalPos + new Vector2Int(0, -1);
            if (IsInRange(neighbourPos))
            {
                if (GetNeighbour(room, neighbourPos) != null)
                {
                    count++;
                }
            }

            return count;
        }

        public bool PlaceRoom(Room room, Vector2Int pos, Vector2Int parentPos)
        {
            if (IsInRange(pos))
            {

                foreach (Vector2Int tile in room.Figure.RoomTiles)
                {
                    Vector2Int v = pos + tile;

                    level[v.x, v.y] = room;

                }


                room.position = pos;
                room.parent = parentPos;
                rooms.Add(room);
                curentGenRooms.Add(room);

                return true;
            }

            return false;
        }

        public Room GetRoom(Vector2Int pos)
        {
            if (IsInRange(pos))
            {
                return level[pos.x, pos.y];
            }

            return null;
        }

        private Room GetRoom_Unsafe(Vector2Int pos)
        {
            return level[pos.x, pos.y];
        }

        public bool IsInRange(Vector2Int pos)
        {
            return LevelMath.IsInRange(pos, LevelX, LevelY);
        }

        public bool TryMoveRoom(Room room, Vector2Int newPos)
        {

            if (room == null)
            {
                return false;
            }

            if (!IsInRange(newPos))
            {
                return false;
            }

            //check if canMove
            if (!CanFitRoom(room, newPos))
            {
                return false;
            }

            //clear old
            foreach (Vector2Int roomInsides in room.Figure.RoomTiles)
            {
                Vector2Int pos = roomInsides + room.position;

                if (IsInRange(pos))
                {
                    if (level[pos.x, pos.y] == room)
                        level[pos.x, pos.y] = null;
                }
            }

            //Set New
            foreach (Vector2Int tile in room.Figure.RoomTiles)
            {
                Vector2Int v = newPos + tile;

                level[v.x, v.y] = room;
            }


            room.position = newPos;

            return true;
        }

        public LevelMap()
        {
            level = new Room[LevelX, LevelY];
            rooms = new List<Room>();
            curentGenRooms = new List<Room>();
        }

        public LevelMap(int levelX, int levelY)
        {
            level = new Room[levelX, levelY];
            rooms = new List<Room>();
            curentGenRooms = new List<Room>();
        }
    }
}
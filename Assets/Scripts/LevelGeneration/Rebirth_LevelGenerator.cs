using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.LevelGeneration
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rebirth_Generator")]
    public class Rebirth_LevelGenerator : LevelGenerator
    {
        public Room_Figure starRoom;

        //public Room_Figure[] allRooms;

        public int curentRoomCount;
        private int maxRoomCount;

        public Queue<Room> roomQueue;
        List<Room> deadEndRooms;

        LevelGeneratorParams data;

        private float giveUpRandomlyChance = 0.5f;

        public bool CancelGenerationIfNotEnoughSpecialRoomSpace;

        public const int StartRoomX = 7;
        public const int StartRoomY = 7;

        Dictionary<Vector2Int, float> secretRoomMap = new Dictionary<Vector2Int, float>();

        public override bool GenerateLevel(LevelMap levelMap, LevelGeneratorParams data)
        {
            roomQueue = new Queue<Room>();
            deadEndRooms = new List<Room>();
            this.data = data;
            curentRoomCount = 1;
            maxRoomCount = data.maxRoomsCount;
            Room start = new Room(starRoom, 0);
            giveUpRandomlyChance = data.giveUpRandomlyChance;

            levelMap.PlaceRoom(start, new Vector2Int(StartRoomX, StartRoomY), new Vector2Int(-1,-1));
            roomQueue.Enqueue(start);


            int mainLoopCount = 0;
            int maxLoop = maxRoomCount * 3;
            while(curentRoomCount < maxRoomCount)
            {
                CheckAddRoom(roomQueue.Peek(), levelMap);
                mainLoopCount++;

                //if (mainLoopCount % 15 == 0)
                //{
                //    roomQueue.Enqueue(start);
                //}

                if (roomQueue.Count == 0)
                {
                    foreach (Room room in levelMap.rooms)
                    {
                        deadEndRooms.Remove(room);

                        roomQueue.Enqueue(room);
                    }
                }

                if (mainLoopCount > maxLoop)
                {
                    Debug.LogError("main generation loop stuck");
                    return false;
                }
            }

            while(roomQueue.Count > 0)
            {
                Room room = roomQueue.Dequeue();
                if(levelMap.GetNeighbourCount(room, room.position) == 1)
                {
                    if(!room.Figure.isLarge)
                    deadEndRooms.Add(room);
                }
            }

            if(deadEndRooms.Count < data.minDeadEndsCount)
            {
                //Debug.Log("deadEndRooms.Count < data.minDeadEndsCount ->" + deadEndRooms.Count.ToString());

                return false;
            }

            if (curentRoomCount < maxRoomCount)
            {
                return false;
            }

            if (!AddSpecialRooms(levelMap))
            {
                return false;
            }

            return true;
        }

        private void CheckAddRoom(Room room, LevelMap levelMap)
        {
            Vector2Int[] neighbours = room.Figure.GetRoomExitsOfTileRandomized(new Vector2Int(0, 0));

            //bool AddedNeighbour = false;

            bool isDeadEnd = true;

            int neigbourCountMain = levelMap.GetNeighbourCount(room, room.position);

            

            if (neigbourCountMain > 1)
            {
                //Debug.Log("Room -> " + room.position + " Count -> " + neigbourCountMain);
                isDeadEnd = false;
            }

            //Debug.Log("Checking tile " + room.position);

            foreach (Vector2Int neighbour in neighbours)
            {

                //if have enough rooms already;
                if(curentRoomCount >= maxRoomCount)
                {
                    break;
                }

                if (!levelMap.IsInRange(room.position + neighbour))
                {
                    continue;
                }

                Room nRoom = levelMap.GetNeighbour(room, room.position + neighbour);

                //if ocupied - give up;
                if(nRoom != null)
                {
                    continue;
                }

                //0.5 prob to give up
                if (Random.Range(0f, 1f) <= giveUpRandomlyChance)
                {
                    continue;
                }

                int neigbourCount = levelMap.GetNeighbourCount(nRoom, room.position + neighbour);

                //if has more than 1 filled heighbours
                if(neigbourCount > 1)
                {
                    continue;
                }


                //Add Neighbour to map
                Room newRoom = AddRandomRoom(room.position + neighbour, levelMap, room, neighbour);

                if(newRoom != null)
                {
                    roomQueue.Enqueue(newRoom);
                    //AddedNeighbour = true;
                    isDeadEnd = false;

                    if (newRoom.Figure.isLarge)
                    {
                        curentRoomCount+=2;
                    }
                    else
                    {
                        curentRoomCount++;
                    }
                    
                }
      
                

            }

            //Remove room from queue
            roomQueue.Dequeue();

            if (isDeadEnd && !room.Figure.isLarge)
            {
                //Debug.Log("Deadend in" + room.position.ToString());
                deadEndRooms.Add(room);
            }
        }

        private Room AddRandomRoom(Vector2Int globalPos, LevelMap levelMap, Room owner, Vector2Int direction)
        {

            Room room = new Room(data.RoomLayoutPicker.GetRandomLayout(owner.type, owner.Figure, direction, true), owner.distance + 1);

            if (!levelMap.CanFitRoom(room,globalPos))
            {
                room = new Room(data.RoomLayoutPicker.GetRandomLayout(owner.type, owner.Figure, direction, false), owner.distance + 1);

                if (levelMap.CanFitRoom(room, globalPos))
                {
                    if (levelMap.PlaceRoom(room, globalPos, owner.position))
                    {
                        return room;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                if (levelMap.PlaceRoom(room, globalPos, owner.position))
                {
                    return room;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        private bool AddSpecialRooms(LevelMap map)
        {
            if (map == null)
            {
                return false;
            }

            deadEndRooms.Sort(new RoomComparer_ByDistanceFromStart());

            RoomType[] bossRooms = data.GetBossRoomTypes();

            //super secret room id = 11;
            RoomType superSecret = data.GetSecretRoomTypes(11, out int superSecretCount);
            RoomType[] specialRooms = data.GetAllTypes();

            int specialCount = bossRooms.Length + specialRooms.Length + superSecretCount;

            if (CancelGenerationIfNotEnoughSpecialRoomSpace)
            {
                if(specialCount > deadEndRooms.Count)
                {
                    Debug.Log("DeadEnd count less than needed");
                    return false;
                }
            }

            foreach(RoomType roomType in bossRooms)
            {
                if (deadEndRooms.Count > 0)
                {
                    int Index = deadEndRooms.Count - 1;

                    deadEndRooms[Index].type = roomType;

                    if (roomType.HasSpecialFigure)
                    {
                        Room neighbour = map.FindFirstNeighbour(deadEndRooms[Index]);

                        if (neighbour != null)
                            deadEndRooms[Index].Figure = data.RoomLayoutPicker.GetRandomLayout(roomType, neighbour.Figure,
                                neighbour.position, deadEndRooms[Index].position, false);
                    }

                    deadEndRooms.RemoveAt(Index);
                }
                else
                {
                    break;
                }


            }


            for(int i = 0; i < superSecretCount; i++)
            {
                if (deadEndRooms.Count > 0)
                {
                    int Index = deadEndRooms.Count - 1;

                    deadEndRooms[Index].type = superSecret;

                    deadEndRooms.RemoveAt(Index);
                }
                else
                {
                    break;
                }


            }


            foreach (RoomType roomType in specialRooms)
            {
                if (deadEndRooms.Count > 0)
                {
                    int randomIndex = Random.Range(0, deadEndRooms.Count);

                    deadEndRooms[randomIndex].type = roomType;
                    deadEndRooms[randomIndex].Figure = data.RoomLayoutPicker.emptyLayout.Room_Figure;

                    if (roomType.HasSpecialFigure)
                    {
                        Room neighbour = map.FindFirstNeighbour(deadEndRooms[randomIndex]);

                        if (neighbour != null)
                            deadEndRooms[randomIndex].Figure = data.RoomLayoutPicker.GetRandomLayout(roomType, neighbour.Figure,
                                neighbour.position, deadEndRooms[randomIndex].position, false);
                    }

                    deadEndRooms.RemoveAt(randomIndex);
                }
                else
                {
                    break;
                }


            }

            if (!AddSecretRooms(map))
            {
                return false;
            }

            return true;
        }

        private bool AddSecretRooms(LevelMap map)
        {
            RoomType SecretRooms = data.GetSecretRoomTypes(10, out int secretCount);

            SecretRooms.rule.GenerateRooms(map, data, secretCount);

            if (SecretRooms.rule.CanTryGenerate(map, data, secretCount))
                SecretRooms = data.GetSecretRoomTypes(12, out secretCount);

            if(SecretRooms.rule.CanTryGenerate(map,data, secretCount))
                SecretRooms.rule.GenerateRooms(map, data, secretCount);

            if (SecretRooms.rule.CanTryGenerate(map, data, secretCount))
                SecretRooms = data.GetSecretRoomTypes(13, out secretCount);

            if (SecretRooms.rule.CanTryGenerate(map, data, secretCount))
                return SecretRooms.rule.GenerateRooms(map, data, secretCount);

            return false;
        }

        class RoomComparer_ByDistanceFromStart : IComparer<Room>
        {
            public int Compare(Room x, Room y)
            {
                return x.distance.CompareTo(y.distance);
            }
        }
    }
}

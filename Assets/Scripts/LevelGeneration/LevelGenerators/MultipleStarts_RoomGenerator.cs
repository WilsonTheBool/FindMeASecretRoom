using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.LevelGenerators
{
    [CreateAssetMenu(menuName = "LevelGeneration/Mult_Starts_Generator")]
    public class MultipleStarts_RoomGenerator : LevelGenerator
    {
        [Range(1, 4)]
        public int CountOfStarts;

        private List<Vector2Int> startPositions = new List<Vector2Int>();

        //public Rebirth_LevelGenerator Rebirth_LevelGenerator;

        [Range(1, 5)]
        public int multDistnceFromBorder;

        public override Vector2Int[] GetStartRooms()
        {
            return startPositions.ToArray();
        }


        public override bool GenerateLevel(LevelMap levelMap, LevelGeneratorParams data)
        {
           startPositions = new List<Vector2Int>();

            List<Room> rooms = new List<Room>();

            if(CountOfStarts >= 1)
            {
                Vector2Int vec = new Vector2Int(levelMap.StartRoomX / multDistnceFromBorder, levelMap.StartRoomY / multDistnceFromBorder);
                startPositions.Add(vec);
                var newRoom = new Room(starRoom, 0);
                if(!levelMap.PlaceRoom(newRoom, vec, new Vector2Int(-1, -1)))
                {
                    CountOfStarts++;
                }
                else
                {
                    rooms.Add(newRoom);
                }
                
            }
            
            if (CountOfStarts >= 2)
            {
                Vector2Int vec = new Vector2Int(levelMap.LevelX - levelMap.StartRoomX / multDistnceFromBorder, levelMap.LevelY - levelMap.StartRoomY / multDistnceFromBorder);
                startPositions.Add(vec);
                var newRoom = new Room(starRoom, 0);
                if (!levelMap.PlaceRoom(newRoom, vec, new Vector2Int(-1, -1)))
                {
                    CountOfStarts++;
                }
                else
                {
                    rooms.Add(newRoom);
                }
            }


            if (CountOfStarts >= 3)
            {
                Vector2Int vec = new Vector2Int(levelMap.StartRoomX / multDistnceFromBorder, levelMap.LevelY - levelMap.StartRoomY / multDistnceFromBorder);
                startPositions.Add(vec);
                var newRoom = new Room(starRoom, 0);
                if (!levelMap.PlaceRoom(newRoom, vec, new Vector2Int(-1, -1)))
                {
                    CountOfStarts++;
                }
                else
                {
                    rooms.Add(newRoom);
                }
            }

            if (CountOfStarts >= 4)
            {
                Vector2Int vec = new Vector2Int(levelMap.LevelX - levelMap.StartRoomX / multDistnceFromBorder, levelMap.StartRoomY / multDistnceFromBorder);
                startPositions.Add(vec);
                var newRoom = new Room(starRoom, 0);
                if (!levelMap.PlaceRoom(newRoom, vec, new Vector2Int(-1, -1)))
                {
                    CountOfStarts++;
                }
                else
                {
                    rooms.Add(newRoom);
                }
            }


            //Room start = new Room(starRoom, 0);
            //levelMap.PlaceRoom(start, new Vector2Int(levelMap.StartRoomX, levelMap.StartRoomY), new Vector2Int(-1, -1));

            for (int i = 0; i < rooms.Count; i++)
            {
                levelMap.StartRoomX = startPositions[i].x;
                levelMap.StartRoomY = startPositions[i].y;

                levelMap.curentGenRooms.Clear();
                levelMap.curentGenRooms.Add(rooms[i]);
                
                if (!RebirthGeneration(levelMap, data, rooms[i]))
                {
                    return false;
                }
            }

            if (!AddSecretRooms(levelMap))
            {
                return false;
            }

           

            return true;
        }
        public Room_Figure starRoom;

        public Queue<Room> roomQueue;
        List<Room> deadEndRooms;

        int curentRoomCount;
        int maxRoomCount;

        LevelGeneratorParams data;

        private float giveUpRandomlyChance = 0.5f;

        public bool CancelGenerationIfNotEnoughSpecialRoomSpace;

        public bool RebirthGeneration(LevelMap levelMap, LevelGeneratorParams data, Room start)
        {

            if (data == null)
            {
                return false;
            }


            roomQueue = new Queue<Room>();
            deadEndRooms = new List<Room>();

            this.data = data;

            //Room start = new Room(starRoom, 0);

            curentRoomCount = 1;
            maxRoomCount = data.maxRoomsCount;

            giveUpRandomlyChance = data.giveUpRandomlyChance;

            //levelMap.PlaceRoom(start, new Vector2Int(levelMap.StartRoomX, levelMap.StartRoomY), new Vector2Int(-1, -1));
            roomQueue.Enqueue(start);


            int mainLoopCount = 0;
            int maxLoop = maxRoomCount * 3;

            while (curentRoomCount < maxRoomCount)
            {

                

                CheckAddRoom(roomQueue.Peek(), levelMap);
                mainLoopCount++;



                if (roomQueue.Count == 0)
                {
                    foreach (Room room in levelMap.curentGenRooms)
                    {
                        deadEndRooms.Remove(room);

                        roomQueue.Enqueue(room);
                    }

                    if (roomQueue.Count == 0)
                    {
                        Debug.LogError("roomQueue.Count == 0");
                        return false;
                    }
                }

                if (mainLoopCount > maxLoop)
                {
                    Debug.LogError("main generation loop stuck");
                    return false;
                }
            }

            while (roomQueue.Count > 0)
            {
                Room room = roomQueue.Dequeue();
                if (levelMap.GetNeighbourCount(room, room.position) == 1)
                {
                    if (!room.Figure.isLarge)
                        deadEndRooms.Add(room);
                }
            }

            if (deadEndRooms.Count < data.minDeadEndsCount)
            {
                //Debug.Log("deadEndRooms.Count < data.minDeadEndsCount ->" + deadEndRooms.Count.ToString());
                //print("Error");
                return false;
            }

            if (curentRoomCount < maxRoomCount)
            {
                //print("Error");
                return false;
            }

            if (!AddSpecialRooms(levelMap))
            {
                //print("Error");
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
                isDeadEnd = false;
            }

            foreach (Vector2Int neighbour in neighbours)
            {
                if (startPositions.Contains(neighbour))
                {
                    continue;
                }

                //if have enough rooms already;
                if (curentRoomCount >= maxRoomCount)
                {
                    break;
                }

                if (!levelMap.IsInRange(room.position + neighbour))
                {
                    continue;
                }

                Room nRoom = levelMap.GetNeighbour(room, room.position + neighbour);

                //if ocupied - give up;
                if (nRoom != null)
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
                if (neigbourCount > 1)
                {
                    continue;
                }


                //Add Neighbour to map
                Room newRoom = AddRandomRoom(room.position + neighbour, levelMap, room, neighbour);


                if (newRoom != null)
                {
                    roomQueue.Enqueue(newRoom);
                    //AddedNeighbour = true;
                    isDeadEnd = false;

                    if (newRoom.Figure.isLarge)
                    {
                        curentRoomCount += 2;
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
            int d = owner.distance;

            if (owner.Figure.isLarge)
            {
                d += CalculateDistance(levelMap, owner, direction);
                //Debug.Log("Room (after large): " + globalPos.ToString() + " - Distance = " + d.ToString());
            }
            else
            {

                d += 1;
                //Debug.Log("Room: " + globalPos.ToString() + " - Distance = " + d.ToString());
            }



            Room room = new Room(data.RoomLayoutPicker.GetRandomLayout(owner.type, owner.Figure, direction, true), d);

            if (!levelMap.CanFitRoom(room, globalPos))
            {
                room = new Room(data.RoomLayoutPicker.GetRandomLayout(owner.type, owner.Figure, direction, false), d);

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

        private int CalculateDistance(LevelMap levelMap, Room owner, Vector2Int exit_2_pos)
        {
            Vector2Int exit_1_pos = new Vector2Int(-1000, -1000);
            Room parent = levelMap.GetRoom(owner.parent);

            //if room does not have a parent we take any exit connected to its origin point;
            if (parent == null)
            {
                var e = owner.Figure.GetRoomExitsOfTile(new Vector2Int(0, 0));

                if (e.Length > 0)
                {
                    exit_1_pos = e[0];
                }
                else
                {
                    throw new System.Exception("cant find rooms connected to origin point");
                }
            }
            else
            {
                if (parent.Figure.isLarge)
                {
                    if (parent.Figure.isLarge)
                    {
                        bool exitfound = false;
                        Vector2Int offset = owner.position - parent.position;
                        foreach (Vector2Int exit in owner.Figure.RoomExits)
                        {
                            if (parent.Figure.IsRoomContainsPos(exit + offset))
                            {
                                exit_1_pos = exit;
                                exitfound = true;
                            }
                        }


                        if (!exitfound)
                            throw new System.Exception("cant find connecting rooms (2 large rooms)");
                    }
                    else
                    {
                        exit_1_pos = parent.position - owner.position;

                    }
                }
                else
                {
                    exit_1_pos = parent.position - owner.position;

                }

            }
           
            int distance = owner.Figure.GetDistance_FromLocal(exit_1_pos, exit_2_pos);

            if (distance == -1)
            {
                throw new System.Exception("Cant find exits");
            }

            return distance;
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
            RoomType[] specialRooms = data.GetAllTypes();

            int specialCount = bossRooms.Length + specialRooms.Length;

            if (CancelGenerationIfNotEnoughSpecialRoomSpace)
            {
                if (specialCount > deadEndRooms.Count)
                {
                    Debug.Log("DeadEnd count less than needed");
                    return false;
                }
            }

            foreach (RoomType roomType in bossRooms)
            {
                if (deadEndRooms.Count > 0)
                {
                    int Index = deadEndRooms.Count - 1;

                    deadEndRooms[Index].type = roomType;
                    deadEndRooms[Index].Figure = starRoom;

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

           

            return true;
        }

        private bool AddSecretRooms(LevelMap map)
        {
            int count = 0;

            int maxCount = data.secretRoomsCount;

            List<LevelGeneratorParams.RoomTypeContainer> containers = new List<LevelGeneratorParams.RoomTypeContainer>();

            while (count < maxCount)
            {
                foreach (var room in data.secretRooms)
                {
                    for (int i = 0; i < room.count; i++)
                    {
                        if (Random.Range(0f, 1f) <= room.chanceToGen)
                        {

                            containers.Add(room);
                            count++;

                            if (count >= maxCount)
                            {
                                break;
                            }

                        }
                    }

                    if (count >= maxCount)
                    {
                        break;
                    }
                }
            }

            containers.Sort();

            foreach (var room in containers)
            {
                if (room.RoomType.rule != null && room.RoomType.rule.CanTryGenerate(map, data, 1))
                {
                    if (!room.RoomType.rule.GenerateRooms(map, data, 1) && room.cancelGenerationIfNotGenerated)
                    {
                        return false;
                    }
                }
            }


            return true;
        }
    }
}

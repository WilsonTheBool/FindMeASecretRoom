﻿using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.LevelGeneration
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rebirth_Generator")]
    public class Rebirth_LevelGenerator : LevelGenerator
    {
        public Room_Figure starRoom;

        //public Room_Figure[] allRooms;

        //private int maxRoomCount;

        public Queue<Room> roomQueue;
        List<Room> deadEndRooms;

        int curentRoomCount;
        int maxRoomCount;

        LevelGeneratorParams data;

        private float giveUpRandomlyChance = 0.5f;

        public bool CancelGenerationIfNotEnoughSpecialRoomSpace;

        Vector2Int startPosition;

        public override bool GenerateLevel(LevelMap levelMap, LevelGeneratorParams data)
        {

            if (data == null)
            {
                return true;
            }


            roomQueue = new Queue<Room>();
            deadEndRooms = new List<Room>();

            this.data = data;

            Room start = new Room(starRoom, 0);

            //0 - because -1 for superSecretRoom
            curentRoomCount = 0;
            maxRoomCount = data.maxRoomsCount;
            
            giveUpRandomlyChance = data.giveUpRandomlyChance;

            startPosition = new Vector2Int(levelMap.StartRoomX, levelMap.StartRoomY);

            levelMap.PlaceRoom(start, startPosition, new Vector2Int(-1,-1));
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
                //print(roomQueue.Count);

                if (roomQueue.Count == 0)
                {
                    foreach (Room room in levelMap.curentGenRooms)
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

            //levelMap.GetRoom(new Vector2Int(levelMap.StartRoomX, levelMap.StartRoomY)).Figure = starRoom;
            //levelMap.GetRoom(new Vector2Int(levelMap.StartRoomX, levelMap.StartRoomY)).type = null;

            Room st = levelMap.GetRoom(new Vector2Int(levelMap.StartRoomX, levelMap.StartRoomY));
            bool found = false;

            foreach (Room room in deadEndRooms)
            {
                if(room == st)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                Debug.Log("Found And Removed starter from DeadEnds");
                deadEndRooms.Remove(st);
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

            if (!levelMap.CanFitRoom(room,globalPos))
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
            if(parent == null)
            {
                var e = owner.Figure.GetRoomExitsOfTile(new Vector2Int(0, 0));

                if(e.Length > 0)
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
            //Debug.Log("Exit_1 = " + exit_1_pos.ToString() + "// Exit_2 = " + exit_2_pos.ToString());
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

            //Add super secrets
            //for(int i = 0; i < superSecretCount; i++)
            //{
            //    if (deadEndRooms.Count > 0)
            //    {
            //        int Index = deadEndRooms.Count - 1;

            //        deadEndRooms[Index].type = superSecret;
            //        deadEndRooms[Index].Figure = starRoom;
            //        deadEndRooms.RemoveAt(Index);
            //    }
            //    else
            //    {
            //        break;
            //    }


            //}


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

            if (data.GenerationLimiter != null) data.GenerationLimiter.ResetCounter();

            if (!AddSecretRooms(map))
            {
                return false;
            }
            
            if(data.GenerationLimiter != null)
            {
                if (!data.GenerationLimiter.IsCorrect_Overall())
                    return false;
            }

            return true;
        }

        private bool AddSecretRooms(LevelMap map)
        {
            int count = 0;

            int maxCount = data.secretRoomsCount;

            SecretRoomContainer containers = new SecretRoomContainer();

            while (count < maxCount)
            {
                foreach (var room in data.secretRooms)
                {
                    if (Random.Range(0f, 1f) <= room.chanceToGen)
                    {

                        containers.Add(room);
                        count += room.count;

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

            containers.Sort();

            foreach (var room in containers.containers)
            {
                for(count = 0; count < room.count - 1; count++)
                {
                    if (room.RoomType.rule != null && room.RoomType.rule.CanTryGenerate(map, data, 1))
                    {

                        if (!room.RoomType.rule.GenerateRooms(map, data, 1) && room.cancelGenerationIfNotGenerated)
                        {
                            //Debug.Log("Rooms Could not generate");
                            return false;
                        }
                    }
                }

                //for the last room of this type check limiter
                if (room.RoomType.rule != null && room.RoomType.rule.CanTryGenerate(map, data, 1))
                {

                    if (!room.RoomType.rule.GenerateRooms(map, data, 1, true) && room.cancelGenerationIfNotGenerated)
                    {
                        //Debug.Log("Rooms Could not generate");
                        return false;
                    }
                }
            }



            return true;
        }

        

        public override Vector2Int[] GetStartRooms()
        {
            return new Vector2Int[1] {startPosition};
        }

        private class SecretRoomContainer
        {
            public List<LevelGeneratorParams.RoomTypeContainer> containers = new List<LevelGeneratorParams.RoomTypeContainer>();

            public void Add(LevelGeneratorParams.RoomTypeContainer c)
            {
                foreach(var container in containers)
                {
                    if(container.RoomType == c.RoomType)
                    {
                        container.count += c.count;
                        return;
                    }
                }

                containers.Add(new LevelGeneratorParams.RoomTypeContainer() { RoomType = c.RoomType, count = c.count, 
                    cancelGenerationIfNotGenerated = c.cancelGenerationIfNotGenerated, chanceToGen = c.chanceToGen});
            }

            public void Sort()
            {
                containers.Sort();
            }

            
    }
    }

    

    public class RoomComparer_ByDistanceFromStart : IComparer<Room>
    {
        public int Compare(Room x, Room y)
        {
            return x.distance.CompareTo(y.distance);
        }
    }
}

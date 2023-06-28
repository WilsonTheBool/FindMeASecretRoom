using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/starter secert")]
    public  class StarterSecretRoom: SecretRoomGenerationRule
    {

        public RoomType SecretRoomType;



        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate, bool useLimiter = false)
        {
            RuleCostMap costMap = new RuleCostMap();

            foreach (Room room in map.rooms)
            {
                float cost = -1;

                if (room.type != null/* && room.type.isSecretRoom*/)
                {

                    cost = -10;

                }

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        if (!costMap.HasKey(key))
                            costMap.AddValue(key, 1 + cost);
                        else
                        {
                            costMap.AddValue(key, cost);
                        }

                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        costMap.SetValue(key, -100);
                }
            }

            foreach (Room room in map.rooms)
            {
                

                if (room.type != null && room.type.isSecretRoom)
                {

                    continue;

                }

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;

                    if (!(map.IsInRange(key) && map.GetRoom(key) == null))
                    {
                        continue;
                    }

                    float cost = 30 - CalculateDistance(map, room, exit) - room.distance;




                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        if (costMap.HasKey(key) && costMap.GetValue(key) >= 0)
                        {
                            if (costMap.GetValue(key) < cost)
                            {
                                costMap.SetValue(key, cost);
                            }
                        }

                }
            }

            costMap.ReSort();

            if (useLimiter && data.GenerationLimiter != null && !data.GenerationLimiter.IsCorrect(SecretRoomType, costMap, countToGenerate))
            {
                return false;
            }

            for (int i = 0; i < countToGenerate; i++)
            {
                if (!costMap.CanGetMax())
                {
                    return false;
                }

                Room secret = new Room(startRoom, 0)
                {
                    type = SecretRoomType
                };

                Vector2Int max = costMap.GetMaxKey_Randomized();

                if (map.IsInRange(max))
                {
                    map.PlaceRoom(secret, max, max);
                }
            }

            return true;
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



            //Debug.Log("Exit_1 = " + exit_1_pos.ToString() + "// Exit_2 = " + exit_2_pos.ToString());
            int distance = owner.Figure.GetDistance_FromLocal(exit_1_pos, exit_2_pos);

            if (distance == -1)
            {
                throw new System.Exception("Cant find exits");
            }



            return distance;
        }
    }
}

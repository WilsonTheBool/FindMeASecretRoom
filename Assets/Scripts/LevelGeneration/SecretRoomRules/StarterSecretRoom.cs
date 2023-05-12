using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/starter secert")]
    public  class StarterSecretRoom: SecretRoomGenerationRule
    {

        public RoomType SecretRoomType;



        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
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
                float cost = 20 - room.distance;

                if (room.type != null && room.type.isSecretRoom)
                {

                    continue;

                }

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        if (costMap.HasKey(key) && costMap.GetValue(key) >= 0)
                        {
                            if (costMap.GetValue(key) < cost)
                            {
                                costMap.SetValue(key, cost);
                            }
                        }
                        //else
                        //{
                        //    if(costMap.GetValue(key) < cost && costMap.GetValue(key) >= 0)
                        //    {
                        //        costMap.SetValue(key, cost);
                        //    }
                        //}

                }

                //foreach (Vector2Int exit in room.Figure.BlockedExits)
                //{
                //    Vector2Int key = exit + room.position;


                //    if (map.IsInRange(key) && map.GetRoom(key) == null)
                //        costMap.SetValue(key, -100);
                //}
            }

            costMap.ReSort();

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
    }
}

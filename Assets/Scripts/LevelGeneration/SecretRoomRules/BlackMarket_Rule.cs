﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Black_market_Room")]
    /// <summary>
    /// Connected to most specialRooms diaginally
    /// </summary>
    public class BlackMarket_Rule : SecretRoomGenerationRule
    {
        public RoomType SecretRoomType;

        public RoomType treasureRoom;
        public RoomType Shop;

        private List<Room> specialRooms;

        public override bool CanTryGenerate(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            specialRooms = map.rooms.FindAll((room) => room.type != null && (room.type == treasureRoom || room.type == Shop));

            return specialRooms.Count > 0;
        }


        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            RuleCostMap costMap = new RuleCostMap();

            int cost;

            foreach (Room room in map.rooms)
            {
                if(room.type != null && room.type.isSecretRoom)
                {
                    cost = -1000;
                }
                else
                {
                    cost = 1000;
                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;

                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        costMap.AddValue(key, -10000);
                }

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                            if (!costMap.HasKey(key))
                                costMap.AddValue(key, cost + IsPosIntercetcsSpecial(key, specialRooms));


                }


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

        private int IsPosIntercetcsSpecial(Vector2Int p, List<Room> sp)
        {
            int reward = 0;
           
            foreach (Room room in sp)
            {
                Vector2Int offSet = room.position - p;
                reward -= offSet.sqrMagnitude;
            }

            return reward;
        }
    }
}
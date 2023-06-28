using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Ultra secret_Room")]
    public class Ultra_SecretRoom_Rule : SecretRoomGenerationRule
    {
        public RoomType SecretRoomType;

        public int searchRange;

        private List<Room> specialRooms;

        public override bool CanTryGenerate(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            specialRooms = map.rooms.FindAll((room) => room.type != null && room.type.isSecretRoom);

            return specialRooms.Count > 0;
        }


        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate, bool useLimiter)
        {
            RuleCostMap costMap = new RuleCostMap();


            int cost;
            foreach (Room room in map.rooms)
            {
                if (room.type != null && room.type.isSecretRoom)
                {
                    cost = -10000;
                }
                else
                {
                    cost = 0;
                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;

                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        costMap.AddValue(key, -100000);
                }

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        if (!costMap.HasKey(key))
                            costMap.AddValue(key, cost + GetRaward(key, specialRooms));


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

        private float GetRaward(Vector2Int p, List<Room> sp)
        {
            float dis = 0;

            foreach (Room room in sp)
            {
                float t = Vector2Int.Distance(room.position, p);

                if (t > dis)
                {
                    dis = t;
                }
            }

            return 1000 - dis;
        }

        private int IsPosIntercetcsSpecial(Vector2Int p, List<Room> sp)
        {
            int reward = 1000;

            foreach (Room room in sp)
            {
                Vector2Int offSet = room.position - p;
                reward -= offSet.sqrMagnitude;
            }

            return reward;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Blocked_Secret_Room")]

    internal class BlockedRoom_Rule : SecretRoomGenerationRule
    {

        public RoomType SecretRoomType;

        public RuleCostMap costMap;

        public override bool CanTryGenerate(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            return true;
        }

        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate, bool useLimiter = false)
        {
            RuleCostMap costMap = new RuleCostMap(2);

            this.costMap = costMap;

            foreach (Room room in map.rooms)
            {
                float cost = 1;

                if (room.type != null)
                {
                    if (room.type.isSecretRoom)
                    {
                        cost = -10;
                    }

                }


                if(cost < 0)
                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;

                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        costMap.AddValue(key, cost);
                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;

                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        costMap.AddValue(key, cost);
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
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Normal_Secret_Room")]
    public class NormalSecretRoom_Rule : SecretRoomGenerationRule
    {

        public RoomType SecretRoomType;

        public RuleCostMap costMap;

        public override bool CanTryGenerate(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            return true;
        }

        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            //Dictionary<Vector2Int, float> secretRoomMap = new Dictionary<Vector2Int, float>();

            RuleCostMap costMap = new RuleCostMap();

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
                    else
                    {
                        cost = 1.3f;
                    }

                }



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
                        costMap.AddValue(key, -100);
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
    }
}
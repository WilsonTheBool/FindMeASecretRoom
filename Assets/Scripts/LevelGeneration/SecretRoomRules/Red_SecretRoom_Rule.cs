using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Red_Secret_Room")]
    public class Red_SecretRoom_Rule : SecretRoomGenerationRule
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
                float cost;

                if(room.type != null && room.type.isSecretRoom)
                {
                    cost = 0;
                }
                else
                {
                    cost = 1;
                }

                Vector2Int[] fourDirections = { new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1) };

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    foreach(Vector2Int direction in fourDirections)
                    {
                        Vector2Int key = exit + room.position + direction;

                        if (map.IsInRange(key) && map.GetRoom(key) == null)
                            costMap.AddValue(key, cost);
                    }

                    
                }



                //Room itself dows not connected to any rooms
                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;

                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        costMap.AddValue(key, -100);
                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;

                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        costMap.AddValue(key, -100);
                }
                //end
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

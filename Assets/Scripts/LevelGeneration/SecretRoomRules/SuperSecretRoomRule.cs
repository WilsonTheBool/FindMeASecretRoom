using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    /// <summary>
    /// hermit room - switch for super secret. 
    /// is furthest from start and connected to least rooms
    /// </summary>
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Super secert")]
    public class SuperSecretRoomRule : SecretRoomGenerationRule
    {

        public RoomType SecretRoomType;



        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            RuleCostMap costMap = new RuleCostMap();

            foreach (Room room in map.rooms)
            {
                float cost = 100 + room.distance;

                if (room.type != null)
                {

                    cost = -10;

                }

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                            if (!costMap.HasKey(key))
                                costMap.AddValue(key, cost);
                            else
                                costMap.AddValue(key, -10);



                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                            costMap.AddValue(key, -10);
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
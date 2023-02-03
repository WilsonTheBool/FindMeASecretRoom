using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{

    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Devil_Room")]
    public class DevilRoom_Rule : SecretRoomGenerationRule
    {
        public RoomType SecretRoomType;

        public RoomType BossRoomType;
        public RoomType SacrificeRoomType;

        public override bool CanTryGenerate(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            return LevelHasRoomOfType(map, BossRoomType, 1) && LevelHasRoomOfType(map, SacrificeRoomType, 1);
        }


        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            RuleCostMap costMap = new RuleCostMap();

            foreach (Room room in map.rooms)
            {
                float cost = 1;

                if (room.type != null)
                {
                    if (room.type.isSecretRoom)
                    {
                        continue;
                    }
                   
                }

                var BossPositions = GetRoomPositions(map, BossRoomType);
                var SacrificePos = GetRoomPositions(map, SacrificeRoomType);

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;

                    
                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        if (IsPosIntercetcsSpecial(key, BossPositions, SacrificePos))
                            if(!costMap.HasKey(key))
                                costMap.AddValue(key, -1);
                            else
                                costMap.AddValue(key, cost);



                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;

                    
                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        if (IsPosIntercetcsSpecial(key, BossPositions, SacrificePos))
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

        private bool IsPosIntercetcsSpecial(Vector2Int p, List<Vector2Int> sp_1, List<Vector2Int> sp_2)
        {
            foreach(Vector2Int p_1 in sp_1)
            {
                foreach(Vector2Int p_2 in sp_2)
                {
                    int max_x = Mathf.Max(p_1.x, p_2.x);
                    int max_y = Mathf.Max(p_1.y, p_2.y);

                    int min_x = Mathf.Min(p_1.x, p_2.x);
                    int min_y = Mathf.Min(p_1.y, p_2.y);


                    if ((p.x == max_x && p.y <= max_y && p.y >= min_y) ||
                        (p.x == min_x && p.y <= max_y && p.y >= min_y) ||
                        (p.y == min_y && p.x <= max_x && p.x >= min_x) ||
                        (p.y == max_y && p.x <= max_x && p.x >= min_x))
                    {
                        return true;
                    }
                    
                }
            }

            return false;
        }


    }
}
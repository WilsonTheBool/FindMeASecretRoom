using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.LevelGeneration
{
    public class SecretRoomGenerationRule : ScriptableObject
    {
        public Room_Figure startRoom;
        public virtual bool CanTryGenerate(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            return true;
        }

        public virtual bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            return false;
        }

        protected bool LevelHasRoomOfType(LevelMap map, RoomType type, int countToFind)
        {
            if (countToFind <= 0)
            {
                return true;
            }

            foreach (Room room in map.rooms)
            {
                if (room.type == type)
                {
                    countToFind--;

                    if (countToFind <= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected bool LevelHasRoomOfType(LevelMap map, int typeID, int countToFind)
        {
            if (countToFind <= 0)
            {
                return true;
            }

            foreach (Room room in map.rooms)
            {
                if (room.type.id == typeID)
                {
                    countToFind--;

                    if (countToFind <= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected List<Vector2Int> GetRoomPositions(LevelMap map, RoomType type)
        {
            List<Vector2Int> positions = new List<Vector2Int>();

            foreach (Room room in map.rooms)
            {
                if (room.type == type)
                {
                    positions.Add(room.position);
                   
                }

            }

            return positions;
        }


        public class RuleCostMap
        {
            readonly Dictionary<Vector2Int, float> secretRoomMap;

            List<KeyValuePair<Vector2Int, float>> list;

            float minValueToGet = 0; 

            public int ListCount
            {
                get { return list.Count; }
            }

            public bool HasKey(Vector2Int key)
            {
                return secretRoomMap.ContainsKey(key);
            }

            public float GetValue(Vector2Int key)
            {
                if (secretRoomMap.TryGetValue(key, out float value))
                {
                    return value;
                }

                return 0;
            }

            public void AddValue(Vector2Int key, float valueToAdd)
            {
                if (secretRoomMap.ContainsKey(key))
                {
                    secretRoomMap[key] += valueToAdd;
                }
                else
                {
                    secretRoomMap.Add(key, valueToAdd);
                }
            }

            public void SetValue(Vector2Int key, float value)
            {
                if (secretRoomMap.ContainsKey(key))
                {
                    secretRoomMap[key] = value;
                }
                else
                {
                    secretRoomMap.Add(key, value);
                }
            }

            public void Clear()
            {
                secretRoomMap.Clear();
            }

            public void ReSort()
            {
                list = new List<KeyValuePair<Vector2Int, float>>(secretRoomMap);

                list.Sort((x, y) => -x.Value.CompareTo(y.Value));
            }

            public bool CanGetMax()
            {
                if (list == null || list.Count == 0 || list[0].Value < minValueToGet)
                {
                    return false;
                }

                return true;
            }

            public Vector2Int GetMaxKey()
            {

                Vector2Int key = list[0].Key;

                list.RemoveAt(0);

                return key;
            }

            public Vector2Int[] GetAllMax()
            {
                List<Vector2Int> positions = new List<Vector2Int>();

                int lastMaxIndex = 1;
                float maxValue = list[0].Value;

                for (int i = 1; i < list.Count; i++)
                {
                    if (list[i].Value < maxValue)
                    {
                        lastMaxIndex = i;

                        break;
                    }
                }

                for(int i = 0; i < lastMaxIndex; i++)
                {
                    positions.Add(list[i].Key);
                }

                list.RemoveRange(0, lastMaxIndex);

                return positions.ToArray();
            }

            public Vector2Int GetMaxKey_Randomized()
            {

                int lastMaxIndex = 1;
                float maxValue = list[0].Value;

                for(int i = 1; i < list.Count; i++)
                {
                    if (list[i].Value < maxValue)
                    {
                        lastMaxIndex = i;
                        break;
                    }
                }

                int randomIndex = Random.Range(0, lastMaxIndex);
                Vector2Int key = list[randomIndex].Key;

                list.RemoveAt(randomIndex);

                return key;
            }

            public RuleCostMap()
            {
                secretRoomMap = new Dictionary<Vector2Int, float>();
            }

            public RuleCostMap(float minValue)
            {
                secretRoomMap = new Dictionary<Vector2Int, float>();

                minValueToGet = minValue;
            }

            public  string GetTileText(Vector2Int tile)
            {
                if(secretRoomMap.TryGetValue(tile, out float value))
                {
                    return "Tile: " + tile.ToString() + "\nValue: " + value;
                }
                else
                {
                    return "Tile: " + tile.ToString() + "\nNO DATA";
                }
            }
        }

    }
}
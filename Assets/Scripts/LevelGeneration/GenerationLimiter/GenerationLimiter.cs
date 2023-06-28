using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.GenerationLimiters
{
    [CreateAssetMenu(menuName = "LevelGeneration/GenerationLimiter")]
    public class GenerationLimiter : ScriptableObject
    {
        public LimiterData[] limiterDatas;

        int overallCount;

        public int maxOverallCount;

        public bool IsCorrect(RoomType RoomType, SecretRoomGenerationRule.RuleCostMap costMap, int RoomCount)
        {
            return GetLimiter(RoomType).IsCorrect(costMap, RoomCount);
        }

        private SecretRoomLimiter GetLimiter(RoomType RoomType)
        {

            var data = GetData(RoomType);
            return new SecretRoomLimiter(data.roomType, this, data.maxCount);
        }
        
        private LimiterData GetData(RoomType roomType)
        {
            foreach(var data in limiterDatas)
            {
                if(data.roomType == roomType) return data;

            }
            throw new System.Exception("Can't find needed roomType LimiterData");
        }

        public bool IsCorrect_Overall()
        {
            Debug.Log("Limiter Overall = " + overallCount);
            return overallCount <= maxOverallCount;
        }

        public void AddCount(int count)
        {
            overallCount += count;
        }

        public void ResetCounter()
        {
            overallCount = 0;
        }

        [System.Serializable]
        public struct LimiterData
        {
            public RoomType roomType;
            public int maxCount;
        }

        public class SecretRoomLimiter
        {
            RoomType RoomType;

            GenerationLimiter main;

            int maxPositionsCount;

            public SecretRoomLimiter(RoomType roomType, GenerationLimiter main, int maxPositionsCount)
            {
                RoomType = roomType;
                this.main = main;
                this.maxPositionsCount = maxPositionsCount;
            }

            public bool IsCorrect(SecretRoomGenerationRule.RuleCostMap costMap, int roomCount)
            {
                int count = GetCostMapMaxCount(costMap, roomCount);

              
                bool correct = count <= maxPositionsCount;

                if (correct)
                {
                    main.AddCount(count);
                }

                return correct;
            }

            private int GetCostMapMaxCount(SecretRoomGenerationRule.RuleCostMap costMap, int roomCount)
            {
                float[] values = costMap.GetAllValues();

                if(values.Length < roomCount)
                {
                    return int.MaxValue;
                }

                float curentMax = float.MinValue;
                int countMax = 0;


                for(int i = 0; i < values.Length; i++)
                {
                    if (values[i] != curentMax)
                    {
                        curentMax = values[i];

                        if(roomCount == 0)
                        {
                            return countMax;
                        }
                        else
                        {
                            roomCount--;
                        }
                    }
                    else
                    {
                        if(roomCount == 0)
                        {
                            countMax++;
                        }
                        else
                        {
                            roomCount--;
                        }
                    }
                }

                if(roomCount == 0)
                {
                    return countMax;
                }
                else
                {
                    return int.MaxValue;
                }
            }
        }
    }
}
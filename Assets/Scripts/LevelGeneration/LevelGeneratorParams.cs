using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.LevelGeneration.GenerationLimiters;

namespace Assets.Scripts.LevelGeneration
{
    [CreateAssetMenu(menuName = "LevelGeneration/Generation_Params")]
    public class LevelGeneratorParams : ScriptableObject
    {
        public RoomTypeContainer[] specailRoomTypes;

        public RoomTypeContainer[] bossRooms;

        public RoomTypeContainer[] secretRooms;

        public LevelGenerator LevelGenerator;

        public RoomLayoutPicker RoomLayoutPicker;

        public GenerationLimiter GenerationLimiter;

        public int maxRoomsCount;
        public int minDeadEndsCount;

        public int secretRoomsCount;

        [Range(0f,1f)]
        public float giveUpRandomlyChance = 0.5f;

        public Room_Figure GetRandomRoomFigure()
        {
            throw new System.Exception();
        }

        public RoomType[] GetAllTypes()
        {
            List<RoomType> roomTypes = new List<RoomType>();

            foreach(RoomTypeContainer roomType in specailRoomTypes)
            {

                for(int i = 0; i < roomType.count; i++)
                {
                    roomTypes.Add(roomType.RoomType);
                }
            }

            return roomTypes.ToArray();
        }

        public RoomType[] GetBossRoomTypes()
        {
            List<RoomType> roomTypes = new List<RoomType>();

            foreach (RoomTypeContainer roomType in bossRooms)
            {
                for (int i = 0; i < roomType.count; i++)
                {
                    roomTypes.Add(roomType.RoomType);
                }
            }

            return roomTypes.ToArray();
        }

       

        public RoomType[] GetSecretRoomTypes(string name)
        {
            List<RoomType> roomTypes = new List<RoomType>();

            foreach (RoomTypeContainer roomType in secretRooms)
            {
                if(roomType.RoomType.typeName != name)
                {
                    continue;
                }

                for (int i = 0; i < roomType.count; i++)
                {
                    roomTypes.Add(roomType.RoomType);
                }
            }

            return roomTypes.ToArray();
        }

        public RoomType GetSecretRoomTypes(int id, out int count)
        {
            count = 0;

            foreach (RoomTypeContainer roomType in secretRooms)
            {
                if (roomType.RoomType.id == id)
                {
                    count = roomType.count;

                    return roomType.RoomType;
                }
            }
            return null;

        }

        [System.Serializable]
        public class RoomTypeContainer: System.IComparable<RoomTypeContainer>
        {
            public RoomType RoomType;
            public int count;
            public bool cancelGenerationIfNotGenerated;

            [Range(0f, 1f)]
            public float chanceToGen = 1;

            public int CompareTo(RoomTypeContainer obj)
            {
                return RoomType.id.CompareTo(obj.RoomType.id);
            }
        }
    }
}
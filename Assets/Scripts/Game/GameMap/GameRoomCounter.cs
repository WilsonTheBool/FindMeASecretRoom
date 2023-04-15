using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.GameMap
{
    public class GameRoomCounter : MonoBehaviour
    {

        public GameMapRoomUnlockController GameMapRoomUnlockController;

        public List<RoomCounter> roomCounters = new List<RoomCounter>();

        public UnityEvent onVictory;

        public UnityEvent onCounterChanged;

        private void Awake()
        {
            if(GameMapRoomUnlockController == null)
            {
                GameMapRoomUnlockController = GetComponent<GameMapRoomUnlockController>();
            }

            GameMapRoomUnlockController.roomUnlocked.AddListener(OnRoomUnlocked);
            GameMapRoomUnlockController.roomLocked.AddListener(OnRoomLocked);
        }

        public void SetUp(LevelMap levelMap)
        {
            roomCounters.Clear();

            foreach(Room room in levelMap.rooms)
            {
                if(room.type != null && room.type.isSecretRoom)
                {
                    bool isUnlocked = GameMapRoomUnlockController.IsUnlocked(room);

                    AddNewRoomToCount(room.type, isUnlocked);
                }
            }

            onCounterChanged.Invoke();
        }

        private void AddNewRoomToCount(RoomType roomType, bool isUnlocked)
        {
            if(roomType == null || !roomType.isSecretRoom) { return; }

            RoomCounter roomCounter = roomCounters.Find(r => r.type == roomType);

            if(roomCounter != null)
            {
                if (isUnlocked)
                {
                    roomCounter.maxCount++;
                    roomCounter.unlockedCount++;
                }
                else
                {
                    roomCounter.maxCount++;
                }
            }
            else
            {
                if (isUnlocked)
                {
                    roomCounters.Add(new RoomCounter() { type = roomType, maxCount = 1, unlockedCount = 1 });
                }
                else
                {
                    roomCounters.Add(new RoomCounter() { type = roomType, maxCount = 1, unlockedCount = 0 });
                }

                
            }
        }

        public void CheckPlayerVictory()
        {
            foreach(RoomCounter roomCounter in roomCounters)
            {
                if(roomCounter.unlockedCount != roomCounter.maxCount)
                {
                    return;
                }
            }

            onVictory.Invoke();
        }

        private void OnRoomUnlocked(Room room)
        {
            if (room.type == null || !room.type.isSecretRoom) { return; }

            RoomCounter roomCounter = roomCounters.Find(r => r.type == room.type);

            if (roomCounter != null)
            {

                roomCounter.unlockedCount++;

            }
            else
            {
                UnityEngine.Debug.LogError("Trying to unlock WRONG room");
            }

            onCounterChanged.Invoke();

            CheckPlayerVictory();
        }

        private void OnRoomLocked(Room room)
        {
            if (room.type == null || !room.type.isSecretRoom) { return; }

            RoomCounter roomCounter = roomCounters.Find(r => r.type == room.type);

            if (roomCounter != null)
            {

                roomCounter.unlockedCount--;

                if(roomCounter.unlockedCount <= 0)
                {
                    UnityEngine.Debug.LogError("Room counter is negative");
                }

            }
            else
            {
                UnityEngine.Debug.LogError("Trying to lock WRONG room");
            }

            onCounterChanged.Invoke();
        }

        public RoomCounter GetRoomCounter(Room room)
        {
            if(room != null && room.type != null)
            {
                return roomCounters.Find(r => r.type == room.type);
            }
            else
            {
                return null;
            }
        }

        public RoomCounter GetRoomCounter(RoomType type)
        {
            if (type != null)
            {
                return roomCounters.Find(r => r.type == type);
            }
            else
            {
                return null;
            }
        }



        [System.Serializable]
        public class RoomCounter
        {
            public RoomType type;

            public int maxCount;

            public int unlockedCount;
        }


        public void Save(ref SaveData saveData)
        {
            if(saveData == null)
            {
                saveData = new SaveData();
            }

            saveData.roomCounters = new List<RoomCounter>(this.roomCounters);

            //print("Save Counters Count: " + roomCounters.Count.ToString());
        }

        public bool Load(ref SaveData data)
        {
            if (data == null || data.roomCounters == null)
            {
                return false;
            }

            this.roomCounters = data.roomCounters;

            //print("Load Counters Count: " + roomCounters.Count.ToString());

            //onCounterChanged.Invoke();

            return true;
        }

        public class SaveData
        {
            public List<RoomCounter> roomCounters;
        }
    }
}
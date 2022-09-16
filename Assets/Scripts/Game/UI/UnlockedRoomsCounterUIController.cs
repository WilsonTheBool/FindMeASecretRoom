using Assets.Scripts.Game.GameMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.UI
{
    public class UnlockedRoomsCounterUIController : MonoBehaviour
    {
        [SerializeField]
        RoomCounterUIElement prefab;

        MainGameLevelMapController main;

        GameRoomCounter counter;

        List<RoomCounterUIElement> roomCounterUIs;

        private void Start()
        {
            roomCounterUIs = new List<RoomCounterUIElement>();

            main = MainGameLevelMapController.Instance;

            counter = main.GameRoomCounter;

            if (main.isSetUpComplete)
            {
                SetUp();
            }

            main.SetUpComplete.AddListener(SetUp);
            counter.onCounterChanged.AddListener(UpdateUI);
        }

        private void UpdateUI()
        {
            foreach (var count in counter.roomCounters)
            {
                RoomCounterUIElement el = roomCounterUIs.Find(r => r.type == count.type);

                if(el != null)
                {
                    el.UpdateCount(count);
                }
            }
        }

        private void SetUp()
        {
            DeleteAll();

            counter = main.GameRoomCounter;

            foreach(var count in counter.roomCounters)
            {
                RoomCounterUIElement el = Instantiate(prefab, this.transform);

                el.SetUp(count);

                roomCounterUIs.Add(el);
            }
        }

        private void DeleteAll()
        {
            while(roomCounterUIs.Count > 0)
            {
                var counter = roomCounterUIs[0];

                roomCounterUIs.RemoveAt(0);

                Destroy(counter.gameObject);
            }
        }
    }
}
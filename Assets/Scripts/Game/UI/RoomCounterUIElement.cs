using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using Assets.Scripts.LevelGeneration;
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay.SecretRoomBook;

namespace Assets.Scripts.Game.UI
{
    public class RoomCounterUIElement : MonoBehaviour
    {

        public Image RoomTypeImage;

        public TMPro.TMP_Text text;

        public Color whiteColor;

        public Color GreenColor;

        private int maxCount;
        private int curentCount;

        [HideInInspector]
        public RoomType type;

        public RoomToBookPageTanslator translator;

        public void OpenRoomPage()
        {
            SecretRoomBookController.Instance.OpenBook(SecretRoomBookController.ChapterType.rooms);
            SecretRoomBookController.Instance.TryOpenPage(translator.GetRoomPage(type));
        }

        public void SetUp(GameRoomCounter.RoomCounter counter)
        {
            RoomTypeImage.sprite = counter.type.icon;

            type = counter.type;

            UpdateCount(counter);

        }

        public void UpdateCount(GameRoomCounter.RoomCounter counter)
        {
            if(maxCount == counter.maxCount && curentCount == counter.unlockedCount)
            {
                return;
            }

            maxCount = counter.maxCount;
            curentCount = counter.unlockedCount;

            text.text = curentCount + "/" + maxCount;

            if(curentCount >= maxCount)
            {
                text.color = GreenColor;
            }
            else
            {
                text.color = whiteColor;
            }
        }
    }
}
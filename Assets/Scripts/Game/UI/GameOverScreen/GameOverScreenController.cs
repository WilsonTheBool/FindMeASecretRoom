using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Game.Items;
using Assets.Scripts.LevelGeneration;
using Assets.Scripts.InputManager;
using Assets.Scripts.Game.SoundManagment;

namespace Assets.Scripts.Game.UI.GameOverScreen
{
    public class GameOverScreenController : MonoBehaviour
    {
        public InputListener InputListener;
        [HideInInspector]
        public GameSoundController SoundController;

        public TMP_Text Title;
        public TMP_Text description;

        public TMP_Text levelText;
        public TMP_Text levelCount;

        public TMP_Text itemsTitle;
        public Transform itemHolder;
        public Image itemPrefab;

        public TMP_Text  roomsTitle;
        public Transform roomIconHolder;
        public Image iconPrefab;

        public Button buttonShowWindow;

        public float MainStepDelay;

        public float itemsStepDelay;
        public float maxItemsStepDelay;
        public int countToSwitchSpeedItem;


        public float roomIconsStepDelay;
        public float maxRoomStepDelay;
        public int countToSwitchSpeedRoom;

        [SerializeField]
        private Item[] items;
        [SerializeField]
        private RoomType[] rooms;
        private int levelInt;
        private int maxlevel;

        private bool isSkip = false;
        private bool isHiden = false;

        private void Awake()
        {
            gameObject.SetActive(false);
            Title.gameObject.SetActive(false);
            description.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            levelCount.gameObject.SetActive(false);
            itemsTitle.gameObject.SetActive(false);
            itemHolder.gameObject.SetActive(false);
            roomsTitle.gameObject.SetActive(false);
            roomIconHolder.gameObject.SetActive(false);
            if (buttonShowWindow != null)
            {
                buttonShowWindow.onClick.AddListener(HideShowWindow);
                buttonShowWindow.gameObject.SetActive(false);
            }

            InputListener.OnActivate.AddListener(Skip);
            InputListener.OnAccept.AddListener(Skip);
            InputListener.OnEscape.AddListener(HideShowWindow);
        }

        public void HideShowWindow()
        {
            isSkip = true;
            isHiden = !isHiden;

            StopAllCoroutines();

            if(isHiden)
            FullSkip();

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(!isHiden);
            }

            if (!isHiden)
                FullSkip();

            if(buttonShowWindow != null)
            if (isHiden)
                buttonShowWindow.gameObject.SetActive(true);
            else
                buttonShowWindow.gameObject.SetActive(false);
        }

        private void Start()
        {
            if(SoundController == null)
            {
                SoundController = GameSoundController.Instance;
            }
        }


        public void SetUp(Item[] items, RoomType[] rooms, int levelCount, int maxlevel)
        {
            this.items = items;
            this.rooms = rooms;
            this.levelInt = levelCount;
            this.maxlevel = maxlevel;

            gameObject.SetActive(true);


            StartCoroutine(MainCo());
        }

        private void FullSkip()
        {
            Title.gameObject.SetActive(true);
            description.gameObject.SetActive(true);

            levelText.gameObject.SetActive(true);
            levelCount.gameObject.SetActive(true);
            levelCount.text = levelInt + "/" + maxlevel;

            itemsTitle.gameObject.SetActive(true);
            itemHolder.gameObject.SetActive(true);

            for(int i = 0; i < itemHolder.childCount; i++)
            {
                Destroy(itemHolder.GetChild(i).gameObject);
            }

            itemHolder.DetachChildren();

            //items showcase
            foreach (Item item in items)
            {

                Image i = Instantiate(itemPrefab, itemHolder);
                i.sprite = item.Sprite;
            }

            roomsTitle.gameObject.SetActive(true);
           
            roomIconHolder.gameObject.SetActive(true);

            for (int i = 0; i < roomIconHolder.childCount; i++)
            {
                Destroy(roomIconHolder.GetChild(i).gameObject);
            }

            roomIconHolder.DetachChildren();

            //rooms showcase
            foreach (RoomType room in rooms)
            {
                Image i = Instantiate(itemPrefab, roomIconHolder);
                i.sprite = room.icon;
            }
        }

        public IEnumerator MainCo()
        {
            Title.gameObject.SetActive(true);
            if (!isSkip) yield return new WaitForSeconds(MainStepDelay);

            description.gameObject.SetActive(true);
            if (!isSkip) yield return new WaitForSeconds(MainStepDelay);

            levelText.gameObject.SetActive(true);
            levelCount.gameObject.SetActive(true);
            levelCount.text = levelInt + "/" + maxlevel;
            if (!isSkip) yield return new WaitForSeconds(MainStepDelay);


            itemsTitle.gameObject.SetActive(true);
            if (!isSkip) yield return new WaitForSeconds(MainStepDelay);
            itemHolder.gameObject.SetActive(true);
            if (!isSkip) yield return new WaitForSeconds(MainStepDelay);

            isSkip = false;

            int count = 0;
            float delay = itemsStepDelay;
            //items showcase
            foreach (Item item in items)
            {
                count++;

                Image i = Instantiate(itemPrefab, itemHolder);
                i.sprite = item.Sprite;

                if (!isSkip)
                {
                    SoundController.PlayClip(SoundController.data.player_SwitchItem);
                    yield return new WaitForSeconds(delay);
                }

                if(count > countToSwitchSpeedItem)
                {
                    delay = maxItemsStepDelay;
                }
            }

            isSkip = false;
            count = 0;
            delay = roomIconsStepDelay;

            roomsTitle.gameObject.SetActive(true);
            if (!isSkip) yield return new WaitForSeconds(MainStepDelay);
            roomIconHolder.gameObject.SetActive(true);
            if (!isSkip) yield return new WaitForSeconds(MainStepDelay);

            //items showcase
            foreach (RoomType room in rooms)
            {
                count++;

                Image i = Instantiate(itemPrefab, roomIconHolder);
                i.sprite = room.icon;

                if (!isSkip)
                {
                    SoundController.PlayClip(SoundController.data.player_SwitchItem);
                    yield return new WaitForSeconds(delay);
                }

                if (count > countToSwitchSpeedRoom)
                {
                    delay = maxRoomStepDelay;
                }
            }

        }

        private void Skip()
        {
            isSkip = true;
        }
    }
}
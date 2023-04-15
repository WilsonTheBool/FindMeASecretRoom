using Assets.Scripts.Game.Gameplay.BossRush;
using Assets.Scripts.Game.UI.GameOverScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.UI
{
    public class GameUIController : MonoBehaviour
    {
        public Transform Background_Holder;
        public Transform GameUI_Holder;
        public Transform MenuUI_Holder;
        public Transform OnTopUI_Holder;

        public TransferAnimationUIObject gold_prefab;

        public TransferAnimationUIObject genericPrefab;

        public PlayerHpUIController playerHpUIController;
        public PlayerActiveItemsUIController playerActiveItemsUIController;
        public UnlockedRoomsCounterUIController unlockedRoomsCounterUIController;

        public PlayerGoldUIController playerGoldUIController;
        public ComboCounterUI ComboCounterUI;

        public ScreenTransitionController ScreenTransitionController;

        public TresureRoomUIController TresureRoomUIController;
        public ShopUIController ShopUIController;

        public PlayerPassiveItemsUIController PlayerPassiveItemsUIController;

        public GameOverScreenController GameOverScreenController;
        public GameOverScreenController VictoryScreenController;

        public PlayerTrinketUI PlayerTrinketUI;

        public static GameUIController Instance { get; private set; }

        public Camera Camera;

        public Canvas mainCanvas;

        private List<Queue<TransferAnimData>> list;
        private List<float> delayList;

        public float TransferAnimDelay;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(Instance);
                Instance = this;
            }

            list = new List<Queue<TransferAnimData>>();
            delayList = new List<float>();

            if (Camera == null)
            {
                Camera = Camera.main;
            }

            if (playerHpUIController == null)
            {
                playerHpUIController = GetComponentInChildren<PlayerHpUIController>(true);
            }

            if (playerActiveItemsUIController == null)
            {
                playerActiveItemsUIController = GetComponentInChildren<PlayerActiveItemsUIController>(true);
            }

            if (unlockedRoomsCounterUIController == null)
            {
                unlockedRoomsCounterUIController = GetComponentInChildren<UnlockedRoomsCounterUIController>(true);
            }

            if (playerGoldUIController == null)
            {
                playerGoldUIController = GetComponentInChildren<PlayerGoldUIController>(true);
            }

            if (ComboCounterUI == null)
            {
                ComboCounterUI = GetComponentInChildren<ComboCounterUI>(true);
            }

            if (ScreenTransitionController == null)
            {
                ScreenTransitionController = GetComponentInChildren<ScreenTransitionController>(true);
            }

            if (TresureRoomUIController == null)
            {
                TresureRoomUIController = GetComponentInChildren<TresureRoomUIController>(true);
            }


            if (PlayerPassiveItemsUIController == null)
            {
                PlayerPassiveItemsUIController = GetComponentInChildren<PlayerPassiveItemsUIController>(true);
            }

            if (ShopUIController == null)
            {
                ShopUIController = GetComponentInChildren<ShopUIController>(true);
            }

            if (GameOverScreenController == null)
            {
                GameOverScreenController = GetComponentInChildren<GameOverScreenController>(true);
            }

            if(PlayerTrinketUI == null)
            {
                PlayerTrinketUI = GetComponentInChildren<PlayerTrinketUI>(true);
            }

            list = new List<Queue<TransferAnimData>>();
        }

        private void Update()
        {
            for(int i = 0; i < delayList.Count; i++)
            {
                delayList[i] -= Time.deltaTime;
            }

            for (int i = 0; i < list.Count; i++)
            {
                var queue = list[i];
                float delay = delayList[i];

                if(delay <= 0)
                {
                    delayList[i] = TransferAnimDelay;

                    if (queue.Count > 0)
                        CreateTransSingle(queue.Dequeue());

                    if(queue.Count <= 0)
                    {
                        list.RemoveAt(i);
                        delayList.RemoveAt(i);
                    }
                }
            }
        }

        public Vector3 GetPosition_Ui_To_World(Vector3 uiPos)
        {
            
            Vector3 pos = Camera.ScreenToWorldPoint(uiPos);
            pos.z = 0;
            return pos;
        }

        public void CreateTransferAnimation(TransferAnimData data,  int count)
        {
            Queue<TransferAnimData> queue = new Queue<TransferAnimData>();

            for(int i = 0; i < count; i++)
            {
                queue.Enqueue(data);
            }

            list.Add(queue);
            delayList.Add(TransferAnimDelay);
        }

        private void CreateTransSingle(TransferAnimData data)
        {
            var obj = Instantiate(data.prefab, data.origin, Quaternion.Euler(0, 0, 0));

            if(data.spriteToChange != null)
            obj.SpriteRenderer.sprite = data.spriteToChange;

            obj.transform.localScale = data.startScale;
            obj.StartTransfer(data.destination, data.endScale);
        }

        private Transform GetCanvasHolder(UIRenderType type)
        {
            switch (type)
            {
                case UIRenderType.Background:
                    {
                        return Background_Holder;
                    }
                case UIRenderType.GameUI:
                    {
                        return GameUI_Holder;
                    }
                case UIRenderType.MenuUI:
                    {
                        return MenuUI_Holder;
                    }
                case UIRenderType.OnTopUI:
                    {
                        return OnTopUI_Holder;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        public GameObject InstantiateToCanvas(GameObject prefab, UIRenderType type)
        {
            return Instantiate(prefab, GetCanvasHolder(type));
        }

        public T InstantiateToCanvas<T>(T prefab, UIRenderType type) where T : MonoBehaviour
        {
            return Instantiate<T>(prefab, GetCanvasHolder(type));
        }

        public struct TransferAnimData
        {
            public Vector3 origin;
            public Vector3 destination;
            public Vector3 startScale; 
            public Vector3 endScale;

            public TransferAnimationUIObject prefab;

            public Sprite spriteToChange;
        }

        public enum UIRenderType
        {
            Background,
            GameUI,
            MenuUI,
            OnTopUI,
        }
    }
}
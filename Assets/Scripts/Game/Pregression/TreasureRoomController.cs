using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.Items.ItemPools;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Pregression
{
    public class TreasureRoomController : MonoBehaviour
    {
        private TresureRoomUIController uiContrller;

        private PlayerItemsController PlayerItemsController;

        private PlayerGoldController PlayerGoldController;

        private GameProgressionController progression;

        private MainGameLevelMapController main;

        private GameUIController mainUi;

        private ItemPoolController ItemPoolController;

        public int itemsToGenerate;

        public int goldOnSkip;

        public float waitTimeOnEnd;

        public List<Item> items;

        public UnityEvent OnTresureOpened;
        public UnityEvent ItemSelected;
        public UnityEvent OnTresureClosed;

        private void Start()
        {
            main = MainGameLevelMapController.Instance;
            progression = GameProgressionController.Instance;
            ItemPoolController = progression.ItemPoolController;
            PlayerItemsController = Player.instance.itemsController;
            PlayerGoldController = Player.instance.goldController;
            mainUi = GameUIController.Instance;
            uiContrller = GameUIController.Instance.TresureRoomUIController;
            uiContrller.ItemSelected.AddListener(OnItemSelected);
            uiContrller.SkipRequested.AddListener(OnSkip);
        }

        private bool isSkipping;
        private void OnSkip()
        {
            if (isSkipping)
            {
                return;
            }

            isSkipping = true;

            PlayerGoldController.AddGold(goldOnSkip);

            GameUIController.Instance.CreateTransferAnimation(new GameUIController.TransferAnimData { 
                origin = mainUi.GetPosition_Ui_To_World(uiContrller.skipButton.transform.position), 
                destination = mainUi.GetPosition_Ui_To_World(mainUi.playerGoldUIController.transform.position),
                startScale = new Vector3(0.5f, 0.5f),
                endScale = new Vector3(0.5f, 0.5f),
                prefab = mainUi.gold_prefab,
            }, goldOnSkip);

            StartCoroutine(WaitThenEnd());
        }

        private IEnumerator WaitThenEnd()
        {
            

            yield return new WaitForSeconds(waitTimeOnEnd);

            ItemSelected.Invoke();

            isSkipping = false;
        }

        private void OnItemSelected(ItemSelectUI item)
        {
            if (isSkipping)
            {
                return;
            }

            if (item != null)
            {
                PlayerItemsController.AddItem(item.item, new Item.ItemExternalEventArgs { mainGameController = GameMap.MainGameLevelMapController.Instance,
                player = Player.instance,
                tilePos = Vector2Int.zero});
            }


            Vector3 des;

            if (item.item.isUseItem)
            {
                des = mainUi.GetPosition_Ui_To_World(mainUi.playerActiveItemsUIController.transform.position);
                GameUIController.Instance.CreateTransferAnimation(new GameUIController.TransferAnimData
                {
                    origin = mainUi.GetPosition_Ui_To_World(item.transform.position),
                    destination = des,
                    startScale = new Vector3(0.8f, 0.8f),
                    endScale = new Vector3(1, 1),
                    prefab = mainUi.genericPrefab,
                    spriteToChange = item.item.Sprite,
                }, 1); ;
            }


            uiContrller.StartFadeAnimation(item);

            StartCoroutine(WaitThenEnd());
        }



        public void CreateWindow()
        {
            items.Clear();
            for(int i = 0; i < itemsToGenerate; i++)
            {
                items.Add(ItemPoolController.GetItemFromPool(ItemPoolController.PoolType.trasure));
            }

            uiContrller.CreateWindow(items.ToArray());

            OnTresureOpened.Invoke();
        }

        public void CloseWindow()
        {
            uiContrller.CloseWIndow();

            OnTresureClosed.Invoke();
        }
    }
}
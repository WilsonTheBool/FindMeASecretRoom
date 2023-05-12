using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.UI;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game.VictoryController;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game.SoundManagment;
using Assets.Scripts.InputManager;

namespace Assets.Scripts.Game.Gameplay.BossRush
{
    public class BossRushController : MonoBehaviour
    {
        public List<LevelSaveData> levelDatas;

        public MainGameLevelMapController main;

        private int index = 0;

        public UnityEvent OnComplete;

        public UnityEvent anyLevelFinished;

        public GameObject tvStatic;

        public PlaySound PlaySound;

        public BossRush_Timer timer;

        public BossRush_Counter counter_prefab;
        private BossRush_Counter counter;

        public int maxLevels;
        public int completedLevels;

        public float transitionLength = 1.0f;
        public float staticLength = 1.0f;

        public float timerLength = 10f;

        private void Awake()
        {
            tvStatic.SetActive(false);
        }

        public void SetUp(LevelGeneratorParams[] genParams)
        {
            main = MainGameLevelMapController.Instance;

            levelDatas = new List<LevelSaveData>();

            maxLevels = genParams.Length;

            completedLevels = 0;

            SetUp_Levels(genParams);

            counter = GameUIController.Instance.InstantiateToCanvas(counter_prefab, GameUIController.UIRenderType.GameUI);

            PlayerActionController.ActionPerformed.AddListener(OnPlayerAction);

            GameVictoryController.instance.SetBehaviour(new BossRush_VictoryBehaviour(this));
            main.GameRoomCounter.onVictory.AddListener(OnLevelComplete);

            //GameProgressionController.Instance.transitionEnded.AddListener(OnTransitionEnd);
            GameProgressionController.Instance.transitionFadeInEnd.AddListener(OnTransitionFadeIn);

            timer.SetUp(timerLength);
            timer.OnTimerEnd.AddListener(OnLevelTimerEnd);
        }

        private void SetUp_Levels(LevelGeneratorParams[] genParams)
        {
            foreach (LevelGeneratorParams p in genParams)
            {
                main.LevelGenerationController.LevelGeneratorParams = p;
                LevelMap map = main.LevelGenerationController.Generate();

                if (map != null)
                {
                    levelDatas.Add(SetUpLevel(map, false));
                    //print("Add new map: " + p.name);
                }
                else
                {
                    //print("SKIPED map: " + p.name + "!!!!!");
                }

                foreach (var data in levelDatas)
                {
                    //print(data.counterSaveData.roomCounters.Count);
                }
            }

            index = -1;


        }

        private void OnTransitionFadeIn(GameProgressionController gp)
        {
            counter.SetUp(this);
            StartCoroutine(SwitchAnim(true));
            GameProgressionController.Instance.transitionEnded.RemoveListener(OnTransitionFadeIn);
        }

        //private void OnTransitionEnd(GameProgressionController gp)
        //{
        //    timer.StartTImer(timerLength + 5);
        //    GameProgressionController.Instance.transitionFadeInEnd.RemoveListener(OnTransitionEnd);
        //}

        private void OnLevelComplete()
        {
            levelDatas.RemoveAt(index);

            index--;

            levelComplete = true;

            completedLevels++;

            anyLevelFinished.Invoke();

            if (levelDatas.Count <= 0)
            {
                StopAllCoroutines();

                OnComplete.Invoke();
            }
        }

        bool levelComplete;

        private void OnDestroy()
        {
            PlayerActionController.ActionPerformed.RemoveListener(OnPlayerAction);
        }

        public void OnPlayerAction()
        {
            if(!levelComplete)
            {
                var levelData = levelDatas[index];

                SaveData(ref levelData);
            }
            else
            {
                levelComplete = false;
            }
          
            SwitchLevel();
        }

        private void SaveData(ref LevelSaveData levelData)
        {
            main.GameMapRoomUnlockController.Save(ref levelData.unlockSaveData);

            main.GameRoomCounter.Save(ref levelData.counterSaveData);

            main.GameTilemapController.Save(ref levelData.tilemapController);

            main.cam.GetComponent<CameraSaveStateController>()?.Save(ref levelData.cameraData);
        }

        public void SwitchLevel()
        {
            timer.StopTimer();

            StartCoroutine(SwitchAnim(false));
        }

        private void IncrementLevelIndex()
        {
            index++;

            if (index >= levelDatas.Count)
            {
                index = 0;
            }
        }

        private LevelSaveData SetUpLevel(LevelMap level, bool isFinal)
        {
            main.SetUpLevel(level, true);

            LevelSaveData levelData = new LevelSaveData()
            {
                levelMap = level,
                counterSaveData = new GameRoomCounter.SaveData(),
                unlockSaveData = new GameMapRoomUnlockController.SaveData(),
                tilemapController = new GameTilemapController.SaveData(),
                cameraData = new CameraSaveStateController.SaveData(),
            };

            SaveData(ref levelData);

            return levelData;
        }

        private void SetLevel(LevelSaveData level)
        {
            main.LevelMap = level.levelMap;
            main.GameMapRoomUnlockController.Load(ref level.unlockSaveData);
            main.GameRoomCounter.Load(ref level.counterSaveData);
            main.GameTilemapController.Load(ref level.tilemapController);
            main.cam.GetComponent<CameraSaveStateController>()?.Load(ref level.cameraData);
            main.LevelMapRenderer.RenderAll(level.levelMap);

            GameUIController.Instance.unlockedRoomsCounterUIController.SetUp();
        }

        [SerializeField]
        private InputListener InputListener;

        private IEnumerator SwitchAnim(bool firstStart)
        {
            InputListener.enabled = true;

            if (!firstStart)
                yield return new WaitForSeconds(transitionLength);


            if (levelDatas.Count > 1)
                tvStatic.SetActive(true);

            PlaySound.Play();

            if (!firstStart)
                timer.DeleteTimer();
            //Start Background fade;

            yield return new WaitForSeconds(staticLength);

            levelComplete = false;

           
            IncrementLevelIndex();

            if (index >= 0 && index < levelDatas.Count)
            SetLevel(levelDatas[index]);

            timer.StartTImer();

            tvStatic.SetActive(false);

            InputListener.enabled = false;
        }

        private void OnLevelTimerEnd()
        {
           OnPlayerAction();
        }

        public class LevelSaveData
        {
            public LevelMap levelMap;

            public GameMapRoomUnlockController.SaveData unlockSaveData;

            public GameRoomCounter.SaveData counterSaveData;

            public GameTilemapController.SaveData tilemapController;

            public CameraSaveStateController.SaveData cameraData;
        }

        public class BossRush_VictoryBehaviour : VictoryBehaviour
        {
            public BossRushController BossRushController;

            public BossRush_VictoryBehaviour(BossRushController bossRushController)
            {
                BossRushController = bossRushController;
            }

            public override void OnAdd()
            {

                if (BossRushController != null)
                {
                    BossRushController.OnComplete.AddListener(InvokeVictory);
                }
            }

            public override void OnRemove()
            {

                if (BossRushController != null)
                {
                    BossRushController.OnComplete.RemoveListener(InvokeVictory);
                }
            }

        }
    }
}
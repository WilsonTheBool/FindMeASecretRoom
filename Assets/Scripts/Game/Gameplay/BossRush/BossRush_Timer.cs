using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game.UI;
using Assets.Scripts.Game.CameraControlls;
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game.Gameplay.BossRush
{
    public class BossRush_Timer: MonoBehaviour
    {
        public TMPro.TMP_Text timerText_prefab;

        private TMPro.TMP_Text timerText;

        public UnityEvent OnTimerStart;
        public UnityEvent OnTimerEnd;

        public float targetTime;
        public float curentTime;

        public bool isDecending;

        bool isWorking;

        public Color NormalColor;
        public Color DangerColor;

        public float DangerValue;

        public bool resetOnAction;

        public void SetUp(float seconds)
        {
            targetTime = seconds;
            curentTime = 0;

            MainGameLevelMapController.Instance.LevelMapRenderer.onRenderEnded.AddListener(StartTImer);
            MainGameLevelMapController.Instance.onLevelOver.AddListener(() => Destroy(this.gameObject));

            if (resetOnAction)
            {
                PlayerActionController.ActionPerformed.AddListener(OnPlayerAction);
            }
        }

        public void OnPlayerAction()
        {
            StopTimer();
            StartTImer();
        }

        public void StartTImer()
        {
            isWorking = true;
            curentTime = 0;
            if(timerText == null)
            timerText = GameUIController.Instance.InstantiateToCanvas<TMPro.TMP_Text>(timerText_prefab, GameUIController.UIRenderType.GameUI);
        }

        public void TimerEnd()
        {
            StopTimer();

            Player.instance.playerHPController.RequestTakeDamage(new PlayerHPController.HpEventArgs(1, this.gameObject));

            OnTimerEnd.Invoke();
        }

        public void StopTimer()
        {
            isWorking = false;
        }

        private void OnDestroy()
        {
            DeleteTimer();
        }

        public void DeleteTimer()
        {
            DeleteText();
        }

        private void DeleteText()
        {
            if(timerText != null)
            {
                Destroy(timerText.gameObject);
            }
        }

        public void Update()
        {
            if (!isWorking)
            {
                return;
            }

            curentTime += Time.deltaTime;

            if (curentTime >= targetTime)
            {
                TimerEnd();
            }

            UpdateText();
        }

        private void UpdateText()
        {
            if(timerText != null)
            {
                if (isDecending)
                {
                    var value = targetTime - curentTime;

                    if(value < DangerValue)
                    {
                        timerText.color = DangerColor;
                    }
                    else
                    {
                        timerText.color = NormalColor;
                    }

                    timerText.text = Math.Round(value,2).ToString();
                }
                else
                {
                    var value =  curentTime;

                    if (value > DangerValue)
                    {
                        timerText.color = DangerColor;
                    }
                    else
                    {
                        timerText.color = NormalColor;
                    }

                    timerText.text = Math.Round(value, 2).ToString();
                }

                
            }
        }
    }
}

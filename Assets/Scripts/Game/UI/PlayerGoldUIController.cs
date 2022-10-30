using Assets.Scripts.Game.PlayerController;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.UI
{
    public class PlayerGoldUIController: MonoBehaviour
    {
        public TMPro.TMP_Text goldText;

        private PlayerGoldController PlayerGoldController;

        public float CountDelay;
        private float delay;

        private int goldCount;
        private bool needChange;
        private int targetCount;

        private void Start()
        {
            PlayerGoldController = Player.instance.goldController;
            PlayerGoldController.GoldChanged.AddListener(UpdateText);

            goldText.text = PlayerGoldController.gold.ToString();

            UpdateText();
        }

        void UpdateText()
        {
            targetCount = PlayerGoldController.gold;

            if(targetCount != goldCount)
            {
                needChange = true;
            }
        }

        private void Update()
        {
            if (needChange)
            {
                if(delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else
                {
                    delay = CountDelay;

                    if (goldCount == targetCount)
                    {
                        needChange = false;
                        
                        return;
                    }

                    if (goldCount > targetCount)
                    {
                        goldCount--;
                        goldText.text = goldCount.ToString();
                    }
                    else
                    {
                        goldCount++;
                        goldText.text = goldCount.ToString();
                    }
                }

                
            }
        }
    }


}

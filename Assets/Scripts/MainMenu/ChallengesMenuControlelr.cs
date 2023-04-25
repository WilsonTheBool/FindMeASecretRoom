﻿using Assets.Scripts.Challenges;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class ChallengesMenuControlelr : MonoBehaviour
    {
        public ChallengeRunController controller;

        public StartChallengeButton prefab;

        public Transform ChallengeButtonsHolder;

        public TMPro.TMP_Text ChallengeDescription;

        public TMPro.TMP_Text ChallengeCounter;

        public SceneLoader mainGame;

        public StartChallengeButton normalRun;

        public GameObject LoadingScreen;

        private void Start()
        {
            normalRun.OnClick.AddListener(LoadRun);

            foreach (ChallengeRunData data in controller.allChalanges)
            {
                bool isUnlocked = controller.IsChallangeUnlocked(data);

                var button = Instantiate(prefab, ChallengeButtonsHolder);
                button.SetUP(data, ChallengeDescription, controller.IsChallengeComplete(data), isUnlocked);
                button.OnClick.AddListener(LoadRun);

            }

            ChallengeCounter.text = controller.completedChallenges.Count.ToString() + "/" + controller.allChalanges.Length.ToString();
        }

        public void LoadRun(StartChallengeButton button)
        {
            if(button == normalRun)
            {

            }

            LoadingScreen.SetActive(true);
            controller.CurentChallenge = button.ChallengeRunData;

            mainGame.LoadScene();
        }

    }
}
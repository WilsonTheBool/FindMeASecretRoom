using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Achievements
{
    public class Achievment : ScriptableObject
    {
        public int id;

        public AchievementsController controller;

        public UnityEvent<Achievment> onAchivementUnlock;

        public bool isSigleUnlock;

        public virtual void OnAwakeWhenUnlocked(AchievmentAction.AchivementArgs achivementArgs)
        {

        }

        public virtual void OnRunStarted(AchievmentAction.AchivementArgs achivementArgs)
        {
            Debug.Log("AChievement OnRunStaerted Call");
        }

        public virtual void OnRunVictory(AchievmentAction.AchivementArgs achivementArgs)
        {

        }

        public virtual void OnRunDefeat(AchievmentAction.AchivementArgs achivementArgs)
        {

        }

        public virtual void UnlockAchivement(AchievmentAction.AchivementArgs achivementArgs)
        {
            Debug.Log("AChievement UNlocked Call");
            onAchivementUnlock.Invoke(this);
        }

    }
}
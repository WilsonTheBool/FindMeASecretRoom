using Assets.Scripts.Achievements;
using Assets.Scripts.Unlocks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUnlockAndAchievementCounter : MonoBehaviour
{
    public UnlockControllerData_SO unlocks;
    public AchievementsController AchievementsController;

    public TMPro.TMP_Text items;
    public TMPro.TMP_Text achievements;
    
    // Start is called before the first frame update
    void Start()
    {
        items.text = "Items unlocked:" + unlocks.UnlockedItems.Count.ToString();
        achievements.text = "Achievements unlocked:" + AchievementsController.unlockedAchivements.Count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

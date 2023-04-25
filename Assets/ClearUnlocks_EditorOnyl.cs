using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Unlocks;

public class ClearUnlocks_EditorOnyl : MonoBehaviour
{
    [SerializeField]
    UnlockControllerData_SO UnlockControllerData_SO;

    private void Awake()
    {
        UnlockControllerData_SO.UnlockedItems = new List<Assets.Scripts.Game.Items.Item>();
    }

    public void ClearUnlocks()
    {
        UnlockControllerData_SO.UnlockedItems = new List<Assets.Scripts.Game.Items.Item>();
    }
}

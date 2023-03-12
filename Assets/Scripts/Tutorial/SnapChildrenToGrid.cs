using Assets.Scripts;
using Assets.Scripts.LevelGeneration;
using UnityEngine;

public class SnapChildrenToGrid : MonoBehaviour
{
    public GridMap grid;

    public void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.position = grid.GetCellCenter(grid.WorldToCell(child.position)) + new Vector3(0,0,child.position.z);
        }
    }
}

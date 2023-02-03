using System.Collections;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class SnapLevelsToGrid : MonoBehaviour
    {
        FloatingLevel level;

        GridMap grid;

        private void Awake()
        {
            level = GetComponent<FloatingLevel>();
            grid = FindObjectOfType<GridMap>();
        }

        public void Start()
        {
            foreach(var room in level.rooms)
            {
                room.transform.position = grid.GetCellCenter(grid.WorldToCell(room.transform.position));
            }
        }
    }
}
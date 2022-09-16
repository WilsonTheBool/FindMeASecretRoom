using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.InputManager
{
    public class GameLevelInputManager : MonoBehaviour
    {
        public GridMap Grid;

        public InputListener Listener;

        public Vector2Int CurentTileMousePosition;

        public Vector3 curentCellCeneter;
        public Vector3 curentCell;

        public UnityEvent TilePositionChanged;

        private void Update()
        {
            Vector2Int newPos = Grid.WorldToCell(Listener.worldMousePosition);

            if(CurentTileMousePosition != newPos)
            {
                CurentTileMousePosition = newPos;

                curentCell = Grid.CellToWorld(newPos);

                curentCellCeneter = Grid.GetCellCenter(newPos);

                TilePositionChanged.Invoke();
            }
        }
    }
}
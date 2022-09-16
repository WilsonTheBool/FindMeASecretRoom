using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class GridMap : MonoBehaviour
    {

        public Vector3 cellSize;

        public Vector3 GetCellCenter(Vector2Int cell)
        {
            return transform.position + new Vector3(cell.x * cellSize.x, cell.y * cellSize.y, 0) + cellSize / 2;
        }

        public Vector3 CellToWorld(Vector2Int cell)
        {
            return transform.position + new Vector3(cell.x * cellSize.x, cell.y * cellSize.y, 0) ; 
        }


        public Vector2Int  WorldToCell(Vector3 position)
        {
            Vector3 offset =  position - transform.position;

            return new Vector2Int(Mathf.FloorToInt(offset.x / cellSize.x), Mathf.FloorToInt(offset.y / cellSize.y));
        }

    }
}
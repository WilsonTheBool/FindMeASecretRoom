using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
    public static class TileFigures
    {
        public static Vector2Int[] GetSquare_Filled(int size, Vector2Int centerGlobalPos)
        {
            List<Vector2Int> list = new List<Vector2Int>((size + 1) * (size + 1));

            for(int i = -size; i <= size; i++)
            {
                for(int j = -size; j <= size; j++)
                {
                    list.Add(new Vector2Int(i, j) + centerGlobalPos);
                }
            }

            return list.ToArray();
        }

        public static Vector2Int[] GetCircle(int size, Vector2Int centerGlobalPos)
        {
            List<Vector2Int> list = new List<Vector2Int>((size + 1) * (size + 1));

            for (int i = -size; i <= size; i++)
            {
                for (int j = -size; j <= size; j++)
                {
                    if (i + j == size)
                    {
                        list.Add(new Vector2Int(i, j) + centerGlobalPos);
                    }

                }
            }

            return list.ToArray();
        }

        public static Vector2Int[] GetCircle_Filled(int size, Vector2Int centerGlobalPos)
        {
            List<Vector2Int> list = new List<Vector2Int>((size + 1) * (size + 1));

            for (int i = -size; i <= size; i++)
            {
                for (int j = -size; j <= size; j++)
                {
                    if(i + j <= size)
                    {
                        list.Add(new Vector2Int(i, j) + centerGlobalPos);
                    }
                    
                }
            }

            return list.ToArray();
        }

        public static Vector2Int[] GetCross_Poked(int size, Vector2Int centerGlobalPos)
        {
            List<Vector2Int> list = new List<Vector2Int>((size + 1) * (size + 1));

            for (int i = -size; i <= size; i++)
            {
                for (int j = -size; j <= size; j++)
                {
                    if ((i == 0 || j == 0) && !(i == 0 && j == 0))
                    {
                        list.Add(new Vector2Int(i, j) + centerGlobalPos);
                    }

                }
            }

            return list.ToArray();
        }

        public static Vector2Int[] GetDiagonalCross(int size, Vector2Int centerGlobalPos)
        {
            List<Vector2Int> list = new List<Vector2Int>((size + 1) * (size + 1));

            for (int i = -size; i <= size; i++)
            {
                for (int j = -size; j <= size; j++)
                {
                    if (i == j)
                    {
                        list.Add(new Vector2Int(i, j) + centerGlobalPos);
                    }

                }
            }

            return list.ToArray();
        }
    }
}

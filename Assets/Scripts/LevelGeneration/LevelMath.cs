using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public static class LevelMath
    {
        public static bool IsInRange(Vector2Int position, Vector2Int minBounds, Vector2Int maxBounds)
        {
            return position.x >= minBounds.x && position.y >= minBounds.y && position.x < maxBounds.x && position.y < maxBounds.y;
        }

        public static bool IsInRange(Vector2Int position, Array array)
        {
            return position.x >= 0 && position.y >= 0 && position.x < array.GetLength(0) && position.y < array.GetLength(1);
        }

        public static bool IsInRange(Vector2Int position, int MaxX, int MaxY)
        {
            return position.x >= 0 && position.y >= 0 && position.x < MaxX && position.y < MaxY;
        }

        public static void ShuffleArray<T>(in T[] array)
        {
            int curentIndex = array.Length - 1;
            int randomIndex;

            while(curentIndex > 0)
            {
                randomIndex = UnityEngine.Random.Range(0, curentIndex);

                //Tuple
                (array[curentIndex], array[randomIndex]) = (array[randomIndex], array[curentIndex]);
               
                curentIndex--;
            }

            return;
        }

        public static T[] ShuffleArrayCreateNew<T>(in T[] inArray)
        {
            T[] array = new T[inArray.Length];

            inArray.CopyTo(array, 0);

            int curentIndex = array.Length - 1;
            int randomIndex;

            while (curentIndex > 0)
            {
                randomIndex = UnityEngine.Random.Range(0, curentIndex);

                //Tuple
                (array[curentIndex], array[randomIndex]) = (array[randomIndex], array[curentIndex]);

                curentIndex--;
            }

            return array;
        }
    }
}

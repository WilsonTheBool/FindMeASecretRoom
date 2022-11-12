using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class MainMenuBackgroundController: MonoBehaviour
    {
        [SerializeField]
        Transform[] spawnPositions;

        [SerializeField]
        Transform[] startPositions;

        private List<Transform> tempPositions;

        [SerializeField]
        FloatingLevel[] gmLevels;

        [SerializeField]
        FloatInDirection Floater;

        [SerializeField]
        FloatLayer[] floatingLevels;

        public float tryToGenDelay;
        private float delay;
        public int spawnCountOnStart;
        public int minPositionsCountToRefresh;

        bool isStart = true;

        private void Start()
        {
            isStart = true;
            tempPositions = new List<Transform>(startPositions);
            for(int i = 0; i < spawnCountOnStart; i++)
            {
                TrySpawnLevel();
            }
            isStart = false;
        }

        private void SpawnLevel(FloatLayer layer)
        {
            FloatInDirection parent = Instantiate(Floater);

            parent.direction = layer.direction;
            parent.speed = layer.speed;
            parent.SetTimer(layer.destroyDelay);
            parent.transform.localScale = layer.size;

            var level = Instantiate(gmLevels[Random.Range(0, gmLevels.Length)], GetSpawnPosition(), Quaternion.Euler(0,0,0), parent.transform);
            level.ChangeColor(layer.color);
            level.ChangeOrderInLayer(layer.orderLayer);
        }

        private Vector3 GetSpawnPosition()
        {
            if (isStart)
            {
                return tempPositions[Random.Range(0, tempPositions.Count)].position;
            }

            if(tempPositions.Count <= minPositionsCountToRefresh)
            {
                tempPositions.Clear();
                tempPositions.AddRange(spawnPositions);
            }

            int index = Random.Range(0, tempPositions.Count);
            var pos =  tempPositions[index].position;
            tempPositions.RemoveAt(index);

            return pos;
        }

        private void TrySpawnLevel()
        {
            float ran = Random.Range(0f, 1f);
            foreach(FloatLayer layer in floatingLevels)
            {
                if(ran <= layer.appearValue)
                {
                    SpawnLevel(layer);
                    return;
                }
            }
        }


        private void FixedUpdate()
        {
            if(delay > 0f)
            {
                delay -= Time.fixedDeltaTime;
            }
            else
            {
                TrySpawnLevel();
                delay = tryToGenDelay;
            }
        }

        [System.Serializable]
        public struct FloatLayer
        {
            public Vector3 direction;
            public int orderLayer;
            public float speed;
            public float destroyDelay;
            public Vector3 size;
            public Color color;

            [Range(0f, 1f)]
            public float appearValue;
        }
    }
}

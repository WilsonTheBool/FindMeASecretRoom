using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.SoundManagment
{
    [CreateAssetMenu(menuName = "Sound/SoundControllerData")]
    public class SoundControllerData : ScriptableObject
    {
        public GameAudioData bombExplosion;

        public GameAudioData playerHit;

        public GameAudioData coinsPickUp;

        public GameAudioData unlockSecretRoom;

        public GameAudioData breakWall;

        public GameAudioData player_SwitchItem;

        [System.Serializable]
        public class GameAudioData
        {
            public string dataName;

            public AudioClip[] clips;

            [Range(0f, 1f)]
            public float volumeScale = 1;

            public AudioClip GetRandom()
            {
                if(clips != null && clips.Length > 0)
                {
                    return clips[Random.Range(0, clips.Length)];
                }
                else
                {
                    return null;
                }

            }
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    [CreateAssetMenu(menuName = "LevelGeneration/Room_Type")]
    public class RoomType : ScriptableObject
    {
        public int id;
        public string typeName;
        public Sprite icon;

        public bool HasSpecialFigure;

        public bool isSecretRoom;

        public SecretRoomGenerationRule rule;

        public bool overridesColor;

        public Color colorOfBase;

        public bool customRenderObject;

        public Room_GM renderObject;
    }
}
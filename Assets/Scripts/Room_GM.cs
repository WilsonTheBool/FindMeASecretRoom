using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Room_GM : MonoBehaviour
    {

        public SpriteRenderer baseRenderer;

        public SpriteRenderer iconRenderer;

       public void SetColor_Base(Color color)
        {
            baseRenderer.color = color;
        }

        public void SetIcon(Sprite icon)
        {
            iconRenderer.sprite = icon;
        }

        public void SetBase(Sprite sprite)
        {
            baseRenderer.sprite = sprite;
        }
    }
}
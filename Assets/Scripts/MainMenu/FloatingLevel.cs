using System.Collections;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class FloatingLevel : MonoBehaviour
    {
        public Room_GM_TypeHolder[] rooms;

        public void ChangeColor(Color dif)
        {
            foreach (Room_GM_TypeHolder room in rooms)
            {
                room.baseRenderer.color -= dif;
                room.iconRenderer.color -= dif;

            }
        }

        public void ChangeOrderInLayer(int dif)
        {
            foreach(Room_GM_TypeHolder room in rooms)
            {
                room.baseRenderer.sortingOrder -= dif;
                room.iconRenderer.sortingOrder -= dif;
            }
        }
    }
}
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.ErrorRoom
{
    public class ErrorRoom_GM : Room_GM
    {
        TMPro.TMP_Text icon_text;

        [SerializeField]
        TMPro.TMP_Text icon_text_prefab;

        [SerializeField]
        Sprite[] possibleSprites;

        [SerializeField]
        Sprite[] possibleIcons;

        [SerializeField]
        float iconUpdateDelaySeconds;

        float curentDelay;

        private void Start()
        {
        }

        private void UpdateIcon()
        {
            if(possibleSprites.Length > 0)
            baseRenderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];

            if (possibleIcons.Length > 0)
                iconRenderer.sprite = possibleIcons[Random.Range(0, possibleIcons.Length)];
        }

        private void Update()
        {

            if (curentDelay <= 0)
            {
                curentDelay = iconUpdateDelaySeconds;

                UpdateIcon();
            }
            else
            {
                curentDelay -= Time.deltaTime;
            }
        }
    }
}
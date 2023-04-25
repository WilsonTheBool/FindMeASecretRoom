using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImageSprite : MonoBehaviour
{
    public Image image;

    public Sprite[] sprites;
    private int index = 0;

    public float swapDelay;
    private float curent = 0;

    private void Start()
    {
        StartCoroutine(MainCo());
    }

    private IEnumerator MainCo()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(swapDelay);

            index++;

            if(index == sprites.Length)
            {
                index = 0;
            }

            image.sprite = sprites[index];
        }
        
    }
}

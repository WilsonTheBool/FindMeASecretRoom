using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class WorldCanvasController : MonoBehaviour
{
   public static WorldCanvasController Instance { get; private set; }

    public Canvas canvas;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            canvas = GetComponent<Canvas>();
        }
        else
        {
            Destroy(this);
        }
    }


    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}

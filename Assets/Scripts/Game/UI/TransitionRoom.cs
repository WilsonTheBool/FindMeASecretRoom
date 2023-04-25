using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionRoom : MonoBehaviour
{
    public Type type;

    public Image room;
    public Image bridge;

    public void SetColor(Color color)
    {
        if(room != null)
        {
            room.color = color;
        }

        if(bridge != null)
        {
            bridge.color = color;
        }
    }

    public enum Type
    {
        regular, treasure, shop,
    }

    public bool IsType(Type type)
    {
        return this.type == type;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionRoom : MonoBehaviour
{
    public Type type;

    public enum Type
    {
        regular, treasure, shop,
    }

    public bool IsType(Type type)
    {
        return this.type == type;
    }
}

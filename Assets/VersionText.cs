using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    private void Start()
    {
        TMPro.TMP_Text text = GetComponent<TMPro.TMP_Text>();
        text.text = Application.version;
    }
}

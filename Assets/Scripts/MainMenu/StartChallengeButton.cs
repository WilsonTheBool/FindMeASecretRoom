using Assets.Scripts.Challenges;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartChallengeButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ChallengeRunData ChallengeRunData;

    public UnityEvent<StartChallengeButton> OnClick;

    public Button Button;

    public TMPro.TMP_Text cahllengeName;
    public TMPro.TMP_Text description;

    public Color green;
    public Color red;
    public Color normal;

    private void Awake()
    {
        if(Button == null)
        Button = GetComponent<Button>();

        Button.onClick.AddListener(() => OnClick.Invoke(this));
    }

    public void SetUP(ChallengeRunData data, TMPro.TMP_Text desc, bool isCompleted, bool isUnlocked)
    {
        ChallengeRunData = data;

        if (isUnlocked)
        {
            cahllengeName.text = data.Name;
            description = desc;

            if (isCompleted)
            {
                cahllengeName.color = green;
            }
            else
            {
                cahllengeName.color = normal;
            }

            //EnableButton();
        }
        else
        {
            cahllengeName.text = "Locked";
            cahllengeName.color = red;
            DisableButton();
        }

    }

    public void DisableButton()
    {
        Button.interactable = false;
    }

    public void EnableButton()
    {
        Button.interactable = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (description != null)
            description.text = ChallengeRunData.description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(description != null)
        description.text = ChallengeRunData.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (description != null)
            description.text = "";
    }
}

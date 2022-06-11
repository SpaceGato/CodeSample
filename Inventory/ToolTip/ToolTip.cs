using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [System.NonSerialized]
    public string title, description;
    private float waitTime = 0.25f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        ToolTipManager.OnMouseExit();
    }
    private IEnumerator StartTimer()
    {
        FindObjectOfType<AudioManager>().Play("ButtonHover");
        yield return new WaitForSecondsRealtime(waitTime);
        ShowMessage();
    }

    private void ShowMessage()
    {
        ToolTipManager.OnMouseEnter(title, description);
    }
}

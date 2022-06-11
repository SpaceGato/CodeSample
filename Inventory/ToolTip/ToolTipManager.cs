using System;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public RectTransform tipPanel;
    public TextMeshProUGUI titleText, descText;

    public static Action<string, string> OnMouseEnter;
    public static Action OnMouseExit;

    Vector2 mousePos, pivotPoint;

    private void OnEnable()
    {
        OnMouseEnter += ShowTip;
        OnMouseExit += HideTip;
    }
    private void OnDisable()
    {
        OnMouseEnter -= ShowTip;
        OnMouseExit -= HideTip;
    }

    private void Start()
    {
        HideTip();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (mousePos.x > 0.5f)
            pivotPoint.x = 1.1f;
        else
            pivotPoint.x = -0.1f;
        if (mousePos.y > 0.58f)
            pivotPoint.y = 1f;
        else
            pivotPoint.y = 0f;

        tipPanel.pivot = pivotPoint;
        tipPanel.position = Input.mousePosition;
    }

    private void ShowTip(string title, string description)
    {
        titleText.text = title;
        descText.text = description;
        tipPanel.sizeDelta = new Vector2(125, descText.preferredHeight > 115 ? 185 : descText.preferredHeight + 70);
        tipPanel.gameObject.SetActive(true);
    }

    public void HideTip()
    {
        titleText.text = string.Empty;
        descText.text = string.Empty;
        tipPanel.gameObject.SetActive(false);
    }
}

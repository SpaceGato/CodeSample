using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    private int currentValue = 0, previousValue = 0;
    public int CurrentCredits
    {
        get
        {
            return currentValue;
        }
        set
        {
            StopAllCoroutines();
            previousValue = currentValue;
            currentValue = value;
            StartCoroutine(UpdateText(value));
        }
    }

    private float numberTick = 0.01f;
    public TextMeshProUGUI creditsText;
    GameManager gameManager;
    AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        gameManager = GetComponent<GameManager>();
        currentValue = gameManager.saveData.credits;
        creditsText.text = "<sprite=0>" + CurrentCredits.ToString("n0");
    }

    private IEnumerator UpdateText(int newValue)
    {
        int i;
        if (newValue < previousValue)
            i = -10;
        else
            i = 10;
        audioManager.Play("CoinFX");
        while (previousValue != newValue)
        {
            previousValue += i;
            creditsText.text = "<sprite=0>" + previousValue.ToString("n0");            
            yield return new WaitForSecondsRealtime(numberTick);
        }
        creditsText.text = "<sprite=0>" + newValue.ToString("n0");
    }
}

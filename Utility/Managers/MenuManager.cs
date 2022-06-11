using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenu, optionsMenu, gameOverMenu, inventoryMenu, shopPanel, shopMenu, toolTip;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void Pause()
    {
        GameManager.gameState = GameManager.State.Paused;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void Resume()
    {
        GameManager.gameState = GameManager.State.Play;
        Time.timeScale = 1f;
        CloseAllMenus();
    }

    public void CloseAllMenus()
    {
        toolTip.GetComponent<ToolTipManager>().HideTip();
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        shopMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    public void OpenShopMenu()
    {
        GameManager.gameState = GameManager.State.Shop;
        Time.timeScale = 0f;
        gameManager.audioManager.Play("ShopTrack");
        pauseMenu.SetActive(true);
        shopMenu.SetActive(true);
    }

    public void QuitButton()
    {
        Debug.Log("Quitting to Main Menu...");
        gameManager.LoadLevel(0);
    }
}

using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static State gameState;
    public enum State
    {
        None,
        Play,
        Paused,
        Shop,
    }
    public SaveData saveData;
    public TextMeshProUGUI zoneText, levelText;
    public Timer timer;

    [System.NonSerialized] public MoneyManager money;
    [System.NonSerialized] public ScoreManager score;
    [System.NonSerialized] public Inventory inventory;
    [System.NonSerialized] public AudioManager audioManager;
    [System.NonSerialized] public SceneTransition sceneTransition;
    GameOverMenu gameOver;
    MenuManager menu;
    LevelManager level;
    PlayerMovement playerMovement;
    Player player;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        sceneTransition = FindObjectOfType<SceneTransition>();
        level = GameObject.Find("Level").GetComponent<LevelManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        playerMovement = player.GetComponent<PlayerMovement>();
        money = GetComponent<MoneyManager>();
        score = GetComponent<ScoreManager>();
        menu = GetComponent<MenuManager>();
        gameOver = menu.gameOverMenu.GetComponent<GameOverMenu>();
        inventory = menu.inventoryMenu.GetComponent<Inventory>();

        player.Init();
        inventory.Init();
        menu.shopPanel.SetActive(true);

        gameState = State.None;
        Time.timeScale = 1f;

        if (sceneTransition != null)
        {
            sceneTransition.Init();
            sceneTransition.animator.SetTrigger("FadeOut");
        }
    }

    public void SaveData()
    {
        saveData.score = score.CurrentScore;
        saveData.credits = money.CurrentCredits;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        switch (gameState)
        {
            case State.Play:
                menu.Pause();
                break;

            case State.Paused:
                menu.Resume();
                break;

            case State.Shop:
                LoadLevel(2);
                break;

            default:
                break;
        }
    }    

    public void UpdateCredits(int amountToAdd)
    {
        money.CurrentCredits += amountToAdd;
    }
    public void UpdateScore(int amountToAdd)
    {
        score.CurrentScore += amountToAdd * saveData.scoreMultiplier;
    }

    public IEnumerator StartLevel()
    {
        Time.timeScale = 1f;
        playerMovement.StartCoroutine(playerMovement.EnterScreen());
        level.StartCoroutine(level.FirstSpawn());

        while (playerMovement.rb.isKinematic)
            yield return new WaitForFixedUpdate();

        level.StartBossTimer();
        timer.StartTimer();
        gameState = State.Play;
        menu.CloseAllMenus();
    }
    public void LoadLevel(int index)
    {
        audioManager.FadeAll(1, true);
        gameState = State.None;
        sceneTransition.levelToLoad = index;
        sceneTransition.gameObject.SetActive(true);
        sceneTransition.animator.SetTrigger("FadeIn");
    }
    public IEnumerator EndLevel()
    {
        bool finalLevel = false;
        if (level.zoneNum == 5 && level.levelNum == 3)
        {
            finalLevel = true;
            level.hazardManager.ClearHazards();
            audioManager.Play("GameComplete");
        }
        level.levelActive = false;
        timer.StopTimer();
        yield return new WaitForSeconds(1.5f);

        audioManager.FadeLevelMusic(0.5f);
        yield return new WaitForSeconds(0.25f);
        if (!finalLevel)
            audioManager.Play("LevelComplete");
        yield return new WaitForSeconds(0.25f);

        gameState = State.None;
        level.levelCompleteText.SetActive(true);
        level.SpawnPickup(Vector2.zero, true);
        level.hazardManager.ClearHazards();
        yield return new WaitForSeconds(0.5f);

        playerMovement.StartCoroutine(playerMovement.ExitScreen());
        score.StartCoroutine(score.AddBonusScore(timer.elapsedTime, level.zoneNum, level.bossLevel));
        while (playerMovement.rb.isKinematic)
            yield return null;
        timer.StartCoroutine(timer.ResetTimer());
        score.ResetBonusText();
        if (finalLevel)
            while (audioManager.GetAudioSource("GameComplete").isPlaying)
                yield return null;
        else
            while (audioManager.GetAudioSource("LevelComplete").isPlaying)
                yield return null;
        audioManager.FadeAll(0.5f, false);
        audioManager.FadeAll(0.5f, true);

        if (finalLevel)
            GameOver(false);
        else
            menu.OpenShopMenu();
    }

    public void GameOver(bool death)
    {
        gameState = State.None;
        Time.timeScale = 0;
        if (death)
        {
            gameOver.gameOverText.text = "Game Over!";
            audioManager.ChangeTrack("GameLose");
        }
        else
        {
            gameOver.gameOverText.text = "You Win!";
            audioManager.ChangeTrack("GameWin");
        }
        menu.gameOverMenu.SetActive(true);
    }
}
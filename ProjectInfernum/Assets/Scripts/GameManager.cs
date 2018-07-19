using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerCoinPoints = 0;
    public int playerHealth = 3;
    [HideInInspector] public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;
    public static int menuScreenBuildIndex = 0;
    public static int coins;
    string hScore = "HighScore";
    string coinTotal = "CoinTotal";


    void Awake () {

        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        coins = PlayerPrefs.GetInt(coinTotal);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        //InitGame();
        SceneManager.activeSceneChanged += DestroyOnMenuScreen;


    }

    void DestroyOnMenuScreen(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == menuScreenBuildIndex) 
        {
            Destroy(this); 
        }
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        level++;
        InitGame();

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading; 
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }




    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Level " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {

        if (level > PlayerPrefs.GetInt(hScore))
        {
            PlayerPrefs.SetInt(hScore, level);

            Debug.Log(PlayerPrefs.GetInt(hScore).ToString());
        }

        coins += playerCoinPoints;
        PlayerPrefs.SetInt(coinTotal, coins);
        Debug.Log(PlayerPrefs.GetInt(coinTotal).ToString());


        levelText.text = "You reached level " + level + ".\n You earned " + playerCoinPoints + " coins. ";
        levelImage.SetActive(true);
        enabled = false;
        StartCoroutine("MainMenu");
         
    }

    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(5);
            SceneManager.LoadScene("Menu");
        
    }

	

	void Update () {

        if (playersTurn || enemiesMoving || doingSetup)
        {
            return; 
        }

        StartCoroutine(MoveEnemies());
		
	}

    public void AddEnemyToList (Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i <enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;

    }

}

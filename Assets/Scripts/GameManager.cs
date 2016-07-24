using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 0f;
    public float turnDelay = 0f;
    public int playerFoodPoints = 100;
    public static GameManager instance = null;

    public GameObject button;
    [HideInInspector]
    public bool playersTurn = true;
    
    private Text levelText;
    private GameObject levelImage;
    private BoardManager boardScript;
    private int level = 1;
    private bool enemiesMoving;
    private bool doingSetup = true;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }
    
    void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }
    
    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        HideLevelImage();
        boardScript.SetupScene(level);

    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
    
    void Update()
    {
        
    }
    

    public void GameOver()
    {
        levelText.text = "으앙 쥬금";
        levelImage.SetActive(true);
        enabled = false;
        Time.timeScale = 0;
    }




    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
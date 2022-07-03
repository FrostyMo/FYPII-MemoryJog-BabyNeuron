using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameScript : MonoBehaviour
{
    // Static instance of this class
    public static GameScript GS;


    public GameObject endGamePanel;
    public GameObject postCorsiPanel;

    public TMP_InputField post_Corsi;

    // The highest level achieved by this player in any game
    public static int highestLevelOverall;

    // The highest level achieved in the current game
    public static int levelsCompleted;

    // The highest level achieved by any player in any game
    public static int MPHighestLevel;

    // The total score for  this game
    public static int score;

    // The total Tiles for a particular level
    private int totalTiles;

    // Whether the GAME has ended in a win or a loss (NOT A LEVEL)
    private bool gameEnded;

    // "The max amount of path for a game"
    public static int maxLevels = 10;

    // Is the game timed?
    public bool isTimed;

    // Set the baseTime provided each level
    private int baseTime = 5;

    // Set the multiplier to the time in each level
    private int multiplier = 3;

    // Max lives for a particular difficulty
    public static int maxLives;

    // Lives used for a particular game
    public static int livesUsed;

    // The game ID, set by API Manager in InitializeGame()
    public static int gameID;

    // is 2D or 3D?
    public static int dimension;

    

    [Space(5)]


    // These headers will show up in game
    // whenever they are needed
    [Header("The Score texts shown on Screen")]
    [Space(5)]
    public TextMeshProUGUI HL_Text;
    public TextMeshProUGUI MPHL_Text;
    public TextMeshProUGUI points_Text;
    public TextMeshProUGUI TotalTiles_Text;
    public TextMeshProUGUI livesLeft_Text;


    // Total Time starting each level
    private static float totalTime = 0;

    private void Awake()
    {

        if (GS != null)
            GameObject.Destroy(GS);
        else
            GS = this;

        //DontDestroyOnLoad(this);
    }


    // Start is called before the first frame update
    void Start()
    {
       
        //MPHighestLevel = 0;
        
        totalTiles = 0;


        // All scores are empty in the beginning
        HL_Text.text = "";
        MPHL_Text.text = "";
        points_Text.text = "";
        TotalTiles_Text.text = "";
        livesLeft_Text.text = "";
        
        gameEnded = false;
    }

    // Getter setters for private variables we created
    public int HL
    {
        get { return highestLevelOverall; }
        set { highestLevelOverall = value; }
    }
    public int MP_HL
    {
        get { return MPHighestLevel; }
        set { MPHighestLevel = value; }
    }
    public int SCORE
    {
        get { return score; }
        set { score = value; }
    }
    public int TOTAL_TILES
    {
        get { return totalTiles; }
        set { totalTiles = value; }
    }

    public bool GAME_END
    {
        get { return gameEnded; }
        set { gameEnded = value; }
    }

    // Set the text to current highestleveloverall for the user
    public void updateHL_Text()
    {
        HL_Text.text = $"Highest Level\n{highestLevelOverall}";
    }

    // Set the text to current highest level out of all users
    public void updateMPHL_Text()
    {
        MPHL_Text.text = $"MP Highest Level\n{MPHighestLevel}";
    }

    // Set the text to current points of user
    public void updatepoints_Text()
    {
        points_Text.text = $"Points: {score}";
    }

    // Set the text to current correct tiles stepped-on of user
    public void updateTotalTiles_Text()
    {
        TotalTiles_Text.text = $"Total Tiles: {totalTiles}";
    }

    // Set the text to current lives left of user
    public void updateLivesLeft_Text()
    {
        livesLeft_Text.text = $"Lives: {maxLives - livesUsed}";
    }


    // Empty out all the text fields
    public void DisableAll()
    {
        HL_Text.text = "";
        MPHL_Text.text = "";
        points_Text.text = "";
        TotalTiles_Text.text = "";
        livesLeft_Text.text = "";
    }

    public void updateAllTexts()
    {
        updateHL_Text();
        updateMPHL_Text();
        updatepoints_Text();
        updateTotalTiles_Text();
        updateLivesLeft_Text();
    }

    public void EnableAll()
    { 
        updateAllTexts();
    }

    /// FUNCTION FOR TIMED GAME ONLY
    /// <summary>
    /// Start the time at the beginning of each level
    /// basetime - 5s
    /// totaltime = basetime + multiplier * level
    /// </summary>
    public void startTime()
    {
        TextMeshProUGUI timer = GameObject.Find("/Canvas/Timer").GetComponent<TextMeshProUGUI>();

        int remainTime = baseTime + multiplier * Spawngrid.maxPathSize;
        StartCoroutine(reloadTimer(timer, remainTime));
    }
    IEnumerator reloadTimer(TextMeshProUGUI timerText, float reloadTimeInSeconds)
    {
        float counter = 0;

        while (counter < reloadTimeInSeconds)
        {
            counter += Time.deltaTime;
            timerText.text = (reloadTimeInSeconds - (int)counter).ToString();
            yield return null;
            if (gameEnded)
            {
                break;
            }
        }
        if (!gameEnded)
        {
            score -= 5;
            timerText.text = "";
            resetGame(0);
        }

    }

    // A function that goes side by side with the level and keeps
    // track of the time passed
    //public void totalTimeStart()
    //{
    //    StartCoroutine(LevelTimer());
    //}

    //// Coroutine to help totalTimeStart();
    //IEnumerator LevelTimer()
    //{


    //    while (!gameEnded)
    //    {
    //        totalTime++;
    //        print(totalTime);
    //        yield return new WaitForSeconds(1);
    //    }

    //}


    public void resetGame(int isWin)
    {
        totalTime += (int)Time.timeSinceLevelLoad;
        print(totalTime);

        // If the player won
        // Increment the path size
        if (isWin == 1)
        {
            Spawngrid.maxPathSize++;
            if (Spawngrid.maxPathSize > levelsCompleted)
            {
                levelsCompleted = Spawngrid.maxPathSize-1;
            }
            if (highestLevelOverall < levelsCompleted)
            {
                highestLevelOverall = levelsCompleted;
            }
        }


        // If the player lost
        // Decrement the path size
        else
        {
            livesUsed++;
            updateLivesLeft_Text();
            Spawngrid.maxPathSize--;
        }

        if (Spawngrid.maxPathSize > maxLevels)
        {
            isWin = 1;
        }
        else
        {
            isWin = 0;
        }

        //APIManager.API.updateStats(gameID, livesUsed, score, levelsCompleted, (int)totalTime, isWin);

        // If the player has completed the desired path amount
        // Complete the game
        // Load the GameCompletion Scene
        if (Spawngrid.maxPathSize > maxLevels)
        {
            DisableAll();
            HL_Text.text = "Congratulations, you have completed all the Levels :)";
            //updateHL_Text();
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02F;

            // Load the post corsi screen test

            endGamePanel.SetActive(true);
            resetVariables();

        }
        else if (livesUsed > maxLives)
        {
            DisableAll();
            HL_Text.text = "Sorry, you have lost all Lives :(";
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02F;

            // Load the post corsi screen test
            endGamePanel.SetActive(true);
            resetVariables();
        }

        

        // Reload the level with
        // updated SpawnGrid size
        else
        {

            // If the spawngrid size has zeroes out
            // reset it to 1
            if (Spawngrid.maxPathSize <= 0)
            {
                Spawngrid.maxPathSize = 1;
            }

            
            // Load the current scene again
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // Return to normal time scale (no slo-mo)
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02F;
        }


        
    }

    // When exit button pressed on endGamePanel
    public void showCorsiPanel()
    {
        endGamePanel.SetActive(false);
        postCorsiPanel.SetActive(true);
    }

    public void exitGame()
    {
        
        Debug.Log(post_Corsi.text);
        //APIManager.API.submitPostCorsi(int.Parse(post_Corsi.text), false);
        Application.Quit();
    }

    private void resetVariables()
    {
        highestLevelOverall=0;

    
        levelsCompleted=0;

        // The highest level achieved by any player in any game
        MPHighestLevel=0;

        // The total score for  this game
        score=0;

        // The total Tiles for a particular level
        totalTiles =0;

        // Whether the GAME has ended in a win or a loss (NOT A LEVEL)
        gameEnded = false;

        // "The max amount of path for a game"
        //maxLevels = 1;

        totalTime = 0;

        // Lives used for a particular game
        livesUsed = 0;

        Spawngrid.maxPathSize = 1;

        

        Movement.moves = 0;
        Movement._path_visited = new Dictionary<int, GameObject>();
        Movement.flag = false;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LoadScene : MonoBehaviour
{

    // The panel for main menu screen
    public GameObject mainmenuPanel;

    // The difficulty panel once a mode selected from Main menu
    public GameObject difficultyPanel;

    // The scores panel when Scores selected from Main Menu
    public GameObject scoresPanel;

    // The buttons available (2D, 3D etc)
    public Button[] buttons;

    // Is 2D or 3D selected?
    // set the material accordingly
    public Material selected;
    public Material unselected;

    // These will be the GameObjects for the 2D/3D option you see on main menu
    public GameObject twoD;
    public GameObject threeD;

    // A warning text for something wrong
    public TextMeshProUGUI warning;

    
    // if twoD selected, set this to 0 else 1
    private int gameType;

    // This remains constant throughout one gameplay
    public static bool dimen_2D;
    public static bool dimen_3D;

    // Similar to level loader
    public void Load(string sceneName)
    {
        // Load the scene desired
        SceneManager.LoadScene(sceneName);


    }

    private void Update()
    {
        // just to checked if one of the buttons is selected
        if (EventSystem.current.currentSelectedGameObject == buttons[4] || EventSystem.current.currentSelectedGameObject == buttons[5])
        {
            print("selected");
        }
        
    }

    // A function to facilitate button selection for 2D
    public void select2D()
    {
        dimen_2D = true;
        dimen_3D = false;
        warning.text = "";
    }

    // A function to facilitate button selection for 3D
    public void select3D()
    {
        dimen_3D = true;
        dimen_2D = false;
        warning.text = "";
    }

    // Disable Main menu panel and show all scores
    IEnumerator showScore()
    {
        buttons[1].interactable = false;
        buttons[2].interactable = false;
        buttons[3].interactable = false;
        yield return new WaitForSeconds(0.5f);
        mainmenuPanel.SetActive(false);
        scoresPanel.SetActive(true);
        buttons[1].interactable = true;
        buttons[2].interactable = true;
        buttons[3].interactable = true;
        
    }

    // Hide main menu and show difficulty for mode chosen
    IEnumerator hideMain()
    {
        
        buttons[1].interactable = false;
        buttons[2].interactable = false;
        buttons[3].interactable = false;
        yield return new WaitForSeconds(0.5f);
        mainmenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        buttons[1].interactable = true;
        buttons[2].interactable = true;
        buttons[3].interactable = true;
        gameType = 0;
    }

    // Similar to hideMain() but for timed mode
    IEnumerator hideMainTimed()
    {
        
        buttons[1].interactable = false;
        buttons[2].interactable = false;
        buttons[3].interactable = false;
        yield return new WaitForSeconds(0.5f);
        mainmenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
        buttons[1].interactable = true;
        buttons[2].interactable = true;
        buttons[3].interactable = true;
        gameType = 1;
    }

    // Start a free mode game with all the variables
    // needed to launch
    public void FreeModeGame()
    {
        GameScript.maxLives = 1000;
        GameScript.maxLevels = 15;
        if (gameType == 0)
        {

            if (dimen_2D)
            {
                GameScript.dimension = 0;
                
                //APIManager.API.initialize("Normal", "2D", GameScript.maxLives, "FreeMode");
                Load("TestGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                //APIManager.API.initialize("Normal", "3D", GameScript.maxLives, "FreeMode");
                Load("TestGame");
            }
        }
        else if (gameType == 1)
        {

            if (dimen_2D)
            {
                GameScript.dimension = 0;
                //APIManager.API.initialize("Timed", "2D", GameScript.maxLives, "FreeMode");
                Load("TimedGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                //APIManager.API.initialize("Timed", "3D", GameScript.maxLives, "FreeMode");
                Load("TimedGame");
            }
        }
    }

    // Start an easy game with all the variables
    // needed to launch
    public void EasyGame()
    {
        GameScript.maxLives = 5;
        GameScript.maxLevels = 10;
        if (gameType == 0)
        {
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                //APIManager.API.initialize("Normal", "2D", GameScript.maxLives, "Easy");
                Load("TestGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                //APIManager.API.initialize("Normal", "3D", GameScript.maxLives, "Easy");
                Load("TestGame");
            }
        }
        else if (gameType == 1)
        {
            
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                //APIManager.API.initialize("Timed", "2D", GameScript.maxLives, "Easy");
                Load("TimedGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                //APIManager.API.initialize("Timed", "3D", GameScript.maxLives, "Easy");
                Load("TimedGame");
            }
        }

    }

    // Start a medium difficulty game with all the variables
    // needed to launch
    public void MediumGame()
    {
        GameScript.maxLives = 3;
        GameScript.maxLevels = 10;
        if (gameType == 0)
        {
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                //APIManager.API.initialize("Normal", "2D", GameScript.maxLives, "Medium");
                Load("TestGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                //APIManager.API.initialize("Normal", "3D", GameScript.maxLives, "Medium");
                Load("TestGame");
            }
        }
        else if (gameType == 1)
        {
            
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                //APIManager.API.initialize("Timed", "2D", GameScript.maxLives, "Medium");
                Load("TimedGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                //APIManager.API.initialize("Timed", "3D", GameScript.maxLives, "Medium");
                Load("TimedGame");
            }
        }
    }


    // Start a hard difficulty game with all the variables
    // needed to launch
    public void HardGame()
    {
        GameScript.maxLives = 1;
        GameScript.maxLevels = 10;
        if (gameType == 0)
        {
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                //APIManager.API.initialize("Normal", "2D", GameScript.maxLives, "Hard");
                Load("TestGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                //APIManager.API.initialize("Normal", "3D", GameScript.maxLives, "Hard");
                Load("TestGame");
            }
        }
        else if (gameType == 1)
        {

            if (dimen_2D)
            {
                GameScript.dimension = 0;
                //APIManager.API.initialize("Timed", "2D", GameScript.maxLives, "Hard");
                Load("TimedGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                //APIManager.API.initialize("Timed", "3D", GameScript.maxLives, "Hard");
                Load("TimedGame");
            }
        }
    }


    // If normal game is selected
    // and a dimension is selected as well 
    public void NormalGame()
    {
        if (!dimen_2D && !dimen_3D)
        {
            warning.text = "Please select a dimension";
        }
        else
        {
            StartCoroutine(hideMain());
        }
        
    }

    // If timed game is selected
    // and a dimension is selected as well 
    public void TimedGame()
    {
        if (!dimen_2D && !dimen_3D)
        {
            warning.text = "Please select a dimension";
        }
        else
        {
            StartCoroutine(hideMainTimed());
        }
    }

    public void ScoreBoard()
    {
        StartCoroutine(showScore());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        mainmenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
        scoresPanel.SetActive(false);
        dimen_2D = false;
        dimen_3D = false;
    }


    // Main Hypo : 30 mins
    // Sub hyp 1: They can play as much as they want -
    // Sub hyp 2: Timed objective, will reach higher levels in 2D game more
    // Sub hyp 3: Provided lives, 3D will have greater score

    // Independent Variable : Time
    // Dependent Variable : Working Memory 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LoadScene : MonoBehaviour
{

    public GameObject mainmenuPanel;
    public GameObject difficultyPanel;
    public Button[] buttons;
    public Material selected;
    public Material unselected;
    public GameObject twoD;
    public GameObject threeD;
    public TextMeshProUGUI warning;

    

    private int gameType;
    public static bool dimen_2D;
    public static bool dimen_3D;


    public void Load(string sceneName)
    {
        // Load the scene desired
        SceneManager.LoadScene(sceneName);


    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == buttons[4] || EventSystem.current.currentSelectedGameObject == buttons[5])
        {
            print("selected");
        }
        
    }

    public void select2D()
    {
        dimen_2D = true;
        dimen_3D = false;
        warning.text = "";
    }
    public void select3D()
    {
        dimen_3D = true;
        dimen_2D = false;
        warning.text = "";
    }

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

    public void FreeModeGame()
    {
        GameScript.maxLives = 1000;
        GameScript.maxLevels = 15;
        if (gameType == 0)
        {

            if (dimen_2D)
            {
                GameScript.dimension = 0;
                
                APIManager.API.initialize("Normal", "2D", GameScript.maxLives, "FreeMode");
                Load("TestGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                APIManager.API.initialize("Normal", "3D", GameScript.maxLives, "FreeMode");
                Load("TestGame");
            }
        }
        else if (gameType == 1)
        {

            if (dimen_2D)
            {
                GameScript.dimension = 0;
                APIManager.API.initialize("Timed", "2D", GameScript.maxLives, "FreeMode");
                Load("TimedGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                APIManager.API.initialize("Timed", "3D", GameScript.maxLives, "FreeMode");
                Load("TimedGame");
            }
        }
    }

    public void EasyGame()
    {
        GameScript.maxLives = 5;
        GameScript.maxLevels = 10;
        if (gameType == 0)
        {
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                APIManager.API.initialize("Normal", "2D", GameScript.maxLives, "Easy");
                Load("TestGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                APIManager.API.initialize("Normal", "3D", GameScript.maxLives, "Easy");
                Load("TestGame");
            }
        }
        else if (gameType == 1)
        {
            
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                APIManager.API.initialize("Timed", "2D", GameScript.maxLives, "Easy");
                Load("TimedGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                APIManager.API.initialize("Timed", "3D", GameScript.maxLives, "Easy");
                Load("TimedGame");
            }
        }

    }

    public void MediumGame()
    {
        GameScript.maxLives = 3;
        GameScript.maxLevels = 10;
        if (gameType == 0)
        {
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                APIManager.API.initialize("Normal", "2D", GameScript.maxLives, "Medium");
                Load("TestGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                APIManager.API.initialize("Normal", "3D", GameScript.maxLives, "Medium");
                Load("TestGame");
            }
        }
        else if (gameType == 1)
        {
            
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                APIManager.API.initialize("Timed", "2D", GameScript.maxLives, "Medium");
                Load("TimedGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                APIManager.API.initialize("Timed", "3D", GameScript.maxLives, "Medium");
                Load("TimedGame");
            }
        }
    }

    public void HardGame()
    {
        GameScript.maxLives = 1;
        GameScript.maxLevels = 10;
        if (gameType == 0)
        {
            if (dimen_2D)
            {
                GameScript.dimension = 0;
                APIManager.API.initialize("Normal", "2D", GameScript.maxLives, "Hard");
                Load("TestGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                APIManager.API.initialize("Normal", "3D", GameScript.maxLives, "Hard");
                Load("TestGame");
            }
        }
        else if (gameType == 1)
        {

            if (dimen_2D)
            {
                GameScript.dimension = 0;
                APIManager.API.initialize("Timed", "2D", GameScript.maxLives, "Hard");
                Load("TimedGame2D");
            }
            else
            {
                GameScript.dimension = 1;
                APIManager.API.initialize("Timed", "3D", GameScript.maxLives, "Hard");
                Load("TimedGame");
            }
        }
    }

    
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

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        mainmenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
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

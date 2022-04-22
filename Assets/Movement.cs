using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityChan;

public class Movement : MonoBehaviour
{
    // An array of unity chan prefabs
    public GameObject[] unityChanTypes;

    // The character at hand
    public GameObject unityChan;

    // Number of moves made by unity-chan
    public static int moves = 0;

    // Path Visited so far by unity-chan
    public static Dictionary<int, GameObject> _path_visited;

    // The flag for game played right or wrong
    public static bool flag = false;

    public TextMeshProUGUI endText;

    //private static System.Tuple<int, int, int> unityIndexes;

    private int[] unityArray;

    private void Awake()
    {
        indexUnityChans();
        chooseUnityChan();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Iniatilize everything
        endText.text= "";
        _path_visited = new Dictionary<int, GameObject>();
        moves = 0;
        flag = false;
        
        unityChan.GetComponent<UnityChanControlScriptWithRgidBody>().enabled = false;
        
    }

    private void replaceMesh(int index)
    {
        GameObject new_mesh;
        GameObject mesh_root;

        mesh_root = GameObject.Find("mesh_root");

        new_mesh = unityChanTypes[index];

        // get a reference to the instantiated object:
        GameObject newObj = GameObject.Instantiate(new_mesh) as GameObject;

        // copy parent,pos,rot and set name
        newObj.transform.parent = mesh_root.transform.parent;
        newObj.transform.rotation = mesh_root.transform.rotation;
        newObj.transform.position = mesh_root.transform.position;
        newObj.transform.name = "mesh_root";

        mesh_root.transform.parent = null;
       

        //GameObject.Destroy(mesh_root);

    }


    // Called on Awake()
    // Indexes different skins based on maxLevels for the game
    private void indexUnityChans()
    {
        int goddessUnity;

        int evilUnity;

        int divaUnity;

        print("MAX GAME COUNT: " + GameScript.maxLevels);

        // If max levels allowed >= 10
        // Goddess unity will be at max - 2
        // Evil at max - 4
        // and Diva at max - 6
        if (GameScript.maxLevels >= 10) {
            goddessUnity = GameScript.maxLevels - 2;

            evilUnity = GameScript.maxLevels - 4;

            divaUnity = GameScript.maxLevels - 6;

        }

        // If max levels allowed >= 6
        // Goddess unity will be at max - 1
        // Evil at max - 2
        // and Diva at max - 3
        else if (GameScript.maxLevels >= 6)
        {
            goddessUnity = GameScript.maxLevels - 1;

            evilUnity = GameScript.maxLevels - 2;

            divaUnity = GameScript.maxLevels - 3;
        }

        // else keep the same character
        else
        {
            goddessUnity = -1;

            evilUnity = -1;

            divaUnity = -1;
        }
        unityArray = new int[3];

        unityArray[0] = divaUnity;
        unityArray[1] = evilUnity;
        unityArray[2] = goddessUnity;
        
    }
    // Simple unity : unityChanTypes[0]
    // Evil unity : unityChanTypes[1]
    // Goddess unity : unityChanTypes[2]
    // Diva unity : unityChanTypes[3]
    private void chooseUnityChan()
    {

        // if current level is between Diva and Evil unity's
        // Choose Diva
        if (unityArray[0] <= Spawngrid.maxPathSize && Spawngrid.maxPathSize < unityArray[1])
        {
            
            unityChanTypes[0].SetActive(false);
            unityChanTypes[1].SetActive(false);
            unityChanTypes[2].SetActive(false);
            unityChanTypes[3].SetActive(true);
            unityChan = unityChanTypes[3];
        }

        // if current level is between Evil and Goddess unity's
        // Choose Evil
        else if (unityArray[1] <= Spawngrid.maxPathSize && Spawngrid.maxPathSize < unityArray[2])
        {
            unityChanTypes[0].SetActive(false);
            unityChanTypes[1].SetActive(true);
            unityChanTypes[2].SetActive(false);
            unityChanTypes[3].SetActive(false);
            unityChan = unityChanTypes[1];
        }

        // if current level is greater than goddess unity
        // Choose Goddess
        else if (unityArray[2] <= Spawngrid.maxPathSize)
        {
            unityChanTypes[0].SetActive(false);
            unityChanTypes[1].SetActive(false);
            unityChanTypes[2].SetActive(true);
            unityChanTypes[3].SetActive(false);
            unityChan = unityChanTypes[2];
        }

        // Else just choose the simple unity
        else
        {
            unityChanTypes[0].SetActive(true);
            unityChanTypes[1].SetActive(false);
            unityChanTypes[2].SetActive(false);
            unityChanTypes[3].SetActive(false);
            unityChan = unityChanTypes[0];
        }

        
    }

    // Enable unity chan's movement
    public void enableUnityChan()
    {
        unityChan.GetComponent<UnityChanControlScriptWithRgidBody>().enabled = true;
        
    }

    // Disable unity chan's movement
    public void disableUnityChan()
    {
        unityChan.GetComponent<UnityChanControlScriptWithRgidBody>().enabled = false;
    }

    
    // isWin
    // 0 : lose
    // 1 : win
    public void endGame(int isWin)
    {
        GameScript.GS.GAME_END = true;
        StartCoroutine(GameEnd(isWin));
    }



    // The function to end the game
    private IEnumerator GameEnd(int isWin)
    {
        // Enable the text to end
        //endText.enabled = true;

        // Start the slow mo effect
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

        // The seconds on text to show
        for (int i = 3; i >= 0; i--)
        {
            endText.text = $"Ending in {i}...";
            yield return new WaitForSeconds(1);
        }

        // Stop the time
        //Time.timeScale = 0;
        disableUnityChan();

        if (GameScript.dimension == 0)
        {
            userMovement.enabled = false;
        }
        

        // Disable the text
        endText.text = "";

        // Call the reset according to win or loss
        GameScript.GS.resetGame(isWin);

    }


}

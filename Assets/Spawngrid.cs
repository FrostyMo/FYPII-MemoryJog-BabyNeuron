using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawngrid : MonoBehaviour
{

    private Movement movement;

    // The prefabs of cubes
    public GameObject [] itemsToPickFrom;

    // The amount of tiles to be highlighted at the beginning
    public static int maxPathSize = 1;

    // Number of columns
    public int gridX;

    // Number of rows
    public int gridZ;

    // Padding for tiles
    public float gridOffsetSpacing=2f;

    // Padding for border
    public float gridOffsetSpacingBorder;

    // Initial place of all the stuff
    public Vector3 gridOrigin = Vector3.zero;
    
    // Save all the path tiles only
    private static Dictionary<int, GameObject> _path;

    // Save all the times with their names
    private Dictionary<string, GameObject> _tiles;

    // The yellow material when correct tile traversed
    public Material Material1;

    // The original material of tiles
    public Material mOriginal;

    // The material of border tiles
    public Material borderMat;

    // The z point to begin initial stage from
    private int borderPlatformCount = -5;

    // Little radar on top right at the beginning of game
    public Camera topViewCamera;

    // Start timer in the beginning
    public TextMeshProUGUI startTimer;

    // When to start the game
    public static bool gameStarted;

    

    // Start is called before the first frame update
    void Start()
    {
        movement = FindObjectOfType<Movement>();
        startTimer.text = "Starting in 3...";
        _path = new Dictionary<int, GameObject>();
        _tiles = new Dictionary<string, GameObject>();
        gameStarted = false;
        SpawnGrid();
    }

    // GETTER SETTER FOR _path
    public static Dictionary<int, GameObject> _PATH
    {
        get { return _path; }
        private set { _path = value; }
    }

    // A boundary that scales either
    // x axis
    // OR
    // z axis
    void drawBoundary(float x, float y, float z, int whatToScale)
    {
        if (GameScript.dimension == 0)
        {

            Vector3 spawnPosition = new Vector3(x, 1, z) + gridOrigin;
            GameObject clone = Instantiate(itemsToPickFrom[2], spawnPosition, Quaternion.identity);
            clone.name = $"boundary {1}";
            clone.GetComponent<MeshRenderer>().material = borderMat;


            // scale X
            if (whatToScale == 0)
            {
                clone.transform.localScale = new Vector3(gridX * gridOffsetSpacing, 0.5f, 1);
            }
            // scale z
            else
            {
                clone.transform.localScale = new Vector3(1, 0.5f, gridZ * gridOffsetSpacing);
            }
            
        }
        else
        {
            for (int i = 1; i <= y; i++)
            {
                Vector3 spawnPosition = new Vector3(x, i, z) + gridOrigin;
                GameObject clone = Instantiate(itemsToPickFrom[2], spawnPosition, Quaternion.identity);
                clone.name = $"boundary {i}";
                clone.GetComponent<MeshRenderer>().material = borderMat;


                // scale X
                if (whatToScale == 0)
                {
                    clone.transform.localScale = new Vector3(gridX * gridOffsetSpacing, 0.5f, 1);
                }
                // scale z
                else
                {
                    clone.transform.localScale = new Vector3(1, 0.5f, gridZ * gridOffsetSpacing);
                }
            }
        }
        

        

    }

    // Draw a cube at x and z
    // Keep y = 0
    void drawCube(int x, int z)
    {
        
        Vector3 spawnPosition = new Vector3(x, 0.7f, z) + gridOrigin;
        GameObject clone = Instantiate(itemsToPickFrom[2], spawnPosition, Quaternion.identity);
        clone.name = $"boundary {x} {0} {z}";
        // clone.tag = $"tile {x} {i} {z}";
        clone.GetComponent<MeshRenderer>().material = borderMat;

    }

    // Takes x and y positions
    // Runs y times in a loop for boundary
    // If y == 0, it is a normal Tile
    void drawCube(int x, int y, int z){
        if (y == 0){
            Vector3 spawnPosition=new Vector3(x*gridOffsetSpacing, 0 ,z*gridOffsetSpacing) +gridOrigin;
            GameObject clone=Instantiate(itemsToPickFrom[0],spawnPosition,Quaternion.identity);
            clone.name = $"tile {x} {0} {z}";
            _tiles.Add(clone.name, clone);
            // clone.tag = $"tile {x} {0} {z}";
            return;
        }
        for (int i=1; i<=y; i++){
            Vector3 spawnPosition=new Vector3(x* gridOffsetSpacingBorder, i ,z* gridOffsetSpacingBorder) +gridOrigin;
            GameObject clone=Instantiate(itemsToPickFrom[2],spawnPosition,Quaternion.identity);
            clone.name = $"boundary {x} {i} {z}";
            clone.GetComponent<MeshRenderer>().material = borderMat;
            // clone.tag = $"tile {x} {i} {z}";
        }
    }

    void SpawnGrid(){

        // Draw the boundary
        for (int x = (int)((gridX*gridOffsetSpacing)/3); x < (int) 2*((gridX * gridOffsetSpacing )/ 3); x++)
        {
            for (int z = borderPlatformCount; z<-2; z++)
            {
                drawCube(x, z);
            }
            
        }
        
        // Draw the original tiles
        for (int x = 0; x < gridX; x++)
        {
            for(int z=0;z<gridZ;z++){

                drawCube(x, 0, z);
                
            }
        }

        // Draw the left column boundary
        drawBoundary(-2, 3, (gridZ / 2) * gridOffsetSpacing, 1);

        // Draw the front row boundary
        drawBoundary((gridX / 2) * gridOffsetSpacing, 3, gridZ * gridOffsetSpacing, 0);

        // Draw the right column boundary
        drawBoundary(gridX * gridOffsetSpacing + 1, 3, (gridZ / 2) * gridOffsetSpacing, 1);

        // Generate a path and save it in _path
        pathGenerator();

        // Start the game and highlight the path
        StartCoroutine(wait(3));
        
        
    }



    /// <summary>
    /// Check the current position and see if
    /// going up is possible
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>A boolean to tell if possible or not</returns>
    private bool upPossible(Vector3 pos)
    {

        if (_tiles.ContainsKey($"tile {pos.x} {0} {pos.z + 1}"))
        {
            if (!_path.ContainsValue(_tiles[$"tile {pos.x} {0} {pos.z + 1}"]))
            {
                return true;
            }
        }

        return false;

    }

    /// <summary>
    /// Check the current position and see if
    /// going down is possible
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>A boolean to tell if possible or not</returns>
    private bool downPossible(Vector3 pos)
    {
        if (_tiles.ContainsKey($"tile {pos.x} {0} {pos.z - 1}"))
        {
            if (!_path.ContainsValue(_tiles[$"tile {pos.x} {0} {pos.z - 1}"]))
            {
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// Check the current position and see if
    /// going left is possible
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>A boolean to tell if possible or not</returns>
    private bool leftPossible(Vector3 pos)
    {
        if (_tiles.ContainsKey($"tile {pos.x - 1} {0} {pos.z}"))
        {
            if (!_path.ContainsValue(_tiles[$"tile {pos.x - 1} {0} {pos.z}"]))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check the current position and see if
    /// going right is possible
    /// </summary>
    /// <param name="pos"></param>
    /// <returns>A boolean to tell if possible or not</returns>
    private bool rightPossible(Vector3 pos)
    {
        if (_tiles.ContainsKey($"tile {pos.x+1} {0} {pos.z}"))
        {
            if (!_path.ContainsValue(_tiles[$"tile {pos.x+1} {0} {pos.z}"]))
            {
                return true;
            }
        }

        return false;
    }

    // The algorithm to generate our path
    // given the parameter {maxPathSize}
    // Refer to the bottom for the procedural explanation
    public void pathGenerator()
    {
        int val;
        Vector3 pos = gridOrigin;
        _path = new Dictionary<int, GameObject>();
        
        int index = 0;

        int randX = Random.Range((int)gridX / 3, (int)(gridX / 3) * 2);
        int z = 0;
        int y = 0;

        pos = new Vector3(randX, y, z);
        _path[index] = _tiles[$"tile {pos.x} {y} {pos.z}"];
        index++;

        while (index < maxPathSize)
        {
            val = Random.Range(0, 4);

            // If not path is possible
            // Reset the current path
            // and start again
            if (!upPossible(pos) && !downPossible(pos) && !leftPossible(pos) && !rightPossible(pos))
            {
                index = 0;

                // Reset the path
                _path = new Dictionary<int, GameObject>();

                // Generate a random x around the center
                randX = Random.Range((int)gridX / 3, (int)(gridX / 3) * 2);
                z = 0;
                y = 0;

                // give the new position
                pos = new Vector3(randX, y, z);

                // set it in path
                _path[index] = _tiles[$"tile {pos.x} {y} {pos.z}"];

                index++;
            }


            // check up
            if (val == 0 && pos.z != gridZ - 1)
            {

                if (!_path.ContainsValue(_tiles[$"tile {pos.x} {0} {pos.z + 1}"]))
                {
                    _path[index] = _tiles[$"tile {pos.x} {0} {pos.z + 1}"];
                    pos = new Vector3(pos.x, pos.y, pos.z + 1);
                    index++;
                }

            }

            // Check down
            else if (val == 1 && pos.z != 0)
            {
                if (!_path.ContainsValue(_tiles[$"tile {pos.x} {0} {pos.z - 1}"]))
                {
                    _path[index] = _tiles[$"tile {pos.x} {0} {pos.z - 1}"];
                    pos = new Vector3(pos.x, pos.y, pos.z - 1);
                    index++;
                }
            }

            // Check left
            else if (val == 2 && pos.x != 0)
            {
                if (!_path.ContainsValue(_tiles[$"tile {pos.x-1} {0} {pos.z}"]))
                {
                    _path[index] = _tiles[$"tile {pos.x-1} {0} {pos.z}"];
                    pos = new Vector3(pos.x-1, pos.y, pos.z);
                    index++;
                }
            }

            // Check right
            else if (val == 3 && pos.x != gridX - 1)
            {
                if (!_path.ContainsValue(_tiles[$"tile {pos.x + 1} {0} {pos.z}"]))
                {
                    _path[index] = _tiles[$"tile {pos.x + 1} {0} {pos.z}"];
                    pos = new Vector3(pos.x + 1, pos.y, pos.z);
                    index++;
                }
            }

        }

    }

    
    // A function before starting the game
    // To run the game with start Text
    IEnumerator wait(int time)
    {
        for (int i = time; i >= 0; i--)
        {

            startTimer.text = $"Starting in {i}...";
            yield return new WaitForSeconds(1);
        }

        // Disable the start text
        startTimer.text = "";

        // Enable the topview camera for highlight period
        topViewCamera.enabled = true;
        //GameScript.GS.TotalTiles_Text.enabled = true;
        //GameScript.GS.points_Text.enabled = true;
        GameScript.GS.updateTotalTiles_Text();
        GameScript.GS.updatepoints_Text();
        GameScript.GS.updateLivesLeft_Text();

        yield return new WaitForSeconds(2);

        // Start highlighting the path
        highlightPath();
    }


    // Loop the path and start a coroutine for each tile
    // With a delay
    public void highlightPath()
    {

        for (int i = 0; i < _path.Count; i++)
        {

            StartCoroutine(highlight_tile(_path, i));
            
        }

    }


    // Highlight tile on 's' index
    // and wait for 's' seconds in total
    IEnumerator highlight_tile(Dictionary<int, GameObject> t, int s)
    {

        yield return new WaitForSeconds(s);

        GameScript.GS.TOTAL_TILES = s + 1;
        GameScript.GS.updateTotalTiles_Text();

        // Yellow material
        t[s].GetComponent<MeshRenderer>().material = Material1;

        // If this is second tile or higher
        // Turn the previous to original material
        if (s > 0)
        {
            t[s - 1].GetComponent<MeshRenderer>().material = mOriginal;
        }

        // If this is the last tile, turn it to original after 2 second wait
        if (s == maxPathSize - 1)
        {

            // This static variable will be used to enable
            // Character movement
            gameStarted = true;

            if (GameScript.dimension == 0)
            {
                userMovement.enabled = true;
            }
            else
            {
                movement.enableUnityChan();
            }
            
            

            yield return new WaitForSeconds(2);
            t[s].GetComponent<MeshRenderer>().material = mOriginal;

            // Disable the mini radar on top right
            topViewCamera.enabled = false;
            GameScript.GS.EnableAll();
            //GameScript.GS.totalTimeStart();
            if (GameScript.GS.isTimed)
            {
                GameScript.GS.startTime();
            }
            

        }


    }




    /*
    *************************** ALGO FOR PATH GENERATION ***************************

    Random 0-4
        @ 0 = up
        @ 1 = down
        @ 2 = left
        @ 3 = right

    if ANY move not possible and index < maxPath
        discard path
        index = 0
        path = newPath

    checkUp
        if okay
            move
        else
            randomize again
    checkDown
        if okay
            move
        else
            randomize again
    checkLeft
        if okay
            move
        else
            randomize again
    checkRight
        if okay
            move
        else
            randomize again

    */

}






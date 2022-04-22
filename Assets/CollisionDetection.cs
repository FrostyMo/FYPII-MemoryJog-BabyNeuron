using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    // Check if tile hit
    public bool alreadyHit = false;

    // Hit material should be something different from original
    public Material hitMaterial;

    // If wrong path chosen, tile turns to wrong material
    public Material wrongHit;

    // Number of moves
    public int count;

    // Script variable
    private Spawngrid spawngrid;

    // Movement's Script variable
    private Movement movement;

    // The path chosen to visit by player
    // Stores index as key
    // and the tile as value
    public Dictionary<int, GameObject> _path_visited;

   

    private void Start()
    {

        // Find the script and load it
        spawngrid = FindObjectOfType<Spawngrid>();

        // Find the script and load it
        movement = FindObjectOfType<Movement>();

        // Initialize the path
        _path_visited = new Dictionary<int, GameObject>();
    }

    // If GameEnded : Do not do anything
    // Else
    // - Add the object to visited
    // - Increment number of total moves
    // - Check if moves <= Path Count
    //   - Check if visited = actual
    //      - Highlight Yellow
    //   - Else: Highlight Red and END Game
    // - End game
    private void OnTriggerEnter(Collider other)
    {
        this.GetComponent<Collider>().isTrigger = false;

        if (GameScript.GS.GAME_END == false)
        {
            Debug.Log("hit");
            print(this.transform.localPosition);
            alreadyHit = true;
            
            int ind=Movement.moves;

            Movement._path_visited[ind] = gameObject;
            Movement.moves++;

            // The moves should be less than original Path count
            // or equal at most
            if (Movement.moves <= Spawngrid._PATH.Count)
            {

                // If both equal, highlight yellow
                if (Movement._path_visited[ind] == Spawngrid._PATH[ind])
                {

                    this.GetComponent<Renderer>().material = hitMaterial;

                    // Add score
                    GameScript.GS.SCORE += 5;
                    GameScript.GS.updatepoints_Text();

                    // End game after highlighting the yellow correct visited path
                    if (Movement.moves == Spawngrid._PATH.Count)
                    {
                        movement.endGame(1);
                    }
                }

                // Wrong step taken, turn red
                else
                {

                    print("Total Moves " + Movement.moves);
                    print("Total Path Count " + Spawngrid.maxPathSize);
                    
                    this.GetComponent<Renderer>().material = wrongHit;

                    GameScript.GS.SCORE = GameScript.GS.SCORE - (Spawngrid.maxPathSize - (Movement.moves - 1));
                    GameScript.GS.updatepoints_Text();

                    movement.endGame(0);
                    
                }
            }

            // Definitely can't exceed path so wrong moves onwards
            else
            {
                
                this.GetComponent<Renderer>().material = wrongHit;
                movement.endGame(0);
            }
        }
        
        
    }

}

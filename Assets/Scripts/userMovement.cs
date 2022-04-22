using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userMovement : MonoBehaviour
{


    // The character at hand
    public GameObject unityChan;
    public Vector3 startPosition;
    public Vector3 startRotation;
    private bool _up,_down,_left,_right;
    public static bool enabled= false;
    // Update is called once per frame
    private void Update() {
        if( enabled ){
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            _left = true;
        }
        else
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
            _right= true;
        }
        else
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
            _down = true;
        }
        else
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
            _up = true;
        }
        moveUser();// this function is called to move the user when a button is clicked
        }
    }

 
    public void moveUser(){
        if(_left && unityChan.transform.position.x > 0){

            //change the position of user to the tile
            unityChan.transform.position=new Vector3(unityChan.transform.position.x-1.1f, unityChan.transform.position.y,unityChan.transform.position.z);
            unityChan.transform.eulerAngles = new Vector3(0f, -90f, -90f);
            StartCoroutine(restartPosition());
            _left=false;
            
        }
        else
        if(_right && unityChan.transform.position.x < 8){
            //change the position of user to the tile
            unityChan.transform.position=new Vector3(unityChan.transform.position.x+1.1f, unityChan.transform.position.y,unityChan.transform.position.z);
            unityChan.transform.eulerAngles = new Vector3(-180f, -90f,-90f);
            StartCoroutine(restartPosition());
            _right = false;
        }
        else
        if(_up && unityChan.transform.position.z < 8){

            //change the position of user to the tile
            unityChan.transform.position=new Vector3(unityChan.transform.position.x, unityChan.transform.position.y,unityChan.transform.position.z+1.1f);
            unityChan.transform.eulerAngles = new Vector3(-90f, -90f, -90f);
           
           _up = false;
        }
        else
        if(_down && unityChan.transform.position.z > 0){

            //change the position of user to the tile
            unityChan.transform.position=new Vector3(unityChan.transform.position.x,unityChan.transform.position.y,unityChan.transform.position.z-1.1f);
            unityChan.transform.eulerAngles = new Vector3(-90f, -90f, -90f);

            _down = false;
        }
        
        _left=_right=_up=_down=false;
        
    }


    IEnumerator restartPosition(){
         
        yield return new WaitForSeconds(0.2f);
        unityChan.transform.eulerAngles = new Vector3(-90f, -90f, -90f);

    }

}

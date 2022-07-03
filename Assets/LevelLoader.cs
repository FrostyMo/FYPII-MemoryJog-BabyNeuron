using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// A simple class to load a new scene
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public static LevelLoader instance;


    // Load the next level with the passed string as scene Name
    public void LoadNextLevel(string sceneName)
    {
        //StartCoroutine(LoadLevel(sceneName));
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadLevel(string levelIndex)
    {
        // yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

}

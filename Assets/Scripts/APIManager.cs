using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class APIManager : MonoBehaviour
{
    public static APIManager API;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;
    public TMP_InputField preCorsiRegisterField;
    public TMP_Text warningRegisterText;

    [Header("CorsiScore")]
    public TMP_InputField preCorsiScoreField;
    public TMP_InputField postCorsiScoreField;
    private const string gameType = "3D";

    private double time;

    public static string userName;

    // Make sure not to destory this class's instance
    // It will be used to call APIs later on
    private void Awake()
    {
        if (API != null)
            Destroy(API);
        else
            API = this;
        DontDestroyOnLoad(API);
    }

    // This time will be used to record in
    // all api calls to save any data
    void Start()
    {
        time = Time.realtimeSinceStartup;    
    }

    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        preCorsiScoreField.text = "";
    }

    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        passwordRegisterField.text = "";
        confirmPasswordRegisterField.text = "";
        preCorsiRegisterField.text = "";
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(LoginNow(emailLoginField.text, passwordLoginField.text));
    }


    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(usernameRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text, preCorsiRegisterField.text));
    }

    public void initialize(string gameType, string gameDimension, int totalLives, string gameDifficulty)
    {
        StartCoroutine(InitializeGame(gameType, gameDimension, totalLives, gameDifficulty));
    }

    // Thread the initializing game API so it loads all data while game is launching
    IEnumerator InitializeGame(string gameType, string gameDimension, int totalLives, string gameDifficulty)
    {
        Debug.Log("Before1");
        using (UnityWebRequest www = UnityWebRequest
            .Get($"https://smd-a4.000webhostapp.com/initializeGame.php?mUserName={userName}&gameType={gameType}&gameDimension={gameDimension}&totalLives={totalLives}&gameDifficulty={gameDifficulty}"))
        {
            yield return www.SendWebRequest();
            Debug.Log("After1");

            // Display errors if any
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                
                Debug.Log(www.error);
            }

            // If all goes well, save the user name
            else
            {
                Debug.Log(www.downloadHandler.text);
                JSONNode jsonObject = JSON.Parse(www.downloadHandler.text);

                Debug.Log("IN ELSE");
                if (traverseJSON(www.downloadHandler.text))
                {
                    
                    Debug.Log("Before2");
                    GameScript.gameID = jsonObject.GetValueOrDefault("msg", null);
                    GameScript.highestLevelOverall = jsonObject.GetValueOrDefault("HL", null);
                    GameScript.MPHighestLevel = jsonObject.GetValueOrDefault("MHL", null);
                    Debug.Log("After2");
                }
                else
                {
                    Debug.Log("IN ELSE ELSE");
                    Debug.Log("Game not initiailized");
                    jsonObject.GetValueOrDefault("q1", null);
                }
            }
        }

    }

    // Update all the  user stats after each level ends
    public void updateStats(int gameID, int livesUsed, int totalScore, int levelsCompleted, int totalTime, int isWin)
    {
        string gameDimension;
        if (GameScript.dimension == 0)
        {
            gameDimension = "2D";
        }
        else
        {
            gameDimension = "3D";
        }
        StartCoroutine(UpdateLevel(gameID, livesUsed, totalScore, levelsCompleted, totalTime, isWin, gameDimension));
    }

    IEnumerator UpdateLevel(int gameID, int livesUsed, int totalScore, int levelsCompleted, int totalTime, int isWin, string gameDimension)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"https://smd-a4.000webhostapp.com/updateLevels2.php?gameID={gameID}&livesUsed={livesUsed}&totalScore={totalScore}&levelsCompleted={levelsCompleted}&totalTime={totalTime}&isWin={isWin}&gameDimension={gameDimension}"))
        {
            yield return www.SendWebRequest();

            // Display errors if any
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }

            // If all goes well, save the user name
            else
            {
                Debug.Log(www.downloadHandler.text);
                JSONNode jsonObject = JSON.Parse(www.downloadHandler.text);
                if (traverseJSON(www.downloadHandler.text))
                {
                    if (GameScript.highestLevelOverall > jsonObject.GetValueOrDefault("MHL", null))
                    {
                        GameScript.MPHighestLevel = GameScript.highestLevelOverall;
                    }
                    else{
                        GameScript.MPHighestLevel = jsonObject.GetValueOrDefault("MHL", null);
                    }
                    
                    Debug.Log("SUCESS");
                }
                
            }
        }
    }

    bool canQuit = false;

    public void submitPostCorsi(int post_corsi, bool isPre)
    {
        
        StartCoroutine(SubmitCorsi(post_corsi, isPre));
        //Application.wantsToQuit += WantsToQuit;
    }

    bool WantsToQuit()
    {
        return canQuit;
    }

    IEnumerator SubmitCorsi(int score, bool isPre)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"https://smd-a4.000webhostapp.com/updateCorsi.php?mUserName={userName}&postCorsi={score}"))
        {
            yield return www.SendWebRequest();

            // Display errors if any
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }

            // If all goes well, save the user name
            else
            {
                Debug.Log(www.downloadHandler.text);
                JSONNode jsonObject = JSON.Parse(www.downloadHandler.text);


                if (traverseJSON(www.downloadHandler.text))
                {
                    //canQuit = true;
                    Application.Quit();
                }
                else
                {
                    Debug.Log("Unsuccessful");
                }
            }
        }
    }

    

    IEnumerator LoginNow(string _user, string _pass)
    {
        if (_user == "")
        {
            warningLoginText.text = "Missing Username";
        }
        else if (_pass == "")
        {
            warningLoginText.text = "Missing Password";
        }
        else
        {
            using (UnityWebRequest www = UnityWebRequest.Get($"https://smd-a4.000webhostapp.com/login.php?mUserName={_user}&mPassword={_pass}"))
            {
                yield return www.SendWebRequest();

                // Display errors if any
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    warningRegisterText.text = www.error;
                }

                // If all goes well, save the user name
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    JSONNode jsonObject = JSON.Parse(www.downloadHandler.text);


                    if (traverseJSON(www.downloadHandler.text))
                    {
                        userName = _user;
                        warningLoginText.text = "";
                        confirmLoginText.text = jsonObject.GetValueOrDefault("msg", null);
                        yield return new WaitForSeconds(2);
                        confirmLoginText.text = "";
                        ClearLoginFields();
                        ClearRegisterFields();
                        SceneManager.LoadScene("MainMenu");
                    }
                    else
                    {
                        warningRegisterText.text = jsonObject.GetValueOrDefault("msg", null);
                    }
                }
            }
        }
    }
    
    IEnumerator Register(string _user, string _pass, string _confirmPass, string _preCorsi)
    {

        // username must not be empty
        if (_user == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
            yield break;
        }
        if (_pass == "" || _confirmPass == "")
        {
            //If the password field is blank show a warning
            warningRegisterText.text = "Missing Password";
            yield break;
        }

        // passwords must match
        if (_pass != _confirmPass)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
            
        }

        // Generate a GET request
        else
        {
            using (UnityWebRequest www = UnityWebRequest.Get($"https://smd-a4.000webhostapp.com/registerUser.php?mUserName={_user}&mPassword={_pass}&preCorsi={_preCorsi}&postCorsi=0")) {
                yield return www.SendWebRequest();

                

                // Display errors if any
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    warningRegisterText.text = www.error;
                }

                // If all goes well, save the user name
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    JSONNode jsonObject = JSON.Parse(www.downloadHandler.text);

                    

                    if (traverseJSON(www.downloadHandler.text))
                    {
                        userName = _user;

                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";

                        ClearLoginFields();

                        emailLoginField.text = userName;
                        passwordLoginField.text = _pass;

                        ClearRegisterFields();
                    }
                    else
                    {
                        warningRegisterText.text = jsonObject.GetValueOrDefault("msg", null);
                    }
                    
                }


            }
        }
       
    }

    private bool traverseJSON(string jsonString)
    {
        Debug.Log("BEFORE JSON OBJ");
        JSONNode jsonObject = JSON.Parse(jsonString);
        Debug.Log("AFTER JSON OBJ");
        foreach (string element in jsonObject.Keys)
        {
            //print(element);
            if (element.Equals("success"))
            {
                if (jsonObject.GetValueOrDefault(element, null) == 1)
                {
                    Debug.Log("RETURN TRUE");
                    return true;
                    
                }
                else
                {
                    return false;
                    
                }
            }
        }
        return false;
    }

    
}

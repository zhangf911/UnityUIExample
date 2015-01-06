using UnityEngine;
using System.Collections;
using System;

public enum GameStates
{
    MainMenu, GamePlay, GameOver, Credits, Settings, Leaderboards
}

public class GameController : MonoBehaviour 
{
    public static GameStates gameState;
    public static float gameScore;
    public static float gameHighScore;
    public static float gameSoundVolume;
    public static bool gameAudioActive;

    public AudioClip audioMenu;
    public int scoreDecimals;

    private GameObject[] go0, go1, go2, go3, go4, go5;
    private GameObject goBack;
    private bool changeState = false;
    private float score;

    void SettingsDefault()
    {
        gameScore = 0.0f;
        gameHighScore = PlayerPrefs.GetFloat("HighScore");
        gameSoundVolume = 0.75f;
        gameAudioActive = true;
    }

	// Use this for initialization
	void Start ()
    {
        SettingsDefault();

        // find all game objects that use specified tag
        go0 = GameObject.FindGameObjectsWithTag("UI_MainMenu");
        go1 = GameObject.FindGameObjectsWithTag("UI_GamePlay");
        go2 = GameObject.FindGameObjectsWithTag("UI_GameOver");
        go3 = GameObject.FindGameObjectsWithTag("UI_Credits");
        go4 = GameObject.FindGameObjectsWithTag("UI_Settings");
        go5 = GameObject.FindGameObjectsWithTag("UI_Leaderboards");
        goBack = GameObject.FindGameObjectWithTag("UI_Back");

        // init our game states
        GameState();
	}
	
	// Update is called once per frame
	void Update () 
    {
        KeysPressed();
        GameScore();

        if (changeState) GameState();
	}

    // public so we can access via UI inspectors
    public void ButtonControls(int state)
    {
        // map int to enum values for readability
        gameState = (GameStates)state;

        // change the game state
        changeState = true;
    }

    public void AudioVolume(float volume)
    {
        gameSoundVolume = volume;
    }

    public void AudioEnabled(bool active)
    {
        gameAudioActive = active;
    }

    void AudioPlay(AudioClip clipName)
    {
        // update our audio volume
        audio.volume = gameSoundVolume;

        // if audio is active...
        if (gameAudioActive)
        {
            // play specified clip
            audio.clip = clipName;
            audio.Play();
        }
    }

    void GameState()
    {
        // game state switch
        switch (gameState)
        {
            case GameStates.MainMenu:
                AudioPlay(audioMenu);
                UIStates("UI_Back", false);
                UIStates("MainMenu", true);
                UIStates("GamePlay", false);
                UIStates("GameOver", false);
                UIStates("Credits", false);
                UIStates("Settings", false);
                UIStates("Leaderboards", false);
                break;
            case GameStates.GamePlay:
                AudioPlay(audioMenu);
                UIStates("UI_Back", false);
                UIStates("MainMenu", false);
                UIStates("GamePlay", true);
                UIStates("GameOver", false);
                break;
            case GameStates.GameOver:
                AudioPlay(audioMenu);
                UIStates("UI_Back", true);
                UIStates("GamePlay", false);
                UIStates("GameOver", true);
                break;
            case GameStates.Credits:
                AudioPlay(audioMenu);
                UIStates("UI_Back", true);
                UIStates("MainMenu", false);
                UIStates("Credits", true);
                break;
            case GameStates.Settings:
                AudioPlay(audioMenu);
                UIStates("UI_Back", true);
                UIStates("MainMenu", false);
                UIStates("Settings", true);
                break;
            case GameStates.Leaderboards:
                AudioPlay(audioMenu);
                UIStates("UI_Back", true);
                UIStates("MainMenu", false);
                UIStates("Leaderboards", true);
                break;
            default:
                break;
        }

        // stop updating this function
        changeState = false;
    }

    void UIStates(string tag, bool active)
    {
        switch (tag)
        {
            case "MainMenu":
                for (int i = 0; i < go0.Length; i++)
                    go0[i].SetActive(active);
                break;
            case "GamePlay":
                for (int i = 0; i < go1.Length; i++)
                    go1[i].SetActive(active);
                break;
            case "GameOver":
                for (int i = 0; i < go2.Length; i++)
                    go2[i].SetActive(active);
                break;
            case "Credits":
                for (int i = 0; i < go3.Length; i++)
                    go3[i].SetActive(active);
                break;
            case "Settings":
                for (int i = 0; i < go4.Length; i++)
                    go4[i].SetActive(active);
                break;
            case "Leaderboards":
                for (int i = 0; i < go5.Length; i++)
                    go5[i].SetActive(active);
                break;
            case "UI_Back":
                goBack.SetActive(active);
                break;

            default:
                break;
        }
    }

    void GameScore()
    {
        // if during game play...
        if (gameState == GameStates.GamePlay)
        {
            // score actually equals elapsed seconds in this example
            score += Time.deltaTime;

            // score is just a temp var to copy to gameScore
            // which will allow maniuplation of the decimals
            gameScore = score;
            // round the float to # of decimals
            // requires 'using System;' above...
            gameScore = (float)Math.Round(gameScore, scoreDecimals);
        }

        // reset the scores while changing from game over state
        if (changeState && gameState != GameStates.GameOver)
        {
            score = 0.0f;
        }

        // if game over...
        if (gameState == GameStates.GameOver)
        {
            // if game score is greater than high score
            if (gameScore > gameHighScore)
            {
                // save score to player prefs... these are unsecure
                // and can be hacked.  just saying...
                PlayerPrefs.SetFloat("HighScore", gameScore);
                PlayerPrefs.Save();

                // update the high score with the newly saved score
                gameHighScore = PlayerPrefs.GetFloat("HighScore");
            }
        }
    }

    void KeysPressed()
    {
        // if enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (gameState == GameStates.MainMenu || gameState == GameStates.GameOver)
            {
                // switch to game play state
                gameState = GameStates.GamePlay;
                // update the game states
                changeState = true;
            }
        }

        // if escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameStates.MainMenu)
            {
                // quit the app, and return to OS
                Application.Quit();
            }
            else if (gameState == GameStates.GamePlay)
            {
                // switch to game over state
                gameState = GameStates.GameOver;
                // update the game state
                changeState = true;
            }
            else
            {
                // switch to main menu state
                gameState = GameStates.MainMenu;
                // update the game states
                changeState = true;
            }
        }
    }
}

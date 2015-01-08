//-----------------------------------------
//   GameController.cs
//
//   Jason Walters
//   http://glitchbeam.com
//   @jasonrwalters
//
//   last edited on 1/7/2015
//-----------------------------------------

using UnityEngine;
using System.Collections;
using System;

public enum GameStates
{
    MainMenu, GamePlay, GameOver, Settings, Credits
}

public class GameController : MonoBehaviour 
{
    public AudioClip audioMenu;
    public int scoreDecimals;

    private GameObject[] go0, go1, go2, go3, go4;
    private GameObject goBack;
    private bool changeState = false;
    private float score;

    public static GameStates gameState;
    public static float gameScore;
    public static float gameHighScore;
    public static float gameSoundVolume;
    public static bool gameAudioActive;

	// Use this for initialization
	void Start ()
    {
        // setup game settings
        SettingsDefault();

        // setup game objects
        FindGameObjects();

        // start our game states
        GameState();
	}
	
	// Update is called once per frame
	void Update () 
    {
        KeysPressed();
        GameScore();

        if (changeState) GameState();
	}

    //-----------------------------------------
    //   SETUP
    //-----------------------------------------
    void SettingsDefault()
    {
        // check player prefs for current highscore and update;
        gameHighScore = PlayerPrefs.GetFloat("HighScore");

        // defaults...
        gameScore = 0.0f;
        gameSoundVolume = 0.75f;
        gameAudioActive = true;
    }

    void FindGameObjects()
    {
        // find all game objects that use specified tag
        go0 = GameObject.FindGameObjectsWithTag("MainMenu");
        go1 = GameObject.FindGameObjectsWithTag("GamePlay");
        go2 = GameObject.FindGameObjectsWithTag("GameOver");
        go3 = GameObject.FindGameObjectsWithTag("Settings");
        go4 = GameObject.FindGameObjectsWithTag("Credits");
        goBack = GameObject.FindGameObjectWithTag("UI_Back");
    }

    //-----------------------------------------
    //   GAME STATES
    //-----------------------------------------
    void GameState()
    {
        // game state switch
        switch (gameState)
        {
            case GameStates.MainMenu:
                AudioController(audioMenu);
                State("UI_Back", false);
                State("MainMenu", true);
                State("GamePlay", false);
                State("GameOver", false);
                State("Settings", false);
                State("Credits", false);
                break;

            case GameStates.GamePlay:
                AudioController(audioMenu);
                State("UI_Back", false);
                State("MainMenu", false);
                State("GamePlay", true);
                State("GameOver", false);
                break;

            case GameStates.GameOver:
                AudioController(audioMenu);
                State("UI_Back", true);
                State("GamePlay", false);
                State("GameOver", true);
                break;

            case GameStates.Settings:
                AudioController(audioMenu);
                State("UI_Back", true);
                State("MainMenu", false);
                State("Settings", true);
                break;

            case GameStates.Credits:
                AudioController(audioMenu);
                State("UI_Back", true);
                State("MainMenu", false);
                State("Credits", true);
                break;
            
            default:
                break;
        }

        // stop updating this function
        changeState = false;
    }

    void State(string tag, bool active)
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

            case "Settings":
                for (int i = 0; i < go3.Length; i++)
                    go3[i].SetActive(active);
                break;
            
            case "Credits":
                for (int i = 0; i < go4.Length; i++)
                    go4[i].SetActive(active);
                break;

            case "UI_Back":
                goBack.SetActive(active);
                break;

            default:
                break;
        }
    }


    //-----------------------------------------
    //   GAME SCORE
    //-----------------------------------------
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


    //-----------------------------------------
    //   AUDIO
    //-----------------------------------------
    // volume control, max = 1.0f
    public void AudioVolume(float volume)
    {
        gameSoundVolume = volume;
    }

    // audio toggle - on/off
    public void AudioEnabled(bool active)
    {
        gameAudioActive = active;
    }

    // audio file controller
    void AudioController(AudioClip clipName)
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

    //-----------------------------------------
    //   INPUT
    //-----------------------------------------
    public void ButtonControls(int state)
    {
        // map int to enum values for readability
        gameState = (GameStates)state;

        // change the game state
        changeState = true;
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

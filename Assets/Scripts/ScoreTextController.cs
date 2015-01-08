//-----------------------------------------
//   ScoreTextController.cs
//
//   Jason Walters
//   http://glitchbeam.com
//   @jasonrwalters
//
//   last edited on 1/7/2015
//-----------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreTextController : MonoBehaviour 
{
    void OnDisable()
    {
        // clear the text
        GetComponent<Text>().text = string.Empty;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float gameScore = GameController.gameScore;
        float gameHighScore = GameController.gameHighScore;

        // update the score based on following object names
        if (gameObject.name == "MainMenu_Text_HighScore")
        {
            GetComponent<Text>().text = "Highscore " + gameHighScore;
        }
        else if (gameObject.name == "GamePlay_Text_Score_Time")
        {
            GetComponent<Text>().text = "Score " + gameScore;
        }
        else if (gameObject.name == "GameOver_Text_Score_Time")
        {
            GetComponent<Text>().text = "You survived " + gameScore + " seconds";
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour {

    public Text goScore;
    public Text goBullets;
    public Text goTime;

    public InputField usernameInput;
    public Button submit;

    bool gameEnded = false;

	// Use this for initialization
	void Start () {
		usernameInput.text = GameMaster.playerName;
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (gameEnded) {
            gameObject.SetActive(true);
        }

        goScore.text = GameMaster.enemiesKilled.ToString();
        goBullets.text = "Bullets fired: "+GameMaster.bulletsFired.ToString();
        goTime.text = "Total time: " + GameMaster.timeTaken.ToTimestamp();
	}

    void EndGame() {
        gameEnded = true;
        GameMaster.gameEnded = true;
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies) {
            Destroy(enemy);
        }

        Time.timeScale = 0;
    }

    void SubmitScore()
    {
        Debug.Log("toggling submit");
        Leaderboard.AddToLeaderboard(GameMaster.enemiesKilled, usernameInput.text);
        GameMaster.playerName = usernameInput.text;
        submit.interactable = false;
        submit.GetComponentsInChildren<Text>()[0].text = "Saved!";
    }

    void RestartGame()
    {
        Time.timeScale = 1;
		GameMaster.ResetGame();
        SceneManager.LoadScene("Game");
    }


    void ReturnToStart()
    {
        Time.timeScale = 1;
		GameMaster.ResetGame();
        SceneManager.LoadScene("Start");
    }
}

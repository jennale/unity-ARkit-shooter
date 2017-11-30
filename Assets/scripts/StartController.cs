using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour {

    public GameObject homeSlime;
    private SortedList leaderboard;
    public GameObject leaderboardGo;

    //Buttons
    public Button backButton;
    public Button testButton;

    //Text UI objects
    public Text leaderboardNames;
    public Text leaderboardNumbers;


	// Use this for initialization
	void Start () {
        Leaderboard.InitGame();

        leaderboard = Leaderboard.leaderboard;
        leaderboardGo.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
        homeSlime.transform.Rotate(new Vector3(0, 25, 0) * Time.deltaTime);


        //If the leaderboard was updated, create the new leaderboard text.
        if (Leaderboard.updated)
        {
            CreateLeaderboard();
        }
    }

    private void StartGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Toggles the leaderboard.
    /// </summary>
    void ToggleLeaderboard()
    {
        leaderboardGo.SetActive(!leaderboardGo.activeSelf);
    }

    /// <summary>
    /// Generates the Leaderboard text and shows it on the canvas
    /// </summary>
    private void CreateLeaderboard()
    {
        if (leaderboard.Count < 1)
        {
            leaderboardNames.text = "There are no scores!";
        }

        string names = "";
        string scores = "";
        int orderLabel = 1;

        //Scores are sorted from lowest to highest scores (reverse for loop)
        for (int i = 0; i < leaderboard.Count; i++)
        {
            Leaderboard.LeaderboardScore val = (Leaderboard.LeaderboardScore)leaderboard.GetByIndex(i);
            names += orderLabel.ToString() + "\t" + val.name + "\n";
            scores += val.score.ToString() + "\n";
            orderLabel++;
        }

        leaderboardNames.text = names;
        leaderboardNumbers.text = scores;

        Leaderboard.updated = false;
    }

    /// <summary>
    /// Tests adding to the leaderboard
    /// </summary>
    void TestAdd()
    {
        Leaderboard.AddToLeaderboard((int)Random.Range(0.0f, 100.0f), "Username" + leaderboard.Count.ToString());
    }

}

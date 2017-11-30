using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class UIController : MonoBehaviour {

    public Text HeartsText;
    private int health = GameMaster.playerHealth;
    private string heartString = "";

    public Text ScoreText;

    public GameObject pauseScreen;
    bool gamePaused = false;

    public GameObject endGame;

    public GameObject damagePanel;

    public GameObject searchingForPlanePanel;
    public GameObject foundPlanePanel;
    private UnityARAnchorManager unityARAnchorManager;


    private bool foundPlane = false;


	// Use this for initialization
	void Start () {
        unityARAnchorManager = new UnityARAnchorManager();

        pauseScreen.SetActive(false);
        damagePanel.SetActive(false);

        searchingForPlanePanel.SetActive(true);
        foundPlanePanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        LookForPlanes();

        if (!foundPlane) {
            return;
        }


        if (GameMaster.playerHealth < 1 && GameMaster.gameStarted) {
			damagePanel.SetActive(false);
            endGame.SetActive(true);
            endGame.SendMessage("EndGame");
        }

        damagePanel.SetActive(false);

        if (GameMaster.playerHealth < health) {
            //Flash the screen red when you lose a heart.
            damagePanel.SetActive(true);
			health = GameMaster.playerHealth;
        }

        heartString = "";
        for (int i = 0; i < health; i++)
        {
            heartString += "♥ ";
        }


        HeartsText.text = heartString;
        ScoreText.text = GameMaster.enemiesKilled.ToString("000");
	}

    void TogglePause() {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0 : 1;
        pauseScreen.SetActive(!pauseScreen.activeSelf);
    }

    void StartGame() {
        GameMaster.gameStarted = true;
        foundPlanePanel.SetActive(false);
    }

    void LookForPlanes(){
        if (foundPlane && GameMaster.gameStarted) {
            return;
        }

        List<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors();
        if (arpags.Count > 0) {
            foundPlane = true;
			searchingForPlanePanel.SetActive(false);
            foundPlanePanel.SetActive(true);
        }
    }

}
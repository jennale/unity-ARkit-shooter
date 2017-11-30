using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.XR.iOS;


public class EnemiesController : MonoBehaviour {
    public GameObject enemyPrefab;
    GameObject[] enemies;
    private bool gameStarted = false;


	private UnityARAnchorManager unityARAnchorManager;
    private ARPlaneAnchor mainPlane;
    private Vector3 leftWorldPoint;
    private Vector3 rightWorldPoint;

    private int enemyLimit = 5;

	// Use this for initialization
	void Start () {
        unityARAnchorManager = new UnityARAnchorManager();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        InvokeRepeating("GenerateEnemy", 0f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameMaster.gameStarted || GameMaster.gameEnded)
            return;

        GetLargestPlane();
        GetScreenEdges();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

    void GenerateEnemy() {
        if (mainPlane.Equals(default(ARPlaneAnchor)) || enemies.Length >= enemyLimit) {
            Debug.Log("No plane yet or monster limit reached");
			return;
        }


        Vector3 planePosition = UnityARMatrixOps.GetPosition(mainPlane.transform);
        //Vector3 planePosition = mainPlane.center;
        float[] choices = new float[2];
        choices[0] = leftWorldPoint.x - Random.Range(3f, 8f);
        choices[1] = rightWorldPoint.x + Random.Range(3f, 8f);

        var randomIndex = Random.Range(0, choices.Length);

        Debug.Log(randomIndex);

        var x = choices[randomIndex]; //The leftmost worldpoint minus 1m to the left 
        var y = planePosition.y + 1f; //The bottom of the plane plus 1m
        var z = 6f; //Starts about 6m away
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(x, y, z), Quaternion.identity);
        enemy.SendMessage("LevelUp", Random.Range(0, 3));
        Debug.Log("Enemy created!");
    }


    void GetScreenEdges() {
        leftWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0f, (float)Screen.height / 2f, 5f));
        rightWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height / 2f, 5f));
    }

    void GetLargestPlane()
    {
        List<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors();
        if (arpags.Count > 0)
        {
            if (mainPlane.Equals(default(ARPlaneAnchor)))
            {
				mainPlane = arpags[0].planeAnchor;
                Debug.Log("mainPlane was set");
            }

            foreach (ARPlaneAnchorGameObject argo in arpags)
            {
                //If there's a larger plane, update the mainPlane as the larger one
                if (argo.planeAnchor.extent.sqrMagnitude > mainPlane.extent.sqrMagnitude)
                {
                    mainPlane = argo.planeAnchor;
                    Debug.Log("Larger mainPlane was replaced");
                }
            }
        }
    }

    void SpeedUpAll() {
        foreach(GameObject enemy in enemies) {
            enemy.SendMessage("SpeedUp");
        }
    }

    void SlowDownAll()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SendMessage("SlowDown");
        }
    }
}

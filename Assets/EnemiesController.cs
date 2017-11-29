using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.XR.iOS;


public class EnemiesController : MonoBehaviour {
    public GameObject enemyPrefab;
    GameObject[] enemies;
    Transform[] startPositions;
    private UnityARAnchorManager unityARAnchorManager;
    private ARPlaneAnchor mainPlane;
    private bool gameStarted = false;

	// Use this for initialization
	void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        startPositions = new Transform[enemies.Length];
        InvokeRepeating("SetPlane", 0f, 2f);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetPlane()
    {
        List<ARPlaneAnchorGameObject> arpags = unityARAnchorManager.GetCurrentPlaneAnchors();

        if (arpags.Count > 0)
        {
            //GUI.Box (new Rect (100, 100, 800, 60), string.Format ("Found a plane!"));
            ARPlaneAnchor plane = arpags[0].planeAnchor;
            foreach (ARPlaneAnchorGameObject argo in arpags)
            {
                if (argo.planeAnchor.extent.sqrMagnitude > plane.extent.sqrMagnitude)
                {
                    plane = argo.planeAnchor;
                }
            }

            Debug.Log("Found a plane!!!!!!!!");

            Vector3 planePosition = UnityARMatrixOps.GetPosition(plane.transform);

            var bottomRight = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width, Screen.height));


            var x = planePosition.x - bottomRight.x; //
            var y = planePosition.y + 1f; //The bottom of the plane plus 1m
            var z = Random.Range(planePosition.z, 1f);

            Instantiate(enemyPrefab, new Vector3(x, y, z), Quaternion.identity);

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

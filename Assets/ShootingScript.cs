using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ShootingScript : MonoBehaviour {

    public GameObject bulletPrefab;
    private UnityARAnchorManager unityARAnchorManager;

	// Use this for initialization
	void Start () {
        unityARAnchorManager = new UnityARAnchorManager();
        InvokeRepeating("SetPlane", 0f, 2f);
	}
	
	// Update is called once per frame
	void Update () {


		
	}

    void SetPlane() {
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

            Instantiate(bulletPrefab, new Vector3(Random.Range(planePosition.x, 1f), planePosition.y + 1f, Random.Range(planePosition.z, 1f)), Quaternion.identity);

        }
    }

    void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;

        Destroy(bullet, 2.0f);
    }
}

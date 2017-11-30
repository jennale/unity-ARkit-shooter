using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ShootingController : MonoBehaviour {

    public GameObject bulletPrefab;
    private UnityARAnchorManager unityARAnchorManager;

	// Use this for initialization
	void Start () {
        unityARAnchorManager = new UnityARAnchorManager();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.transform.position = Vector3.MoveTowards(this.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 10f)), 1f);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;

        Destroy(bullet, 2.0f);

        GameMaster.bulletsFired++;

        //GameMaster.enemiesKilled++;
        //Debug.Log(GameMaster.enemiesKilled);
		//GameMaster.gameStarted = true;
        //GameMaster.playerHealth--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    Transform moveTo;
    float speed = 0.005f;
    int level = 1;

	// Use this for initialization
	void Start () {
        moveTo = Camera.main.transform;
		InvokeRepeating("FacePlayer", 0f, 2f);
		InvokeRepeating("UpdateDirection", 0f, 2f);
    }
    
    // Update is called once per frame
    void Update () {
        Move();

        gameObject.transform.localScale = new Vector3(level, level, level);
	}

    void FacePlayer() {
        transform.LookAt(moveTo.position);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    void UpdateDirection() {
        moveTo = Camera.main.transform;
    }

    void Move() {
        //transform.Translate(new Vector3(1,1,1) * Time.deltaTime * (transform.localScale.x * .05f));
        transform.position = Vector3.MoveTowards(transform.position, moveTo.position, speed);
    }

    public void SpeedUp() {
        speed += 0.01f;   
    }

    public void SlowDown() {
        speed -= 0.01f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
            Debug.Log("Omg shot down!");
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameMaster.playerHealth--;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);

            GameMaster.enemiesKilled++;
            Debug.Log("Omg shot down!");
        }
    }


    void LevelUp(float newLevel) {
        level = (int)newLevel;
    }

    //Stretch and disappear
    private void Die() {
        
    }

}

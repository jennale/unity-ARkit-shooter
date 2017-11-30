using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMaster {

	public static string playerName = "";
	public static float timeTaken = 0f;
    public static int enemiesKilled = 0;
    public static int bulletsFired = 0;
    public static int playerHealth = 5;
    public static bool gameStarted = false;
    public static bool gameEnded = false;


    public static void ResetGame() {
        timeTaken = 0f;
        enemiesKilled = 0;
        bulletsFired = 0;
        playerHealth = 5;
        gameStarted = false;
        gameEnded = false;
    }

}

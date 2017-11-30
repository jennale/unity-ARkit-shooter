using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Leaderboard
{
	public static SortedList leaderboard = new SortedList();

    public static PlayerPrefs saveData = new PlayerPrefs();

    private const int MaxNumRecords = 10;

    public static bool updated = false;
    public static string lastUser;

    private const string SavedLeaderboardName = "leaderboard";
    private const string SavedLastUserName = "lastUsedName";

    /// <summary>
    /// Adds to leaderboard, using a sorted list. The list is sorted by
    /// the following order: Score (0000XX) > User > Date
    /// </summary>
    /// <param name="score">Score integer.</param>
    /// <param name="user">User.</param>
    public static void AddToLeaderboard(float score, string user, string date = null)
    {

        if (leaderboard.Count >= MaxNumRecords)
        {
            leaderboard.RemoveAt(MaxNumRecords - 1);
        }

        var newScore = new LeaderboardScore();
        newScore.name = user;
        newScore.date = date ?? System.DateTime.Now.ToString();
        newScore.score = (int)score;

        string key = newScore.ToKey();

        try
        {
            leaderboard.Add(key, newScore);
        }
        catch (System.Exception e)
        {
            Debug.Log("A score already exists with this info\n" + e.Message);
        }

        updated = true;
        SaveData();
    }

    public static void AddToLeaderboard(LeaderboardScore score)
    {
        AddToLeaderboard(score.score, score.name, score.date);
    }

    public static void InitGame()
    {
        LoadData();
    }

    private static void SaveData()
    {
        LeaderboardScore[] scores = new LeaderboardScore[leaderboard.Count];

        for (int i = 0; i < leaderboard.Count; i++)
        {
            LeaderboardScore lScore = (LeaderboardScore)leaderboard.GetByIndex(i);
            scores[i] = lScore;
        }

        string scoresToJson = ToJson<LeaderboardScore>(scores);
        Debug.Log(scoresToJson);
        PlayerPrefs.SetString(SavedLeaderboardName, scoresToJson);
        PlayerPrefs.SetString(SavedLastUserName, lastUser);
        PlayerPrefs.Save();
    }

    private static void LoadData()
    {
        string scoresJson = PlayerPrefs.GetString(SavedLeaderboardName);
        lastUser = PlayerPrefs.GetString(SavedLastUserName);

        if (!string.IsNullOrEmpty(scoresJson))
        {
            LeaderboardScore[] scores = FromJson<LeaderboardScore>(scoresJson);

            for (int i = 0; i < scores.Length; i++)
            {
                LeaderboardScore lScore = scores[i];
                AddToLeaderboard(lScore);
            }
        }
    }

    /// <summary>
    /// Score object.
    /// </summary>
    [System.Serializable]
    public class LeaderboardScore
    {
        public string name;
        public float score;
        public string date;
    }

    [System.Serializable]
    private class JsonWrapper<T>
    {
        public T[] array;
    }

    private static string ToKey(this LeaderboardScore score)
    {
        return score + "0-" + score.name + score.date;
    }

    private static T[] FromJson<T>(string json)
    {
        JsonWrapper<T> wrapper = JsonUtility.FromJson<JsonWrapper<T>>(json);
        return wrapper.array;
    }

    private static string ToJson<T>(T[] array)
    {
        JsonWrapper<T> wrapper = new JsonWrapper<T>();
        wrapper.array = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToTimestamp(this float scoreFloat)
    {
        var milliseconds = (scoreFloat * 100 % 60).ToString("00");
        var seconds = (Math.Floor(scoreFloat % 60)).ToString("00:");
        var minutes = (Math.Floor(scoreFloat / 60) % 60).ToString("00:");

        return minutes + seconds + milliseconds;
    }
}

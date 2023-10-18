using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public string playerName;

    public BestScoreData bestScoreData;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        LoadBestScore();
        if (bestScoreData != null)
            GameObject.Find("BestScoreText").GetComponent<Text>().text = "Best Score: " + bestScoreData.playerName + " : " + bestScoreData.score;
    }


    public void LoadGameScene()
    {
        playerName = GameObject.Find("textboxPlayername").GetComponent<Text>().text;
        GameObject.Find("Canvas").SetActive(false);
        SceneManager.LoadScene("main");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [Serializable]
    public class BestScoreData
    {
        public string playerName;
        public int score;
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BestScoreData data = JsonUtility.FromJson<BestScoreData>(json);

            bestScoreData = new BestScoreData();
            bestScoreData.playerName = data.playerName;
            bestScoreData.score = data.score;
        }
    }

    public void SaveBestScore(int score)
    {
        BestScoreData tempScoreData = new BestScoreData();
        tempScoreData.playerName = playerName;
        tempScoreData.score = score;

        if (bestScoreData != null && bestScoreData.score < tempScoreData.score)
            bestScoreData = tempScoreData;

        string json = JsonUtility.ToJson(bestScoreData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }

}

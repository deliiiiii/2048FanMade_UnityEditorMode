using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public class Score
    {
        public int score;
    }
    public static DataManager instance;
    public static Score scoree = new Score();
    
    public static bool initialize = true ;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(initialize)
        {
            //Debug.Log("数据初始化");
            string path_highscore = Application.streamingAssetsPath + "/HighScore_" + GameManager.lines + "lines_" + GameManager.columns + "columns.json";
            if (!File.Exists(path_highscore))
            {
                scoree.score = 0;
                string json = JsonUtility.ToJson(scoree, true);
                File.WriteAllText(path_highscore, json);
            }
            else
            {
                string json = File.ReadAllText(path_highscore);
                scoree = JsonUtility.FromJson<Score>(json);
            }
            UIManager.instance.UpdateHighScore();
            initialize= false;
        }
    }
    public void SaveScore()
    {
        scoree.score = GameManager.currentScore;
        string json0 = JsonUtility.ToJson(DataManager.scoree, true);
        string path_highscore = Application.streamingAssetsPath + "/HighScore_" + GameManager.lines + "lines_" + GameManager.columns + "columns.json";
        File.WriteAllText(path_highscore, json0);
    }
}

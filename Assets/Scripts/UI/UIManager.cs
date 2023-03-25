using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject button_Start;
    public GameObject button_Restart;
    public GameObject button_PlusLines;
    public GameObject button_MinusLines;
    public GameObject button_PlusColumns;
    public GameObject button_MinusColumns;
    public GameObject panel_GameOver;
    public GameObject panel_BreakRecord;
    public GameObject panel_Score;
    public GameObject point_Panel_AddedScore;
    public GameObject panel_AbandonSetting;
    public Text currentScore;
    public Text MAXScore;
    public Text LinesNum;
    public Text ColumnsNum;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        button_Start.GetComponent<Button>().onClick.AddListener(StartGame);
        button_Restart.GetComponent<Button>().onClick.AddListener(RestartGame);
        button_Restart.GetComponent<Button>().interactable = false;

        button_PlusLines.GetComponent<Button>().onClick.AddListener(PlusLinesNum);
        button_MinusLines.GetComponent<Button>().onClick.AddListener(MinusLinesNum);

        button_PlusColumns.GetComponent<Button>().onClick.AddListener(PlusColumnsNum);
        button_MinusColumns.GetComponent<Button>().onClick.AddListener(MinusColumnsNum);

        MAXScore.text = DataManager.scoree.score.ToString();
        LinesNum.text = GameManager.lines.ToString();
        ColumnsNum.text = GameManager.columns.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        currentScore.text = GameManager.currentScore.ToString();
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            RestartGame();
        }
        else if(Input.GetKeyDown(KeyCode.Space)&& button_Start.GetComponent<Button>().interactable)
        {
            StartGame();
        }else if(Input.GetKeyDown(KeyCode.Q) && !panel_AbandonSetting.activeSelf)
        {
            MinusLinesNum();
        }
        else if (Input.GetKeyDown(KeyCode.E) && !panel_AbandonSetting.activeSelf)
        {
            PlusLinesNum();
        }
        else if (Input.GetKeyDown(KeyCode.Z) && !panel_AbandonSetting.activeSelf)
        {
            MinusColumnsNum();
        }
        else if (Input.GetKeyDown(KeyCode.C) && !panel_AbandonSetting.activeSelf)
        {
            PlusColumnsNum();
        }
    }
    public void StartGame()
    {
        //GameManager.initialize = true;
        GameManager.instance.GenerateNum();
        panel_AbandonSetting.SetActive(true);
        button_Start.transform.GetChild(0).gameObject.GetComponent<Text>().text = "已开始";
        button_Start.GetComponent<Button>().interactable = false;
        button_Restart.GetComponent<Button>().interactable = true;
    }
    public void RestartGame()
    {
        GameManager.countNums = 0;
        GameManager.currentScore = 0;
        panel_BreakRecord.SetActive(false);
        panel_GameOver.SetActive(false);
        panel_AbandonSetting.SetActive(false);
        GameManager.gameRestart = true;
        button_Start.transform.GetChild(0).gameObject.GetComponent<Text>().text = "开始";
        button_Start.GetComponent<Button>().interactable = true;
        button_Restart.GetComponent<Button>().interactable = false;
        
    }
    public void GameOver()
    {
        panel_GameOver.SetActive(true); 
        if (GameManager.currentScore > DataManager.scoree.score)
        {
            panel_BreakRecord.SetActive(true);
            MAXScore.text = GameManager.currentScore.ToString();
            DataManager.instance.SaveScore();
        }
    }
    public void AddScore(int addedScore)
    {
        GameObject temp =  Instantiate(point_Panel_AddedScore, panel_Score.transform);
        temp.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        temp.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = addedScore.ToString();
        temp.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void PlusLinesNum()
    {
        if(GameManager.lines < 9)
        {
            if(GameManager.lines == 8)
            {
                button_PlusLines.GetComponent<Button>().interactable = false;
            }
            GameManager.lines++;
            GameManager.countCells = GameManager.lines * GameManager.columns;
            button_MinusLines.GetComponent<Button>().interactable = true;
            LinesNum.text = GameManager.lines.ToString();
            GameManager.initialize = true;
            DataManager.initialize = true;
        }
    }

    public void MinusLinesNum()
    {
        if(GameManager.lines > 1)
        {
            if(GameManager.lines == 2)
            {
                button_MinusLines.GetComponent<Button>().interactable = false;
            }
            GameManager.lines--;
            GameManager.countCells = GameManager.lines * GameManager.columns;
            button_PlusLines.GetComponent<Button>().interactable = true;
            LinesNum.text = GameManager.lines.ToString();
            GameManager.initialize = true;
            DataManager.initialize = true;
        }
    }

    public void PlusColumnsNum()
    {
        if(GameManager.columns < 9)
        {
            if(GameManager.columns == 8)
            {
                button_PlusColumns.GetComponent<Button>().interactable = false;
            }
            GameManager.columns++;
            GameManager.countCells = GameManager.lines * GameManager.columns;
            button_MinusColumns.GetComponent<Button>().interactable = true;
            ColumnsNum.text = GameManager.columns.ToString();
            GameManager.initialize = true;
            DataManager.initialize = true;
        }
        
    }

    public void MinusColumnsNum()
    {
        if(GameManager.columns > 1)
        {
            if (GameManager.columns == 2)
            {
                button_MinusColumns.GetComponent<Button>().interactable = false;
            }
            GameManager.columns--;
            GameManager.countCells = GameManager.lines * GameManager.columns;
            button_PlusColumns.GetComponent<Button>().interactable = true;
            ColumnsNum.text = GameManager.columns.ToString();
            GameManager.initialize = true;
            DataManager.initialize = true;
        }
        
    }

    public void UpdateHighScore()
    {
        MAXScore.text = DataManager.scoree.score.ToString();
    }
}
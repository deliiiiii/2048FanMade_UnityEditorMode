using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static float sizeOfCell = 100;
    public static int lines = 4,columns=4;
    public static int countCells = lines*columns;
    public static int countNums = 0;
    public int countMove = 0;
    public int countIntegrate = 0;
    public static float moveVelocity = 6f;

    public static int direction = -1;
    public static int[,] directions = new int[4, 2]
    {
        {-1,0},
        {1,0 },
        {0,1 },
        {0,-1 }
    };

    public int randomSeed = lines*columns;

    public static int currentScore = 0;
    //public static int MAXScore = 0;

    public static List <List<int>> statesInCells = new List<List<int>>();

    public GameObject Map;
    public List<GameObject> Nums = new List<GameObject>();
    public List<List<GameObject>> createdNums = new List<List<GameObject>>();

    //public static bool gameStart = false;
    public static bool gameRestart = false;
    public static bool isMoving = false;
    public bool everMoved = false;
    public static bool initialize = true;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
    }
    // Update is called once per frame
    void Update()
    {
        if(initialize)
        {
            createdNums.Clear();
            statesInCells.Clear();
            for (int i=0;i<lines;i++)
            {
                
                statesInCells.Add (new List<int>());
                createdNums.Add(new List<GameObject>());
                for(int j=0;j<columns;j++)
                {
                    createdNums[i].Add(new GameObject());
                    statesInCells[i].Add(0);        //0:没有数字 1:有数字s
                }
            }
            MapGenerator.instance.GenerateMap();
            initialize = false;
        }
        if(gameRestart)
        {
            for (int i = 0; i < lines; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (statesInCells[i][j]!=0)
                    {
                        Destroy(createdNums[i][j]);
                        statesInCells[i][j] = 0;
                    }
                }
            }
            //UIManager.instance.StartGame();
            
            gameRestart = false;
        }
        if(!isMoving)//不在移动
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                countMove = 0;
                countIntegrate = 0;
                direction = 0;
                everMoved = false;
                isMoving = true;
                MoveNum();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                countMove = 0;
                countIntegrate = 0;
                direction = 1;
                everMoved = false;
                isMoving = true;
                MoveNum();
            }
            else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                countMove = 0;
                countIntegrate = 0;
                direction = 2;
                everMoved = false;
                isMoving = true;
                MoveNum();
            }
            else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                countMove = 0;
                countIntegrate = 0;
                everMoved = false;
                direction = 3;
                isMoving = true;
                MoveNum();
            }
        }
        if(countNums == countCells)
        {
            if(!isMoving&&GameOver())
            {
                countNums = 0;
                UIManager.instance.GameOver();
            }
        }
    }
    public void GenerateNum()
    {
        if(countNums<countCells)
        {
            
            for (int q = 0; q < 1; q++)//q : 生成个数
            {
                int r1 = UnityEngine.Random.Range(0, randomSeed + 1);
                int r2 = UnityEngine.Random.Range(0, randomSeed + 1);
                int r3 = UnityEngine.Random.Range(0, randomSeed + 1);
                r1 %= lines;
                r2 %= columns;
                for (int  i = r1, j = r2; ;)
                {
                    if (j == columns)
                    {
                        j -= columns;
                        i++;
                    }
                    if (i == lines)
                    {
                        i -= lines;
                    }
                    //Debug.Log("随机经过第" + i + "行第" + j + "列");
                    if (statesInCells[i][j] == 0)
                    {
                        createdNums[i][j] = Instantiate(Nums[r3 % 2], new Vector3(Map.transform.position.x + i * sizeOfCell * 1.05f, Map.transform.position.y + j * sizeOfCell * 1.05f, 0), Map.transform.rotation, Map.transform);//生成0或1号数字，即2或4
                        countNums++;
                        statesInCells[i][j] = 1;
                        createdNums[i][j].GetComponent<NumsMover>().level = (r3 % 2) + 1;
                        createdNums[i][j].GetComponent<NumsMover>().num = 1;
                        for(int k=0;k< createdNums[i][j].GetComponent<NumsMover>().level;k++)
                        {
                            createdNums[i][j].GetComponent<NumsMover>().num *= 2;
                        }
                        
                        break;
                    }
                    else
                    {
                        j++;
                    }
                }
            }
        }
    }
    public void MoveNum()
    {
        countMove ++;
        if (direction == 0)
        {
            for(int m=1;m<lines; m++) //不在左边缘
            {
                for(int n=0;n<columns; n++)
                {
                    if (statesInCells[m][n]==1 && statesInCells[m+directions[direction,0]][n+directions[direction,1]] == 0)//有数字且能移动
                    {
                        everMoved = true;
                        createdNums[m][n].GetComponent<NumsMover>().target = new Vector3(createdNums[m][n].transform.localPosition.x + directions[direction, 0] *sizeOfCell* 1.05f, createdNums[m][n].transform.localPosition.y + directions[direction, 1] * sizeOfCell *1.05f, 0);
                        createdNums[m + directions[direction, 0]][n + directions[direction, 1]] = createdNums[m][n];
                        createdNums[m + directions[direction, 0]][n + directions[direction, 1]].transform.localPosition= createdNums[m][n].GetComponent<NumsMover>().target;
                        statesInCells[m][n] = 0;
                        statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] = 1;
                    }
                    
                }
            }
        }
        else if (direction == 1)
        {
            for (int m = lines-2; m>=0; m--) //不在右边缘
            {
                for (int n = 0; n < columns; n++)
                {
                    if (statesInCells[m][n] == 1 && statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] == 0)//有数字且能移动
                    {
                        everMoved = true;
                        createdNums[m][n].GetComponent<NumsMover>().target = new Vector3(createdNums[m][n].transform.localPosition.x + directions[direction, 0] * sizeOfCell * 1.05f, createdNums[m][n].transform.localPosition.y + directions[direction, 1] * sizeOfCell * 1.05f, 0);
                        createdNums[m + directions[direction, 0]][n + directions[direction, 1]] = createdNums[m][n];
                        createdNums[m + directions[direction, 0]][n + directions[direction, 1]].transform.localPosition = createdNums[m][n].GetComponent<NumsMover>().target;
                        statesInCells[m][n] = 0;
                        statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] = 1;
                    }

                }
            }
        }
        else if (direction == 2)
        {
            for (int m = 0; m < lines; m++) //不在上边缘
            {
                for (int n = columns-2; n >=0; n--)
                {
                    if (statesInCells[m][n] == 1 && statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] == 0)//有数字且能移动
                    {
                        everMoved = true;
                        createdNums[m][n].GetComponent<NumsMover>().target = new Vector3(createdNums[m][n].transform.localPosition.x + directions[direction, 0] * sizeOfCell * 1.05f, createdNums[m][n].transform.localPosition.y + directions[direction, 1] * sizeOfCell * 1.05f, 0);
                        createdNums[m + directions[direction, 0]][n + directions[direction, 1]] = createdNums[m][n];
                        createdNums[m + directions[direction, 0]][n + directions[direction, 1]].transform.localPosition = createdNums[m][n].GetComponent<NumsMover>().target;
                        statesInCells[m][n] = 0;
                        statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] = 1;
                    }

                }
            }
        }
        else if (direction == 3)
        {
            for (int m = 0; m < lines; m++) //不在下边缘
            {
                for (int n = 1; n < columns; n++)
                {
                    if (statesInCells[m][n] == 1 && statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] == 0)//有数字且能移动
                    {
                        everMoved = true;
                        createdNums[m][n].GetComponent<NumsMover>().target = new Vector3(createdNums[m][n].transform.localPosition.x + directions[direction, 0] * sizeOfCell * 1.05f, createdNums[m][n].transform.localPosition.y + directions[direction, 1] * sizeOfCell * 1.05f, 0);
                        createdNums[m + directions[direction, 0]][n + directions[direction, 1]] = createdNums[m][n];
                        createdNums[m + directions[direction, 0]][n + directions[direction, 1]].transform.localPosition = createdNums[m][n].GetComponent<NumsMover>().target;
                        statesInCells[m][n] = 0;
                        statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] = 1;
                    }

                }
            }
        }
        int max ;
        if (lines > columns) max = lines;
        else max = columns;
        if (countMove<max)
        {
            MoveNum();
        }else if(countIntegrate == 0)
        {
            IntegrateNum();
        }else
        {
            if(everMoved)
            {
                GenerateNum();
            }
            
            isMoving = false;
        }
        if (GameManager.currentScore > DataManager.scoree.score)
        {
            UIManager.instance.panel_BreakRecord.SetActive(true);
            UIManager.instance.MAXScore.text = GameManager.currentScore.ToString();
            DataManager.instance.SaveScore();
        }
    }
    public void IntegrateNum()
    {
        countIntegrate++;
        countMove = 0;
        //Debug.Log("准备合并，方向"+direction);
        if (direction == 0)
        {
            for (int m = 1; m < lines; m++) //不在左边缘
            {
                for (int n = 0; n < columns; n++)
                {
                    
                    Integrating(m, n);
                }
            }
        }
        else if (direction == 1)
        {
            for (int m = lines - 2; m >= 0; m--) //不在右边缘
            {
                for (int n = 0; n < columns; n++)
                {
                    Integrating(m, n);
                }
            }
        }
        else if (direction == 2)
        {
            for (int m = 0; m < lines; m++) //不在上边缘
            {
                for (int n = columns - 2; n >= 0; n--)
                {
                    Integrating(m, n);
                }
            }
        }
        else if (direction == 3)
        {
            for (int m = 0; m < lines; m++) //不在下边缘
            {
                for (int n = 1; n < columns; n++)
                {
                    Integrating(m, n);
                }
            }
        }
        MoveNum();
    }
    public bool IsEqual(GameObject g1,GameObject g2)
    {
        if(g1.GetComponent<NumsMover>().num == g2.GetComponent<NumsMover>().num)
        {
            return true;
        }else
        {
            return false;
        }
    }
    public void Integrating(int mm,int nn)
    {
        if ((statesInCells[mm][nn] == 1) && (statesInCells[mm + directions[direction, 0]][nn + directions[direction, 1]] == 1))//有相邻数字
        {
            if (IsEqual(createdNums[mm][nn], createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]]))
            {
                //Debug.Log("方向"+direction+"合并！");
                everMoved = true;
                countNums--;
                Destroy(createdNums[mm][nn]);
                statesInCells[mm][nn] = 0;
                GameObject temp = Instantiate(Nums[createdNums[mm + directions[direction, 0]][  nn + directions[direction, 1]].GetComponent<NumsMover>().level], createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]].transform.position, createdNums[mm + directions[direction, 0]][nn+ directions[direction, 1]].transform.rotation, createdNums[mm + directions[direction, 0]][nn+ directions[direction, 1]].transform.parent);
                temp.GetComponent<NumsMover>().level = createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]].GetComponent<NumsMover>().level+1;
                Destroy(createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]]);
                createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]] = temp;
                createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]].GetComponent<NumsMover>().num = 1;
                for (int k = 0; k < createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]].GetComponent<NumsMover>().level; k++)
                {
                    createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]].GetComponent<NumsMover>().num *= 2;
                }
                currentScore += createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]].GetComponent<NumsMover>().num;
                statesInCells[mm + directions[direction, 0]][nn + directions[direction, 1]] = 1;
                UIManager.instance.AddScore(createdNums[mm + directions[direction, 0]][nn + directions[direction, 1]].GetComponent<NumsMover>().num);
            }
        }
    }
    public bool GameOver()
    {
        direction = 0;
        for (int m = 1; m < lines; m++) //不在左边缘
        {
            for (int n = 0; n < columns; n++)
            {
                if ((statesInCells[m][n] == 1) && (statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] == 1))
                {
                    if (IsEqual(createdNums[m][n], createdNums[m + directions[direction, 0]][n + directions[direction, 1]]))
                    {
                        return false;//有相邻相同数字
                    }
                }
            }
        }
        direction = 1;
        for (int m = lines - 2; m >= 0; m--) //不在右边缘
        {
            for (int n = 0; n < columns; n++)
            {
                if ((statesInCells[m][n] == 1) && (statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] == 1))
                {
                    if (IsEqual(createdNums[m][n], createdNums[m + directions[direction, 0]][n + directions[direction, 1]]))
                    {
                        return false;//有相邻相同数字
                    }
                }
            }
        }
        direction = 2;
        for (int m = 0; m < lines; m++) //不在上边缘
        {
            for (int n = columns - 2; n >= 0; n--)
            {
                if ((statesInCells[m][n] == 1) && (statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] == 1))
                {
                    if (IsEqual(createdNums[m][n], createdNums[m + directions[direction, 0]][n + directions[direction, 1]]))
                    {
                        return false;//有相邻相同数字
                    }
                }
            }
        }
        direction = 3;
        for (int m = 0; m < lines; m++) //不在下边缘
        {
            for (int n = 1; n < columns; n++)
            {
                if ((statesInCells[m][n] == 1) && (statesInCells[m + directions[direction, 0]][n + directions[direction, 1]] == 1))
                {
                    if (IsEqual(createdNums[m][n], createdNums[m + directions[direction, 0]][n + directions[direction, 1]]))
                    {
                        return false;//有相邻相同数字
                    }
                }
            }
        }
        return true;
    }
}

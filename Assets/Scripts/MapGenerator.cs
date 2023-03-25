using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    public GameObject Cell;
    // Start is called before the first frame update
    void Start()
    {
        instance  = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameManager.lines; i++)
        {
            for (int j = 0; j < GameManager.columns; j++)
            {
                Vector3 created_position = new Vector3(transform.position.x + i * GameManager.sizeOfCell * 1.05f, transform.position.y + j * GameManager.sizeOfCell * 1.05f, 0);
                Instantiate(Cell, created_position, transform.rotation, transform);
                //Created.GetComponent<NumsMover>().index_line = i;
                //Created.GetComponent<NumsMover>().index_line = j;
                //Debug.Log("第" + Created.GetComponent<NumsMover>().index_line + "行第"+ Created.GetComponent<NumsMover>().index_line + "列初始化成功");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RemoveAddedScore()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}

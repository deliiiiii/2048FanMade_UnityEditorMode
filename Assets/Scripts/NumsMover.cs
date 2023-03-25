using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NumsMover : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 target;
    public int num = 0;
    public int level = 0;
    public float moveVelocity = 20f;
    void Start()
    {
        target= transform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.isMoving)
        {
            if (target != transform.localPosition)
            {
                Debug.Log("ÒÆ¶¯ing");
                transform.localPosition = new Vector3(transform.localPosition.x + GameManager.directions[GameManager.direction, 0] * moveVelocity, transform.localPosition.y + GameManager.directions[GameManager.direction, 1] * moveVelocity, 0);
            }

            if (GameManager.direction == 0)
            {
                if (transform.localPosition.x < target.x)
                {
                    transform.localPosition = target;
                }
            }
            if (GameManager.direction == 1)
            {
                if (transform.localPosition.x > target.x)
                {
                    transform.localPosition = target;
                }
            }
            if (GameManager.direction == 2)
            {
                if (transform.localPosition.y > target.y)
                {
                    transform.localPosition = target;
                }
            }
            if (GameManager.direction == 3)
            {
                if (transform.localPosition.y < target.y)
                {
                    transform.localPosition = target;
                }
            }
        }
        
    }
}

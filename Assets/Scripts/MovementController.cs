using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public int countOfEnemys = 3;
    public float minSwipeDistance;
    public float moveSpeed = 5f;
    public float moveTime;
    public Transform movePiont;
    public LayerMask whatStopsMovement;
    [HideInInspector]
    public Vector3 direction = new Vector3(1, 0, 0);
    private Vector2 startTouchPosition, endTouchPosition;
    private float startTime;

    ObjectPooler objectPooler;


    // Start is called before the first frame update
    void Start()
    {
        movePiont.parent = null;
        startTime = Time.time;
        objectPooler = ObjectPooler.Instance;

        for (int i = 0; i <= countOfEnemys; i++)
        {
            objectPooler.SpawnFromPool("Enemy");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            direction = new Vector3(0, 0, 0);
            Staying();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;
            
            ChangeDirection(endTouchPosition);
        }


        transform.position = Vector3.MoveTowards(transform.position, movePiont.position,  moveSpeed * Time.deltaTime);
        
        
        
        if (Math.Abs(Time.time - startTime) > moveTime)
        {


            startTime = Time.time;
        }

       

        if (Vector3.Distance(transform.position, movePiont.position) <= .03f)
        {
            if (!Physics2D.OverlapCircle(movePiont.position + direction, 0.01f, whatStopsMovement))
            { 
                movePiont.position += direction;
                if (direction != new Vector3(0, 0, 0))
                {
                objectPooler.SpawnFromPool("Sound");
                
                
                

                    FindObjectOfType<AudioManager>().Volume("Theme", .3f);
                }
            }
            else 
            { Staying(); }
        }

        
    }

    void Staying()
    {
        FindObjectOfType<AudioManager>().Volume("Theme", .1f);
        direction = new Vector3(0, 0, 0);
    }

    Vector3 Direction()
    {
        return direction;
    }

    void ChangeDirection(Vector2 endTouchPosition)
    {

        float xSwipe = Math.Abs(endTouchPosition.x - startTouchPosition.x);
        float ySwipe = Math.Abs(endTouchPosition.y - startTouchPosition.y);


        if (xSwipe > ySwipe)
        {
            if (endTouchPosition.x > startTouchPosition.x && xSwipe > minSwipeDistance)
            {
                direction = new Vector3(1, 0, 0);
            }
            else
            if (endTouchPosition.x < startTouchPosition.x && xSwipe > minSwipeDistance)
            {
                direction = new Vector3(-1, 0, 0);
            }
        }
        else
        {
            if (endTouchPosition.y > startTouchPosition.y && ySwipe > minSwipeDistance)
            {
                direction = new Vector3(0, 1, 0);
            }
            else
            if (endTouchPosition.y < startTouchPosition.y && ySwipe > minSwipeDistance)
            {
                direction = new Vector3(0, -1, 0);
            }
        }
    }
}

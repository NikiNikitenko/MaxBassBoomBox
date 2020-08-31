using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = System.Random;

public class EnemyMovementController : MonoBehaviour, IPooledObject
{
    public Transform movePiont;
    public float moveSpeed = 5f;
    public LayerMask whatStopsMovement;
    public LayerMask sound;
    public LayerMask enemy;
    public GameObject enemyGO;

    Vector3 direction = new Vector3(0, 0, 0);
    Vector3 playerPosition = new Vector3(0, 0, 0);
    Random rnd = new Random();
    ObjectPooler objectPooler;
    double distanceX, distanceY;

    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
       // movePiont.parent = null;

        float x = rnd.Next(1, 6) - 3.5f;
        float y = rnd.Next(1, 10) - 5.5f;

        transform.position = new Vector3(x, y, 0);

        while(Physics2D.OverlapCircle(transform.position, 0.01f, whatStopsMovement))
        {
             x = rnd.Next(1, 6) - 3.5f;
             y = rnd.Next(1, 10) - 5.5f;

            transform.position = new Vector3(x, y, 0);
        }

        ChangeWalkingDirection();


        movePiont.position = transform.position + direction;

    }



    public void Remove()
    {
        movePiont.parent = transform;
       // enemyGO.SetActive(false);
        objectPooler.SpawnFromPool("Enemy");

    }
    public void Start()
    {
        movePiont.parent = null;
        objectPooler = ObjectPooler.Instance;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 soundWawePosition = new Vector3(0, 0, 0);

        transform.position = Vector3.MoveTowards(transform.position, movePiont.position, moveSpeed * Time.deltaTime);

        soundWawePosition = Shift();

        if (soundWawePosition != new Vector3(0, 0, 0))
        {
            transform.position = soundWawePosition;
            movePiont.position = transform.position;
        }
        else
        {
            if (Vector3.Distance(transform.position, movePiont.position) <= 0f)
            {
                if (FindObjectOfType<MovementController>().direction != new Vector3(0, 0, 0))
                {
                    ChangeWalkingDirection();
                        movePiont.position += direction;

                }
            }

        }
                    

        if (Physics2D.OverlapCircle(transform.position, 0.01f, whatStopsMovement))
        {
            Remove();
        }
        if (Physics2D.OverlapCircle(transform.position, 1.3f, enemy))
        {
            moveX();
        }
        

        //Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, 0.2f, enemy);

        //if (enemys.Length > 2)
        //{
        //    transform.position -= movePiont.position;
        //    //OnObjectSpawn();
        //}

    }
    void ChangeWalkingDirection()
    {
        playerPosition = FindObjectOfType<MovementController>().transform.position;
        distanceX = Math.Abs(transform.position.x - playerPosition.x);
        distanceY = Math.Abs(transform.position.y - playerPosition.y);

        #region wrong if
        //if (distanceX >= distanceY)
        //{
        //    if (transform.position.x < playerPosition.x)
        //    {
        //        direction = new Vector3(1, 0, 0);
        //    }
        //    else
        //    {
        //        direction = new Vector3(-1, 0, 0);
        //    }
        //    if (Physics2D.OverlapCircle(movePiont.position + direction, 0.01f, whatStopsMovement))
        //    {
        //        if (transform.position.y < playerPosition.y)
        //        { direction = new Vector3(0, 1, 0); }
        //        else
        //        { direction = new Vector3(0, -1, 0); }
        //    }
        //}
        //else
        //{
        //    if (transform.position.y < playerPosition.y)
        //    {
        //        direction = new Vector3(0, 1, 0);
        //    }
        //    else
        //    {
        //        direction = new Vector3(0, -1, 0);
        //    }
        //    if (Physics2D.OverlapCircle(movePiont.position + direction, 0.01f, whatStopsMovement))
        //    {
        //        if (transform.position.x < playerPosition.x)
        //        { direction = new Vector3(1, 0, 0); }
        //        else
        //        { direction = new Vector3(-1, 0, 0); }
        //    }
        //}
        #endregion


        if (distanceX >= distanceY)
        {
            moveX();
        }
        else
        {
            moveY();
        }
    }

    Vector3 Shift()
    {
        Vector3 soundWawePosition = new Vector3(0, 0, 0);
        Collider2D[] soundWawes = Physics2D.OverlapCircleAll(transform.position, 0.2f, sound);
        if (soundWawes.Length>0)
            foreach (Collider2D enem in soundWawes)
            {
                Debug.Log("We hit " + enem.name + " in x=" + enem.transform.position.x + " in y=" + enem.transform.position.y);
                soundWawePosition = enem.transform.position;
            }

        return soundWawePosition;
    }

    bool CheckDirection(Vector3 tempDirection)
    {
        return Physics2D.OverlapCircle(movePiont.position + tempDirection, 0.01f, whatStopsMovement);
    }
    void moveX()
    {
        Vector3 tempDirection = new Vector3(0, 0, 0);

        if (transform.position.x < playerPosition.x)
            tempDirection = new Vector3(1, 0, 0);
        else
            tempDirection = new Vector3(-1, 0, 0);

        if (CheckDirection(tempDirection))
        {
            moveY();
        }
        else
        {
            direction = tempDirection;
        }
    }
    void moveY()
    {
        Vector3 tempDirection = new Vector3(0, 0, 0);

        if (transform.position.y < playerPosition.y)
            tempDirection = new Vector3(0, 1, 0);
        else
            tempDirection = new Vector3(0, -1, 0);

        if (CheckDirection(tempDirection))
        {
            moveX();
        }
        else
        {
            direction = tempDirection;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaweMovementController : MonoBehaviour , IPooledObject
{
    public LayerMask enemyLayer;
    public float moveSpeed = 5f;
    public Transform movePiont;
    public Vector3 direction = new Vector3(0, 0, 0);
    public GameObject soundWawe;

    int counter = 0;
    
    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        movePiont.parent = null;
        direction = FindObjectOfType<MovementController>().direction;
        direction.x *= -1f;
        direction.y *= -1f;
        transform.position = FindObjectOfType<MovementController>().transform.position;// + direction;
        movePiont.position = transform.position + direction;
        
    }

    public void Remove()
    {
        movePiont.parent = transform;
        soundWawe.SetActive(false);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Math.Abs(transform.position.x - startPosition.x) > 3 || Math.Abs(transform.position.y - startPosition.y) > 3)
               
        {
            transform.position = Vector3.MoveTowards(transform.position, movePiont.position, moveSpeed * Time.deltaTime);
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 1f, enemyLayer);
            foreach(Collider2D enemy in enemies)
            {
                Debug.Log("we hit" + enemy.name);
            }
            
            if (Vector3.Distance(transform.position, movePiont.position) <= 0f)
            {
                if (FindObjectOfType<MovementController>().direction != new Vector3(0, 0, 0))
                {
                    if (counter < 4)
                    {
                        movePiont.position += direction;
                        counter++;
                    }
                    else 
                    {
                        Remove();
                        counter = 0;
                    }
                }
            }
        }
    }
}

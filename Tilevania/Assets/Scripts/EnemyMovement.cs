using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;
    BoxCollider2D walkingCollider;

    bool firstTimeChangeDirection = true;
    bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        walkingCollider = GetComponent<BoxCollider2D>();
        RandomizeCollider();
    }

    private void ChangeDirection()
    {
        if (firstTimeChangeDirection)
        {
            if (!isRight)
            {
                moveSpeed *= -1;
                gameObject.transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            }
            firstTimeChangeDirection = false;
            return;
        }

        gameObject.transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        moveSpeed *= -1f;
        
    }

    private void RandomizeCollider()
    {
        if (Random.Range(0, 2) == 0)
        {
            isRight = true;
        }
        else
        {
            isRight = false;
        }
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Ground"))
        {
            ChangeDirection();
        }
    }
}

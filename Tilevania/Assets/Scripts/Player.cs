using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(1f, 1f);

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    Vector2 movement;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeet;
    bool isClimbing = false;
    bool isTouchingGround;
    float gravityScaleAtStart;

    bool isAlive = true;
    

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Movement();
            ClimbLadder();
            Die();
        }
    }

    private void LateUpdate()
    {
        choosingAnimation();
    }

    private void choosingAnimation()
    {
        float xSpeed = myRigidbody.velocity.x;
        if (xSpeed != 0)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else if (xSpeed == 0)
        {
            myAnimator.SetBool("isRunning", false);
        }
        FlipSprite();
        
        

    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        bool vertical = Input.GetButtonDown("Vertical");
        movement = new Vector2(horizontal * runSpeed, myRigidbody.velocity.y);
        if (vertical && CanJump() && !myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            movement.y += jumpSpeed;
        }
        myRigidbody.velocity = movement;
    }

    private void ClimbLadder()
    {
        bool playerHasVerticalSpeed;
        
        if (isTouchingGround)
        {
            isClimbing = false;
            myAnimator.SetBool("isClimbing", false);
        }

        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            myAnimator.SetBool("isClimbing", false);
            myRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }

        myRigidbody.gravityScale = 0f;
        float controlThrow = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, controlThrow * climbSpeed);
        myRigidbody.velocity = climbVelocity;


        if (!isClimbing)
        {
            playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > 0;
            if (playerHasVerticalSpeed && !myRigidbody.IsTouchingLayers(LayerMask.GetMask("Foreground")))
            {
                isClimbing = true;
                myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
            }
        }
    }

    private bool CanJump()
    {
        return myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > 0;
        if ( playerHasHorizontalSpeed)
        {
            
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isTouchingGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingGround = false;
    }

    private void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            myAnimator.SetTrigger("Dying");
            GetComponent<Rigidbody2D>().velocity = deathKick;
            isAlive = false;
            StartCoroutine(WaitToLoad());
        }
    }

    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(3);
        FindObjectOfType<GameSession>().LoseLife();
    }
}

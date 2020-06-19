using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    // State
    bool isAlive = true;
    float gravityScaleAtStart;

    // Cached component references
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;

    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        ClimbLadder();
        FlipSprite();
        Jump();
    }

    private void ClimbLadder()
    {
        bool isTouchingLadder = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        
        if (!isTouchingLadder) {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

        myRigidBody.gravityScale = 0f;

        var controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // Value between -1 and +1
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);

        bool isClimbing = Mathf.Abs(controlThrow) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", isClimbing);
    }

    private void Jump()
    {
        int groundMask = LayerMask.GetMask("Ground");
        bool isTouchingGround = myFeetCollider.IsTouchingLayers(groundMask);
        if (!isTouchingGround) { return; }

        if ( CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool isPlayerRunning = Mathf.Abs(playerVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", isPlayerRunning);
    }



    private void FlipSprite()
    {
        bool isPlayerRunning = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (isPlayerRunning)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
}

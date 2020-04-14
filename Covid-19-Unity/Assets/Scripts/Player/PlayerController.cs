﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Bools:
    private bool isPaused = false;
    private bool isMove = false;
    private bool isRight = false; // keep track of what direction player is facing

    // Movement:
    public float playerSpeed = 5.0f; // player movement speed
    private Rigidbody2D playerRigidBody; // player rigid-body component
    private Vector2 playerVector; // keep track of the player's movement vector
    private Animator playerAnim; // animator to change player animations
    private SpriteRenderer playerSprite; // used to flip sprite

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerVector = new Vector2(0, 0);
    }   

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            // reset player vector
            playerVector.x = 0;
            playerVector.y = 0;

            // reset bools
            isMove = false;

            // get vertical movement
            if (Input.GetKey(KeyCode.W))
            {
                playerVector.y += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerVector.y -= 1;
            }
            // get horizontal movement
            if (Input.GetKey(KeyCode.D))
            {
                playerVector.x += 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                playerVector.x -= 1;
            }

            // if player has inputed to move
            if (playerVector.x != 0 || playerVector.y != 0)
            {
                isMove = true;
            }
            else 
            {
                playerAnim.SetFloat("x-move", 0f);
                playerAnim.SetFloat("y-move", 0f);
            }
            
        }
    }
    
    void FixedUpdate() 
    {
        if (isMove)
        {
            // apply player vector to rigid-body
            Vector3 movementVector = new Vector3(playerVector.x, playerVector.y, 0);
            movementVector = movementVector.normalized * playerSpeed * Time.deltaTime;
            // reduce speed when moving diagonal
            if (playerVector.x != 0 && playerVector.y != 0)
            {
                movementVector /= Mathf.Sqrt(2);
            }
            // flip player sprite if needed
            if (playerVector.x > 0 && isRight)
            {
                FlipPlayerSprite();
            }
            else if (playerVector.x < 0 && !isRight)
            {
                FlipPlayerSprite();
            }

            playerRigidBody.MovePosition(playerRigidBody.transform.position + movementVector);
            // change animator values to change animation
            playerAnim.SetFloat("x-move", (float)playerVector.x);
            playerAnim.SetFloat("y-move", (float)playerVector.y);
            playerAnim.SetFloat("x-prev", (float)playerVector.x);
            playerAnim.SetFloat("y-prev", (float)playerVector.y);
        }
    }

    private void FlipPlayerSprite()
    {
        isRight = !isRight;
        playerSprite.flipX = isRight;
    }   
}

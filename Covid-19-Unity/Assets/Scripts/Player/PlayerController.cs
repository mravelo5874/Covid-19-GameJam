using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Bools:
    private bool isMove = false;
    private bool isRight = false; // keep track of what direction player is facing

    // Movement:
    private float playerSpeed; // player movement speed
    private Rigidbody2D playerRigidBody; // player rigid-body component
    private Vector2 playerVector; // keep track of the player's movement vector
    private Vector2 directionVector; // what direction is the player facing
    private Vector3 movementVector; // player's movement vector
    private Animator playerAnim; // animator to change player animations
    private SpriteRenderer playerSprite; // used to flip sprite

    // Start is called before the first frame update
    void Start()
    {
        // load data from GameData:
        playerSpeed = GameData.instance.playerSpeed;

        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerVector = new Vector2(0, 0);
        directionVector = new Vector2(0, -1);
    }   

    // Update is called once per frame
    void Update()
    {
        // reset player vector
        playerVector.x = 0;
        playerVector.y = 0;

        if (!GameData.instance.isPaused)
        {
            // reset bools
            isMove = false;

            // get vertical movement
            if (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0)
            {
                playerVector.y += 1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < 0)
            {
                playerVector.y -= 1;
            }
            // get horizontal movement
            if (Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0)
            {
                playerVector.x += 1;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0)
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
                StopPlayerMovement();
            }    
        }
        else
        {
            StopPlayerMovement();
        }

        // pause feature
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
        {
            GameData.instance.PauseGame();
        }
    }

    private void StopPlayerMovement()
    {
        // stop player movement
        playerRigidBody.velocity = Vector2.zero;
        playerAnim.SetFloat("x-move", 0f);
        playerAnim.SetFloat("y-move", 0f);
    }
    
    void FixedUpdate() 
    {
        if (isMove)
        {
            // apply player vector to rigid-body
            movementVector = new Vector3(playerVector.x, playerVector.y, 0);
            movementVector = movementVector.normalized * playerSpeed * Time.fixedDeltaTime;
            // flip player sprite if needed
            if (playerVector.x > 0 && isRight)
            {
                FlipPlayerSprite();
            }
            else if (playerVector.x < 0 && !isRight)
            {
                FlipPlayerSprite();
            }

            playerRigidBody.velocity = movementVector;
            // change animator values to change animation
            playerAnim.SetFloat("x-move", (float)playerVector.x);
            playerAnim.SetFloat("y-move", (float)playerVector.y);
            playerAnim.SetFloat("x-prev", (float)playerVector.x);
            playerAnim.SetFloat("y-prev", (float)playerVector.y);

            // update player direction
            directionVector = playerVector;
        }
    }

    private void FlipPlayerSprite()
    {
        isRight = !isRight;
        playerSprite.flipX = isRight;
    } 

    public Vector2 GetPlayerDirection()
    {
        return directionVector;
    }

    public Vector2 getPlayerVelocity()
    {
        return playerRigidBody.velocity;
    }

    public void UpgradeSpeed()
    {
        playerSpeed += 25;
    }
}

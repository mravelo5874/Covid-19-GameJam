using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Bools:
    private bool isPaused = false;
    private bool isMove = false;

    // Movement:
    public float playerSpeed = 5.0f; // player movement speed
    private Rigidbody2D playerRigidBody; // player rigid-body component
    private Vector2 playerVector; // keep track of the player's movement vector

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
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

            if (playerVector.x != 0 || playerVector.y != 0)
            {
                isMove = true;
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

            playerRigidBody.MovePosition(playerRigidBody.transform.position + movementVector);
        }
    }
}

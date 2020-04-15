using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour
{
    // Position and Movement:
    public float speed;
    private Vector2Int position;
    private Vector2Int target;
    private Vector2Int npcVector;
    private Vector3 movementVector;


    // Components:
    private Animator animator;
    private SpriteRenderer npcSprite;
    private Rigidbody2D npcRigidbody;

    // Bools:
    [HideInInspector] public bool isMoving = false;
    private bool isRight = false;

    // Start is called before the first frame update
    void Start()
    {
        npcRigidbody = GetComponent<Rigidbody2D>();
        npcSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetFloat("x-move", 0f);
        animator.SetFloat("y-move", 0f);

        position = new Vector2Int(0, 0);
        position.x = (int)transform.localPosition.x;
        position.y = (int)transform.localPosition.y;
        transform.SetPositionAndRotation(new Vector3(position.x, position.y, 0f), Quaternion.identity);

        npcVector = new Vector2Int(0, 0);
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            movementVector = new Vector3(npcVector.x, npcVector.y, 0f);
            movementVector = movementVector.normalized * speed * Time.deltaTime;
            npcRigidbody.MovePosition(npcRigidbody.transform.position + movementVector);

            // get current position of npc
            position.x = (int)transform.localPosition.x;
            position.y = (int)transform.localPosition.y;
            
            // reached target destination
            if (position == target)
            {
                isMoving = false;
                // stop moving animation
                animator.SetFloat("x-move", 0f);
                animator.SetFloat("y-move", 0f);
            }
        }
    } 

    public void SimpleWalk(Direction direction, int units)
    {
        print ("direction: " + direction);
        print ("units: " + units);
        if (!isMoving)
        {
            isMoving = true;
            target = position;

            if (direction == Direction.up)
            {
                target.y += units;
                npcVector = new Vector2Int(0, 1);
            }
            else if (direction == Direction.up_right)
            {
                target.y += units;
                target.x += units;
                npcVector = new Vector2Int(1, 1);
            }
            else if (direction == Direction.right)
            {
                target.x += units;
                npcVector = new Vector2Int(1, 0);
            }
            else if (direction == Direction.down_right)
            {
                target.y -= units;
                target.x += units;
                npcVector = new Vector2Int(1, -1);
            }
            else if (direction == Direction.down)
            {
                target.y -= units;
                npcVector = new Vector2Int(0, -1);
            }
            else if (direction == Direction.down_left)
            {
                target.y -= units;
                target.x -= units;
                npcVector = new Vector2Int(-1, -1);
            }
            else if (direction == Direction.left)
            {
                target.x -= units;
                npcVector = new Vector2Int(-1, 0);
            }
            else if (direction == Direction.up_left)
            {
                target.y += units;
                target.x -= units;
                npcVector = new Vector2Int(-1, 1);
            }

            // change animator values to change animation
            animator.SetFloat("x-move", (float)npcVector.x);
            animator.SetFloat("y-move", (float)npcVector.y);
            animator.SetFloat("x-prev", (float)npcVector.x);
            animator.SetFloat("y-prev", (float)npcVector.y);

            // flip npc sprite if needed
            if (npcVector.x > 0 && isRight)
            {
                FlipSprite();
            }
            else if (npcVector.x < 0 && !isRight)
            {
                FlipSprite();
            }
        }
    }

    private void FlipSprite()
    {
        isRight = !isRight;
        npcSprite.flipX = isRight;
    } 
}

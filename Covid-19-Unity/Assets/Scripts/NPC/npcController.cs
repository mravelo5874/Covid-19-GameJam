using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour
{
    // Position and Movement:
    private float healthySpeed;
    private float infectedSpeed;
    private float currSpeed;
    private Vector2Int position;
    private Vector2Int target;
    private Vector2Int npcVector;
    private Vector2 directionVector; // what direction is the npc facing
    private Vector3 movementVector;
    // path for movement
    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    // Components:
    private Animator animator;
    private SpriteRenderer npcSprite;
    private Rigidbody2D npcRigidbody;

    // Bools:
    [HideInInspector] public bool isMoving = false;
    private bool isRight = false;

    // Colors:
    public Color healthyColor;
    public Color infectedColor;
    public Color deadColor;

    // Start is called before the first frame update
    void Start()
    {
        // Load data from GameData
        healthySpeed = GameData.instance.npcSpeedHealthy;
        infectedSpeed = GameData.instance.npcSpeedInfected;

        npcRigidbody = GetComponent<Rigidbody2D>();
        npcSprite = GetComponent<SpriteRenderer>();
        npcSprite.color = healthyColor;

        animator = GetComponent<Animator>();
        animator.SetFloat("x-move", 0f);
        animator.SetFloat("y-move", 0f);

        position = new Vector2Int(0, 0);
        position.x = (int)transform.localPosition.x;
        position.y = (int)transform.localPosition.y;
        transform.SetPositionAndRotation(new Vector3(position.x, position.y, 0f), Quaternion.identity);

        npcVector = new Vector2Int(0, 0);
        directionVector = new Vector2(0, -1);

        currSpeed = healthySpeed;
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement() 
    {
        if (pathVectorList != null) 
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f) 
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                int x = 0, y = 0;
                if (moveDir.x != 0)
                {
                    if (moveDir.x > 0)
                    {
                        x = 1;
                    }
                    else
                    {
                        x = -1;
                    }
                }
                if (moveDir.y != 0)
                {
                    if (moveDir.y > 0)
                    {
                        y = 1;
                    }
                    else
                    {
                        y = -1;
                    }
                }

                npcVector = new Vector2Int(x, y);
                directionVector = npcVector;

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

                // move distance
                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                transform.position = transform.position + moveDir * currSpeed * Time.deltaTime;
            } 
            else 
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) 
                {
                    pathVectorList = null;
                    animator.SetFloat("x-move", 0f);
                    animator.SetFloat("y-move", 0f);
                    isMoving = false;
                }
            }
        } 
        else 
        {
            animator.SetFloat("x-move", 0f);
            animator.SetFloat("y-move", 0f);
            isMoving = false;
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.instance.FindPath(transform.position, targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1) 
        {
            foreach (Vector3 vector in pathVectorList)
            {
                print (vector);
            }
            pathVectorList.RemoveAt(0);
            isMoving = true;
        }
    }

    private void FlipSprite()
    {
        isRight = !isRight;
        npcSprite.flipX = isRight;
    }

    public void BecomeInfected()
    {
        npcSprite.color = infectedColor;
        currSpeed = infectedSpeed;
    }

    public Vector2 getNPCVelocity()
    {
        return npcRigidbody.velocity;
    }

    public Vector2 GetNPCDirection()
    {
        return directionVector;
    }
}

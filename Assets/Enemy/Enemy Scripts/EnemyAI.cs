using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;

    [Header("Custom Behaviour")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;
    public float jumpForce = 0.3f;
    public string toJump;
    public bool enemyIsWalking = false;
    public float attackRange = 6f;
    public bool isJumping = false;
    public bool isAttacking1 = false;
    public bool isIdling = false;
    public float jumpHeight = 100f;
    public Vector2 enemyLateralVelocity;
    public LayerMask jumpLayerMask;
    public string leftRayHit;
    public string rightRayHit;
    public static bool enemyCanJump = false;
    public bool jumpCooldownOver = true;
    //public bool canWalk = true;

    private Path path;
    private int currentWaypoint = 0;
    bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;
    EnemyLookRotation enemyLookRot;
    
    

    //EnemyLookRotation enemyLookAt;

    //test code variables
    public GameObject colliderEdge;
    public float yvalue;
    public int jumpedNoOfTimes = 0;
    public float distanceFromLeftJumpCol = 0f;


    //test code variables

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);


        //test code
        yvalue = colliderEdge.transform.position.y;

        enemyLookRot = GetComponent<EnemyLookRotation>();

        //test code
    }
    private void Update()
    {
        enemyLateralVelocity = rb.velocity;
        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, Vector2.right, 3f, jumpLayerMask);
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, Vector2.left, 3f, jumpLayerMask);
        Debug.DrawRay(transform.position, Vector2.right * 3f, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * 3f, Color.blue);
        
        if (!enemyLookRot.isFlipped)
        {
            if (leftRay.collider != null)
            {
                leftRayHit = leftRay.collider.gameObject.tag;
                enemyIsWalking = false;
                if (leftRay.collider.gameObject.tag == "Jump" && isJumping == false && enemyCanJump == true && jumpCooldownOver)
                {
                    enemyIsWalking = false;
                    isAttacking1 = false;
                    StartCoroutine(EnemyJump());
                }
                distanceFromLeftJumpCol = Vector2.Distance(target.position, leftRay.collider.gameObject.transform.position);
                if (distanceFromLeftJumpCol <= 3f && distanceFromLeftJumpCol != 0f)
                {
                    enemyIsWalking = false;
                }
            }
            else
            {
                leftRayHit = "";
            }
           
        }

        else if(enemyLookRot.isFlipped)
        {
            if (rightRay.collider != null)
            {
                rightRayHit = rightRay.collider.gameObject.tag;
                enemyIsWalking = false;
                if (rightRay.collider.gameObject.tag == "Jump" && isJumping == false && enemyCanJump == true && jumpCooldownOver)
                {
                    enemyIsWalking = false;
                    isAttacking1 = false;
                    StartCoroutine(EnemyJump());
                }
            }
            else
            {
                rightRayHit = "";
            }
        }
        
    }

   

    private void FixedUpdate()
    {
        if(TargetInDistance() && followEnabled && Vector2.Distance(target.position, rb.position) >= attackRange)
        {
            isAttacking1 = false;
            enemyIsWalking = true;
            if(Vector2.Distance(target.position, rb.position) >= activateDistance)
            {
                isIdling = true;
            }
            else
            {
                isIdling = false;
            }
            if(enemyIsWalking)
            {
                PathFollow();
            }
        }
        else
        {
            if (!isJumping)
            {
                enemyIsWalking = false;
            }
        }

        if(Vector2.Distance(target.position, rb.position) <= attackRange)
        {
            isAttacking1 = true;
            enemyIsWalking = false;
        }
        else
        {
            isAttacking1 = false;
            enemyIsWalking = true;
            if(Vector2.Distance(target.position, rb.position) <= activateDistance)
            {
                isIdling = false;
            }
            else
            {
                isIdling = true;
            }
        }
    }

    private void UpdatePath()
    {
        if(followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        // Jump
        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                
                //rb.AddForce(Vector2.up * speed * jumpModifier);
            }
        }

        // Movement
        rb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        
        // Direction Graphics Handling
        //if (directionLookEnabled)
        //{
        //    if (rb.velocity.x > 0.05f)
        //    {
        //        transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //    }
        //    else if (rb.velocity.x < -0.05f)
        //    {
        //        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //    }
        //}
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    public void EnemyJumpadsfds()
    {
        rb.velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 4, jumpHeight);
    }

    IEnumerator EnemyJump()
    {
        jumpCooldownOver = false;
        
        isJumping = true;
        isAttacking1 = false;
        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 4, jumpHeight);
        yield return new WaitForSeconds(1.2f);
        isJumping = false;
        
        StartCoroutine(EnemyJumpCooldown());
    }

    IEnumerator EnemyJumpCooldown()
    {
        yield return new WaitForSeconds(5f);
        jumpCooldownOver = true;
        
    }
}

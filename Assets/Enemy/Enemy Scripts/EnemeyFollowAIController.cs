using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemeyFollowAIController : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public float attackDistance = 6f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;
    //public bool isGrounded = false;
    public LayerMask jumpLayerMask;
    public LayerMask groundLayers;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    
    Seeker seeker;
    Rigidbody2D rb;

    //Animation 
    const string ENEMY_IDLE = "Idle";
    const string ENEMY_WALK = "Walk";
    const string ENEMY_JUMP = "Jump";
    const string ENEMY_ATTACK = "Attack";
    const string ENEMY_GETTING_HIT = "GetHit";
    const string ENEMY_FALL_IDLE = "FallIdle";
   // const string ENEMY_FALL_LAND = "FallLand";
    [SerializeField]
    private EnemyAnimationController enemyAnimController;
    [SerializeField]
    private Animator enemyAnim;
    [SerializeField]
    private string currentState;

    //Custom variables
    [SerializeField]
    //private Vector2 enemyLateralVelocity;
    private float jumpHeight = 100f;
    

    EnemyLookRotation enemyLookRot;
    

    [Header("Debug Variables")]
    [SerializeField]
    string leftRayHit = "";
    [SerializeField]
    string rightRayHit = "";
    [SerializeField]
    string gettingHit = "";
    [SerializeField]
    Vector2 rb_velocity;
    [SerializeField]
    int avg_vertical_vel = 0;
    [SerializeField]
    string Land = "";
    //AnimationStateBooleans
    [Header("Animation State Booleans")]
    public bool isGrounded;
    public bool isJumping = false;
    public bool isFalling = false;
    //public bool isCloseToLanding = false;
    
    public bool canJump = true;
    public bool isLanding = false;
    public bool isAttacking = false;
    public bool isGettingHit = false;
    public bool attackMode;
    public int meleeIdle = 0;
    public int meleeAttack = 0;
    
   


    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
        enemyLookRot = GetComponent<EnemyLookRotation>();
        enemyAnim = GetComponentInChildren<Animator>();
        //isCloseToLanding = false;
        
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f),
            new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), groundLayers);

        
        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, Vector2.right, 3f, jumpLayerMask);
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, Vector2.left, 3f, jumpLayerMask);
        Debug.DrawRay(transform.position, Vector2.right * 3f, Color.red);
        Debug.DrawRay(transform.position, Vector2.left * 3f, Color.blue);

        //debug code starts here
        rb_velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        if(rb_velocity.y >= -0.5f && rb_velocity.y <= 0.5f)
        {
            avg_vertical_vel = 0;
        }
        else
        {
            avg_vertical_vel = Mathf.CeilToInt(rb_velocity.y);
        }

        //if(avg_vertical_vel < -1 && !isJumping)
        //{
        //    isFalling = true;
        //}
        //else
        //{
        //    isFalling = false;
        //}
        

        if(!isGrounded && !isJumping)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
        
        //debug code ends here



        if (!enemyLookRot.isFlipped)
        {
            if (leftRay.collider != null)
            {
                leftRayHit = leftRay.collider.gameObject.tag;

                if (leftRay.collider.gameObject.tag == "Jump" && !isJumping && canJump && !TargetInAttackDistance() && !isGettingHit)
                {
                    StartCoroutine(EnemyJump());
                }

            }
            else
            {
                leftRayHit = "";
            }

        }

        else if (enemyLookRot.isFlipped)
        {
            if (rightRay.collider != null)
            {
                rightRayHit = rightRay.collider.gameObject.tag;

                if (rightRay.collider.gameObject.tag == "Jump" && !isJumping && canJump && !TargetInAttackDistance() && !isGettingHit)
                {
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
        if(isGettingHit)
        {
            ChangeAnimationState(ENEMY_GETTING_HIT);
        }
        if(TargetInAttackDistance() && isGrounded && !isJumping && !isGettingHit && !isFalling)
        {
            attackMode = true;
            ChangeAnimationState(ENEMY_ATTACK);
        }
        else if(TargetInAttackDistance() && isGrounded && !isJumping && !isGettingHit && !isFalling)
        {
            ChangeAnimationState(ENEMY_IDLE);
        }
        else if (TargetInDistance() && followEnabled && !isGettingHit && !isFalling)
        {
            attackMode = false;
            PathFollow();
        }
        else if (isFalling)
        {
            //if(!isCloseToLanding)
            //{
            //    ChangeAnimationState(ENEMY_FALL_IDLE);
            //}
            //if (!isGrounded)
            //{
            //    isCloseToLanding = Physics2D.OverlapArea(new Vector2(transform.position.x - 1.5f, transform.position.y - 1.5f),
            //    new Vector2(transform.position.x + 1.5f, transform.position.y + 1.5f), groundLayers);
            //    if(isCloseToLanding && !isGrounded && !isLanding)
            //    {
            //        //Debug code
            //        Land = "landing time!";
            //        //Debug code
            //        isLanding = true;
            //        //ChangeAnimationState(ENEMY_FALL_LAND);
            //        StartCoroutine(EnemyLanding());
            //    }
            //}

            if (!isGrounded && !isJumping)
            {
                ChangeAnimationState(ENEMY_FALL_IDLE);
            }

            //if (isCloseToLanding && canLand)
            //{
            //    canLand = false;
                
            //    ChangeAnimationState(ENEMY_FALL_LAND);
            //    StartCoroutine(EnemyLanding());
            //}
        }
        else
        {
            if(!isGettingHit)
            {
                ChangeAnimationState(ENEMY_IDLE);
            }
            
        }

        


    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
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
        //isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        if(isGrounded && !isJumping && !isGettingHit && !TargetInAttackDistance())
        {
            ChangeAnimationState(ENEMY_WALK);
        }
        //isCloseToLanding = false;


        

        // Movement
        rb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private bool TargetInAttackDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < attackDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator EnemyJump()
    {
        isJumping = true;
        canJump = false;
        if (!isGettingHit)
            ChangeAnimationState(ENEMY_JUMP);
        
        rb.velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 1f, jumpHeight);
        yield return new WaitForSeconds(1.5f);
        isJumping = false;
        StartCoroutine(EnemyJumpCooldown());
    }

    void ChangeAnimationState(string newState)
    {
        // stop animation from repeating itself
        if (currentState == newState)
            return;

        //play new animation
        enemyAnim.Play(newState);

        //reassign new state to current state
        currentState = newState;
    }

    IEnumerator EnemyJumpCooldown()
    {
        yield return new WaitForSeconds(3f);
        canJump = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyHit"))
        {
            isGettingHit = true;
            gettingHit = ">___<";
            rb.velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * -4, 0);
            StartCoroutine(EnemyHitCooldown());
        }
        else
        {
            gettingHit = "";
        }
        
    }
    IEnumerator EnemyHitCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isGettingHit = false;
    }

    
    //IEnumerator EnemyLanding()
    //{
    //    yield return new WaitForSeconds(0.12f);
    //    isLanding = false;
    //    //Debug code
    //    Land = "";
    //    //Debug code
        
    //}

    

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ukiyoe
{
    public class Jump : Abilities
    {
        [SerializeField]
        protected bool limitAirJumps;
        [SerializeField]
        protected int MaxJumps; 
        [SerializeField]
        protected float jumpForce;
        [SerializeField]
        protected float holdForce;
        [SerializeField]
        protected float buttonHoldTime;
        [SerializeField]
        protected float distanceToCollider;
        [SerializeField]
        protected float maxJumpSpeed;
        [SerializeField]
        protected float maxFallSpeed;
        [SerializeField]
        protected float acceptedFallSpeed;
        [SerializeField]
        protected float glideTime;
        [SerializeField]
        [Range(-2, 2)]
        protected float gravity;
        [SerializeField]
        protected LayerMask collisionLayer;
        [SerializeField]
        
        private bool isJumping;
        private float jumpCountDown;
        private float fallCountDown;
        private int numberOfJumpsLeft;


        protected override void Initialization()
        {
            base.Initialization();
            numberOfJumpsLeft = MaxJumps;
            jumpCountDown = buttonHoldTime;
            fallCountDown = glideTime;
        }
        // Update is called once per frame
        protected virtual void Update()
        {
            JumpPressed();
            JumpHeld();

        }

        protected virtual bool JumpPressed()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            { 
                if (!character.isGrounded  && numberOfJumpsLeft==MaxJumps )
                {
                    isJumping = false;
                    return false;
                }
                if(limitAirJumps && Falling(acceptedFallSpeed))
                {
                    isJumping = false;
                    return false;
                }
                numberOfJumpsLeft--;
                if(numberOfJumpsLeft >= 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    jumpCountDown = buttonHoldTime;
                    isJumping = true;
                    fallCountDown = glideTime;
                }
              
                return true;
            }
            else
                return false;
        }
        protected virtual bool JumpHeld()
        {
            if(Input.GetKey(KeyCode.Space))
            {
                return true;

            }
            return false;
        }
        protected virtual void FixedUpdate()
        {
            IsJumping();
            Gliding();
            GroundCheck();
        }

        protected virtual void IsJumping()
        {
            if (isJumping)
            {
                //rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);
                AdditionalAir();
            }

            if(rb.velocity.y>maxJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
            }
        }

        protected virtual void Gliding()
        {
            if(Falling(0) && JumpHeld())
            {
                fallCountDown -= Time.deltaTime;
                if(fallCountDown > 0 && rb.velocity.y > acceptedFallSpeed)
                {
                    anim.SetBool("Gliding", true);
                    FallSpeed(gravity);
                    return;
                }

            }
            anim.SetBool("Gliding", false);
        }

        protected virtual void AdditionalAir()
        {
            if (JumpHeld())
            {
                jumpCountDown -= Time.deltaTime;
                if (jumpCountDown <= 0)
                {
                    jumpCountDown = 0;
                    isJumping = false;
                }
                else
                    rb.AddForce(Vector2.up * holdForce);

            }
            else
                isJumping = false;
        }
        protected virtual void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !isJumping)
            {
                anim.SetBool("Grounded", true);
                character.isGrounded = true;
                EnemyAI.enemyCanJump = true;
                numberOfJumpsLeft = MaxJumps;
            }
            else
            {
                anim.SetBool("Grounded", false);
                character.isGrounded = false;
                EnemyAI.enemyCanJump = false;
                if(Falling(0) && rb.velocity.y < maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
                }
            }
            anim.SetFloat("VerticalSpeed", rb.velocity.y); 
        }
    }
}

/*2017 Alec Day*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutController : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator anim;

    private bool grounded;
    private bool canWJump;
    private int attacking;
    [SerializeField] private bool wallJumping;
    private bool knockback;

    private GameManager gman;

    public WeaponChanger weaponSys;
    public float topSpeed;
    public float runSpeed;
    public float brakeSpeed;
    
    public float jumpStrength;
    public float checkDist = 3.0f;

    public bool invincibile = false;
    public float invincibilityLength = 5.0f;
    public float invincibilityTimer = 0.0f;

    public uint usedJumps = 0;
    public uint maxJumps = 1;

    [SerializeField] private BoxCollider2D defaultCollider;
    [SerializeField] private BoxCollider2D halfCollider;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gman = GameObject.Find("GameManager").GetComponent<GameManager>();


        defaultCollider.enabled = true;
        halfCollider.enabled = false;

    }

    void Update () {
	    if(invincibilityTimer > 0.0f)
        {
            invincibile = true;
            invincibilityTimer -= Time.deltaTime;
        }

        if(invincibilityTimer <= 0.0f)
        {
            invincibile = false;
        }
	}

    private void FixedUpdate()
    {
        //Ground Check  
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.size.y / 2.0f);

        RaycastHit2D groundCheck = Physics2D.Raycast(checkPos, checkDist * Vector2.down, checkDist);
        Debug.DrawRay(checkPos, Vector2.down * checkDist, Color.green);

        if (groundCheck.collider != null)
        {
            usedJumps = 0;
            wallJumping = false;
            canWJump = false;
            knockback = false;
            grounded = true;
            transform.rotation = Quaternion.identity;
            rb.gravityScale = 1.0f;
        }


        else
        {
            grounded = false;
        }

        anim.SetBool("Grounded", grounded);

        
        /* *************** */ 
        //All you'll ever be doing is running right...so...
        Vector2 movement = new Vector2(runSpeed, 0.0f);
        if (!wallJumping || !canWJump || !knockback || !rb.IsTouchingLayers(LayerMask.NameToLayer("FallZone")))
            rb.AddForce(movement);

        if(rb.IsTouchingLayers(LayerMask.NameToLayer("FallZone")) && !grounded)
        {
            rb.gravityScale = 3.0f;
            anim.Play("Fall");
        }
        //Any time actions:
        HandleJump();
        HandleSlide();
        HandleBrake();
        HandleSlash();
        HandleWallJump();
    }


    //All of Scout's Moveset:
    private void HandleJump()
    {
        if (Input.GetButtonDown("Action") && Input.GetAxis("Vertical") >= 0.0f)
        {
            if (usedJumps < maxJumps)
            {
                defaultCollider.enabled = true;
                halfCollider.enabled = false;
                Input.ResetInputAxes();
                rb.gravityScale = 1.0f;
                Vector2 jump = new Vector2(rb.velocity.x, jumpStrength);
                anim.SetBool("Sliding", false);

                if(usedJumps == 0)
                    anim.Play("Jump");

                if (usedJumps == 1)
                    anim.Play("DoubleJump");

                rb.AddForce(jump);
                rb.gravityScale = 3.0f;
                usedJumps++;
            }
        }
    }

    private void HandleWallJump()
    {
        if(Input.GetButtonDown("Action") && canWJump)
        {
            wallJumping = true;
            rb.gravityScale = 1.0f;
            Vector2 jump = new Vector2(-Mathf.Sign(transform.forward.x) * 750.0f, jumpStrength);
            Vector3 rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            transform.rotation = Quaternion.Euler(rot);
            rb.AddForce(jump);
            rb.gravityScale = 3.0f;


        }
    }

    private void HandleBrake()
    {
        if(Input.GetAxis("Horizontal") < 0.0f)
        {
            anim.SetBool("Brake", true);
            if(runSpeed > brakeSpeed)
            {
                runSpeed -= 1.75f;
            }
        }

        if(Input.GetAxis("Horizontal") >= 0.0f)
        {
            anim.SetBool("Brake", false);
            if (runSpeed < topSpeed)
            {
                runSpeed += 2.5f;
            }
        }
    }

    private void HandleSlide()
    {
        if(Input.GetButtonDown("Slide"))
        {
            transform.Translate(0.0f, -0.5f, 0.0f);

        }

        if (Input.GetButton("Slide"))
        {
            anim.SetBool("Sliding", true);
            defaultCollider.enabled = false;
            halfCollider.enabled = true;
        }

        if(Input.GetButtonUp("Slide"))
        {
            anim.SetBool("Sliding", false);
            defaultCollider.enabled = true;
            halfCollider.enabled = false;
        }
    }

    //Slash Encodings:
    /*
     * 0 = Front, 1 = Up, 2 = Down, 3 = Air
     * */
    private void HandleSlash()
    {
        if (Input.GetButtonDown("Slash") && grounded)
        {
            //Front Slash
            if(Input.GetAxis("Vertical") == 0.0f)
            {
                anim.Play("FrontSlash");

            }

            //Up Slash
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                anim.Play("UpSlash");

            }

            //Down Slash
            if (Input.GetAxis("Vertical") < 0.0f)
            {
                anim.Play("DownSlash");
            }
        }

        if(Input.GetButtonDown("Slash") && !grounded)
        {

            anim.Play("AirSlash");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Loop"))
        {
            Transform loopPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
            transform.position = loopPoint.position;
        }

        if(collision.tag.Equals("SmallTreasure"))
        {
            Vector3 spawnMsg = new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
            gman.score += 50;
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 50;

            

            Destroy(collision.gameObject);
        }

        if (collision.tag.Equals("WJump"))
        {
            canWJump = true;
            anim.Play("WallGrab");
        }

        if(collision.tag.Equals("Enemy_Fly") && attacking == 0)
        {
            if (!invincibile)
            {
                gman.life--;
                anim.SetTrigger("Ouch");
                knockback = true;
                invincibilityTimer = invincibilityLength;
                rb.gravityScale = 3.0f;
                rb.AddForce(new Vector2(-1000.0f, -550.0f));
            }
        }

    }

    public void SetAttacking(int value)
    {
        attacking = value;

        weaponSys.activeWeapon.GetComponent<BoxCollider2D>().enabled = (value == 0) ? false : true;

        Debug.Log(value);
    }

    public void EnableAbility(int id)
    {
        switch(id)
        {
            case 0:
                maxJumps = 1;
                break;

            case 1:
                maxJumps = 2;
                break;
        }
    }
}

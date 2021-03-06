﻿/*2017 Alec Day*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutController : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator anim;

    private bool dead = false;
    [SerializeField] private bool grounded;
    private bool canFStep = false;
    private bool canECrash = false;
    private bool earthCrashing = false;
    private bool canWJump;
    private int attacking;
    [SerializeField] private bool wallJumping;
    private bool knockback;
    private bool wallHit = false;

    private GameManager gman;
    private ParticleSystem fStepPs;

    public WeaponChanger weaponSys;
    public float topSpeed;
    public float runSpeed;
    public float brakeSpeed;
    
    public float jumpStrength;
    public float checkDist = 3.0f;

    public bool invincibile = false;
    public float invincibilityLength = 5.0f;
    public float invincibilityTimer = 0.0f;

    public float fStepLength = 2.0f;
    public float fStepTimer = 0.0f;

    public uint usedJumps = 0;
    public uint maxJumps = 1;

    public AudioClip jumpSnd;
    public AudioClip swingSnd;
    public AudioClip slashSnd;
    public AudioClip transSnd;
    public AudioClip chestSnd;
    public AudioClip jewelSnd;
    public AudioClip dmgSnd;
    public AudioClip dieSnd;
    private AudioSource asrc;

    [SerializeField] private BoxCollider2D defaultCollider;
    [SerializeField] private BoxCollider2D halfCollider;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gman = GameObject.Find("GameManager").GetComponent<GameManager>();
        fStepPs = GetComponent<ParticleSystem>();
        asrc = GetComponent<AudioSource>();

        defaultCollider.enabled = true;
        halfCollider.enabled = false;

    }

    void Update () {

        //Timers for invicibility, etc.
	    if(invincibilityTimer > 0.0f)
        {
            invincibile = true;
            invincibilityTimer -= Time.deltaTime;
        }

        if (fStepTimer > 0.0f && canFStep)
        {
            invincibile = true;
            fStepTimer -= Time.deltaTime;
        }

        if (invincibilityTimer <= 0.0f || (fStepTimer <= 0.0f && canFStep))
        {
            invincibile = false;
        }

        if(gman.life == 0 && !dead)
        {
            KillAnimation();
            dead = true;
        }

	}

    private void FixedUpdate()
    {
        if (!dead)
        {
            CheckGrounded();
            /* *************** */
            //All you'll ever be doing is running right...so...
            Vector2 movement = new Vector2(runSpeed, 0.0f);
            if (!wallHit || !wallJumping || !canWJump || !knockback || !rb.IsTouchingLayers(LayerMask.NameToLayer("FallZone")) || (!earthCrashing && canECrash))
                rb.AddForce(movement);

            if (earthCrashing)
                rb.velocity = new Vector2(0.0f, rb.velocity.y);

            if (rb.IsTouchingLayers(LayerMask.NameToLayer("FallZone")) && !grounded)
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

            //Special Actions:
            if (canFStep)
                HandleFlashStep();

            if (canECrash)
                HandleEarthCrash();
        }
    }


    //All of Scout's Moveset:
    private void HandleJump()
    {
        if (Input.GetButtonDown("Action"))
        {
            if (usedJumps < maxJumps)
            {
                FPlay(jumpSnd);
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
            FPlay(jumpSnd);

            rb.velocity = new Vector2(0.0f, 0.0f);
            wallJumping = true;
            rb.gravityScale = 1.0f;
            Vector2 jump = new Vector2(-Mathf.Sign(transform.forward.x) * 820.0f, jumpStrength);
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
        if (Input.GetAxis("Horizontal") == 0.0f && grounded)
        {
            if (Input.GetButtonDown("Slide"))
            {
                transform.Translate(0.0f, -1.0f, 0.0f);

            }

            if (Input.GetButton("Slide"))
            {
                anim.SetBool("Sliding", true);
                defaultCollider.enabled = false;
                halfCollider.enabled = true;
            }

            if (Input.GetButtonUp("Slide"))
            {
                anim.SetBool("Sliding", false);
                defaultCollider.enabled = true;
                halfCollider.enabled = false;
            }
        }
    }

    //Slash Encodings:
    /*
     * 0 = Front, 1 = Up, 2 = Down, 3 = Air
     * */
    private void HandleSlash()
    {
        if (Input.GetButtonDown("Slash") && grounded && fStepTimer <= 0.0f && !anim.GetBool("Sliding"))
        {

            //Front Slash
            if(Input.GetAxis("Vertical") == 0.0f)
            {
                anim.Play("FrontSlash");
                FPlay(swingSnd);

            }

            //Up Slash
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                anim.Play("UpSlash");
                FPlay(swingSnd);
            }

            //Down Slash
            if (Input.GetAxis("Vertical") < 0.0f)
            {
                anim.Play("DownSlash");
                FPlay(swingSnd);
            }
        }

        if(Input.GetButtonDown("Slash") && !grounded)
        {

            anim.Play("AirSlash");
            FPlay(swingSnd);
        }
    }

    private void HandleFlashStep()
    {
        if (Input.GetAxis("Horizontal") > 0.0f)
        {
            if (Input.GetButtonDown("Slide") && grounded && fStepTimer <= 0.0f)
            {
                rb.AddForce(new Vector2(380.0f, 0.0f));
                anim.Play("FlashStep");
                fStepPs.Play();
                fStepTimer = fStepLength;
            }
        }
    }

    private void HandleEarthCrash()
    {
        if(Input.GetButtonDown("Slide") && !grounded && !earthCrashing && Input.GetAxis("Vertical") < 0.0f)
        {

            rb.AddForce(new Vector2(0.0f, -2000.0f));
            anim.SetBool("Crash", true);
            SetAttacking(1);
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
            FPlay(jewelSnd);
            Vector3 spawnMsg = new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
            gman.score += 50;
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 50;

            Destroy(collision.gameObject);
        }

        if (collision.tag.Equals("LargeTreasure"))
        {
            FPlay(chestSnd);

            Vector3 spawnMsg = new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
            gman.score += 1500;
            GameObject pntGet = Instantiate(gman.normalScorePopup, spawnMsg, Quaternion.identity);
            pntGet.GetComponent<ScorePopup>().pointValue = 1500;

            Destroy(collision.gameObject);
        }

        if (collision.tag.Equals("WJump"))
        {
            canWJump = true;
            anim.Play("WallGrab");
        }
        
        if(!collision.tag.Equals("WJump"))
        {
            canWJump = false;
        }
        if(collision.tag.Equals("Enemy_Fly") && attacking == 0)
        {
            if (!invincibile)
            {
                FPlay(dmgSnd);
                gman.life--;
                anim.SetTrigger("Ouch");
                knockback = true;
                invincibilityTimer = invincibilityLength;
                rb.gravityScale = 3.0f;
                rb.AddForce(new Vector2(-1000.0f, -550.0f));
            }
        }

        if ((collision.tag.Equals("Enemy_Ground") || collision.tag.Equals("Enemy_Under")) && attacking == 0)
        {
            if (!invincibile)
            {
                FPlay(dmgSnd);
                gman.life--;
                anim.SetTrigger("Ouch");
                knockback = true;
                invincibilityTimer = invincibilityLength;
                rb.gravityScale = 3.0f;
                rb.AddForce(new Vector2(-1000.0f, 0.0f));
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
                canFStep = false;
                canECrash = false;
                break;

            case 1:
                maxJumps = 2;
                canFStep = false;
                canECrash = false;
                break;

            case 2:
                maxJumps = 1;
                canFStep = true;
                canECrash = false;
                break;

            case 3:
                maxJumps = 1;
                canFStep = false;
                canECrash = true;
                break;
        }
    }

    public void KillAnimation()
    {
        gman.GetComponent<AudioSource>().Stop();
        rb.velocity = new Vector2(0.0f, 0.0f);
        rb.AddForce(new Vector2(0.0f, 900.0f));
        foreach(BoxCollider2D bc in GetComponents<BoxCollider2D>())
        {
            Destroy(bc);
        }
        Camera.main.GetComponent<CameraFollow>().deadPlayer = true;
        anim.SetBool("Dead", true);
        gman.death = true;
        FPlay(dieSnd);

    }

    //Helpers 

    private void CheckGrounded()
    {

        //Ground Check  
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.size.y / 2.0f);

        //Right Vector Check
        Vector2 sidePos = new Vector2(transform.position.x + GetComponent<BoxCollider2D>().bounds.size.x / 4.0f, transform.position.y);

        RaycastHit2D groundCheck = Physics2D.Raycast(checkPos, checkDist * -transform.up, checkDist);
        RaycastHit2D sideCheck = Physics2D.Raycast(sidePos, checkDist / 2.0f * transform.right, checkDist);

        Debug.DrawRay(checkPos, -transform.up * checkDist, Color.green);
        Debug.DrawRay(sidePos, transform.right * checkDist * 0.5f, Color.red);


        if (groundCheck.collider != null)
        {
            float dotProductCheck = Vector2.Dot(-transform.up, groundCheck.transform.up);
            Debug.Log(dotProductCheck);

            if (dotProductCheck < -0.5f)
            {
                usedJumps = 0;

                if (earthCrashing)
                {
                    earthCrashing = false;
                    anim.SetBool("Crash", false);
                    SetAttacking(0);
                }
                wallJumping = false;
                canWJump = false;
                knockback = false;
                grounded = true;
                transform.rotation = Quaternion.identity;
                rb.gravityScale = 1.0f;
            }
        }

        

        else
        {
            grounded = false;
        }

        anim.SetBool("Grounded", grounded);

        if (sideCheck.collider != null && !sideCheck.collider.isTrigger)
        {
            float dotprod = Vector2.Dot(sidePos, -sideCheck.collider.transform.right);

            if (dotprod <= -1.0f)
                wallHit = true;

            else
                wallHit = false;
        }
    }

    public void FPlay(AudioClip clip)
    {
        asrc.clip = clip;
        asrc.Play();
    }
}

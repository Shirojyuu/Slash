﻿/*2017 Alec Day*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutController : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator anim;

    private bool grounded;

    public float runSpeed;
    public float jumpStrength;
    public float checkDist = 3.0f;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	void Update () {
		
	}

    private void FixedUpdate()
    {
        //Ground Check  
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - GetComponent<BoxCollider2D>().bounds.size.y / 2.0f);

        RaycastHit2D groundCheck = Physics2D.Raycast(checkPos, checkDist * Vector2.down, checkDist);
        Debug.DrawRay(checkPos, Vector2.down * checkDist, Color.green);

        if (groundCheck.collider != null)
        {
            Debug.Log(groundCheck.collider);
            grounded = true;
        }

        else
        {
            grounded = false;
        }

        anim.SetBool("Grounded", grounded);

        Debug.Log(grounded);
        //All you'll ever be doing is running right...so...
        Vector2 movement = new Vector2(runSpeed, 0.0f);
        rb.AddForce(movement);

        //Any time actions:
        HandleJump();
        HandleBrake();
        HandleSlash();
    }


    //All of Scout's Moveset
    private void HandleJump()
    {
        if (Input.GetButtonDown("Action") && grounded)
        {
            Input.ResetInputAxes();
            Vector2 jump = new Vector2(rb.velocity.x, jumpStrength);
            anim.SetTrigger("Jump");
            rb.AddForce(jump);
        }
    }

    private void HandleBrake()
    {

    }

    //Slash Encodings:
    /*
     * 0 = Front, 1 = Up, 2 = Down, 3 = Air
     * */
    private void HandleSlash()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Loop"))
        {
            Transform loopPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
            transform.position = loopPoint.position;
        }
    }
}

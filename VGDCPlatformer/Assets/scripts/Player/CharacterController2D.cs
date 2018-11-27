﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour {

    [SerializeField] private float m_JumpForce = 800f;
    [SerializeField] public int m_AirJumps = 0;
    [SerializeField] private float m_FallGravity = 4f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_HorizontalCheck;
    [SerializeField] private bool m_AirControl = true;
    [SerializeField] private float m_JumpForceOnEnemies = 20;

    private bool m_WasSliding = false;
    private bool m_Grounded;
    public bool m_FacingRight = true;
    private bool m_OnJumpPad = false;
    public bool m_Damaged;
    public bool m_Immune = false;
    private int m_AirJumpsLeft;
    private Vector3 m_Velocity = Vector3.zero;

    [HideInInspector] public Rigidbody2D m_RigidBody2D;
    private Animator animator;

    private PlayerHealth health;

    void Awake()
    {
        m_RigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        m_Grounded = Physics2D.Linecast(transform.position, m_GroundCheck.position, m_GroundLayer);
        if (m_Grounded)
        {
            JumpadOff();
            m_AirJumpsLeft = m_AirJumps;
        }
    }

    private void Update()
    {
        if (m_Grounded)
        {
            animator.SetBool("jumping", false);
        }
    }

    public void Move(float move, bool jump, bool slide, bool duck, bool missed, bool on_beat)
    {
        if (missed)
        {
            health.TakeDamage();
        }

        if (jump && !on_beat)
        {
            health.TakeDamage();
            print("Jump On Beat? " + on_beat);
        }

        if (duck && !on_beat)
        {
            health.TakeDamage();
            print("Duck On Beat? " + on_beat);
        }

        if (slide && !m_WasSliding && !on_beat)
        {
            health.TakeDamage();
            m_WasSliding = true;
            print("Starting slide");
            print("Start slide on beat? " + on_beat);
        }

        if (m_WasSliding && !slide && !on_beat)
        {
            health.TakeDamage();
            m_WasSliding = false;
            print("Stop slide");
            print("End slide on beat? " + on_beat);
        }

        if (m_Grounded || m_AirControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_RigidBody2D.velocity.y);
            m_RigidBody2D.velocity = Vector3.SmoothDamp(m_RigidBody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }

            if(move != 0)
            {
                animator.SetBool("running", true);
            }
            else
            {
                animator.SetBool("running", false);
            }

            if(m_RigidBody2D.velocity.y != 0)
            {
                animator.SetBool("jumping", true);
            }
        }

        JumpGravity(jump);

        if (m_Grounded && jump)
        {
            m_Grounded = false;
            m_RigidBody2D.AddForce(new Vector2(m_RigidBody2D.velocity.x, m_JumpForce));
        }
        //air Jump
        else if (jump && m_AirJumpsLeft > 0)
        {
            m_Grounded = false;
            m_RigidBody2D.AddForce(new Vector2(0f, m_JumpForce));
            m_AirJumpsLeft--;
        }

        
    }


    void JumpGravity(bool jump)
    {
        if (jump && m_AirJumpsLeft >= 1)
        {

            m_RigidBody2D.velocity = new Vector2(m_RigidBody2D.velocity.x, 0);
        }

        if (m_RigidBody2D.velocity.y < 0) //we are falling
        {
            m_RigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_FallGravity - 1) * Time.deltaTime;
        }
        /*
        else if ((m_RigidBody2D.velocity.y > 0 || m_OnJumpPad) && !Input.GetButton("Jump"))//tab jump
        {
            m_RigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_FallGravity - 1) * Time.deltaTime;
        }
        else if ((m_RigidBody2D.velocity.y > 0 || m_OnJumpPad) && Input.GetButton("Jump") && m_OnJumpPad)
        {
            m_RigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (m_FallGravity - 1) * Time.deltaTime;
        }
        */
    }

    void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnTriggerEnter2D(Collider2D collide)
    {
        if (collide.gameObject.tag == "hurtbox" && this.gameObject.transform.position.y - collide.gameObject.transform.position.y >= 0)
        {
            m_RigidBody2D.velocity = new Vector2(m_RigidBody2D.velocity.x, m_JumpForceOnEnemies);
        }

    }


    //Used by other objects to check Character status
    public bool IsGrounded()
    {
        return m_Grounded;
    }

    public void JumpadOn()
    {
        m_OnJumpPad = true;
    }
    public void JumpadOff()
    {
        m_OnJumpPad = false;
    }
}

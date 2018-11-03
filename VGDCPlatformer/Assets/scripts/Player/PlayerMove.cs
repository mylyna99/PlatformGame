using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private float runSpeed;
    float horizontalMove = 0f;
    bool jump = false;
    bool slide = false;
    public CharacterController2D controller;
    private Queue<GameObject> jump_shadows;
    private Queue<GameObject> slide_shadows;
    private GameObject shadow;
    private Renderer player_rend;

    void Start()
    {
        player_rend = GetComponent<Renderer>(); 

        GameObject[] l_jumpshadows = GameObject.FindGameObjectsWithTag("jumps");
        Array.Sort(l_jumpshadows, delegate (GameObject shadow1, GameObject shadow2)
        {
            return (int)(shadow1.transform.position.x - shadow2.transform.position.x);
        });

        GameObject[] l_slideshadows = GameObject.FindGameObjectsWithTag("slides");
        Array.Sort(l_slideshadows, delegate (GameObject shadow1, GameObject shadow2)
        {
            return (int)(shadow1.transform.position.x - shadow2.transform.position.x);
        });

        jump_shadows = new Queue<GameObject>(l_jumpshadows);
        slide_shadows = new Queue<GameObject>(l_slideshadows);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = runSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            if (peekNextJumpShadow().transform.position.x - gameObject.transform.position.x <= 2f)
            {
                shadow = findNextJumpShadow();
                // print("Finding next shadow: " + shadow);
            }
        }

        if (Input.GetButtonDown("Right"))
        {
            slide = true;
            if (peekNextSlideShadow().transform.position.x - gameObject.transform.position.x <= 2f)
            {
                shadow = findNextSlideShadow();
            }
        }

        if (Input.GetButtonUp("Right"))
        {
            slide = false;
            if (peekNextSlideShadow().transform.position.x - gameObject.transform.position.x <= 2f)
            {
                shadow = findNextSlideShadow();
            }
        }
    }

    private void FixedUpdate()
    {
        float error_bufferx = 0f;
        float error_buffery = 0f;
        if (player_rend != null && shadow != null)
        {
            error_bufferx = Math.Abs(gameObject.transform.position.x - shadow.transform.position.x) / player_rend.bounds.size.x;
            error_buffery = Math.Abs(gameObject.transform.position.y - shadow.transform.position.y) / player_rend.bounds.size.y;
        }
        
        controller.Move(runSpeed * Time.fixedDeltaTime, jump, slide, (error_bufferx <= 0.5f && error_buffery <= 0.5f));
        jump = false;
    }

    private GameObject peekNextJumpShadow()
    {
        return jump_shadows.Peek();
    }

    private GameObject findNextJumpShadow()
    {
        return jump_shadows.Dequeue();
    }

    private GameObject peekNextSlideShadow()
    {
        return slide_shadows.Peek();
    }

    private GameObject findNextSlideShadow()
    {
        return slide_shadows.Dequeue();
    }
}

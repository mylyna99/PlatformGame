using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private float runSpeed;
    float horizontalMove = 0f;
    bool jump = false;
    bool slide = false;
    bool duck = false;
    public CharacterController2D controller;
    private Queue<GameObject> jump_shadows;
    private Queue<GameObject> slide_shadows;
    private Queue<GameObject> duck_shadows;
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

        GameObject[] l_duckshadows = GameObject.FindGameObjectsWithTag("ducks");
        Array.Sort(l_duckshadows, delegate (GameObject shadow1, GameObject shadow2)
        {
            return (int)(shadow1.transform.position.x - shadow2.transform.position.x);
        });

        jump_shadows = new Queue<GameObject>(l_jumpshadows);
        slide_shadows = new Queue<GameObject>(l_slideshadows);
        duck_shadows = new Queue<GameObject>(l_duckshadows);
    }

    // Update is called once per frame
    void Update()
    {
        // BUG: no health lost when player does not click any buttons

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

        if (Input.GetButtonDown("Down"))
        {
            duck = true;
            if (peekNextDuckShadow().transform.position.x - gameObject.transform.position.x <= 2f)
            {
                shadow = findNextDuckShadow();
            }
        }
    }

    private void FixedUpdate()
    {
        // set large error at the beginning of the game, to prevent faulty jumping on beat
        float error_bufferx = 99f;
        float error_buffery = 99f;
        if (player_rend != null && shadow != null)
        {
            error_bufferx = Math.Abs(gameObject.transform.position.x - shadow.transform.position.x) / player_rend.bounds.size.x;
            error_buffery = Math.Abs(gameObject.transform.position.y - shadow.transform.position.y) / player_rend.bounds.size.y;
        }
        
        controller.Move(runSpeed * Time.fixedDeltaTime, jump, slide, duck, (error_bufferx <= 0.5f && error_buffery <= 0.5f));
        jump = false;
        duck = false;
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

    private GameObject peekNextDuckShadow()
    {
        return duck_shadows.Peek();
    }

    private GameObject findNextDuckShadow()
    {
        return duck_shadows.Dequeue();
    }
}

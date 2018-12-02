using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private float runSpeed;
    float horizontalMove = 0f;
    bool jump = false;
    bool slide = false;
    bool m_WasSliding = false;
    bool duck = false;
    bool missed = false;
    bool prev_passed = true;
    bool skip = false;
    public CharacterController2D controller;
    private Queue<GameObject> shadows_q;
    private Queue<GameObject> shadows;
    private GameObject shadow;
    private Renderer player_rend;

    void Start()
    {
        player_rend = GetComponent<Renderer>();

        List<GameObject> l_shadows = new List<GameObject>();
        l_shadows.AddRange(GameObject.FindGameObjectsWithTag("jumps"));
        l_shadows.AddRange(GameObject.FindGameObjectsWithTag("slides"));
        l_shadows.AddRange(GameObject.FindGameObjectsWithTag("ducks"));

        l_shadows.Sort((x, y) => ((int)(x.transform.position.x - y.transform.position.x)));

        shadows = new Queue<GameObject>(l_shadows);
        shadow = findNextShadow();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = runSpeed;

        if (peekNextShadow().transform.position.x - gameObject.transform.position.x <= 2f)
        {
            shadow = findNextShadow();
            print("Finding next shadow: " + shadow);
        }

        // jump = Input.GetButtonDown("Jump");
        if (Input.GetButtonDown("Jump") && shadow.tag.Equals("jumps"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Right") && shadow.tag.Equals("slides"))
        {
            slide = true;
            m_WasSliding = false;
        }

        if (Input.GetButtonUp("Right") && shadow.tag.Equals("slides"))
        {
            slide = false;
            m_WasSliding = true;
        }

        if (Input.GetButtonDown("Down") && shadow.tag.Equals("ducks"))
        {
            duck = true;
        }
    }

    private void FixedUpdate()
    {
        // set no error at the beginning of the game, to prevent faulty jumping on beat
        float error_bufferx = 99f;
        float error_buffery = 99f;
        if (player_rend != null && shadow != null)
        {
            float temp_bufferx = gameObject.transform.position.x - shadow.transform.position.x;
            // if player does not press anything, set as an jump error and not on beat
            if (!prev_passed && Math.Abs(temp_bufferx - 1f) <= 0.1)
            {
                print("missed beat");
                missed = true;
                prev_passed = true;
            }
            float temp_buffery = gameObject.transform.position.y - shadow.transform.position.y;

            error_bufferx = Math.Abs(temp_bufferx) / player_rend.bounds.size.x;
            error_buffery = Math.Abs(temp_buffery) / player_rend.bounds.size.y;
        }

        bool temp = (error_bufferx <= 0.5f && error_buffery <= 0.5f);
        if (temp && !skip && !(jump || duck || (slide && !m_WasSliding) || (m_WasSliding && !slide)))
        {
            prev_passed = false;
        }

        if (temp && (jump || duck || (slide && !m_WasSliding) || (m_WasSliding && !slide)))
        {
            print("on beat");
            prev_passed = true;
            skip = true;
        }
        if (!temp && (jump || duck || (slide && !m_WasSliding) || (m_WasSliding && !slide)))
        {
            skip = true;
        }
        else if (!temp)
        {
            skip = false;
        }
        
        controller.Move(runSpeed * Time.fixedDeltaTime, jump, slide, duck, missed, temp);

        jump = false;
        duck = false;
        missed = false;
    }

    private GameObject peekNextShadow()
    {
        return shadows.Peek();
    }

    private GameObject findNextShadow()
    {
        return shadows.Dequeue();
    }
}

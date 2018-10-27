using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private float runSpeed;
    float horizontalMove = 0f;
    bool jump = false;
    public CharacterController2D controller;
    private Queue<GameObject> shadows_q;
    private GameObject shadow;
    private Renderer player_rend;

    void Start()
    {
        player_rend = GetComponent<Renderer>(); 

        GameObject[] shadows_list = GameObject.FindGameObjectsWithTag("shadow");
        Array.Sort(shadows_list, delegate (GameObject shadow1, GameObject shadow2)
        {
            return (int)(shadow1.transform.position.x - shadow2.transform.position.x);
        });

        shadows_q = new Queue<GameObject>(shadows_list);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = runSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            if (peekNextShadow().transform.position.x - gameObject.transform.position.x <= 2f)
            {
                shadow = findNextShadow();
                // print("Finding next shadow: " + shadow);
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
        
        controller.Move(runSpeed * Time.fixedDeltaTime, jump, (error_bufferx <= 0.5f && error_buffery <= 0.5f));
        jump = false;
    }

    private GameObject peekNextShadow()
    {
        return shadows_q.Peek();
    }

    private GameObject findNextShadow()
    {
        return shadows_q.Dequeue();
    }
}

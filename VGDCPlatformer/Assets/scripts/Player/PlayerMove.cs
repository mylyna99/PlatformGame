using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private float runSpeed;
    float horizontalMove = 0f;
    bool jump = false;
    public CharacterController2D controller;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = runSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(runSpeed * Time.fixedDeltaTime, jump);
        jump = false;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEnemy : MonoBehaviour {

    //====== Move Logic ============
	public bool moveRight = true; //checks if the enemy moves right or left
	public float movSpeed = 40f; //movement of the enemy

    public GameObject player;
    [HideInInspector] public Rigidbody2D m_RigidBody2D;
    private PlayerHealth health;

    private void Start()
    {
        health = player.GetComponent<PlayerHealth>();
        m_RigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        gameObject.transform.position = new Vector2(player.transform.position.x - 2*health.startHealth, player.transform.position.y);
    }

    // Update is called once per frame
    void Update () {
        Vector3 targetVelocity = new Vector2(movSpeed * Time.fixedDeltaTime * 10f, m_RigidBody2D.velocity.y);
        m_RigidBody2D.velocity = targetVelocity;
    }

    //flips the entire gameObject and its components
    void Flip()
    {
        moveRight = !moveRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void MoveCloser()
    {
        gameObject.transform.position = new Vector2(player.transform.position.x - 2 * health.health, player.transform.position.y);
    }
}

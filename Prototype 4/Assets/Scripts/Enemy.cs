using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;

    private float lowerBound = -10.0f;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        // Get the rigidbody of itself, and the player
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Let the enemy follow the player's position
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        // If the enemy falls and reached lower bound, destroy it
        if (transform.position.y < lowerBound)
        {
            Destroy(gameObject);
        }
    }
}

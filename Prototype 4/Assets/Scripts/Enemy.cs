using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemeyRb;
    private GameObject player;

    public float speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemeyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemeyRb.AddForce(lookDirection * speed);

        if (transform.position.y < -10.0f)
        {
            Destroy(gameObject);
        }
    }
}

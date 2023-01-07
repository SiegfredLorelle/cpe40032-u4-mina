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
        enemeyRb.AddForce((player.transform.position - transform.position).normalized * speed);
    }
}

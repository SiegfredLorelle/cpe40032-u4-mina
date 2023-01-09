using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRocket : MonoBehaviour
{
    private Transform target;
    private float speed = 15.0f;
    private float rocketStrength = 50.0f;
    private float rocketDuration = 5.0f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If rocket has target, then follow the target
        if (target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;

        }
        // If no target then destroy the rocket
        else
        {
            Destroy(gameObject);
        }



    }

    // Called by player controller, assigns a new target to the rocket and destroys it when duration is reached
    public void Fire(Transform newTarget)
    {
        target = newTarget;
        Destroy(gameObject, rocketDuration);

    }

    
    void OnCollisionEnter(Collision collision)
    {
        // If rocket collided with its target, push the target and destroy the rocket
        if (collision.gameObject.CompareTag(target.tag))
        {
            Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromRocket = (collision.transform.position - transform.position).normalized;
            targetRb.AddForce(awayFromRocket * rocketStrength, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}


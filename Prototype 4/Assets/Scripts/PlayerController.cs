using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator;

    public float speed = 5.0f;
    public bool hasPowerup = false;
    private float powerupStrength = 50.0f;

    private IEnumerator powerupCooldown;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical"); 
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);

        powerupIndicator.transform.position =  transform.position - new Vector3 (0, 0.7f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {

            if (powerupCooldown != null)
            {
                StopCoroutine(powerupCooldown);
            }

            powerupCooldown = PowerupCountdownRoutine();
            StartCoroutine(powerupCooldown);
            
            
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);


            Destroy(other.gameObject);

        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log($"Collided with: {collision.gameObject.name} and with powerup set to {hasPowerup}");
        }
    }
}

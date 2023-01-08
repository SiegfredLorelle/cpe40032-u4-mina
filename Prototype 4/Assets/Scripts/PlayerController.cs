using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject KnockbackIndicator;
    public GameObject RocketsIndicator;

    public float speed = 5.0f;
    public bool hasKnockbackPowerup = false;
    private float powerupStrength = 50.0f;
    public bool hasRocketsPowerup = false;

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

        Vector3 indicatorLocation = transform.position - new Vector3(0, 0.7f, 0);
        KnockbackIndicator.transform.position = indicatorLocation;
        RocketsIndicator.transform.position = indicatorLocation;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Knockback Powerup") || other.CompareTag("Rockets Powerup"))
        {

            if (powerupCooldown != null)
            {
                StopCoroutine(powerupCooldown);
            }

            powerupCooldown = PowerupCountdownRoutine();
            StartCoroutine(powerupCooldown);

            if (other.CompareTag("Knockback Powerup"))
            { 
                hasKnockbackPowerup = true;
                KnockbackIndicator.gameObject.SetActive(true);
            }

            else if (other.CompareTag("Rockets Powerup"))
            {
                hasRocketsPowerup = true;
                RocketsIndicator.gameObject.SetActive(true);
            }

            Destroy(other.gameObject);

        }

        
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        hasKnockbackPowerup = false;
        hasRocketsPowerup = false;
        KnockbackIndicator.gameObject.SetActive(false);
        RocketsIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasKnockbackPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }


    }

    private void UnleashHomingRockets()
    { 
    }
}

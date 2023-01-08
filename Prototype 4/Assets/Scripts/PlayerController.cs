using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpawnManager spawnManagerScript;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject knockbackIndicator;
    public GameObject rocketsIndicator;
    public GameObject rocket;


    public float speed = 5.0f;
    public bool hasKnockbackPowerup = false;
    private float powerupStrength = 50.0f;
    public bool hasRocketsPowerup = false;

    private IEnumerator powerupCooldown;

    // Start is called before the first frame update
    void Start()
    {
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical"); 
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);

        Vector3 indicatorLocation = transform.position - new Vector3(0, 0.7f, 0);
        knockbackIndicator.transform.position = indicatorLocation;
        rocketsIndicator.transform.position = indicatorLocation;
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
                knockbackIndicator.gameObject.SetActive(true);
            }

            else if (other.CompareTag("Rockets Powerup"))
            {
                hasRocketsPowerup = true;
                rocketsIndicator.gameObject.SetActive(true);
                SpawnRockets();
            }

            Destroy(other.gameObject);

        }

        
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        hasKnockbackPowerup = false;
        hasRocketsPowerup = false;
        knockbackIndicator.gameObject.SetActive(false);
        rocketsIndicator.gameObject.SetActive(false);
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


    private void SpawnRockets()
    {
        {
            for (int i = 0; i < spawnManagerScript.enemyCount; i++)
            {
                Debug.Log(spawnManagerScript.enemiesAlive[i].transform.position);
                GameObject rocketProjectile = Instantiate(rocket, transform.position, rocket.transform.rotation);
                Rigidbody rocketRb = rocketProjectile.GetComponent<Rigidbody>();
                rocketRb.AddForce((spawnManagerScript.enemiesAlive[i].transform.position - transform.position).normalized * 500);
            }
        }
    }



}

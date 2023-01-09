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

    private float speed = 4.0f;
    private Vector3 powerUpIndicatorOffset = new Vector3(0, 0.7f, 0);
    private float lowerBound = -10.0f;

    private bool hasKnockbackPowerup = false;
    private float powerupStrength = 50.0f;
    private bool hasRocketsPowerup = false;

    private IEnumerator powerupCooldown;

    // Start is called before the first frame update
    void Start()
    {
        // Assign components of player and other objects
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player, when pressing Up or Down Arrow Key
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);

        // Powerup indicators follow the position of the player
        Vector3 indicatorLocation = transform.position - powerUpIndicatorOffset;
        knockbackIndicator.transform.position = indicatorLocation;
        rocketsIndicator.transform.position = indicatorLocation;

        // If no more enemies, then disable rocket powerup
        if (spawnManagerScript.enemyCount == 0)
        {
            hasRocketsPowerup = false;
            rocketsIndicator.gameObject.SetActive(false);
        }

        // If there are still enemies, rocket powerup is activated, and spacebar is pressed, then spawn rockets
        else if (hasRocketsPowerup && Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRockets();
        }

        // If player falls and reached lowerbound, then inform the user that the game is over by printing in the console
        if (transform.position.y < lowerBound)
        {
            Destroy(gameObject);
            Destroy(knockbackIndicator);
            Destroy(rocketsIndicator);
            Debug.Log("Game Over!!");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // If player collided with a powerup
        if (other.CompareTag("Knockback Powerup") || other.CompareTag("Rockets Powerup"))
        {
            // If there is a powerup, then turn it off (will be turned on on the following lines, like a reset)
            if (powerupCooldown != null)
            {
                StopCoroutine(powerupCooldown);
                turnOffPowerups();
            }

            // Start the coroutine for the cooldown of the powerup
            powerupCooldown = PowerupDurationRoutine();
            StartCoroutine(powerupCooldown);

            // If the powerup that collides with the player is knockback, then turn on knockback powerup
            if (other.CompareTag("Knockback Powerup"))
            {
                hasKnockbackPowerup = true;
                knockbackIndicator.gameObject.SetActive(true);
            }

            // If the powerup that collides with the player is rockets, then turn on rocket powerup
            else if (other.CompareTag("Rockets Powerup"))
            {
                hasRocketsPowerup = true;
                rocketsIndicator.gameObject.SetActive(true);
            }

            // Destroy the powerup
            Destroy(other.gameObject);
        }
    }

    // Powerup duration routine, turns off the powerup after the duration
    IEnumerator PowerupDurationRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        turnOffPowerups();
    }

    // Turns off all powerups and its indicator
    void turnOffPowerups()
    {
        hasKnockbackPowerup = false;
        hasRocketsPowerup = false;
        knockbackIndicator.gameObject.SetActive(false);
        rocketsIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If player collided with an enemy while knockback poweup is activated, then push the enemy further
        if (collision.gameObject.CompareTag("Enemy") && hasKnockbackPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }


    // Spawns rockets for each enemy alive
    private void SpawnRockets()
    {
        // Loop through each enemy alive
        for (int i = 0; i < spawnManagerScript.enemyCount; i++)
        {   
            // Create the rocket and get its rigid body
            GameObject rocketProjectile = Instantiate(rocket, transform.position, rocket.transform.rotation);
            Rigidbody rocketRb = rocketProjectile.GetComponent<Rigidbody>();

            // Fire the rocket and assign an enemy target (method in Homing Rocket)
            rocketProjectile.GetComponent<HomingRocket>().Fire(spawnManagerScript.enemiesAlive[i].transform);
            
            // Ignore physics when player colides with a rocket 
            Physics.IgnoreCollision(rocketRb.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}

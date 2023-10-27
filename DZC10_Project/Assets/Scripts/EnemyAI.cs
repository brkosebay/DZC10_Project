using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 5.0f;       // The speed at which the enemy moves
    public float detectionRadius = 10.0f; // The distance within which the enemy detects the player
    public Transform[] patrolPoints;     // The points the enemy will patrol between
    public float idleTime = 2.0f;        // How long the enemy idles for

    private Transform playerTransform;   // Reference to the player's position
    private int currentPatrolPoint = 0;  // Current patrol point index
    private float idleTimer = 0.0f;      // Timer for idling
    public int health = 100;
    public GameObject deathEffect;
    public int noOfPoints = 5;
    public float shootingRange = 5.0f;
    public float shootingCooldown = 10.0f;
    private float shootingTimer = 0.0f;
    public GameObject bulletPrefab;

    private enum State
    {
        Idle,
        Patrol,
        Chase
    }

    private State currentState = State.Patrol; // Start with the Patrol state

    private void Start()
    {
        int difficulty = GameManager.Instance.difficultyLevel;
        if (difficulty == 1)
        {
            noOfPoints = 5;
        }
        else if (difficulty == 2)
        {
            noOfPoints = 10;
        }
        else if (difficulty == 3)
        {
            noOfPoints = 20;
        }
        // Find the player using the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("No player found. Please make sure your player has the 'Player' tag.");
        }

    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleBehavior();
                break;
            case State.Patrol:
                PatrolBehavior();
                break;
            case State.Chase:
                ChaseBehavior();
                break;
        }
        if (shootingTimer > 0.0f)
        {
            shootingTimer -= Time.deltaTime;
        }
    }

    private void IdleBehavior()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0f;
            currentState = State.Patrol;
        }
    }

    private void PatrolBehavior()
    {
        if (patrolPoints.Length > 0)
        {
            // Move towards the current patrol point
            Transform targetPoint = patrolPoints[currentPatrolPoint];
            Vector3 moveDirection = (targetPoint.position - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // If close enough to the target point, switch to the next one
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
                currentState = State.Idle;
            }
        }

        // Check for player detection while patrolling
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
        {
            currentState = State.Chase;
        }
    }

    private void ChaseBehavior()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // If the player is within the detection radius, continue chasing
            if (distanceToPlayer <= detectionRadius)
            {
                Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                if (distanceToPlayer <= shootingRange && shootingTimer <= 0.0f)
                {
                    Shoot();
                }
            }
            else
            {
                currentState = State.Patrol; // Switch back to patrol if player is out of range
            }
        }
    }

    private void Shoot()
    {
        // Instantiate the bullet and set its direction
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, direction * 5.0f, Color.red, 2.0f);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * 10.0f; // Assuming bullet has a Rigidbody2D
        
        // Reset shooting timer
        shootingTimer = shootingCooldown;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <=0)
        {
            Die();
        }
    }

    void Die()
    {
        //GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Destroy(effect,0.05f);
        Destroy(gameObject);
        GameManager.Instance.IncreaseScore(noOfPoints);
    }
}

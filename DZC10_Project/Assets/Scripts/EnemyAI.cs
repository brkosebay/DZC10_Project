using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public float patrolSpeed = 0.0f;
    public float chaseSpeed = 3.5f;
    public float detectionRadius = 10.0f;
    public float shootingDistance = 7.0f;
    public float spellSpeed = 7.5f;
    public float shootingCooldown = 2.0f;
    public int health = 100;
    public int noOfPoints = 5;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    private Vector3 spawnPoint;
    private Transform playerTransform;
    private float shootingTimer = 0.0f;
    private bool isChasing = false;

    private void Start() {
        spawnPoint = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (playerTransform != null) {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= detectionRadius && health > 0) {
                isChasing = true;
                ChasePlayer();
            }
            
            // else 
            // {
            //     isChasing = false;
            //     Patrol();
            // }

            if (distanceToPlayer <= shootingDistance && shootingTimer <= 0.0f) {
                Shoot();
            }
        }

        if (shootingTimer > 0.0f) {
            shootingTimer -= Time.deltaTime;
        }

        UpdateAnimations();
    }

    private void Patrol() {
        transform.position = Vector3.MoveTowards(transform.position, spawnPoint, patrolSpeed * Time.deltaTime);
    }

    private void ChasePlayer() {
        if (Vector3.Distance(transform.position, playerTransform.position) > shootingDistance) {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("IsDamaged");
        health -= damage;
        if(health <=0)
        {
            Die();
        }
    }

    void Die()
    {
        enabled = false;
        animator.SetFloat("Horizontal", (playerTransform.position.x - transform.position.x) * 100);
        animator.SetTrigger("IsDead");
        GetComponent<Collider2D>().enabled = false;

        GameManager.Instance.IncreaseScore(noOfPoints);
        Destroy(gameObject, 3f);
    }   

    private void Shoot() {
        Vector2 shootingDirection = playerTransform.position - transform.position;
        shootingDirection.Normalize();
        GameObject spell = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        spell.gameObject.GetComponent<Rigidbody2D>().velocity = shootingDirection * spellSpeed;
        spell.transform.Rotate(0, 0, -Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        shootingTimer = shootingCooldown;

    }

    private void UpdateAnimations() {
        animator.SetFloat("Horizontal", playerTransform.position.x - transform.position.x);
        animator.SetFloat("Vertical", playerTransform.position.y - transform.position.y);
        animator.SetFloat("Speed", isChasing ? chaseSpeed : patrolSpeed);
    }
}

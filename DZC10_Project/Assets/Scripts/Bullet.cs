using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; 
    public Rigidbody2D rb;
    public int damage;
    private bool hasHit = false;
    private Animator bulletAnimator; // Reference to the animator component

    private void Awake()
    {
        bulletAnimator = GetComponent<Animator>(); // Get the animator component
    }

    // Use this for initialization
    void Start()
    {
        // Play the Bullet_impact animation after 0.5 second
        Invoke("PlayImpactAnimation", 0.5f);
    }

    private void Update() {
        if (!hasHit) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!hasHit && (other.CompareTag("Enemy") || other.CompareTag("Player"))) {
            hasHit = true;
            StickToTarget(other.gameObject);
        }
    }

    private void StickToTarget(GameObject target) {
        // Stop the bullet
        speed = 0;

        // Parent the bullet to the target
        transform.SetParent(target.transform);

        // Apply damage if the target has a Health component
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null) {
            targetHealth.TakeDamage(damage);
        }

        // Destroy the bullet after a delay
        Destroy(gameObject, 1.0f);
    }

    void PlayImpactAnimation()
    {
        bulletAnimator.SetTrigger("Impact"); // Set the impact trigger to transition to the Bullet_impact animation
        Destroy(gameObject, 0.5f); // Adjust this value based on the length of your Bullet_impact animation
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {   
        if (hitInfo.gameObject.tag == "Enemy" && gameObject.tag == "Bullet")
        {
            EnemyAI enemy = hitInfo.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            speed = 0f;
            Destroy(gameObject, 1f);

            PlayImpactAnimation(); // Play the Bullet_impact animation when hitting an enemy
        }
        else if (hitInfo.gameObject.tag == "Player" && gameObject.tag == "EnemyBullet")
        {
            Health player = hitInfo.GetComponent<Health>();
            if(player != null)
            {
                player.TakeDamage(damage);
            }

            speed = 0f;
            Destroy(gameObject, 1f);
        }

    }
}

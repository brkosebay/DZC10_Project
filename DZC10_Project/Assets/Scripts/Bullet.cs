using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; 
    public Rigidbody2D rb;
    public int damage;
    //public GameObject impactEffect;
    private Animator bulletAnimator; // Reference to the animator component
    public GameObject impactEffect;

    private void Awake()
    {
        bulletAnimator = GetComponent<Animator>(); // Get the animator component
    }

    // Use this for initialization
    void Start()
    {
        // Play the Bullet_impact animation after 1 second
        Invoke("PlayImpactAnimation", 0.5f);
    }

    void PlayImpactAnimation()
    {
        bulletAnimator.SetTrigger("Impact"); // Set the impact trigger to transition to the Bullet_impact animation
        Destroy(gameObject, 0.5f); // Adjust this value based on the length of your Bullet_impact animation
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log("I HIT " + hitInfo.gameObject.tag + "!");
        
        if (hitInfo.gameObject.tag == "Enemy" && gameObject.tag == "Bullet")
        {
            EnemyAI enemy = hitInfo.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, 0.5f); // Destroy the effect after 0.5 seconds (adjust as needed)
            Destroy(gameObject, 0.1f);

            //PlayImpactAnimation(); // Play the Bullet_impact animation when hitting an enemy
        }
        else if (hitInfo.gameObject.tag == "Player" && gameObject.tag == "EnemyBullet")
        {
            Debug.Log("hit by enemy");
            Health player = hitInfo.GetComponent<Health>();
            if(player != null)
            {
                player.TakeDamage(damage);
            }

            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, 0.5f); // Destroy the effect after 0.5 seconds (adjust as needed)
            Destroy(gameObject, 0.1f);
        }

    }
}

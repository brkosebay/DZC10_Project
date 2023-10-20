using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 20f; 
	public Rigidbody2D rb;
    public int damage = 40;
    public GameObject impactEffect;

	// Use this for initialization
	void Start () {
		rb.velocity = transform.right * speed;
	}
	void OnTriggerEnter2D (Collider2D hitInfo)
	{
		if(hitInfo.gameObject.tag == "Enemy")
		{
            EnemyAI enemy = hitInfo.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effect, 0.1f);  // Destroy the effect after 0.5 seconds (adjust as needed)
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
	}
}

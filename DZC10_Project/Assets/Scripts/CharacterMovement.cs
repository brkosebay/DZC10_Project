using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;  // Add this
using UnityEngine.UIElements.Experimental;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Tilemap tilemap;  // Reference to the tilemap
    public Tile pathTile;     // Reference to the path tile
    Vector2 movement;
    public Animator animator;

    // Update is called once per frame
    private void Update()
    {
        // Allow movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        if(movement.x == 1 || movement.x == -1 || movement.y == 1 || movement.y == -1) {
            animator.SetFloat("LastHorizontal", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
        }
    }
    
    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "DecorationsTilemap")
        {
            // Get the Tilemap component from the collided GameObject.
            Tilemap collidedTilemap = collision.gameObject.GetComponent<Tilemap>();

            if (collidedTilemap == null)
            {
                // Just a safety check. If for some reason the tag is set but there's no Tilemap component.
                return;
            }

            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;

                TileBase impactedTile = collidedTilemap.GetTile(collidedTilemap.WorldToCell(hitPosition));

                if (impactedTile.name == "BushTile")
                {
                    Debug.Log("hit a bush");
                    // Handle the bush collision. For example, stop the character's movement.
                }
            }
        }
    }
}

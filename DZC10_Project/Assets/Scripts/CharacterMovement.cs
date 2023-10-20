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

    // Update is called once per frame
    private void Update()
    {
        // Allow movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        CaveGeneratorWithFloodFill generator = FindObjectOfType<CaveGeneratorWithFloodFill>();
        Vector3Int playerCell = generator.tilemap.WorldToCell(transform.position);
        Vector3Int direction = new Vector3Int(Mathf.RoundToInt(Input.GetAxis("Horizontal")), Mathf.RoundToInt(Input.GetAxis("Vertical")), 0);

        if (IsNearOpenEdge(playerCell, generator, direction))
        {
            Debug.LogWarning("Character is in close proximity to the edge!");
        }

    }
    
    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }


    bool IsNearOpenEdge(Vector3Int playerCell, CaveGeneratorWithFloodFill generator, Vector3Int direction)
    {
        int edgeThreshold = 5;

        int continuousEmptyTiles = 0;

        // Check for tiles in the direction of movement for a distance of edgeThreshold
        for (int dist = 1; dist <= edgeThreshold; dist++)
        {
            Vector3Int checkCell = playerCell + direction * dist;
            TileBase tile = generator.tilemap.GetTile(checkCell);

            if (tile == null) // No tile means it's an empty space
            {
                continuousEmptyTiles++;
            }
            else
            {
                continuousEmptyTiles = 0; // Reset the counter if we find a tile
            }

            if (continuousEmptyTiles == edgeThreshold) // If we have a sequence of edgeThreshold empty tiles
            {
                return true;
            }
        }

        return false;
    }




}

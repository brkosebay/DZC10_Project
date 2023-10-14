using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;  // Add this
using UnityEngine.UIElements.Experimental;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public Tilemap tilemap;  // Reference to the tilemap
    public Tile pathTile;     // Reference to the path tile

    // Update is called once per frame
    private void Update()
    {
        // Allow movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }

        CaveGeneratorWithFloodFill generator = FindObjectOfType<CaveGeneratorWithFloodFill>();
        Vector3Int playerCell = generator.tilemap.WorldToCell(transform.position);
        Vector3Int direction = new Vector3Int(Mathf.RoundToInt(Input.GetAxis("Horizontal")), Mathf.RoundToInt(Input.GetAxis("Vertical")), 0);

        if (IsNearOpenEdge(playerCell, generator, direction))
        {
            Debug.LogWarning("Character is in close proximity to the edge!");
        }

    }

    // In CharacterMovement.cs
    bool IsNearOpenEdge(Vector3Int playerCell, CaveGeneratorWithFloodFill generator, Vector3Int direction)
    {
        int edgeThreshold = 5;  // Adjust this value as needed.

        // Check if near the bounding box.
        bool nearEdge = playerCell.x <= generator.minFloodFill.x + edgeThreshold ||
                        playerCell.y <= generator.minFloodFill.y + edgeThreshold ||
                        playerCell.x >= generator.maxFloodFill.x - edgeThreshold ||
                        playerCell.y >= generator.maxFloodFill.y - edgeThreshold;

        if (nearEdge)
        {
            // Check tiles in a 3x3 grid in the direction of movement to ensure we have a significant open space.
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector3Int checkCell = playerCell + direction + new Vector3Int(i, j, 0);
                    TileBase tile = generator.tilemap.GetTile(checkCell);

                    // If there's a wall tile in any of these cells, then it's not an open edge.
                    if (tile == generator.wallTile)  // Assuming you have a wallTile reference in the generator script.
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        return false;
    }
}

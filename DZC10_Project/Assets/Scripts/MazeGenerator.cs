﻿using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class CaveGeneratorWithFloodFill : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile openTile;
    public Tile wallTile;
    public Tile pathTile;
    public float threshold = 0.4f;
    public int scale = 1;
    public int x0 = 0, y0 = 0;
    public GameObject player;

    private const float PI50000 = Mathf.PI * 50000f;
    private int canvasWidth = 64;
    private int canvasHeight = 64;
    

    void Start()
    {
        GenerateCave();
        // Try to find a random starting point that is an open tile.
        int maxAttempts = 1000;
        int attempts = 0;
        Vector3Int startPoint = new Vector3Int();
        bool foundOpenTile = false;

        while (attempts < maxAttempts)
        {
            int randomX = UnityEngine.Random.Range(0, canvasWidth);
            int randomY = UnityEngine.Random.Range(0, canvasHeight);
            startPoint = new Vector3Int(randomX, randomY, 0);

            if (tilemap.GetTile(startPoint) == openTile)
            {
                foundOpenTile = true;
                break;
            }

            attempts++;
        }

        if (foundOpenTile)
        {
            FloodFill(startPoint, openTile, pathTile);
            player.transform.position = tilemap.CellToWorld(startPoint) + new Vector3(0.5f, 0.5f, 0); // Added offset to center the character in the tile
        }
        else
        {
            Debug.LogWarning("Could not find an open tile to start from after " + maxAttempts + " attempts.");
        }


        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                Tile currentTile = tilemap.GetTile<Tile>(position);

                if (currentTile != pathTile)
                {
                    tilemap.SetTile(position, null);  // Clear the tile if it is not part of the flood-filled path.
                    Debug.Log("Removing tile at: " + position);  // Debug statement
                }
                else
                {
                    Debug.Log("Keeping tile at: " + position);  // Debug statement
                }
            }
        }

        AddWallsAroundFloodFilledArea();

        tilemap.RefreshAllTiles();  // Refresh the tilemap

    }

    void AddWallsAroundFloodFilledArea()
    {
        foreach (Vector3Int filledTile in floodFilledTiles)
        {
            Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };

            foreach (Vector3Int dir in directions)
            {
                Vector3Int next = filledTile + dir;

                if (!floodFilledTiles.Contains(next) &&
                    next.x >= 0 && next.x < canvasWidth &&
                    next.y >= 0 && next.y < canvasHeight)
                {
                    tilemap.SetTile(next, wallTile);
                }
            }
        }
    }

    void GenerateCave()
    {
        for (int ix = 0, x = x0 - canvasWidth / 2; ix < canvasWidth; ix += 3, x++)
        {
            for (int iy = 0, y = y0 - canvasHeight / 2; iy < canvasHeight; iy += 3, y++)
            {
                bool isOpen = IsCaveOpen(x, y, threshold);

                for (int dx = 0; dx < 3; dx++)
                {
                    for (int dy = 0; dy < 3; dy++)
                    {
                        Vector3Int position = new Vector3Int(ix + dx, iy + dy, 0);
                        tilemap.SetTile(position, isOpen ? openTile : wallTile);
                    }
                }
            }
        }
    }

    bool IsCaveOpen(int x, int y, float threshold)
    {
        float u = PI50000 * Mathf.Sin(x) * Mathf.Cos(y);
        return (u - Mathf.Floor(u)) > threshold;
    }

    // A HashSet to keep track of tiles that are part of the flood-filled area
    HashSet<Vector3Int> floodFilledTiles = new HashSet<Vector3Int>();

    void FloodFill(Vector3Int start, Tile fromTile, Tile toTile)
    {
        if (tilemap.GetTile(start) != fromTile)
        {
            return;
        }

        int filledCount = 0;
        Queue<Vector3Int> open = new Queue<Vector3Int>();
        open.Enqueue(start);
        tilemap.SetTile(start, toTile);
        filledCount++;

        while (open.Count > 0)
        {
            Vector3Int current = open.Dequeue();
            Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };

            foreach (Vector3Int dir in directions)
            {
                Vector3Int next = current + dir;

                if (next.x >= 0 && next.x < canvasWidth && next.y >= 0 && next.y < canvasHeight)
                {
                    if (tilemap.GetTile(next) == fromTile)
                    {
                        tilemap.SetTile(next, toTile);
                        open.Enqueue(next);
                        floodFilledTiles.Add(next); // Add the tile to the floodFilledTiles set
                    }
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class CaveGeneratorWithFloodFill : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile openTile;
    public Tile wallTile;
    public Tile pathTile;
    public EnvironmentDecorator environmentDecorator;
    public PositionColliders positionColliders;

    public float threshold = 0.4f;
    public int scale = 1;
    public int x0 = 0, y0 = 0;
    public GameObject player;
    public Vector3Int minFloodFill;
    public Vector3Int maxFloodFill;
    public int canvasWidth = 64;
    public int canvasHeight = 64;
    public delegate void MazeGenerationCompleteHandler();
    public event MazeGenerationCompleteHandler OnMazeGenerationComplete;
    

    private const float PI50000 = Mathf.PI * 50000f;

    void Start()
    {
        int difficulty = GameManager.Instance.difficultyLevel;
        int maxRegenerationAttempts = 20; // Limit the number of times we regenerate the entire cave
        int regenerationAttempts = 0;
        int minNoOfTiles = 0;
        int maxNoOfTiles = 0;
        Vector3Int startPoint = new Vector3Int();
        if (difficulty == 1)
        {
            minNoOfTiles = 300;
            maxNoOfTiles = 600;
        }
        else if (difficulty == 2)
        {
            minNoOfTiles = 600;
            maxNoOfTiles = 1000;

        }
        else if (difficulty == 3)
        {
            minNoOfTiles = 1000;
            maxNoOfTiles = 1500;
        }
        else
        {
            minNoOfTiles = 300;
            maxNoOfTiles = 600;
        }
        do
        {
            GenerateCave();
            bool foundOpenTile = false;

            // Find an open tile to start from
            int maxAttempts = 15000;
            int attempts = 0;
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

                if (floodFilledTiles.Count < minNoOfTiles || floodFilledTiles.Count > maxNoOfTiles)
                {
                    // Not enough tiles in this cave generation, reset and try again
                    floodFilledTiles.Clear();
                    regenerationAttempts++;
                    continue;
                }

                foreach (var tile in floodFilledTiles)
                {
                    Debug.DrawRay(tilemap.CellToWorld(tile), Vector3.up, Color.green, 10f);
                }
                // Keep the startPoint as an openTile or pathTile instead of making it a wall.
                tilemap.SetTile(startPoint, pathTile);
                player.transform.position = tilemap.CellToWorld(startPoint) + new Vector3(0.5f, 0.5f, 0);

            }
            else
            {
                Debug.LogWarning("Could not find an open tile to start from after " + maxAttempts + " attempts.");
            }
        } while (floodFilledTiles.Count < minNoOfTiles || floodFilledTiles.Count > maxNoOfTiles && regenerationAttempts < maxRegenerationAttempts);

        if (regenerationAttempts == maxRegenerationAttempts)
        {
            Debug.LogWarning("Max regeneration attempts reached. The generated cave may not meet the criteria.");
        }

        for (int x = 0; x <= canvasWidth; x++)
        {
            for (int y = 0; y <= canvasHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                Tile currentTile = tilemap.GetTile<Tile>(position);

                // Skip the tile if it's the starting point or if it's a pathTile
                if (position == startPoint || currentTile == pathTile)
                {
                    continue;
                }

                tilemap.SetTile(position, null);  // Clear the tile
            }
        }

        // Then add the walls around the flood-filled area
        AddWallsAroundFloodFilledArea();

        tilemap.RefreshAllTiles();  // Refresh the tilemap

        //DebugWalls();
        // Calculate bounds based on floodFilledTiles.
        minFloodFill = new Vector3Int(int.MaxValue, int.MaxValue, 0);
        maxFloodFill = new Vector3Int(int.MinValue, int.MinValue, 0);

        foreach (Vector3Int tile in floodFilledTiles)
        {
            if (tile.x < minFloodFill.x) minFloodFill.x = tile.x;
            if (tile.y < minFloodFill.y) minFloodFill.y = tile.y;
            if (tile.x > maxFloodFill.x) maxFloodFill.x = tile.x;
            if (tile.y > maxFloodFill.y) maxFloodFill.y = tile.y;
        }

        positionColliders.PositionBoundaryColliders(minFloodFill.x, minFloodFill.y, maxFloodFill.x, maxFloodFill.y);
        NotifyMazeGenerationComplete();
    }

    private void NotifyMazeGenerationComplete()
    {
        OnMazeGenerationComplete?.Invoke();
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

                if (tilemap.GetTile(next) == null &&
                    next.x >= 0 && next.x < canvasWidth+1 &&
                    next.y >= 0 && next.y < canvasHeight+1)
                {
                    tilemap.SetTile(next, wallTile);
                    environmentDecorator.PlaceBushOnWall(next);

                }
            }
        }
    }

    void GenerateCave()
    {
        for (int ix = 0, x = x0 - canvasWidth / 2; ix < canvasWidth - 4; ix += 3, x++)
        {
            for (int iy = 0, y = y0 - canvasHeight / 2; iy < canvasHeight - 4; iy += 3, y++)
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
        float u = PI50000 * Mathf.Sin(x) * Mathf.Cos(y)+ UnityEngine.Random.Range(-0.1f, 0.1f);
        return (u - Mathf.Floor(u)) > threshold;
    }

    void DebugWalls()
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                Tile currentTile = tilemap.GetTile<Tile>(position);

                if (currentTile == wallTile)
                {
                    Debug.DrawRay(tilemap.CellToWorld(position), Vector3.up, Color.red, 10f);
                }
            }
        }
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
        floodFilledTiles.Add(start);

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

                if (next.x >= 0 && next.x < canvasWidth-1 && next.y >= 0 && next.y < canvasHeight-1)
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
    public bool IsCellWalkable(Vector3Int cellPosition)
    {
        return floodFilledTiles.Contains(cellPosition);
    }

    public List<Vector3Int> GetWalkableTiles()
    {
        return new List<Vector3Int>(floodFilledTiles);
    }
}

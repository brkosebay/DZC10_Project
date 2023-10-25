using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;  // Reference to the enemy prefab. Assign this in the inspector.
    public int numberOfEnemies = 5; // Number of enemies you want to spawn.
    public int safeDistanceFromPlayer = 5;  // Minimum distance from the player to spawn an enemy.

    public CaveGeneratorWithFloodFill generator;
    private Transform playerTransform;
    private int maxRetry = 50;

    private void Start()
    {
        generator = FindObjectOfType<CaveGeneratorWithFloodFill>();
        playerTransform = FindObjectOfType<CharacterMovement>().transform;
        int difficulty = GameManager.Instance.difficultyLevel;
        if (difficulty == 1)
        {
            numberOfEnemies = 20;
        }
        else if (difficulty == 2)
        {
            numberOfEnemies = 40;
        }
        else if (difficulty == 3)
        {
            numberOfEnemies = 60;
        }
        generator.OnMazeGenerationComplete += SpawnEnemiesCallback;

    }

    private Vector3Int GetSpawnLocation()
    {
        // Get the list of walkable tiles
        List<Vector3Int> walkableTiles = generator.GetWalkableTiles();

        // If there are no walkable tiles, return a default location
        if (walkableTiles.Count == 0)
        {
            Debug.LogWarning("No walkable tiles found.");
            return new Vector3Int(0, 0, 0);
        }
        else
        {
            Debug.Log($"Number of walkable tiles: {walkableTiles.Count}");
        }

        // Pick a random tile from the list
        Vector3Int randomCell = walkableTiles[Random.Range(0, walkableTiles.Count)];

        int tries = 0;
        while (Vector3Int.Distance(randomCell, generator.tilemap.WorldToCell(playerTransform.position)) < safeDistanceFromPlayer && tries < maxRetry)
        {
            randomCell = walkableTiles[Random.Range(0, walkableTiles.Count)];
            tries++;
        }

        return randomCell;
    }

    private void SpawnEnemiesCallback()
    {
        SpawnEnemies(numberOfEnemies);
    }




    private void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3Int? spawnLocation = GetSpawnLocation();

            if (spawnLocation.HasValue)
            {
                // Convert the cell position to world position for instantiation.
                Vector3 worldSpawnLocation = generator.tilemap.CellToWorld(spawnLocation.Value) + new Vector3(0.5f, 0.5f, 0);

                // Instantiate the enemy prefab at the determined spawn location.
                Instantiate(enemyPrefab, worldSpawnLocation, Quaternion.identity);

                // Debugging information
                Debug.Log($"Spawned enemy {i + 1} at: {worldSpawnLocation}");
                Debug.DrawRay(worldSpawnLocation, Vector3.up, Color.red, 10f); // Draws a ray upwards from the spawn location. It will last for 10 seconds.
            }
        }
    }


}

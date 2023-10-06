using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomMazeGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile wallTile;
    public Tile pathTile;

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public Difficulty difficulty;

    private int width;
    private int height;
    private int[,] maze;

    private const float PI50000 = Mathf.PI * 50000f;

    void Start()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                width = 11;
                height = 11;
                break;
            case Difficulty.Medium:
                width = 21;
                height = 21;
                break;
            case Difficulty.Hard:
                width = 31;
                height = 31;
                break;
        }

        maze = new int[width, height];
        GenerateMaze();
        DrawMaze();
    }

    void GenerateMaze()
    {
        float threshold = 0.48f; // You can adjust this value for different maze complexities

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = IsCaveOpen(x, y, threshold) ? 0 : 1;
            }
        }
    }

    bool IsCaveOpen(int x, int y, float threshold)
    {
        float u = PI50000 * Mathf.Sin(x) * Mathf.Cos(y);
        return (u - Mathf.Floor(u)) > threshold;
    }

    void DrawMaze()
    {
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                tilemap.SetTile(position, maze[x, y] == 1 ? wallTile : pathTile);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentDecorator : MonoBehaviour
{
    public Tilemap decorationTilemap;
    public Tile bushTile1;
    public Tile bushTile2;
    public Tile bushTile3;


    public void PlaceBushOnWall(Vector3Int position)
    {
        int difficulty = GameManager.Instance.difficultyLevel;
        Debug.Log("difficulty" + difficulty);
        if (difficulty == 1)
        {
           decorationTilemap.SetTile(position, bushTile1);
           Debug.Log("easy");
           Debug.Log(bushTile1);
        }
        else if (difficulty == 2)
        {
            decorationTilemap.SetTile(position, bushTile2);
            Debug.Log("medium");
        }
        else if (difficulty == 3)
        {
            decorationTilemap.SetTile(position, bushTile3);
            Debug.Log("hard");
        }
        else
        {
            decorationTilemap.SetTile(position, bushTile1);
            Debug.Log("none");
        }
        
    }
}

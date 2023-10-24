using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentDecorator : MonoBehaviour
{
    public Tilemap decorationTilemap;
    public Tile bushTile;

    public void PlaceBushOnWall(Vector3Int position)
    {
        decorationTilemap.SetTile(position, bushTile);
    }
}

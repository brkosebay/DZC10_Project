using UnityEngine;
using System.Collections.Generic;

public class PositionColliders : MonoBehaviour
{
    public Vector2 cellSize = new Vector2(5, 5);
    public List<Vector2Int> floodFilledTiles = new List<Vector2Int>();

    public GameObject topCollider;
    public GameObject bottomCollider;
    public GameObject leftCollider;
    public GameObject rightCollider;

    public void PositionBoundaryColliders(int minX, int minY, int maxX, int maxY)
    {
        // Convert the tile coordinates to world positions
        Vector2 bottomLeft = new Vector2(minX * cellSize.x, minY * cellSize.y);
        Vector2 topRight = new Vector2((maxX + 1) * cellSize.x, (maxY + 1) * cellSize.y);
        Vector2 topLeft = new Vector2(bottomLeft.x, topRight.y);
        Vector2 bottomRight = new Vector2(topRight.x, bottomLeft.y);

      

        // Position the top collider with offset upwards
        topCollider.transform.position = new Vector3((bottomLeft.x + bottomRight.x) / 2, topRight.y, 0);
        topCollider.transform.localScale = new Vector3(bottomRight.x - bottomLeft.x, cellSize.y, 1);

        // Position the bottom collider with offset downwards
        bottomCollider.transform.position = new Vector3((topLeft.x + topRight.x) / 2, bottomLeft.y - cellSize.y, 0);
        bottomCollider.transform.localScale = new Vector3(topRight.x - topLeft.x, cellSize.y, 1);

        // Position the left collider with offset to the left
        leftCollider.transform.position = new Vector3(bottomLeft.x - cellSize.x, (topLeft.y + bottomLeft.y) / 2, 0);
        leftCollider.transform.localScale = new Vector3(cellSize.x , topLeft.y - bottomLeft.y, 1);

        // Position the right collider with offset to the right
        rightCollider.transform.position = new Vector3(bottomRight.x + cellSize.x, (topRight.y + bottomRight.y) / 2, 0);
        rightCollider.transform.localScale = new Vector3(cellSize.x, topRight.y - bottomRight.y, 1);
    }



}

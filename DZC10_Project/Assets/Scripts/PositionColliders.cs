using UnityEngine;

public class PositionColliders : MonoBehaviour
{
    public int canvasWidth;
    public int canvasHeight;
    public Vector2 cellSize = new Vector2(5, 5);

    public GameObject topCollider;
    public GameObject bottomCollider;
    public GameObject leftCollider;
    public GameObject rightCollider;

    void Start()
    {
        PositionBoundaryColliders();
    }

    void PositionBoundaryColliders()
    {
        // Position the top collider
        topCollider.transform.position = new Vector3(canvasWidth / 2.0f * cellSize.x, canvasHeight * cellSize.y, 0);
        topCollider.transform.localScale = new Vector3(canvasWidth * cellSize.x + 2 * cellSize.x, cellSize.y, 1);  // Add 2 to cover the corners

        // Position the bottom collider
        bottomCollider.transform.position = new Vector3(canvasWidth / 2.0f * cellSize.x, -cellSize.y, 0);  // -1 to position it just outside the canvas
        bottomCollider.transform.localScale = new Vector3(canvasWidth * cellSize.x + 2 * cellSize.x, cellSize.y, 1);  // Add 2 to cover the corners

        // Position the left collider
        leftCollider.transform.position = new Vector3(-cellSize.x, canvasHeight / 2.0f * cellSize.y, 0);
        leftCollider.transform.localScale = new Vector3(cellSize.x, canvasHeight * cellSize.y, 1);

        // Position the right collider
        rightCollider.transform.position = new Vector3(canvasWidth * cellSize.x, canvasHeight / 2.0f * cellSize.y, 0);
        rightCollider.transform.localScale = new Vector3(cellSize.x, canvasHeight * cellSize.y, 1);
    }
}

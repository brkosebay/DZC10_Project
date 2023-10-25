using UnityEngine;
using UnityEngine.Tilemaps;

public class SecondaryColliderScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.tag);

        if (collision.gameObject.tag == "DecorationsTilemap")
        {
            // Get the Tilemap component from the collided GameObject.
            Tilemap collidedTilemap = collision.gameObject.GetComponent<Tilemap>();

            if (collidedTilemap == null)
            {
                // Just a safety check. If for some reason the tag is set but there's no Tilemap component.
                return;
            }

            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;

                TileBase impactedTile = collidedTilemap.GetTile(collidedTilemap.WorldToCell(hitPosition));

                Debug.Log("Impacted tile: " + impactedTile.name);

                if (impactedTile.name == "bushTile")
                {
                    Debug.Log("hit a bush");
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Exited collision with: " + collision.gameObject.tag);
    }
}

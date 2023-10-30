using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeBoundaryDetect : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.tag);
        if (collision.gameObject.tag == "Player")
        {
            // Logic for completing the level
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        // TODO: Add your level completion logic here.
        Debug.Log("Level Completed!");
        SceneManager.LoadScene("WinScene");
    }
}

using UnityEngine;
using UnityEngine.UI;  // Remember to add this for the Unity UI system

public class ScoreDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;  // Drag and drop your UI Text element here in the inspector

    void Update()
    {
        scoreText.text = "Points: " + GameManager.Instance.points;

    }
}

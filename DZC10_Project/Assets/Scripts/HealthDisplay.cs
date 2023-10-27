using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Health playerHealth;
    public Slider healthSlider;

    private void Update()
    {
        healthSlider.value = playerHealth.currentHealth;
    }
}

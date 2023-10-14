using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Health playerHealth;
    public Slider healthSlider;
    public Gradient healthBarGradient;

    private void Update()
    {
        healthSlider.value = playerHealth.currentHealth;
        UpdateHealthBarColor();  // Call the new function to update color
    }

    void UpdateHealthBarColor() // New function
    {
        float healthPercentage = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        Color currentColor = healthBarGradient.Evaluate(healthPercentage);
        healthSlider.fillRect.GetComponent<Image>().color = currentColor;
    }
}

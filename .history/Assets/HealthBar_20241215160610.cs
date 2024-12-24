using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Bar Settings")]
    public Slider healthBar;

    [Header("Time Settings")]
    public float decreaseRate = 1f; // Rate at which health decreases per second

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Initialize the health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        // Decrease health over time
        DecreaseHealth();

        // Update health bar value
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Handle death (when health reaches zero)
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void DecreaseHealth()
    {
        currentHealth -= decreaseRate * Time.deltaTime; // Decrease health based on time

        // Make sure health doesn't go below 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    void Die()
    {
        // Handle death here (e.g., playing animation, disabling character, etc.)
        Debug.Log("Character died!");
    }
}
